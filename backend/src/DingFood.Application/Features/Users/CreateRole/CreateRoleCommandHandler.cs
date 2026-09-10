using DingFood.Application.Abstractions.Messaging;
using DingFood.Domain.Entities;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.Users.CreateRole;

internal sealed class CreateRoleCommandHandler : BaseCommandHandler<CreateRoleCommand, long>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateRoleCommandHandler(
        IRoleRepository roleRepository,
        ILogTrackerRepository logRepository,
        IUnitOfWork unitOfWork)
        : base(logRepository, unitOfWork)
    {
        _roleRepository = roleRepository;
        _unitOfWork = unitOfWork;
    }

    public override Task<Result<long>> Handle(CreateRoleCommand request, CancellationToken cancellationToken) =>
        ExecuteWithLogAsync(
            nameof(CreateRoleCommandHandler),
            nameof(Handle),
            null, // Substitua por request.IpAddress se aplicável
            async (userIdBox) =>
            {
                if (await _roleRepository.ExistsByNameAsync(request.CompanyId, request.Name, cancellationToken))
                    return Result.Failure<long>(new Error("Role.AlreadyExists", "A profile with this name already exists."));

                var role = Role.Create(request.CompanyId, request.Name, request.Description);
                if (role.IsFailure)
                    return Result.Failure<long>(role.Error);

                await _roleRepository.AddAsync(role.Value, cancellationToken);
                await _unitOfWork.CommitAsync(cancellationToken);

                return Result.Success(role.Value.Id);
            });
}
