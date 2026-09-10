using DingFood.Application.Abstractions.Integrations.Ifood;
using DingFood.Application.Abstractions.Messaging;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.Catalog.ActivateProduct;

internal sealed class ActivateProductCommandHandler : BaseCommandHandler<ActivateProductCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IIfoodCatalogSyncTrigger _catalogSyncTrigger;
    private readonly IUnitOfWork _unitOfWork;

    public ActivateProductCommandHandler(
        IProductRepository productRepository,
        IIfoodCatalogSyncTrigger catalogSyncTrigger,
        ILogTrackerRepository logRepository,
        IUnitOfWork unitOfWork)
        : base(logRepository, unitOfWork)
    {
        _productRepository = productRepository;
        _catalogSyncTrigger = catalogSyncTrigger;
        _unitOfWork = unitOfWork;
    }

    public override Task<Result> Handle(ActivateProductCommand request, CancellationToken cancellationToken) =>
        ExecuteWithLogAsync(
            nameof(ActivateProductCommandHandler),
            nameof(Handle),
            null,
            async (userIdBox) =>
            {
                var product = await _productRepository.GetByIdForUpdateAsync(request.ProductId, cancellationToken);
                if (product is null)
                    return Result.Failure(new Error("Product.NotFound", "Product not found."));
                if (product.IsActive)
                    return Result.Success();

                product.Activate();
                await _unitOfWork.CommitAsync(cancellationToken);
                _catalogSyncTrigger.TriggerCompanySync(product.CompanyId);
                return Result.Success();
            });
}
