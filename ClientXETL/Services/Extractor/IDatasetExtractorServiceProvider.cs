namespace ClientXETL.Services.Extractor;

public interface IDatasetExtractorServiceProvider
{
    IDatasetExtractor GetService(string key);
}
