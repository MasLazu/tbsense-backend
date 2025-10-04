using MasLazu.AspNet.Framework.Domain.Entities;

namespace TbSense.Backend.Domain.Entities;

public class Model : BaseEntity
{
    public string TrainingStatus { get; set; } = null!;
    public string FilePath { get; set; } = null!;
    public bool IsUsed { get; set; }
    public DateTime TrainingDataStart { get; set; }
    public DateTime TrainingDataEnd { get; set; }

    // Performance Metrics
    public double? Accuracy { get; set; }
    public double? MAE { get; set; }
    public double? RMSE { get; set; }
    public double? R2Score { get; set; }

    public ICollection<PlantationYieldPrediction> YieldPredictions { get; set; } = new List<PlantationYieldPrediction>();
}