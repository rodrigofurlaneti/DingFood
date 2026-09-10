using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Abstractions.Storage;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.Catalog.SetProductImage;

internal sealed class SetProductImageCommandHandler : BaseCommandHandler<SetProductImageCommand, string>
{
    private readonly IProductRepository _productRepository;
    private readonly IImageStorage _imageStorage;
    private readonly IUnitOfWork _unitOfWork;

    public SetProductImageCommandHandler(
        IProductRepository productRepository,
        IImageStorage imageStorage,
        ILogTrackerRepository logRepository,
        IUnitOfWork unitOfWork)
        : base(logRepository, unitOfWork)
    {
        _productRepository = productRepository;
        _imageStorage = imageStorage;
        _unitOfWork = unitOfWork;
    }

    public override Task<Result<string>> Handle(SetProductImageCommand request, CancellationToken cancellationToken) =>
        ExecuteWithLogAsync(
            nameof(SetProductImageCommandHandler),
            nameof(Handle),
            null, // Substitua por request.IpAddress se o IP estiver disponível no Command
            async (userIdBox) =>
            {
                var product = await _productRepository.GetByIdForUpdateAsync(request.ProductId, cancellationToken);
                if (product is null || !product.IsActive)
                    return Result.Failure<string>(new Error("Product.NotFound", "Product not found."));

                var url = await _imageStorage.SaveProductImageAsync(
                    product.Id, request.Extension.ToLowerInvariant(), request.Content, cancellationToken);

                product.SetImage(url);
                await _unitOfWork.CommitAsync(cancellationToken);

                return Result.Success(url);
            });
}