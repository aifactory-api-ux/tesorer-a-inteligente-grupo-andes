using Backend.Models.Enums;

namespace Backend.DTOs;

public record CashFlowProjectionDto(
    Guid Id,
    Guid SubsidiaryId,
    string? SubsidiaryName,
    DateOnly ProjectionDate,
    ProjectionDays ProjectionDays,
    decimal ProjectedInflow,
    decimal ProjectedOutflow,
    decimal ProjectedBalance,
    DateTime CalculatedAt
);

public record CashFlowProjectionCalculateDto(
    Guid? SubsidiaryId,
    ProjectionDays ProjectionDays,
    DateOnly Date
);

public record CashFlowSummaryDto(
    decimal TotalInflow,
    decimal TotalOutflow,
    decimal NetFlow,
    List<CashFlowProjectionDto> Projections
);
