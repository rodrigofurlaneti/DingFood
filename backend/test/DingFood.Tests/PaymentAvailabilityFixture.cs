using NSubstitute;
using DingFood.Application.Features.Cash;
using DingFood.Domain.Primitives;

namespace DingFood.Tests;

internal static class PaymentAvailabilityFixture
{
    internal static IPaymentMethodAvailability Allowed()
    {
        var service = Substitute.For<IPaymentMethodAvailability>();
        service.ValidateAsync(Arg.Any<long>(), Arg.Any<IReadOnlyCollection<long>>(), Arg.Any<CancellationToken>()).Returns(Result.Success());
        service.ValidateOrderAsync(Arg.Any<long>(), Arg.Any<IReadOnlyCollection<long>>(), Arg.Any<CancellationToken>()).Returns(Result.Success());
        return service;
    }
}
