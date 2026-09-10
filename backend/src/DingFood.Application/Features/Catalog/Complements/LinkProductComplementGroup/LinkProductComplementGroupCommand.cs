using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Catalog.Complements.LinkProductComplementGroup;

public sealed record LinkProductComplementGroupCommand(long ProductId, long ComplementGroupId, int DisplayOrder) : ICommand<long>;
