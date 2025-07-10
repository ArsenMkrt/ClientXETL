using ClientXETL.Services.Extractor;
using ClientXETL.Services.Loader;
using ClientXETL.Services.Validation;
using Microsoft.Extensions.Logging;

namespace ClientXETL.Services;

public class ClientXETLService(
    IClientXDataExtractorService extractor,
    IClientXDataValidatorService validator,
    IClientXLoaderService clientXLoaderService,
    ILogger<ClientXETLService> logger)
{
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting CloudXETLService...");
        await extractor.ExtractAsync(cancellationToken);
        logger.LogInformation("Data extraction completed.");

        logger.LogInformation("Starting data validation...");
        await validator.ValidateAsync(cancellationToken);
        logger.LogInformation("Data validation completed.");

        logger.LogInformation("Starting data loading...");
        await clientXLoaderService.LoadAsync(cancellationToken);
        logger.LogInformation("Data loading completed.");

    }
}
