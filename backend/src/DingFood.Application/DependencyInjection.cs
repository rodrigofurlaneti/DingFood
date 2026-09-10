using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using DingFood.Application.Features.Checkout.Shared;
using DingFood.Application.Features.Integrations.Keeta.Authorization;
using DingFood.Application.Features.Integrations.Keeta.Order.Polling;

namespace DingFood.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(configuration =>
                configuration.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly, includeInternalTypes: true);

            services.AddScoped<ICheckoutOrderPreparer, CheckoutOrderPreparer>();
            services.AddScoped<DingFood.Application.Features.Cash.IPaymentMethodAvailability, DingFood.Application.Features.Cash.PaymentMethodAvailability>();
            services.AddScoped<IKeetaAccessTokenProvider, KeetaAccessTokenProvider>();
            services.AddScoped<IKeetaOrderEventProcessor, KeetaOrderEventProcessor>();

            return services;
        }
    }
}


