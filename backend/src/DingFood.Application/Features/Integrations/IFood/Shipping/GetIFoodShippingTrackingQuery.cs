using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Integrations.Ifood.Shipping;

public sealed record IfoodShippingTrackingResponse(
    double? Latitude, double? Longitude, DateTime? ExpectedDelivery, double? DeliveryEtaEndMinutes, double? PickupEtaStartMinutes);

// Id aqui é o Id LOCAL (long) do IfoodShippingDelivery — o handler resolve o IfoodDeliveryId
// (string do Ifood) internamente antes de chamar o cliente HTTP.
public sealed record GetIfoodShippingTrackingQuery(long Id) : IQuery<IfoodShippingTrackingResponse>;
