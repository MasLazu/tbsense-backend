using MasLazu.AspNet.Framework.Domain.Entities;

namespace TbSense.Backend.Domain.Entities;

public class PlantationCoordinate : BaseEntity
{
    public Guid PlantationId { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }

    public Plantation Plantation { get; set; } = null!;
}