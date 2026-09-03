using Backend.Models.Enums;

namespace Backend.DTOs;

public record ExpectedCollectionDto(
    Guid Id,
    Guid SubsidiaryId,
    string? SubsidiaryName,
    string CustomerName,
    decimal Amount,
    DateOnly ExpectedDate,
    DateOnly? ActualDate,
    CollectionStatus Status,
    string? Notes,
    Guid? CreatedBy,
    string? CreatedByName,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record ExpectedCollectionCreateDto(
    Guid SubsidiaryId,
    string CustomerName,
    decimal Amount,
    DateOnly ExpectedDate,
    string? Notes
);

public record ExpectedCollectionUpdateDto(
    string? CustomerName,
    decimal? Amount,
    DateOnly? ExpectedDate,
    DateOnly? ActualDate,
    CollectionStatus? Status,
    string? Notes
);
