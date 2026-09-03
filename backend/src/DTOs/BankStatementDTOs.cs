using Backend.Models.Enums;

namespace Backend.DTOs;

public record BankStatementLineDto(
    Guid Id,
    int LineNumber,
    DateOnly TransactionDate,
    string? Description,
    string? Reference,
    decimal Credit,
    decimal Debit,
    decimal? Balance,
    bool IsReconciled,
    Guid? ReconciledWithId,
    DateTime CreatedAt
);

public record BankStatementDto(
    Guid Id,
    Guid BankAccountId,
    string? BankAccountName,
    DateOnly StatementDate,
    string? FileName,
    string? FilePath,
    decimal TotalCredits,
    decimal TotalDebits,
    decimal FinalBalance,
    StatementImportStatus ImportStatus,
    Guid? CreatedBy,
    string? CreatedByName,
    DateTime CreatedAt,
    List<BankStatementLineDto>? Lines
);

public record BankStatementUploadDto(
    Guid BankAccountId,
    DateOnly StatementDate,
    string FileName,
    string FilePath,
    decimal TotalCredits,
    decimal TotalDebits,
    decimal FinalBalance,
    List<BankStatementLineCreateDto> Lines
);

public record BankStatementLineCreateDto(
    int LineNumber,
    DateOnly TransactionDate,
    string? Description,
    string? Reference,
    decimal Credit,
    decimal Debit,
    decimal? Balance
);

public record ReconciliationMatchDto(
    Guid LineId,
    Guid? MatchedWithId
);

public record ReconciliationStatusDto(
    Guid BankStatementId,
    int TotalLines,
    int ReconciledLines,
    int PendingLines,
    decimal DifferenceAmount
);
