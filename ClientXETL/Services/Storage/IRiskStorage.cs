using ClientXETL.Models;
using ClientXETL.Services.Extractor;

namespace ClientXETL.Services.Storage;

public interface IRiskStorage : IDatasetExtractor
{
    const string DatasetName = "risk.txt";

    IReadOnlyCollection<Risk> Risks { get; }
}