using FluentValidation;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Domain.Entities;
using MasLazu.AspNet.Framework.Application.Interfaces;
using MasLazu.AspNet.Framework.Application.Services;

namespace TbSense.Backend.Services;

public class PlantationYieldPredictionService : CrudService<PlantationYieldPrediction, PlantationYieldPredictionDto, CreatePlantationYieldPredictionRequest, UpdatePlantationYieldPredictionRequest>, IPlantationYieldPredictionService
{
    public PlantationYieldPredictionService(
        IRepository<PlantationYieldPrediction> repository,
        IReadRepository<PlantationYieldPrediction> readRepository,
        IUnitOfWork unitOfWork,
        IEntityPropertyMap<PlantationYieldPrediction> propertyMap,
        IPaginationValidator<PlantationYieldPrediction> paginationValidator,
        ICursorPaginationValidator<PlantationYieldPrediction> cursorPaginationValidator,
        IValidator<CreatePlantationYieldPredictionRequest>? createValidator = null,
        IValidator<UpdatePlantationYieldPredictionRequest>? updateValidator = null)
        : base(repository, readRepository, unitOfWork, propertyMap, paginationValidator, cursorPaginationValidator, createValidator, updateValidator)
    {
    }
}