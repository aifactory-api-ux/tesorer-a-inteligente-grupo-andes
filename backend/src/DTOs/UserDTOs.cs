using Backend.Models.Enums;

namespace Backend.DTOs;

public record UserDto(
    Guid Id,
    string AzureAdObjectId,
    string Email,
    string DisplayName,
    UserRole Role,
    Guid? SubsidiaryId,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record UserProfileDto(
    Guid Id,
    string Email,
    string DisplayName,
    UserRole Role,
    string? SubsidiaryName,
    string? SubsidiaryCode
);

public record UserRoleDto(
    UserRole Role,
    string Description
);

public record UserCreateDto(
    string AzureAdObjectId,
    string Email,
    string DisplayName,
    UserRole Role,
    Guid? SubsidiaryId
);

public record UserUpdateDto(
    string? Email,
    string? DisplayName,
    UserRole? Role,
    Guid? SubsidiaryId,
    bool? IsActive
);
