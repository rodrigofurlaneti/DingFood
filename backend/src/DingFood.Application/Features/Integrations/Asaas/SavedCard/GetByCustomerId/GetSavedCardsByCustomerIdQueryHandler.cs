using DingFood.Application.Abstractions.Messaging;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.Integrations.Asaas.SavedCard.GetByCustomerId
{
    internal sealed class GetSavedCardsByCustomerIdQueryHandler
       : BaseQueryHandler<GetSavedCardsByCustomerIdQuery, IReadOnlyList<AsaasIntegrationSavedCardResponse>>
    {
        private readonly IAsaasIntegrationSavedCardRepository _savedCardRepository;

        public GetSavedCardsByCustomerIdQueryHandler(
            IAsaasIntegrationSavedCardRepository savedCardRepository,
            ILogTrackerRepository logRepository,
            IUnitOfWork unitOfWork)
            : base(logRepository, unitOfWork)
        {
            _savedCardRepository = savedCardRepository;
        }

        public override async Task<Result<IReadOnlyList<AsaasIntegrationSavedCardResponse>>> Handle(
            GetSavedCardsByCustomerIdQuery request,
            CancellationToken cancellationToken)
        {
            return await ExecuteWithLogAsync(
                nameof(GetSavedCardsByCustomerIdQueryHandler),
                nameof(Handle),
                null,
                async (userIdBox) =>
                {
                    var cards = await _savedCardRepository.GetByCustomerIdAsync(
                        request.CustomerId,
                        cancellationToken);

                    var response = cards
                        .Select(card => new AsaasIntegrationSavedCardResponse(
                            card.Id,
                            card.CustomerId,
                            card.CompanyId,
                            card.CardBrand,
                            card.Last4Digits,
                            card.HolderName,
                            card.ExpiryMonth,
                            card.ExpiryYear,
                            card.IsDefault,
                            card.CreatedAt,
                            card.IsActive))
                        .ToList();

                    return Result.Success<IReadOnlyList<AsaasIntegrationSavedCardResponse>>(response);
                });
        }
    }
}
