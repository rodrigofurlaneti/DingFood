using DingFood.Application.Abstractions.Messaging;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.Customers.AddLoyaltyPoints;

internal sealed class AddLoyaltyPointsCommandHandler : BaseCommandHandler<AddLoyaltyPointsCommand>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddLoyaltyPointsCommandHandler(
        ICustomerRepository customerRepository,
        ILogTrackerRepository logRepository,
        IUnitOfWork unitOfWork)
        : base(logRepository, unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public override async Task<Result> Handle(AddLoyaltyPointsCommand request, CancellationToken cancellationToken)
    {
        return await ExecuteWithLogAsync(
            nameof(AddLoyaltyPointsCommandHandler),
            nameof(Handle),
            null, // Substitua pelo IP presente no request, caso aplicável
            async (userIdBox) =>
            {
                var customer = await _customerRepository.GetByIdForUpdateAsync(request.CustomerId, cancellationToken);
                if (customer is null || !customer.IsActive)
                    return Result.Failure(new Error("Customer.NotFound", "Customer not found."));

                var result = customer.AddLoyaltyPoints(request.Points);
                if (result.IsFailure)
                    return result;

                await _unitOfWork.CommitAsync(cancellationToken);
                return Result.Success();
            });
    }
}