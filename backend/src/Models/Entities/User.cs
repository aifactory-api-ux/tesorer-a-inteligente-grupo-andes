using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend.Models.Enums;

namespace Backend.Models.Entities;

[Table("users")]
public class User
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Required]
    [Column("azure_ad_object_id")]
    [MaxLength(255)]
    public string AzureAdObjectId { get; set; } = string.Empty;

    [Required]
    [Column("email")]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Column("display_name")]
    [MaxLength(255)]
    public string DisplayName { get; set; } = string.Empty;

    [Required]
    [Column("role")]
    public UserRole Role { get; set; }

    [Column("subsidiary_id")]
    public Guid? SubsidiaryId { get; set; }

    [ForeignKey("SubsidiaryId")]
    public Subsidiary? Subsidiary { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
