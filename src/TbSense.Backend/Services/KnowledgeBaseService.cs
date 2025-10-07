using FluentValidation;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Domain.Entities;
using MasLazu.AspNet.Framework.Application.Interfaces;
using MasLazu.AspNet.Framework.Application.Services;

namespace TbSense.Backend.Services;

public class KnowledgeBaseService : CrudService<KnowledgeBase, KnowledgeBaseDto, CreateKnowledgeBaseRequest, UpdateKnowledgeBaseRequest>, IKnowledgeBaseService
{
    public KnowledgeBaseService(
        IRepository<KnowledgeBase> repository,
        IReadRepository<KnowledgeBase> readRepository,
        IUnitOfWork unitOfWork,
        IEntityPropertyMap<KnowledgeBase> propertyMap,
        IPaginationValidator<KnowledgeBase> paginationValidator,
        ICursorPaginationValidator<KnowledgeBase> cursorPaginationValidator,
        IValidator<CreateKnowledgeBaseRequest>? createValidator = null,
        IValidator<UpdateKnowledgeBaseRequest>? updateValidator = null)
        : base(repository, readRepository, unitOfWork, propertyMap, paginationValidator, cursorPaginationValidator, createValidator, updateValidator)
    {
    }
}