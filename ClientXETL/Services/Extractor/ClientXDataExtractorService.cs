using ClientXETL.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO.Compression;

namespace ClientXETL.Services.Extractor;

public class ClientXDataExtractorService(IDatasetExtractorServiceProvider loaderServiceProvider, IOptions<ClientXLoaderServiceOptions> options, ILogger<ClientXDataExtractorService> logger) : IClientXDataExtractorService
{
    public async Task ExtractAsync(CancellationToken cancellationToken)
    {
        logger.LogDebug("Extracting zip file: {ZipFilePath}", options.Value.ZipFilePath);
        var zipFile = ZipFile.OpenRead(options.Value.ZipFilePath);

        foreach (var item in zipFile.Entries)
        {
            await ProcessZipEntryAsync(item, cancellationToken);
        }

        async Task ProcessZipEntryAsync(ZipArchiveEntry item, CancellationToken token)
        {
            var loaderService = loaderServiceProvider.GetService(item.Name);
            if (loaderService == null)
            {
                logger.LogWarning("No loader service found for dataset {DatasetName}", item.Name);
                return;
            }

            logger.LogInformation("Loading dataset {DatasetName}", item.Name);
            await loaderService.LoadAsync(item.Open(), token);
            logger.LogInformation("Successfully loaded dataset {DatasetName}", item.Name);
        }
    }
}