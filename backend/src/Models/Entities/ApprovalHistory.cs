using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend.Models.Enums;

namespace Backend.Models.Entities;

[Table("approval_history")]
public class ApprovalHistory
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Required]
    [Column("payment_request_id")]
    public Guid PaymentRequestId { get; set; }

    [ForeignKey("PaymentRequestId")]
    public PaymentRequest? PaymentRequest { get; set; }

    [Required]
    [Column("approver_id")]
    public Guid ApproverId { get; set; }

    [ForeignKey("ApproverId")]
    public User? Approver { get; set; }

    [Required]
    [Column("action")]
    public ApprovalAction Action { get; set; }

    [Column("comments")]
    public string? Comments { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
