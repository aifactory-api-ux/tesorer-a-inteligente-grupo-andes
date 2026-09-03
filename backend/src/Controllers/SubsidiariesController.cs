using Backend.DTOs;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/subsidiaries")]
[Authorize]
public class SubsidiariesController : ControllerBase
{
    private readonly ISubsidiaryService _subsidiaryService;

    public SubsidiariesController(ISubsidiaryService subsidiaryService)
    {
        _subsidiaryService = subsidiaryService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseDto<List<SubsidiaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllSubsidiaries()
    {
        var subsidiaries = await _subsidiaryService.GetAllSubsidiariesAsync();
        return Ok(new ApiResponseDto<List<SubsidiaryDto>>(true, subsidiaries));
    }

    [HttpGet("active")]
    [ProducesResponseType(typeof(ApiResponseDto<List<SubsidiaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActiveSubsidiaries()
    {
        var subsidiaries = await _subsidiaryService.GetActiveSubsidiariesAsync();
        return Ok(new ApiResponseDto<List<SubsidiaryDto>>(true, subsidiaries));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponseDto<SubsidiaryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSubsidiaryById(Guid id)
    {
        var subsidiary = await _subsidiaryService.GetSubsidiaryByIdAsync(id);
        if (subsidiary == null)
            return NotFound(new ApiResponseDto<SubsidiaryDto>(false, null, "Filial no encontrada"));

        return Ok(new ApiResponseDto<SubsidiaryDto>(true, subsidiary));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponseDto<SubsidiaryDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSubsidiary([FromBody] SubsidiaryCreateDto dto)
    {
        if (string.IsNullOrEmpty(dto.Name) || string.IsNullOrEmpty(dto.Code))
            return BadRequest(new ApiResponseDto<SubsidiaryDto>(false, null, "Nombre y código son requeridos"));

        try
        {
            var subsidiary = await _subsidiaryService.CreateSubsidiaryAsync(dto);
            return CreatedAtAction(nameof(GetSubsidiaryById),
                new { id = subsidiary.Id },
                new ApiResponseDto<SubsidiaryDto>(true, subsidiary, null, "Filial creada exitosamente"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponseDto<SubsidiaryDto>(false, null, ex.Message));
        }
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin,Cfo")]
    [ProducesResponseType(typeof(ApiResponseDto<SubsidiaryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateSubsidiary(Guid id, [FromBody] SubsidiaryUpdateDto dto)
    {
        var subsidiary = await _subsidiaryService.UpdateSubsidiaryAsync(id, dto);
        if (subsidiary == null)
            return NotFound(new ApiResponseDto<SubsidiaryDto>(false, null, "Filial no encontrada"));

        return Ok(new ApiResponseDto<SubsidiaryDto>(true, subsidiary, null, "Filial actualizada exitosamente"));
    }

    [HttpPost("{id:guid}/deactivate")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponseDto<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeactivateSubsidiary(Guid id)
    {
        var result = await _subsidiaryService.DeactivateSubsidiaryAsync(id);
        if (!result)
            return NotFound(new ApiResponseDto<bool>(false, false, "Filial no encontrada"));

        return Ok(new ApiResponseDto<bool>(true, true, null, "Filial desactivada exitosamente"));
    }

    [HttpPost("{id:guid}/activate")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponseDto<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ActivateSubsidiary(Guid id)
    {
        var result = await _subsidiaryService.ActivateSubsidiaryAsync(id);
        if (!result)
            return NotFound(new ApiResponseDto<bool>(false, false, "Filial no encontrada"));

        return Ok(new ApiResponseDto<bool>(true, true, null, "Filial activada exitosamente"));
    }
}
