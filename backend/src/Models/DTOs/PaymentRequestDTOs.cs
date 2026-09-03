namespace Api.Models.DTOs;

public class PaymentRequestCreateDto
{
    public Guid SubsidiaryId { get; set; }
    public string VendorName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "CLP";
    public DateOnly RequestDate { get; set; }
    public DateOnly? DueDate { get; set; }
}

public class PaymentRequestUpdateDto
{
    public string? Status { get; set; }
    public string? RejectionReason { get; set; }
    public string? PaymentProofPath { get; set; }
}

public class PaymentRequestApproveDto
{
    public string? Comments { get; set; }
}

public class PaymentRequestRejectDto
{
    public string Reason { get; set; } = string.Empty;
}

public class PaymentRequestMarkPaidDto
{
    public string? ProofPath { get; set; }
}

public class PaymentRequestResponseDto
{
    public Guid Id { get; set; }
    public Guid SubsidiaryId { get; set; }
    public string VendorName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public DateOnly RequestDate { get; set; }
    public DateOnly? DueDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? RejectionReason { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? ApprovedBy { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public string? PaymentProofPath { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class ApprovalHistoryResponseDto
{
    public Guid Id { get; set; }
    public Guid PaymentRequestId { get; set; }
    public Guid ApproverId { get; set; }
    public string Action { get; set; } = string.Empty;
    public string? Comments { get; set; }
    public DateTime CreatedAt { get; set; }
}
