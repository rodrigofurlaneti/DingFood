using DingFood.Application.Abstractions.Authentication;
using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.CustomerAppUser.Create;
using DingFood.Domain.Entities;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.CustomerAppUser.Create;

internal sealed class CreateCustomerAppUserCommandHandler : BaseCommandHandler<CreateCustomerAppUserCommand, long>
{
    private readonly ICustomerAppUserRepository _customerAppUserRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCustomerAppUserCommandHandler(
        ICustomerAppUserRepository customerAppUserRepository,
        ICustomerRepository customerRepository,
        ILogTrackerRepository logRepository,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork)
        : base(logRepository, unitOfWork)
    {
        _customerAppUserRepository = customerAppUserRepository;
        _customerRepository = customerRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

    public override async Task<Result<long>> Handle(CreateCustomerAppUserCommand request, CancellationToken cancellationToken)
    {
        return await ExecuteWithLogAsync(
            nameof(CreateCustomerAppUserCommandHandler),
            nameof(Handle),
            null,
            async (userIdBox) =>
            {
                long? customerId = request.CustomerId;
                if (!customerId.HasValue && !string.IsNullOrWhiteSpace(request.UserName))
                {
                    var customerResult = Customer.Create(
                        request.CompanyId,
                        request.UserName,
                        request.Phone,
                        request.Cpf,
                        request.Email
                    );
                    if (customerResult.IsFailure)
                        return Result.Failure<long>(customerResult.Error);

                    var customer = customerResult.Value;
                    await _customerRepository.AddAsync(customer, cancellationToken);
                    await _unitOfWork.CommitAsync(cancellationToken);
                    customerId = customer.Id;
                }

                string passwordHash = _passwordHasher.Hash(request.Password);
                var customerAppUserResult = DingFood.Domain.Entities.CustomerAppUser.Create(
                    request.CompanyId,
                    request.BranchId,
                    customerId,
                    request.UserName,
                    request.Email,
                    passwordHash
                );

                if (customerAppUserResult.IsFailure)
                    return Result.Failure<long>(customerAppUserResult.Error);

                var entity = customerAppUserResult.Value;
                await _customerAppUserRepository.AddAsync(entity, cancellationToken);
                await _unitOfWork.CommitAsync(cancellationToken);
                return Result.Success(customerId ?? 0);
            });
    }
}