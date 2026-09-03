namespace Api.Models.Enums;

public enum PaymentStatus
{
    Pending,
    PendingApprovalGerente,
    PendingApprovalCfo,
    Approved,
    Rejected,
    Paid
}
