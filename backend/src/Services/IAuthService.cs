using Backend.DTOs;
using Backend.Models.Entities;
using Backend.Models.Enums;

namespace Backend.Services;

public interface IAuthService
{
    Task<UserProfileDto?> GetUserProfileAsync(string azureAdObjectId);
    Task<UserDto?> GetUserByAzureAdObjectIdAsync(string azureAdObjectId);
    Task<User?> GetOrCreateUserAsync(string azureAdObjectId, string email, string displayName);
    Task<List<UserRoleDto>> GetRolesAsync();
    Task<bool> AssignRoleAsync(Guid userId, UserRole role);
    Task<bool> AssignSubsidiaryAsync(Guid userId, Guid? subsidiaryId);
}
