using MasLazu.AspNet.Framework.Domain.Entities;

namespace TbSense.Backend.Domain.Entities;

public class PlantationHarvest : BaseEntity
{
    public Guid PlantationId { get; set; }
    public float YieldKg { get; set; }
    public DateTime HarvestDate { get; set; }

    public Plantation Plantation { get; set; } = null!;
}