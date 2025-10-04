using MasLazu.AspNet.Framework.Domain.Entities;

namespace TbSense.Backend.Domain.Entities;

public class Tree : BaseEntity
{
    public Guid PlantationId { get; set; }

    public Plantation Plantation { get; set; } = null!;
    public ICollection<TreeMetric> Metrics { get; set; } = new List<TreeMetric>();
}