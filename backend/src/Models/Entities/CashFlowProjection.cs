using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend.Models.Enums;

namespace Backend.Models.Entities;

[Table("cash_flow_projections")]
public class CashFlowProjection
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
    [Column("projection_date")]
    public DateOnly ProjectionDate { get; set; }

    [Required]
    [Column("projection_days")]
    public ProjectionDays ProjectionDays { get; set; }

    [Column("projected_inflow")]
    public decimal ProjectedInflow { get; set; }

    [Column("projected_outflow")]
    public decimal ProjectedOutflow { get; set; }

    [Column("projected_balance")]
    public decimal ProjectedBalance { get; set; }

    [Column("calculated_at")]
    public DateTime CalculatedAt { get; set; } = DateTime.UtcNow;
}
