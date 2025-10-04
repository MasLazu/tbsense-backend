using FluentValidation;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Domain.Entities;
using MasLazu.AspNet.Framework.Application.Interfaces;
using MasLazu.AspNet.Framework.Application.Services;

namespace TbSense.Backend.Services;

public class TreeMetricService : CrudService<TreeMetric, TreeMetricDto, CreateTreeMetricRequest, UpdateTreeMetricRequest>, ITreeMetricService
{
    public TreeMetricService(
        IRepository<TreeMetric> repository,
        IReadRepository<TreeMetric> readRepository,
        IUnitOfWork unitOfWork,
        IEntityPropertyMap<TreeMetric> propertyMap,
        IPaginationValidator<TreeMetric> paginationValidator,
        ICursorPaginationValidator<TreeMetric> cursorPaginationValidator,
        IValidator<CreateTreeMetricRequest>? createValidator = null,
        IValidator<UpdateTreeMetricRequest>? updateValidator = null)
        : base(repository, readRepository, unitOfWork, propertyMap, paginationValidator, cursorPaginationValidator, createValidator, updateValidator)
    {
    }
}