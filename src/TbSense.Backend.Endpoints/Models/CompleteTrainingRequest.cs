using Microsoft.AspNetCore.Http;

namespace TbSense.Backend.Endpoints.Models;

public class CompleteTrainingRequest
{
    public Guid ModelId { get; set; }
    public IFormFile? ModelFile { get; set; }
    public double Accuracy { get; set; }
    public double MAE { get; set; }
    public double RMSE { get; set; }
    public double R2Score { get; set; }
}