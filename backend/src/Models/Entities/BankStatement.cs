using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend.Models.Enums;

namespace Backend.Models.Entities;

[Table("bank_statements")]
public class BankStatement
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Required]
    [Column("bank_account_id")]
    public Guid BankAccountId { get; set; }

    [ForeignKey("BankAccountId")]
    public BankAccount? BankAccount { get; set; }

    [Required]
    [Column("statement_date")]
    public DateOnly StatementDate { get; set; }

    [Column("file_name")]
    [MaxLength(255)]
    public string? FileName { get; set; }

    [Column("file_path")]
    [MaxLength(500)]
    public string? FilePath { get; set; }

    [Column("total_credits")]
    public decimal TotalCredits { get; set; }

    [Column("total_debits")]
    public decimal TotalDebits { get; set; }

    [Required]
    [Column("final_balance")]
    public decimal FinalBalance { get; set; }

    [Column("import_status")]
    public StatementImportStatus ImportStatus { get; set; } = StatementImportStatus.Pending;

    [Column("created_by")]
    public Guid? CreatedBy { get; set; }

    [ForeignKey("CreatedBy")]
    public User? Creator { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<BankStatementLine> Lines { get; set; } = new List<BankStatementLine>();
}
