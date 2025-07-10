using ClientXETL.Models;
using Microsoft.Extensions.Logging;

namespace ClientXETL.Services.Storage;

public class RiskStorage(ILogger<RiskStorage> logger)
    : IRiskStorage
{
    private readonly List<Risk> risks = [];

    public IReadOnlyCollection<Risk> Risks => risks.AsReadOnly();

    public async Task LoadAsync(Stream stream, CancellationToken cancellationToken)
    {
        using var sr = new StreamReader(stream);

        while (!sr.EndOfStream && !cancellationToken.IsCancellationRequested)
        {
            var line = await sr.ReadLineAsync(cancellationToken);
            if (line == null) continue;
            // Process the line as needed
            logger.LogDebug("Processing line: {Line}", line);
            var risk = CreateRisk(line);
            risks.Add(risk);
        }
    }

    private static Risk CreateRisk(string line)
    {
        var parts = line.Split('\t');
        if (parts.Length < 8)
        {
            throw new FormatException($"Invalid risk line format: {line}");
        }
        if (!int.TryParse(parts[0], out var id))
        {
            throw new FormatException($"Invalid ID in risk line: {line}");
        }

        var riskName = parts[1].Trim();
        if (string.IsNullOrEmpty(riskName))
        {
            throw new FormatException($"Empty risk name in line: {line}");
        }

        if (!Enum.TryParse<Peril>(parts[2].Trim(), true, out var peril) || !Enum.IsDefined(typeof(Peril), peril))
        {
            throw new FormatException($"Invalid Peril in risk line: {line}");
        }

        if (!int.TryParse(parts[3], out var policyId))
        {
            throw new FormatException($"Invalid PolicyID in risk line: {line}");
        }

        if (!double.TryParse(parts[6], out var latitude))
        {
            throw new FormatException($"Invalid Latitude in risk line: {line}");
        }

        if (!double.TryParse(parts[7], out var longitude))
        {
            throw new FormatException($"Invalid Longitude in risk line: {line}");
        }

        return new Risk
        {
            ID = id,
            RiskName = riskName,
            Peril = peril,
            PolicyID = policyId,
            Street = parts[4].Trim(),
            ClientID = parts[5].Trim(),
            Latitude = latitude,
            Longitude = longitude
        };
    }
}