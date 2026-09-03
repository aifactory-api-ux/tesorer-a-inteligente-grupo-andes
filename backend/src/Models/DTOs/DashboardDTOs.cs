namespace Api.Models.DTOs;

public class SubsidiarySummaryDto
{
    public Guid SubsidiaryId { get; set; }
    public string SubsidiaryName { get; set; } = string.Empty;
    public decimal Balance { get; set; }
}

public class DashboardSummaryDto
{
    public decimal TotalBalance { get; set; }
    public decimal TotalInflow { get; set; }
    public decimal TotalOutflow { get; set; }
    public int ReconciledCount { get; set; }
    public int PendingCount { get; set; }
    public int DifferenceCount { get; set; }
    public List<SubsidiarySummaryDto> BySubsidiary { get; set; } = new();
}

public class AlertDto
{
    public Guid Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? EntityId { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CashFlowProjectionDto
{
    public Guid Id { get; set; }
    public Guid SubsidiaryId { get; set; }
    public DateOnly ProjectionDate { get; set; }
    public int ProjectionDays { get; set; }
    public decimal ProjectedInflow { get; set; }
    public decimal ProjectedOutflow { get; set; }
    public decimal ProjectedBalance { get; set; }
    public DateTime CalculatedAt { get; set; }
}

public class CashFlowCalculateDto
{
    public Guid? SubsidiaryId { get; set; }
    public int Days { get; set; }
    public DateOnly Date { get; set; }
}

public class AuditLogResponseDto
{
    public Guid Id { get; set; }
    public Guid? UserId { get; set; }
    public string Action { get; set; } = string.Empty;
    public string EntityType { get; set; } = string.Empty;
    public Guid EntityId { get; set; }
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class PaginatedResultDto<T>
{
    public List<T> Items { get; set; } = new();
    public int Total { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}
