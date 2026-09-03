using Backend.DTOs;
using Backend.Models.Enums;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Cfo,Gerente,Auditor")]
    [ProducesResponseType(typeof(ApiResponseDto<List<UserDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(new ApiResponseDto<List<UserDto>>(true, users));
    }

    [HttpGet("{id:guid}")]
    [Authorize(Roles = "Admin,Cfo,Gerente,Auditor")]
    [ProducesResponseType(typeof(ApiResponseDto<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
            return NotFound(new ApiResponseDto<UserDto>(false, null, "Usuario no encontrado"));

        return Ok(new ApiResponseDto<UserDto>(true, user));
    }

    [HttpGet("by-azure/{azureAdObjectId}")]
    [Authorize(Roles = "Admin,Cfo,Gerente,Auditor")]
    [ProducesResponseType(typeof(ApiResponseDto<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserByAzureAdObjectId(string azureAdObjectId)
    {
        var user = await _userService.GetUserByAzureAdObjectIdAsync(azureAdObjectId);
        if (user == null)
            return NotFound(new ApiResponseDto<UserDto>(false, null, "Usuario no encontrado"));

        return Ok(new ApiResponseDto<UserDto>(true, user));
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin,Cfo")]
    [ProducesResponseType(typeof(ApiResponseDto<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserUpdateDto dto)
    {
        var user = await _userService.UpdateUserAsync(id, dto);
        if (user == null)
            return NotFound(new ApiResponseDto<UserDto>(false, null, "Usuario no encontrado"));

        return Ok(new ApiResponseDto<UserDto>(true, user, null, "Usuario actualizado exitosamente"));
    }

    [HttpPost("{id:guid}/deactivate")]
    [Authorize(Roles = "Admin,Cfo")]
    [ProducesResponseType(typeof(ApiResponseDto<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeactivateUser(Guid id)
    {
        var result = await _userService.DeactivateUserAsync(id);
        if (!result)
            return NotFound(new ApiResponseDto<bool>(false, false, "Usuario no encontrado"));

        return Ok(new ApiResponseDto<bool>(true, true, null, "Usuario desactivado exitosamente"));
    }

    [HttpPost("{id:guid}/activate")]
    [Authorize(Roles = "Admin,Cfo")]
    [ProducesResponseType(typeof(ApiResponseDto<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ActivateUser(Guid id)
    {
        var result = await _userService.ActivateUserAsync(id);
        if (!result)
            return NotFound(new ApiResponseDto<bool>(false, false, "Usuario no encontrado"));

        return Ok(new ApiResponseDto<bool>(true, true, null, "Usuario activado exitosamente"));
    }
}
