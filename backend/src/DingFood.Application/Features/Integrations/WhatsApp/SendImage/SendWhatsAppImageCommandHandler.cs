using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Abstractions.Notifications;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.Integrations.WhatsApp.SendImage
{
    internal sealed class SendWhatsAppImageCommandHandler : BaseCommandHandler<SendWhatsAppImageCommand>
    {
        private readonly IWhatsAppQueue _whatsAppService;

        public SendWhatsAppImageCommandHandler(
            IWhatsAppQueue whatsAppService,
            ILogTrackerRepository logRepository,
            IUnitOfWork unitOfWork)
            : base(logRepository, unitOfWork)
        {
            _whatsAppService = whatsAppService;
        }

        public override async Task<Result> Handle(SendWhatsAppImageCommand request, CancellationToken cancellationToken)
        {
            return await ExecuteWithLogAsync(
                nameof(SendWhatsAppImageCommandHandler),
                nameof(Handle),
                null,
                (_) => _whatsAppService.EnqueueAsync(request.PhoneNumber, request.Message, request.FileUrl, cancellationToken));
        }
    }
}

