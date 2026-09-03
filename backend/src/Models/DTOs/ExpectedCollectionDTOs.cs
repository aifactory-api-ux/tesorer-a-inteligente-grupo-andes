namespace Api.Models.DTOs;

public class ExpectedCollectionCreateDto
{
    public Guid SubsidiaryId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateOnly ExpectedDate { get; set; }
    public string? Notes { get; set; }
}

public class ExpectedCollectionUpdateDto
{
    public DateOnly? ActualDate { get; set; }
    public string? Status { get; set; }
    public string? Notes { get; set; }
}

public class ExpectedCollectionResponseDto
{
    public Guid Id { get; set; }
    public Guid SubsidiaryId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateOnly ExpectedDate { get; set; }
    public DateOnly? ActualDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
