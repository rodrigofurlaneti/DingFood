using DingFood.Application.Abstractions.Messaging;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.Employees.GetJobTitles;

internal sealed class GetJobTitlesQueryHandler(
    IJobTitleRepository jobTitleRepository,
    ILogTrackerRepository logRepository,
    IUnitOfWork unitOfWork)
    : BaseQueryHandler<GetJobTitlesQuery, IReadOnlyCollection<JobTitleResponse>>(logRepository, unitOfWork)
{
    public override async Task<Result<IReadOnlyCollection<JobTitleResponse>>> Handle(
        GetJobTitlesQuery request, CancellationToken cancellationToken)
    {
        return await ExecuteWithLogAsync(
            nameof(GetJobTitlesQueryHandler),
            nameof(Handle),
            null, // Substitua pelo IP presente no request, caso aplicável
            async (userIdBox) =>
            {
                var jobTitles = await jobTitleRepository.GetByCompanyAsync(request.CompanyId, cancellationToken);

                IReadOnlyCollection<JobTitleResponse> response = jobTitles
                    .OrderBy(j => j.Name)
                    .Select(j => new JobTitleResponse(j.Id, j.Name))
                    .ToList();

                return Result.Success(response);
            });
    }
}