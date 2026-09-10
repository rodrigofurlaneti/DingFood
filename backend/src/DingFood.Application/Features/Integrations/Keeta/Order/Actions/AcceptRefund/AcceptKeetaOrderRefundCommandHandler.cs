using DingFood.Application.Abstractions.Integrations.Keeta;
using DingFood.Application.Abstractions.Messaging;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.Integrations.Keeta.Order.Actions.AcceptRefund
{
    internal sealed class AcceptKeetaOrderRefundCommandHandler : BaseCommandHandler<AcceptKeetaOrderRefundCommand>
    {
        private readonly IKeetaIntegrationOrderRepository _orderRepository;
        private readonly IKeetaIntegrationRefundDisputeRepository _disputeRepository;
        private readonly IKeetaOrderClient _orderClient;
        private readonly IUnitOfWork _unitOfWork;

        public AcceptKeetaOrderRefundCommandHandler(
            IKeetaIntegrationOrderRepository orderRepository,
            IKeetaIntegrationRefundDisputeRepository disputeRepository,
            IKeetaOrderClient orderClient,
            ILogTrackerRepository logRepository,
            IUnitOfWork unitOfWork)
            : base(logRepository, unitOfWork)
        {
            _orderRepository = orderRepository;
            _disputeRepository = disputeRepository;
            _orderClient = orderClient;
            _unitOfWork = unitOfWork;
        }

        public override async Task<Result> Handle(AcceptKeetaOrderRefundCommand request, CancellationToken cancellationToken)
        {
            return await ExecuteWithLogAsync(
                nameof(AcceptKeetaOrderRefundCommandHandler),
                nameof(Handle),
                null,
                async (_) =>
                {
                    var order = await _orderRepository.GetByIdForUpdateAsync(request.OrderId, cancellationToken);
                    if (order is null)
                        return Result.Failure(Error.NotFound("KeetaOrder.NotFound", "Pedido Keeta não encontrado."));

                    await _orderClient.AcceptRefundAsync(order.CompanyId, order.BranchId, order.KeetaOrderId, cancellationToken);

                    order.ChangeStatus("REFUND_ACCEPTED");
                    _orderRepository.Update(order);

                    var dispute = await _disputeRepository.GetByOrderIdAsync(order.KeetaOrderId, cancellationToken);
                    if (dispute is not null)
                    {
                        dispute.Resolve(accepted: true);
                        _disputeRepository.Update(dispute);
                    }

                    await _unitOfWork.CommitAsync(cancellationToken);

                    return Result.Success();
                });
        }
    }
}
