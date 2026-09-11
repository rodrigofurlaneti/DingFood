using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DingFood.Application.Abstractions.Authentication;
using DingFood.Application.Abstractions.Tenancy;
using DingFood.Domain.Entities;
using DingFood.Domain.Repositories;
using DingFood.Infrastructure.Authentication;
using DingFood.Infrastructure.Fiscal;
using DingFood.Infrastructure.Integrations.Asaas;
using DingFood.Infrastructure.Integrations.Ifood;
using DingFood.Infrastructure.Integrations.Keeta;
using DingFood.Infrastructure.Integrations.WhatsApp;
using DingFood.Infrastructure.Payments;
using DingFood.Infrastructure.Persistence;
using DingFood.Infrastructure.Persistence.Repositories;
using DingFood.Infrastructure.Printing;
using DingFood.Infrastructure.Security;
using DingFood.Infrastructure.Storage;
using DingFood.Infrastructure.Tenancy;
using System.IO;

namespace DingFood.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<DingFood.Infrastructure.Delivery.IDeliveryGeocoder, DingFood.Infrastructure.Delivery.GoogleDeliveryGeocoder>(client => client.Timeout = TimeSpan.FromSeconds(10));
        services.AddHttpContextAccessor();
        services.AddScoped<CurrentTenantService>();
        services.AddScoped<IPublicWorkplaceScope, PublicWorkplaceScope>();
        services.AddScoped<IWorkplaceAccessService, WorkplaceAccessService>();
        services.AddScoped<ICurrentTenantService>(sp => sp.GetRequiredService<CurrentTenantService>());

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(
                connectionString,
                new MySqlServerVersion(new Version(8, 4, 9)),
                mysql =>
                {
                    mysql.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
                    mysql.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(10), errorNumbersToAdd: null);
                }));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>());
        services.AddScoped<IDiningAreaRepository, DiningAreaRepository>();
        services.AddScoped<IDiningAreaTableRepository, DiningAreaTableRepository>();
        services.AddScoped<IDiningAreaAssignmentRepository, DiningAreaAssignmentRepository>();
        services.AddScoped<IAppUserRepository, AppUserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<ICustomerRefreshTokenRepository, CustomerRefreshTokenRepository>();
        services.AddScoped<ICustomerOrderRepository, CustomerOrderRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IDiningTableRepository, DiningTableRepository>();
        services.AddScoped<IComandaRepository, ComandaRepository>();
        services.AddScoped<ISaleRepository, SaleRepository>();
        services.AddScoped<ICashSessionRepository, CashSessionRepository>();
        services.AddScoped<ICashMovementRepository, CashMovementRepository>();
        services.AddScoped<ICashSessionPaymentReconciliationRepository, CashSessionPaymentReconciliationRepository>();
        services.AddScoped<IBranchPaymentMethodSettingRepository, BranchPaymentMethodSettingRepository>();
        services.AddScoped<ICashRegisterRepository, CashRegisterRepository>();
        services.AddScoped<IShiftClosingRepository, ShiftClosingRepository>();
        services.AddScoped<IShiftClosingSessionRepository, ShiftClosingSessionRepository>();
        services.AddScoped<IStockItemRepository, StockItemRepository>();
        services.AddScoped<IStockMovementRepository, StockMovementRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IJobTitleRepository, JobTitleRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IUserRoleRepository, UserRoleRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IUnitOfMeasureRepository, UnitOfMeasureRepository>();
        services.AddScoped<IAppFeatureRepository, AppFeatureRepository>();
        services.AddScoped<IJobTitleFeatureRepository, JobTitleFeatureRepository>();
        services.AddScoped<IAppUserFeatureRepository, AppUserFeatureRepository>();
        services.AddScoped<IOperatingCostRepository, OperatingCostRepository>();
        services.AddScoped<IRevenueTargetRepository, RevenueTargetRepository>();
        services.AddScoped<IPromotionRepository, PromotionRepository>();
        services.AddScoped<IPrinterRepository, PrinterRepository>();
        services.AddScoped<IPrinterSettingRepository, PrinterSettingRepository>();
        services.AddScoped<IOrderPartialPaymentRepository, OrderPartialPaymentRepository>();
        services.AddScoped<IComandaSettingRepository, ComandaSettingRepository>();
        services.AddScoped<IServiceFeeSettingRepository, ServiceFeeSettingRepository>();
        services.AddScoped<IIfoodIntegrationSettingRepository, IfoodIntegrationSettingRepository>();
        services.AddScoped<IIfoodMerchantMappingRepository, IfoodMerchantMappingRepository>();
        services.AddScoped<IIfoodOrderRepository, IfoodOrderRepository>();
        services.AddScoped<IIfoodLogisticsDeliveryRepository, IfoodLogisticsDeliveryRepository>();
        services.AddScoped<IIfoodShippingDeliveryRepository, IfoodShippingDeliveryRepository>();
        services.AddScoped<IIfoodCategoryMappingRepository, IfoodCategoryMappingRepository>();
        services.AddScoped<IIfoodProductMappingRepository, IfoodProductMappingRepository>();
        services.AddScoped<IIfoodFinancialEventRepository, IfoodFinancialEventRepository>();
        services.AddScoped<IIfoodSettlementRepository, IfoodSettlementRepository>();
        services.AddScoped<IIfoodOpeningHoursRepository, IfoodOpeningHoursRepository>();
        services.AddScoped<IComplementItemRepository, ComplementItemRepository>();
        services.AddScoped<IComplementGroupRepository, ComplementGroupRepository>();
        services.AddScoped<IProductComplementGroupRepository, ProductComplementGroupRepository>();
        services.AddScoped<IIfoodComplementGroupMappingRepository, IfoodComplementGroupMappingRepository>();
        services.AddScoped<IIfoodComplementMappingRepository, IfoodComplementMappingRepository>();
        services.AddScoped<IPizzaFlavorRepository, PizzaFlavorRepository>();
        services.AddScoped<IPizzaConfigurationRepository, PizzaConfigurationRepository>();
        services.AddScoped<IIfoodPizzaMappingRepository, IfoodPizzaMappingRepository>();
        services.AddScoped<IAccessLogRepository, AccessLogRepository>();
        services.AddScoped<ISupplierRepository, SupplierRepository>();
        services.AddScoped<IPurchaseRepository, PurchaseRepository>();
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<IBusinessGroupRepository, BusinessGroupRepository>();
        services.AddScoped<DingFood.Application.Abstractions.Tenancy.ICompanyAccessService, DingFood.Infrastructure.Tenancy.CompanyAccessService>();
        services.AddScoped<IBranchRepository, BranchRepository>();
        services.AddScoped<ITableReservationRepository, TableReservationRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IProductStockRepository, ProductStockRepository>();
        services.AddScoped<ILogTrackerRepository, LogTrackerRepository>();
        services.AddScoped<ICustomerAppUserRepository, CustomerAppUserRepository>();
        services.AddScoped<ICustomerAddressRepository, CustomerAddressRepository>();
        services.AddScoped<IAsaasIntegrationCustomerRepository, AsaasIntegrationCustomerRepository>();
        services.AddScoped<IAsaasIntegrationPaymentRepository, AsaasIntegrationPaymentRepository>();
        services.AddScoped<IAsaasIntegrationSavedCardRepository, AsaasIntegrationSavedCardRepository>();
        services.AddScoped<IAsaasIntegrationSettingRepository, AsaasIntegrationSettingRepository>();
        services.AddScoped<IAsaasIntegrationWebhookLogRepository, AsaasIntegrationWebhookLogRepository>();

        services.AddScoped<IKeetaIntegrationSettingRepository, KeetaIntegrationSettingRepository>();
        services.AddScoped<IKeetaIntegrationMerchantMappingRepository, KeetaIntegrationMerchantMappingRepository>();
        services.AddScoped<IKeetaIntegrationAuthorizationSessionRepository, KeetaIntegrationAuthorizationSessionRepository>();
        services.AddScoped<IKeetaIntegrationOrderRepository, KeetaIntegrationOrderRepository>();
        services.AddScoped<IKeetaIntegrationOrderEventLogRepository, KeetaIntegrationOrderEventLogRepository>();
        services.AddScoped<IKeetaIntegrationRefundDisputeRepository, KeetaIntegrationRefundDisputeRepository>();

        services.AddSingleton<TimeProvider, DingFood.Infrastructure.Time.TimeProviderCustom>();
        services.AddSingleton<DingFood.Application.Abstractions.Storage.IImageStorage, LocalImageStorage>();
        services.AddSingleton<IRawPrinterTransport, WindowsRawPrinterTransport>();
        services.AddSingleton<IRawPrinterTransport, NetworkRawPrinterTransport>();
        services.AddScoped<DingFood.Application.Abstractions.Printing.IPrintingService, PrintingService>();
        services.AddScoped<IWaiterMessageRepository, WaiterMessageRepository>();
        services.AddScoped<ITableItemTransferRepository, TableItemTransferRepository>();
        services.AddScoped<IComandaItemTransferRepository, ComandaItemTransferRepository>();

        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<IJwtTokenProvider, JwtTokenProvider>();
        services.AddScoped<DingFood.Application.Abstractions.Payments.IPaymentGatewayService, FakePaymentGatewayService>();
        services.AddScoped<DingFood.Application.Abstractions.Fiscal.IFiscalDocumentService, FakeFiscalDocumentService>();

        // -----------------------------------------------------------------
        // CONFIGURAÇÃO PERSISTENTE DO DATA PROTECTION (FIM DO ERRO DE CHAVE)
        // -----------------------------------------------------------------
        var keysFolder = Path.Combine(AppContext.BaseDirectory, "app_data", "protecting-keys");
        if (!Directory.Exists(keysFolder))
        {
            Directory.CreateDirectory(keysFolder);
        }

        services.AddDataProtection()
            .PersistKeysToFileSystem(new DirectoryInfo(keysFolder))
            .SetApplicationName("DingFood");
        // -----------------------------------------------------------------

        services.AddSingleton<DingFood.Application.Abstractions.Security.ISecretProtector, DataProtectionSecretProtector>();

        services.AddHttpClient<DingFood.Application.Abstractions.Integrations.Ifood.IIfoodAuthClient, IfoodAuthClient>(
            client => client.Timeout = TimeSpan.FromSeconds(15));

        services.AddMemoryCache();
        services.AddScoped<DingFood.Application.Abstractions.Integrations.Ifood.IIfoodTokenProvider, IfoodTokenProvider>();

        services.AddHttpClient<DingFood.Application.Abstractions.Integrations.Ifood.IIfoodOrderClient, IfoodOrderClient>(
            client => client.Timeout = TimeSpan.FromSeconds(15));

        services.AddHostedService<IfoodOrderPollingBackgroundService>();
        services.AddScoped<DingFood.Application.Abstractions.Integrations.Ifood.IIfoodEventInbox, IfoodEventInboxStore>();
        services.AddScoped<DingFood.Application.Abstractions.Integrations.Ifood.IIfoodWebhookReceiver, IfoodWebhookReceiver>();
        services.AddHostedService<IfoodEventInboxBackgroundService>();
        services.AddScoped<DingFood.Application.Abstractions.Integrations.Ifood.IIfoodCatalogSyncTrigger, IfoodCatalogSyncTrigger>();

        services.AddHttpClient<DingFood.Application.Abstractions.Integrations.Ifood.IIfoodCatalogClient, IfoodCatalogClient>(
            client => client.Timeout = TimeSpan.FromSeconds(15));

        services.AddHttpClient<DingFood.Application.Abstractions.Integrations.Ifood.IIfoodFinancialClient, IfoodFinancialClient>(
            client => client.Timeout = TimeSpan.FromSeconds(30));

        services.AddHostedService<IfoodFinancialSyncBackgroundService>();

        services.AddHttpClient<DingFood.Application.Abstractions.Integrations.Ifood.IIfoodMerchantClient, IfoodMerchantClient>(
            client => client.Timeout = TimeSpan.FromSeconds(15));

        services.AddSingleton<DingFood.Application.Abstractions.Integrations.Ifood.IIfoodOperationalAlertStore, InMemoryIfoodOperationalAlertStore>();
        services.AddHostedService<IfoodMerchantStatusWatcherBackgroundService>();
        services.Configure<IfoodAnalyticsExtractionOptions>(configuration.GetSection("IfoodAnalytics"));
        services.AddHostedService<IfoodAnalyticsExtractionBackgroundService>();
        services.AddScoped<DingFood.Application.Abstractions.Integrations.Ifood.IIfoodShippingTrackingStore, IfoodShippingTrackingStore>();
        services.AddHostedService<IfoodShippingTrackingBackgroundService>();

        services.AddHttpClient<DingFood.Application.Abstractions.Integrations.Ifood.IIfoodLogisticsClient, IfoodLogisticsClient>(
            client => client.Timeout = TimeSpan.FromSeconds(15));

        services.AddHttpClient<DingFood.Application.Abstractions.Integrations.Ifood.IIfoodShippingClient, IfoodShippingClient>(
            client => client.Timeout = TimeSpan.FromSeconds(15));

        services.AddHttpClient<DingFood.Application.Abstractions.Integrations.Ifood.IIfoodReviewClient, IfoodReviewClient>(
            client => client.Timeout = TimeSpan.FromSeconds(15));

        services.AddHostedService<IfoodReviewWatcherBackgroundService>();

        services.AddHttpClient<DingFood.Application.Abstractions.Integrations.Ifood.IIfoodAnalyticsClient, IfoodAnalyticsClient>(
            client => client.Timeout = TimeSpan.FromSeconds(20));

        services.Configure<AsaasSettings>(configuration.GetSection("Asaas"));

        services.AddScoped<DingFood.Application.Abstractions.Integrations.Asaas.IAsaasCredentialsResolver, AsaasCredentialsResolver>();
        services.AddHttpClient<AsaasAuthClient>();
        services.AddScoped<AsaasService>();
        services.AddScoped<IAsaasService>(sp => sp.GetRequiredService<AsaasService>());
        services.AddScoped<DingFood.Application.Abstractions.Integrations.Asaas.IAsaasService, AsaasApplicationService>();
        services.AddScoped<DingFood.Application.Abstractions.Integrations.Asaas.IAsaasCustomerProvisioningService, AsaasCustomerProvisioningService>();

        services.Configure<KeetaSettings>(configuration.GetSection("Keeta"));
        services.AddScoped<DingFood.Application.Abstractions.Integrations.Keeta.IKeetaCredentialsResolver, KeetaCredentialsResolver>();
        services.AddHttpClient<KeetaAuthClient>();
        services.AddScoped<DingFood.Application.Abstractions.Integrations.Keeta.IKeetaAuthClient>(sp => sp.GetRequiredService<KeetaAuthClient>());
        services.AddHttpClient<KeetaOrderClient>();
        services.AddScoped<DingFood.Application.Abstractions.Integrations.Keeta.IKeetaOrderClient>(sp => sp.GetRequiredService<KeetaOrderClient>());
        services.AddHostedService<KeetaEventPollingBackgroundService>();
        services.AddScoped<IOrderOriginRepository, OrderOriginRepository>();
        services.AddSingleton<DingFood.Application.Abstractions.Security.IReadingProofService, DingFood.Infrastructure.Authentication.ReadingProofService>();
        services.Configure<WhatsAppSettings>(configuration.GetSection("WhatsApp"));
        services.AddSingleton<WhatsAppOutbox>();
        services.AddSingleton<DingFood.Application.Abstractions.Notifications.IWhatsAppQueue>(sp => sp.GetRequiredService<WhatsAppOutbox>());
        services.AddHostedService(sp => sp.GetRequiredService<WhatsAppOutbox>());
        services.AddHttpClient<DingFood.Application.Abstractions.Notifications.IWhatsAppService, WhatsAppService>((sp, client) =>
        {
            var settings = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<WhatsAppSettings>>().Value;
            client.BaseAddress = new Uri(settings.BaseUrl);
            client.Timeout = TimeSpan.FromSeconds(30);
        });

        return services;
    }
}
