using MasLazu.AspNet.Framework.Application.Interfaces;
using TbSense.Backend.Abstraction.Models;

namespace TbSense.Backend.Abstraction.Interfaces;

public interface IPlantationService : ICrudService<PlantationDto, CreatePlantationRequest, UpdatePlantationRequest>
{
}