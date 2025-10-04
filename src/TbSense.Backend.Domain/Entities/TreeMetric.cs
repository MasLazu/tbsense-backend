using MasLazu.AspNet.Framework.Domain.Entities;

namespace TbSense.Backend.Domain.Entities;

public class TreeMetric : BaseEntity
{
    public Guid TreeId { get; set; }
    public float SoilMoisture { get; set; }
    public float SoilTemperature { get; set; }
    public float AirTemperature { get; set; }
    public DateTime Timestamp { get; set; }

    public Tree Tree { get; set; } = null!;
}