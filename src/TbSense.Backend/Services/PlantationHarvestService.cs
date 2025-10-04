using FluentValidation;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Domain.Entities;
using MasLazu.AspNet.Framework.Application.Interfaces;
using MasLazu.AspNet.Framework.Application.Services;

namespace TbSense.Backend.Services;

public class PlantationHarvestService : CrudService<PlantationHarvest, PlantationHarvestDto, CreatePlantationHarvestRequest, UpdatePlantationHarvestRequest>, IPlantationHarvestService
{
    public PlantationHarvestService(
        IRepository<PlantationHarvest> repository,
        IReadRepository<PlantationHarvest> readRepository,
        IUnitOfWork unitOfWork,
        IEntityPropertyMap<PlantationHarvest> propertyMap,
        IPaginationValidator<PlantationHarvest> paginationValidator,
        ICursorPaginationValidator<PlantationHarvest> cursorPaginationValidator,
        IValidator<CreatePlantationHarvestRequest>? createValidator = null,
        IValidator<UpdatePlantationHarvestRequest>? updateValidator = null)
        : base(repository, readRepository, unitOfWork, propertyMap, paginationValidator, cursorPaginationValidator, createValidator, updateValidator)
    {
    }
}