using Backend.Models.Enums;

namespace Backend.DTOs;

public record ApprovalHistoryDto(
    Guid Id,
    Guid PaymentRequestId,
    Guid ApproverId,
    string? ApproverName,
    ApprovalAction Action,
    string? Comments,
    DateTime CreatedAt
);
