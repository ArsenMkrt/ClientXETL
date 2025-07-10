using ClientXETL.Options;
using ClientXETL.Services;
using ClientXETL.Services.Extractor;
using ClientXETL.Services.Loader;
using ClientXETL.Services.SearchIndexes;
using ClientXETL.Services.Storage;
using ClientXETL.Services.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ClientXETL.Extensions;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterClientXServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ClientXLoaderServiceOptions>(configuration.GetSection("ClientXLoaderService"));

        services.AddSingleton<IPolicyStorage, PolicyStorage>();
        services.AddSingleton<IRiskStorage, RiskStorage>();
        services.AddTransient<PolicyIdSearchIndex>();


        services.AddTransient<IClientXDataExtractorService, ClientXDataExtractorService>();
        services.AddTransient<IClientXDataValidatorService, ClientXDataValidatorService>();
        services.AddTransient<IClientXLoaderService, DumpClientXAsTable>();
        services.AddSingleton<IDatasetExtractorServiceProvider, DatasetExtractorServiceProvider>();

        services.AddSingleton<ClientXETLService>();

        return services;
    }

    public static IServiceCollection RegisterLoggingServices(this IServiceCollection services, IConfiguration configuration)
    {
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConfiguration(configuration.GetSection("Logging"));
            builder.AddConsole();
        });

        services.AddSingleton(loggerFactory);
        services.AddTransient(typeof(ILogger<>), typeof(Logger<>));

        return services;
    }

    public static IServiceCollection RegisterConfigurationServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        services.AddSingleton<IConfiguration>(configuration);

        return services;
    }
}
