using Backend.DTOs;

namespace Backend.Services;

public interface ISubsidiaryService
{
    Task<List<SubsidiaryDto>> GetAllSubsidiariesAsync();
    Task<List<SubsidiaryDto>> GetActiveSubsidiariesAsync();
    Task<SubsidiaryDto?> GetSubsidiaryByIdAsync(Guid id);
    Task<SubsidiaryDto> CreateSubsidiaryAsync(SubsidiaryCreateDto dto);
    Task<SubsidiaryDto?> UpdateSubsidiaryAsync(Guid id, SubsidiaryUpdateDto dto);
    Task<bool> DeactivateSubsidiaryAsync(Guid id);
    Task<bool> ActivateSubsidiaryAsync(Guid id);
}
