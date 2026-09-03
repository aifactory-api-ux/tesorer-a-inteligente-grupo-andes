namespace Backend.DTOs;

public record AuditLogDto(
    Guid Id,
    string EntityType,
    Guid EntityId,
    string Action,
    string? OldValues,
    string? NewValues,
    Guid UserId,
    string? UserEmail,
    string? IpAddress,
    DateTime CreatedAt
);

public record AuditLogQueryDto(
    string? EntityType = null,
    Guid? EntityId = null,
    string? Action = null,
    Guid? UserId = null,
    DateTime? FromDate = null,
    DateTime? ToDate = null,
    int Page = 1,
    int PageSize = 50
);
