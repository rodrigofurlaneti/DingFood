using DingFood.Application.Abstractions.Messaging;
using DingFood.Domain.Entities;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.Employees.CreateJobTitle;

internal sealed class CreateJobTitleCommandHandler : BaseCommandHandler<CreateJobTitleCommand, long>
{
    private readonly IJobTitleRepository _jobTitleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateJobTitleCommandHandler(
        IJobTitleRepository jobTitleRepository,
        ILogTrackerRepository logRepository,
        IUnitOfWork unitOfWork)
        : base(logRepository, unitOfWork)
    {
        _jobTitleRepository = jobTitleRepository;
        _unitOfWork = unitOfWork;
    }

    public override Task<Result<long>> Handle(CreateJobTitleCommand request, CancellationToken cancellationToken) =>
        ExecuteWithLogAsync(
            nameof(CreateJobTitleCommandHandler),
            nameof(Handle),
            null, // Substitua por request.IpAddress se aplicável
            async (userIdBox) =>
            {
                var jobTitle = JobTitle.Create(request.CompanyId, null, request.Name);
                if (jobTitle.IsFailure)
                    return Result.Failure<long>(jobTitle.Error);

                await _jobTitleRepository.AddAsync(jobTitle.Value, cancellationToken);
                await _unitOfWork.CommitAsync(cancellationToken);

                return Result.Success(jobTitle.Value.Id);
            });
}
