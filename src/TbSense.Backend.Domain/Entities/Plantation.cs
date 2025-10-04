using MasLazu.AspNet.Framework.Domain.Entities;

namespace TbSense.Backend.Domain.Entities;

public class Plantation : BaseEntity
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public float LandAreaHectares { get; set; }
    public DateTime PlantedDate { get; set; }

    public ICollection<PlantationCoordinate> PlantationCoordinates { get; set; } = new List<PlantationCoordinate>();
    public ICollection<Tree> Trees { get; set; } = new List<Tree>();
    public ICollection<PlantationYieldPrediction> YieldPredictions { get; set; } = new List<PlantationYieldPrediction>();
    public ICollection<PlantationHarvest> Harvests { get; set; } = new List<PlantationHarvest>();
}