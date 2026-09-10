using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Catalog;

namespace DingFood.Application.Features.PublicOrdering.GetPublicMenu;

public sealed record PublicMenuResponse(
    string BranchName,
    int TableNumber,
    IReadOnlyCollection<MenuItemResponse> Items,
    bool IsQrViewEnabled,
    bool IsCameraInputEnabled,
    bool IsBarcodeEnabled,
    bool IsQrCodeEnabled);

// Sem autenticação — o "segredo" é o token do QR Code da mesa (GUID imprevisível).
public sealed record GetPublicMenuQuery(Guid Token) : IQuery<PublicMenuResponse>;
