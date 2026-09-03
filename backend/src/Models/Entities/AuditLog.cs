using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models.Entities;

[Table("audit_logs")]
public class AuditLog
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Required]
    [Column("entity_type")]
    [MaxLength(255)]
    public string EntityType { get; set; } = string.Empty;

    [Required]
    [Column("entity_id")]
    public Guid EntityId { get; set; }

    [Required]
    [Column("action")]
    [MaxLength(50)]
    public string Action { get; set; } = string.Empty;

    [Column("old_values")]
    public string? OldValues { get; set; }

    [Column("new_values")]
    public string? NewValues { get; set; }

    [Required]
    [Column("user_id")]
    public Guid UserId { get; set; }

    [ForeignKey("UserId")]
    public User? User { get; set; }

    [Column("user_email")]
    [MaxLength(255)]
    public string? UserEmail { get; set; }

    [Column("ip_address")]
    [MaxLength(50)]
    public string? IpAddress { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
