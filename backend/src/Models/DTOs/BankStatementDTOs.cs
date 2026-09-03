namespace Api.Models.DTOs;

public class BankStatementLineDto
{
    public Guid Id { get; set; }
    public int LineNumber { get; set; }
    public DateOnly TransactionDate { get; set; }
    public string? Description { get; set; }
    public string? Reference { get; set; }
    public decimal Credit { get; set; }
    public decimal Debit { get; set; }
    public decimal? Balance { get; set; }
    public bool IsReconciled { get; set; }
    public Guid? ReconciledWithId { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class BankStatementCreateDto
{
    public Guid BankAccountId { get; set; }
    public DateOnly StatementDate { get; set; }
}

public class BankStatementUploadDto
{
    public Guid BankAccountId { get; set; }
    public DateOnly StatementDate { get; set; }
    public string? FileName { get; set; }
    public string? FilePath { get; set; }
}

public class BankStatementResponseDto
{
    public Guid Id { get; set; }
    public Guid BankAccountId { get; set; }
    public DateOnly StatementDate { get; set; }
    public string? FileName { get; set; }
    public string? FilePath { get; set; }
    public decimal TotalCredits { get; set; }
    public decimal TotalDebits { get; set; }
    public decimal FinalBalance { get; set; }
    public string ImportStatus { get; set; } = string.Empty;
    public Guid? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<BankStatementLineDto> Lines { get; set; } = new();
}

public class ReconciliationStatusDto
{
    public Guid BankStatementId { get; set; }
    public int TotalLines { get; set; }
    public int ReconciledLines { get; set; }
    public int PendingLines { get; set; }
    public decimal DifferenceAmount { get; set; }
}

public class ManualMatchDto
{
    public Guid LineId { get; set; }
    public Guid MatchedWithId { get; set; }
}

public class UnmatchDto
{
    public Guid LineId { get; set; }
}
