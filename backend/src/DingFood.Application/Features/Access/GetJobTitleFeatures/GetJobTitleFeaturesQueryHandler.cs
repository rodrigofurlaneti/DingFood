using DingFood.Application.Abstractions.Messaging;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.Access.GetJobTitleFeatures;

internal sealed class GetJobTitleFeaturesQueryHandler(
    IJobTitleFeatureRepository jobTitleFeatureRepository,
    ILogTrackerRepository logRepository,
    IUnitOfWork unitOfWork)
    : BaseQueryHandler<GetJobTitleFeaturesQuery, IReadOnlyCollection<long>>(logRepository, unitOfWork)
{
    public override Task<Result<IReadOnlyCollection<long>>> Handle(
        GetJobTitleFeaturesQuery request, CancellationToken cancellationToken) =>
        ExecuteWithLogAsync(nameof(GetJobTitleFeaturesQueryHandler), nameof(Handle), null, async (userIdBox) =>
        {
            var links = await jobTitleFeatureRepository.GetByJobTitleAsync(request.JobTitleId, cancellationToken);
            IReadOnlyCollection<long> response = links.Select(l => l.AppFeatureId).ToList();
            return Result.Success(response);
        });
}