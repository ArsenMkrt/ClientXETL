using ClientXETL.Services.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace ClientXETL.Services.Extractor;

public class DatasetExtractorServiceProvider(IServiceProvider serviceProvider)
    : IDatasetExtractorServiceProvider
{
    public IDatasetExtractor GetService(string key)
    {
        return key.ToLower() switch
        {
            IPolicyStorage.DatasetName => serviceProvider.GetRequiredService<IPolicyStorage>(),
            IRiskStorage.DatasetName => serviceProvider.GetRequiredService<IRiskStorage>(),
            _ => throw new KeyNotFoundException($"No service found for key: {key}"),
        };
    }
}