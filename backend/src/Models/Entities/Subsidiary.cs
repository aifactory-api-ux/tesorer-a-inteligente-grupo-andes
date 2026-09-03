using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models.Entities;

[Table("subsidiaries")]
public class Subsidiary
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Required]
    [Column("name")]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Column("code")]
    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<User> Users { get; set; } = new List<User>();
    public ICollection<BankAccount> BankAccounts { get; set; } = new List<BankAccount>();
    public ICollection<ExpectedCollection> ExpectedCollections { get; set; } = new List<ExpectedCollection>();
    public ICollection<PaymentRequest> PaymentRequests { get; set; } = new List<PaymentRequest>();
    public ICollection<CashFlowProjection> CashFlowProjections { get; set; } = new List<CashFlowProjection>();
}
