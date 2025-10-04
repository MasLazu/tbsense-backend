using FluentValidation;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Domain.Entities;
using MasLazu.AspNet.Framework.Application.Interfaces;
using MasLazu.AspNet.Framework.Application.Services;

namespace TbSense.Backend.Services;

public class PlantationService : CrudService<Plantation, PlantationDto, CreatePlantationRequest, UpdatePlantationRequest>, IPlantationService
{
    public PlantationService(
        IRepository<Plantation> repository,
        IReadRepository<Plantation> readRepository,
        IUnitOfWork unitOfWork,
        IEntityPropertyMap<Plantation> propertyMap,
        IPaginationValidator<Plantation> paginationValidator,
        ICursorPaginationValidator<Plantation> cursorPaginationValidator,
        IValidator<CreatePlantationRequest>? createValidator = null,
        IValidator<UpdatePlantationRequest>? updateValidator = null)
        : base(repository, readRepository, unitOfWork, propertyMap, paginationValidator, cursorPaginationValidator, createValidator, updateValidator)
    {
    }
}