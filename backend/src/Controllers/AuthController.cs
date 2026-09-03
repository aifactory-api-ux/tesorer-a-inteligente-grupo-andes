using System.Security.Claims;
using Backend.DTOs;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/auth")]
[Authorize]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IUserService _userService;

    public AuthController(IAuthService authService, IUserService userService)
    {
        _authService = authService;
        _userService = userService;
    }

    [HttpGet("profile")]
    [ProducesResponseType(typeof(ApiResponseDto<UserProfileDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProfile()
    {
        var azureAdObjectId = GetAzureAdObjectId();
        if (string.IsNullOrEmpty(azureAdObjectId))
            return Unauthorized();

        var profile = await _authService.GetUserProfileAsync(azureAdObjectId);
        if (profile == null)
            return NotFound(new ApiResponseDto<UserProfileDto>(false, null, "Usuario no encontrado"));

        return Ok(new ApiResponseDto<UserProfileDto>(true, profile, null, "Perfil obtenido exitosamente"));
    }

    [HttpGet("roles")]
    [ProducesResponseType(typeof(ApiResponseDto<List<UserRoleDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRoles()
    {
        var roles = await _authService.GetRolesAsync();
        return Ok(new ApiResponseDto<List<UserRoleDto>>(true, roles));
    }

    [HttpPost("users")]
    [Authorize(Roles = "Admin,Cfo")]
    [ProducesResponseType(typeof(ApiResponseDto<UserDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUser([FromBody] UserCreateDto dto)
    {
        if (string.IsNullOrEmpty(dto.AzureAdObjectId) || string.IsNullOrEmpty(dto.Email))
            return BadRequest(new ApiResponseDto<UserDto>(false, null, "Datos inválidos"));

        var existingUser = await _authService.GetUserByAzureAdObjectIdAsync(dto.AzureAdObjectId);
        if (existingUser != null)
            return BadRequest(new ApiResponseDto<UserDto>(false, null, "El usuario ya existe"));

        var user = await _userService.CreateUserAsync(dto);
        return CreatedAtAction(nameof(GetProfile), new ApiResponseDto<UserDto>(true, user, null, "Usuario creado exitosamente"));
    }

    [HttpPost("users/{userId:guid}/role")]
    [Authorize(Roles = "Admin,Cfo")]
    [ProducesResponseType(typeof(ApiResponseDto<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> AssignRole(Guid userId, [FromBody] AssignRoleDto dto)
    {
        var result = await _authService.AssignRoleAsync(userId, dto.Role);
        if (!result)
            return NotFound(new ApiResponseDto<bool>(false, false, "Usuario no encontrado"));

        return Ok(new ApiResponseDto<bool>(true, true, null, "Rol asignado exitosamente"));
    }

    [HttpPost("users/{userId:guid}/subsidiary")]
    [Authorize(Roles = "Admin,Cfo")]
    [ProducesResponseType(typeof(ApiResponseDto<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> AssignSubsidiary(Guid userId, [FromBody] AssignSubsidiaryDto dto)
    {
        var result = await _authService.AssignSubsidiaryAsync(userId, dto.SubsidiaryId);
        if (!result)
            return NotFound(new ApiResponseDto<bool>(false, false, "Usuario no encontrado"));

        return Ok(new ApiResponseDto<bool>(true, true, null, "Filial asignada exitosamente"));
    }

    private string? GetAzureAdObjectId()
    {
        return User.FindFirstValue("http://schemas.microsoft.com/identity/claims/objectidentifier")
               ?? User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}

public record AssignRoleDto(Backend.Models.Enums.UserRole Role);
public record AssignSubsidiaryDto(Guid? SubsidiaryId);
