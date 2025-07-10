namespace ClientXETL.Services.Extractor;

public interface IClientXDataExtractorService
{
    Task ExtractAsync(CancellationToken cancellationToken);
}
