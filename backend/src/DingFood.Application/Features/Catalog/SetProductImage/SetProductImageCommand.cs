using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Catalog.SetProductImage;

public sealed record SetProductImageCommand(
    long ProductId,
    string Extension,
    byte[] Content) : ICommand<string>;
