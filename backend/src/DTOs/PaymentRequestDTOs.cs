using Backend.Models.Enums;

namespace Backend.DTOs;

public record PaymentRequestDto(
    Guid Id,
    Guid SubsidiaryId,
    string? SubsidiaryName,
    string VendorName,
    string? Description,
    decimal Amount,
    string Currency,
    DateOnly RequestDate,
    DateOnly? DueDate,
    PaymentRequestStatus Status,
    string? RejectionReason,
    Guid? CreatedBy,
    string? CreatedByName,
    Guid? ApprovedBy,
    string? ApprovedByName,
    DateTime? ApprovedAt,
    string? PaymentProofPath,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    List<ApprovalHistoryDto>? ApprovalHistories
);

public record PaymentRequestCreateDto(
    Guid SubsidiaryId,
    string VendorName,
    string? Description,
    decimal Amount,
    string Currency = "CLP",
    DateOnly? RequestDate = null,
    DateOnly? DueDate = null
);

public record PaymentRequestUpdateDto(
    string? VendorName,
    string? Description,
    decimal? Amount,
    string? Currency,
    DateOnly? DueDate
);

public record PaymentRequestApproveDto(
    string? Comments
);

public record PaymentRequestRejectDto(
    string Reason
);

public record PaymentRequestMarkPaidDto(
    string? PaymentProofPath
);
