using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models.Entities;

[Table("bank_statement_lines")]
public class BankStatementLine
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Required]
    [Column("bank_statement_id")]
    public Guid BankStatementId { get; set; }

    [ForeignKey("BankStatementId")]
    public BankStatement? BankStatement { get; set; }

    [Required]
    [Column("line_number")]
    public int LineNumber { get; set; }

    [Required]
    [Column("transaction_date")]
    public DateOnly TransactionDate { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Column("reference")]
    [MaxLength(255)]
    public string? Reference { get; set; }

    [Column("credit")]
    public decimal Credit { get; set; }

    [Column("debit")]
    public decimal Debit { get; set; }

    [Column("balance")]
    public decimal? Balance { get; set; }

    [Column("is_reconciled")]
    public bool IsReconciled { get; set; }

    [Column("reconciled_with_id")]
    public Guid? ReconciledWithId { get; set; }

    [ForeignKey("ReconciledWithId")]
    public BankStatementLine? ReconciledWith { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
