using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Integrations.Ifood.Orders;

// Fase 9b — disputas Handshake (POST disputes/{disputeId}/reject). Ver ressalva em
// AcceptIfoodDisputeCommand.
public sealed record RejectIfoodDisputeCommand(long BranchId, string DisputeId, string Reason) : ICommand<IfoodDisputeActionResponse>;
