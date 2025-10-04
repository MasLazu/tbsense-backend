using FluentValidation;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Domain.Entities;
using MasLazu.AspNet.Framework.Application.Interfaces;
using MasLazu.AspNet.Framework.Application.Services;

namespace TbSense.Backend.Services;

public class PlantationCoordinateService : CrudService<PlantationCoordinate, PlantationCoordinateDto, CreatePlantationCoordinateRequest, UpdatePlantationCoordinateRequest>, IPlantationCoordinateService
{
    public PlantationCoordinateService(
        IRepository<PlantationCoordinate> repository,
        IReadRepository<PlantationCoordinate> readRepository,
        IUnitOfWork unitOfWork,
        IEntityPropertyMap<PlantationCoordinate> propertyMap,
        IPaginationValidator<PlantationCoordinate> paginationValidator,
        ICursorPaginationValidator<PlantationCoordinate> cursorPaginationValidator,
        IValidator<CreatePlantationCoordinateRequest>? createValidator = null,
        IValidator<UpdatePlantationCoordinateRequest>? updateValidator = null)
        : base(repository, readRepository, unitOfWork, propertyMap, paginationValidator, cursorPaginationValidator, createValidator, updateValidator)
    {
    }
}