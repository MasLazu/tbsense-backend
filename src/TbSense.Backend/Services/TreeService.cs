using FluentValidation;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Domain.Entities;
using MasLazu.AspNet.Framework.Application.Interfaces;
using MasLazu.AspNet.Framework.Application.Services;

namespace TbSense.Backend.Services;

public class TreeService : CrudService<Tree, TreeDto, CreateTreeRequest, UpdateTreeRequest>, ITreeService
{
    public TreeService(
        IRepository<Tree> repository,
        IReadRepository<Tree> readRepository,
        IUnitOfWork unitOfWork,
        IEntityPropertyMap<Tree> propertyMap,
        IPaginationValidator<Tree> paginationValidator,
        ICursorPaginationValidator<Tree> cursorPaginationValidator,
        IValidator<CreateTreeRequest>? createValidator = null,
        IValidator<UpdateTreeRequest>? updateValidator = null)
        : base(repository, readRepository, unitOfWork, propertyMap, paginationValidator, cursorPaginationValidator, createValidator, updateValidator)
    {
    }
}