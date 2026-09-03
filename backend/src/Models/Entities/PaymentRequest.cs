using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend.Models.Enums;

namespace Backend.Models.Entities;

[Table("payment_requests")]
public class PaymentRequest
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
    [Column("vendor_name")]
    [MaxLength(255)]
    public string VendorName { get; set; } = string.Empty;

    [Column("description")]
    public string? Description { get; set; }

    [Required]
    [Column("amount")]
    public decimal Amount { get; set; }

    [Column("currency")]
    [MaxLength(3)]
    public string Currency { get; set; } = "CLP";

    [Required]
    [Column("request_date")]
    public DateOnly RequestDate { get; set; }

    [Column("due_date")]
    public DateOnly? DueDate { get; set; }

    [Column("status")]
    public PaymentRequestStatus Status { get; set; } = PaymentRequestStatus.Pending;

    [Column("rejection_reason")]
    public string? RejectionReason { get; set; }

    [Column("created_by")]
    public Guid? CreatedBy { get; set; }

    [ForeignKey("CreatedBy")]
    public User? Creator { get; set; }

    [Column("approved_by")]
    public Guid? ApprovedBy { get; set; }

    [ForeignKey("ApprovedBy")]
    public User? Approver { get; set; }

    [Column("approved_at")]
    public DateTime? ApprovedAt { get; set; }

    [Column("payment_proof_path")]
    [MaxLength(500)]
    public string? PaymentProofPath { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<ApprovalHistory> ApprovalHistories { get; set; } = new List<ApprovalHistory>();
}
