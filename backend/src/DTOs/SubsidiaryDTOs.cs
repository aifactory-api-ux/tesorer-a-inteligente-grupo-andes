namespace Backend.DTOs;

public record SubsidiaryDto(
    Guid Id,
    string Name,
    string Code,
    bool IsActive,
    DateTime CreatedAt
);

public record SubsidiaryCreateDto(
    string Name,
    string Code
);

public record SubsidiaryUpdateDto(
    string? Name,
    bool? IsActive
);
