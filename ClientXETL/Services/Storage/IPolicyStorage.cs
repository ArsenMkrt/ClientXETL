using ClientXETL.Models;
using ClientXETL.Services.Extractor;

namespace ClientXETL.Services.Storage;

public interface IPolicyStorage : IDatasetExtractor
{
    public const string DatasetName = "policy.txt";

    public IReadOnlyCollection<Policy> Policies { get; }
}
