namespace Backend.DTOs;

public record DashboardSummaryDto(
    decimal TotalBalance,
    decimal TotalInflow,
    decimal TotalOutflow,
    int ReconciledCount,
    int PendingCount,
    int DifferenceCount,
    List<SubsidiarySummaryDto> BySubsidiary
);

public record SubsidiarySummaryDto(
    Guid SubsidiaryId,
    string SubsidiaryName,
    decimal Balance
);

public record AlertDto(
    Guid Id,
    string Type,
    string Title,
    string Message,
    Guid? EntityId,
    DateTime CreatedAt
);

public record RecentTransactionDto(
    Guid Id,
    DateOnly TransactionDate,
    string? Description,
    decimal Credit,
    decimal Debit,
    bool IsReconciled,
    string? BankAccountName
);

public record DashboardDto(
    DashboardSummaryDto Summary,
    List<RecentTransactionDto> RecentTransactions,
    List<AlertDto> Alerts
);
