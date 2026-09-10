using DingFood.Application.Abstractions.Integrations.Ifood;
using DingFood.Application.Abstractions.Messaging;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.Catalog.Complements.UnlinkProductComplementGroup;

internal sealed class UnlinkProductComplementGroupCommandHandler : BaseCommandHandler<UnlinkProductComplementGroupCommand>
{
    private readonly IProductComplementGroupRepository _productComplementGroupRepository;
    private readonly IProductRepository _productRepository;
    private readonly IIfoodCatalogSyncTrigger _catalogSyncTrigger;
    private readonly IUnitOfWork _unitOfWork;

    public UnlinkProductComplementGroupCommandHandler(
        IProductComplementGroupRepository productComplementGroupRepository,
        IProductRepository productRepository,
        IIfoodCatalogSyncTrigger catalogSyncTrigger,
        ILogTrackerRepository logRepository,
        IUnitOfWork unitOfWork)
        : base(logRepository, unitOfWork)
    {
        _productComplementGroupRepository = productComplementGroupRepository;
        _productRepository = productRepository;
        _catalogSyncTrigger = catalogSyncTrigger;
        _unitOfWork = unitOfWork;
    }

    public override Task<Result> Handle(UnlinkProductComplementGroupCommand request, CancellationToken cancellationToken) =>
        ExecuteWithLogAsync(
            nameof(UnlinkProductComplementGroupCommandHandler),
            nameof(Handle),
            null, // Substitua por request.IpAddress se aplicável
            async (userIdBox) =>
            {
                var link = await _productComplementGroupRepository.GetByIdForUpdateAsync(request.ProductComplementGroupId, cancellationToken);
                if (link is null || !link.IsActive)
                    return Result.Failure(new Error("ProductComplementGroup.NotFound", "Link not found."));

                link.Deactivate();
                await _unitOfWork.CommitAsync(cancellationToken);

                var product = await _productRepository.GetByIdAsync(link.ProductId, cancellationToken);
                if (product is not null)
                    _catalogSyncTrigger.TriggerCompanySync(product.CompanyId);

                return Result.Success();
            });
}
