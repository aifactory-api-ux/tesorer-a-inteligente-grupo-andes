namespace Backend.Models.Enums;

public enum PaymentRequestStatus
{
    Pending,
    PendingApprovalGerente,
    PendingApprovalCfo,
    Approved,
    Rejected,
    Paid
}
