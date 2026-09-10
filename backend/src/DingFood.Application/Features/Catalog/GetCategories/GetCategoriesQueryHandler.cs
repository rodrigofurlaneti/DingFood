using DingFood.Application.Abstractions.Messaging;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.Catalog.GetCategories;

internal sealed class GetCategoriesQueryHandler(
    ICategoryRepository categoryRepository,
    ILogTrackerRepository logRepository,
    IUnitOfWork unitOfWork)
    : BaseQueryHandler<GetCategoriesQuery, IReadOnlyCollection<CategoryResponse>>(logRepository, unitOfWork)
{
    public override Task<Result<IReadOnlyCollection<CategoryResponse>>> Handle(
        GetCategoriesQuery request, CancellationToken cancellationToken) =>
        ExecuteWithLogAsync(
            nameof(GetCategoriesQueryHandler),
            nameof(Handle),
            null, // Substitua por request.IpAddress se houver esse campo na Query
            async (userIdBox) =>
            {
                var categories = await categoryRepository.GetByCompanyAsync(request.CompanyId, cancellationToken);

                IReadOnlyCollection<CategoryResponse> response = categories
                    .OrderBy(c => c.DisplayOrder).ThenBy(c => c.Name)
                    .Select(c => new CategoryResponse(c.Id, c.Name, c.DisplayOrder))
                    .ToList();

                return Result.Success(response);
            });
}