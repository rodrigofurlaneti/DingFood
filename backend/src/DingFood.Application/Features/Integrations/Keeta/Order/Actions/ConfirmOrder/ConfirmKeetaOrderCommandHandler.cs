using DingFood.Application.Abstractions.Integrations.Keeta;
using DingFood.Application.Abstractions.Messaging;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.Integrations.Keeta.Order.Actions.ConfirmOrder
{
    internal sealed class ConfirmKeetaOrderCommandHandler : BaseCommandHandler<ConfirmKeetaOrderCommand>
    {
        private readonly IKeetaIntegrationOrderRepository _orderRepository;
        private readonly IKeetaOrderClient _orderClient;
        private readonly IUnitOfWork _unitOfWork;

        public ConfirmKeetaOrderCommandHandler(
            IKeetaIntegrationOrderRepository orderRepository,
            IKeetaOrderClient orderClient,
            ILogTrackerRepository logRepository,
            IUnitOfWork unitOfWork)
            : base(logRepository, unitOfWork)
        {
            _orderRepository = orderRepository;
            _orderClient = orderClient;
            _unitOfWork = unitOfWork;
        }

        public override async Task<Result> Handle(ConfirmKeetaOrderCommand request, CancellationToken cancellationToken)
        {
            return await ExecuteWithLogAsync(
                nameof(ConfirmKeetaOrderCommandHandler),
                nameof(Handle),
                null,
                async (_) =>
                {
                    var order = await _orderRepository.GetByIdForUpdateAsync(request.OrderId, cancellationToken);
                    if (order is null)
                        return Result.Failure(Error.NotFound("KeetaOrder.NotFound", "Pedido Keeta não encontrado."));

                    await _orderClient.ConfirmOrderAsync(
                        order.CompanyId,
                        order.BranchId,
                        order.KeetaOrderId,
                        order.DisplayId,
                        order.OrderCreatedAtUtc,
                        request.Reason,
                        request.PreparationTimeMinutes,
                        cancellationToken);

                    order.MarkAsConfirmed();
                    _orderRepository.Update(order);
                    await _unitOfWork.CommitAsync(cancellationToken);

                    return Result.Success();
                });
        }
    }
}
