using Backend.DTOs;
using Backend.Models.Entities;
using Backend.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class AuthService : IAuthService
{
    private readonly Data.AppDbContext _context;

    public AuthService(Data.AppDbContext context)
    {
        _context = context;
    }

    public async Task<UserProfileDto?> GetUserProfileAsync(string azureAdObjectId)
    {
        var user = await _context.Users
            .Include(u => u.Subsidiary)
            .FirstOrDefaultAsync(u => u.AzureAdObjectId == azureAdObjectId && u.IsActive);

        if (user == null)
            return null;

        return new UserProfileDto(
            user.Id,
            user.Email,
            user.DisplayName,
            user.Role,
            user.Subsidiary?.Name,
            user.Subsidiary?.Code
        );
    }

    public async Task<UserDto?> GetUserByAzureAdObjectIdAsync(string azureAdObjectId)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.AzureAdObjectId == azureAdObjectId);
        return user == null ? null : MapToDto(user);
    }

    public async Task<User?> GetOrCreateUserAsync(string azureAdObjectId, string email, string displayName)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.AzureAdObjectId == azureAdObjectId);

        if (user != null)
            return user;

        user = new User
        {
            Id = Guid.NewGuid(),
            AzureAdObjectId = azureAdObjectId,
            Email = email,
            DisplayName = displayName,
            Role = UserRole.Analista,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public Task<List<UserRoleDto>> GetRolesAsync()
    {
        var roles = new List<UserRoleDto>
        {
            new(UserRole.Analista, "Analista de Tesorería"),
            new(UserRole.Gerente, "Gerente de Finanzas"),
            new(UserRole.Cfo, "Chief Financial Officer"),
            new(UserRole.Auditor, "Auditor"),
            new(UserRole.Admin, "Administrador del Sistema")
        };
        return Task.FromResult(roles);
    }

    public async Task<bool> AssignRoleAsync(Guid userId, UserRole role)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
            return false;

        user.Role = role;
        user.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AssignSubsidiaryAsync(Guid userId, Guid? subsidiaryId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
            return false;

        user.SubsidiaryId = subsidiaryId;
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
