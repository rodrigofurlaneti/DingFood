using DingFood.Application.Abstractions.Messaging;
using DingFood.Domain.Entities;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.Customers.Create;

internal sealed class CreateCustomerCommandHandler : BaseCommandHandler<CreateCustomerCommand, long>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCustomerCommandHandler(
        ICustomerRepository customerRepository,
        ILogTrackerRepository logRepository,
        IUnitOfWork unitOfWork)
        : base(logRepository, unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public override async Task<Result<long>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        return await ExecuteWithLogAsync(
            nameof(CreateCustomerCommandHandler),
            nameof(Handle),
            null,
            async (userIdBox) =>
            {
                var customer = Customer.Create(request.CompanyId, request.Name, request.Phone, request.Cpf, request.Email);
                if (customer.IsFailure)
                    return Result.Failure<long>(customer.Error);

                await _customerRepository.AddAsync(customer.Value, cancellationToken);
                await _unitOfWork.CommitAsync(cancellationToken);

                return Result.Success(customer.Value.Id);
            });
    }
}