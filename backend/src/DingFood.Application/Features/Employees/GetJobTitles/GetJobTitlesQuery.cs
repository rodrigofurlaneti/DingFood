using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Employees.GetJobTitles;

public sealed record GetJobTitlesQuery(long CompanyId) : IQuery<IReadOnlyCollection<JobTitleResponse>>;
