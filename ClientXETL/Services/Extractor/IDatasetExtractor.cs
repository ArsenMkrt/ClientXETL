namespace ClientXETL.Services.Extractor;

public interface IDatasetExtractor
{
    Task LoadAsync(Stream stream, CancellationToken cancellationToken);
}
