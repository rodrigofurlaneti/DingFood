using DingFood.Application.Abstractions.Messaging;
using DingFood.Domain.Entities;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.Access.GetMyFeatures;

internal sealed class GetMyFeaturesQueryHandler(
    IAppUserRepository userRepository,
    IEmployeeRepository employeeRepository,
    IAppFeatureRepository featureRepository,
    IJobTitleFeatureRepository jobTitleFeatureRepository,
    IAppUserFeatureRepository userFeatureRepository,
    ILogTrackerRepository logRepository,
    IUnitOfWork unitOfWork,
    DingFood.Application.Abstractions.Tenancy.ICurrentTenantService? tenant = null,
    DingFood.Application.Abstractions.Tenancy.ICompanyAccessService? companyAccess = null)
    : BaseQueryHandler<GetMyFeaturesQuery, MyFeaturesResponse>(logRepository, unitOfWork)
{
    public override Task<Result<MyFeaturesResponse>> Handle(GetMyFeaturesQuery request, CancellationToken cancellationToken) =>
        ExecuteWithLogAsync(nameof(GetMyFeaturesQueryHandler), nameof(Handle), null, async (userIdBox) =>
        {
            // Informa explicitamente o ID do usuário logado para o rastreador de log salvar na coluna AppUserId
            userIdBox.Value = request.AppUserId;

            var allFeatures = await featureRepository.GetAllAsync(cancellationToken);

            if (request.IsManager)
                return Result.Success(new MyFeaturesResponse(true, allFeatures.Select(f => f.Code).ToList()));

            var user = await userRepository.GetByIdAsync(request.AppUserId, cancellationToken);
            if (user is null && tenant?.CompanyId is { } companyId && companyAccess is not null)
            {
                var membership = await companyAccess.ResolveAsync(request.AppUserId, companyId, cancellationToken);
                if (membership is null) return Result.Failure<MyFeaturesResponse>(new Error("Company.Forbidden", "Empresa não autorizada."));
                var ids = new HashSet<long>();
                if (membership.EmployeeId is { } employeeId)
                {
                    var employee = await employeeRepository.GetByIdAsync(employeeId, cancellationToken);
                    if (employee is { IsActive: true })
                        foreach (var link in await jobTitleFeatureRepository.GetByJobTitleAsync(employee.JobTitleId, cancellationToken)) ids.Add(link.AppFeatureId);
                }
                return Result.Success(new MyFeaturesResponse(false, allFeatures.Where(f => ids.Contains(f.Id)).Select(f => f.Code).ToList()));
            }
            if (user is null || !user.IsActive)
                return Result.Failure<MyFeaturesResponse>(new Error("AppUser.NotFound", "User not found."));

            var featureIds = await GetFeatureIdsForUserAsync(user, cancellationToken);

            var codes = allFeatures
                .Where(f => featureIds.Contains(f.Id))
                .Select(f => f.Code)
                .ToList();

            return Result.Success(new MyFeaturesResponse(false, codes));
        });

    private async Task<HashSet<long>> GetFeatureIdsForUserAsync(AppUser user, CancellationToken cancellationToken)
    {
        var featureIds = new HashSet<long>();

        await AddJobTitleFeatureIdsAsync(user, featureIds, cancellationToken);

        var byUser = await userFeatureRepository.GetByUserAsync(user.Id, cancellationToken);
        foreach (var link in byUser)
            featureIds.Add(link.AppFeatureId);

        return featureIds;
    }

    private async Task AddJobTitleFeatureIdsAsync(AppUser user, HashSet<long> featureIds, CancellationToken cancellationToken)
    {
        if (!user.EmployeeId.HasValue)
            return;

        var employee = await employeeRepository.GetByIdAsync(user.EmployeeId.Value, cancellationToken);
        if (employee is null || !employee.IsActive)
            return;

        var byJobTitle = await jobTitleFeatureRepository.GetByJobTitleAsync(employee.JobTitleId, cancellationToken);
        foreach (var link in byJobTitle)
            featureIds.Add(link.AppFeatureId);
    }
}
