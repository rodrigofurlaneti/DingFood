using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Tables.SetReadingValidation;

/// <summary>
/// Configura os três cenários de validação obrigatória na leitura da comanda/mesa
/// (câmera, código de barras e QR Code) de uma <see cref="Domain.Entities.DiningTable"/>.
/// </summary>
public sealed record SetDiningTableReadingValidationCommand(
    long DiningTableId,
    bool IsCameraInputEnabled,
    bool IsBarcodeEnabled,
    bool IsQrCodeEnabled) : ICommand;
