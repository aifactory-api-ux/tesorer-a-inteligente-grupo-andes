using Backend.DTOs;

namespace Backend.Services;

public interface IUserService
{
    Task<List<UserDto>> GetAllUsersAsync();
    Task<UserDto?> GetUserByIdAsync(Guid id);
    Task<UserDto?> GetUserByAzureAdObjectIdAsync(string azureAdObjectId);
    Task<UserDto> CreateUserAsync(UserCreateDto dto);
    Task<UserDto?> UpdateUserAsync(Guid id, UserUpdateDto dto);
    Task<bool> DeactivateUserAsync(Guid id);
    Task<bool> ActivateUserAsync(Guid id);
}
