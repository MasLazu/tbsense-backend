using MasLazu.AspNet.Framework.Domain.Entities;

namespace TbSense.Backend.Domain.Entities;

public class PlantationYieldPrediction : BaseEntity
{
    public Guid PlantationId { get; set; }
    public Guid ModelId { get; set; }
    public DateTime Timestamp { get; set; }

    public Plantation Plantation { get; set; } = null!;
    public Model Model { get; set; } = null!;
}