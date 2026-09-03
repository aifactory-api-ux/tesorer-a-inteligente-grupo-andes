using Backend.DTOs;
using Backend.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class UserService : IUserService
{
    private readonly Data.AppDbContext _context;

    public UserService(Data.AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<UserDto>> GetAllUsersAsync()
    {
        var users = await _context.Users
            .Where(u => u.IsActive)
            .OrderBy(u => u.DisplayName)
            .ToListAsync();

        return users.Select(MapToDto).ToList();
    }

    public async Task<UserDto?> GetUserByIdAsync(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        return user == null ? null : MapToDto(user);
    }

    public async Task<UserDto?> GetUserByAzureAdObjectIdAsync(string azureAdObjectId)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.AzureAdObjectId == azureAdObjectId);
        return user == null ? null : MapToDto(user);
    }

    public async Task<UserDto> CreateUserAsync(UserCreateDto dto)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            AzureAdObjectId = dto.AzureAdObjectId,
            Email = dto.Email,
            DisplayName = dto.DisplayName,
            Role = dto.Role,
            SubsidiaryId = dto.SubsidiaryId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return MapToDto(user);
    }

    public async Task<UserDto?> UpdateUserAsync(Guid id, UserUpdateDto dto)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
            return null;

        if (dto.Email != null)
            user.Email = dto.Email;
        if (dto.DisplayName != null)
            user.DisplayName = dto.DisplayName;
        if (dto.Role.HasValue)
            user.Role = dto.Role.Value;
        if (dto.SubsidiaryId.HasValue)
            user.SubsidiaryId = dto.SubsidiaryId;
        if (dto.IsActive.HasValue)
            user.IsActive = dto.IsActive.Value;

        user.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return MapToDto(user);
    }

    public async Task<bool> DeactivateUserAsync(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
            return false;

        user.IsActive = false;
        user.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ActivateUserAsync(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
            return false;

        user.IsActive = true;
        user.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    private static UserDto MapToDto(User user)
    {
        return new UserDto(
            user.Id,
            user.AzureAdObjectId,
            user.Email,
            user.DisplayName,
            user.Role,
            user.SubsidiaryId,
            user.IsActive,
            user.CreatedAt,
            user.UpdatedAt
        );
    }
}
