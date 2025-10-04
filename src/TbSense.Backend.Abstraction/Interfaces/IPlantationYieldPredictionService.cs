using MasLazu.AspNet.Framework.Application.Interfaces;
using TbSense.Backend.Abstraction.Models;

namespace TbSense.Backend.Abstraction.Interfaces;

public interface IPlantationYieldPredictionService : ICrudService<PlantationYieldPredictionDto, CreatePlantationYieldPredictionRequest, UpdatePlantationYieldPredictionRequest>
{
}