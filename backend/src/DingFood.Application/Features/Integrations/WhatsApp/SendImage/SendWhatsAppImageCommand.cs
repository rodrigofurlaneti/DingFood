using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Integrations.WhatsApp.SendImage
{
    public sealed record SendWhatsAppImageCommand(string PhoneNumber, string Message, string FileUrl) : ICommand;
}
