using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Catalog.GetMenu;

public sealed record GetMenuQuery(long CompanyId) : IQuery<IReadOnlyCollection<MenuItemResponse>>;
