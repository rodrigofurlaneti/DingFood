using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Catalog.Complements.UnlinkProductComplementGroup;

public sealed record UnlinkProductComplementGroupCommand(long ProductComplementGroupId) : ICommand;
