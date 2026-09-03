using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend.Models.Enums;

namespace Backend.Models.Entities;

[Table("bank_accounts")]
public class BankAccount
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Required]
    [Column("subsidiary_id")]
    public Guid SubsidiaryId { get; set; }

    [ForeignKey("SubsidiaryId")]
    public Subsidiary? Subsidiary { get; set; }

    [Required]
    [Column("bank_name")]
    [MaxLength(255)]
    public string BankName { get; set; } = string.Empty;

    [Required]
    [Column("account_number")]
    [MaxLength(50)]
    public string AccountNumber { get; set; } = string.Empty;

    [Required]
    [Column("account_type")]
    public AccountType AccountType { get; set; }

    [Column("currency")]
    [MaxLength(3)]
    public string Currency { get; set; } = "CLP";

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<BankStatement> BankStatements { get; set; } = new List<BankStatement>();
}
