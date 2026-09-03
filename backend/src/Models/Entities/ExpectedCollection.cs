using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend.Models.Enums;

namespace Backend.Models.Entities;

[Table("expected_collections")]
public class ExpectedCollection
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
    [Column("customer_name")]
    [MaxLength(255)]
    public string CustomerName { get; set; } = string.Empty;

    [Required]
    [Column("amount")]
    public decimal Amount { get; set; }

    [Required]
    [Column("expected_date")]
    public DateOnly ExpectedDate { get; set; }

    [Column("actual_date")]
    public DateOnly? ActualDate { get; set; }

    [Column("status")]
    public CollectionStatus Status { get; set; } = CollectionStatus.Pending;

    [Column("notes")]
    public string? Notes { get; set; }

    [Column("created_by")]
    public Guid? CreatedBy { get; set; }

    [ForeignKey("CreatedBy")]
    public User? Creator { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
