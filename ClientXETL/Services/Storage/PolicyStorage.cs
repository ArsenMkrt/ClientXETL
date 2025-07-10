using ClientXETL.Models;
using Microsoft.Extensions.Logging;

namespace ClientXETL.Services.Storage;

public class PolicyStorage(ILogger<PolicyStorage> logger)
    : IPolicyStorage
{
    private readonly List<Policy> policies = [];

    public IReadOnlyCollection<Policy> Policies => policies.AsReadOnly();

    public async Task LoadAsync(Stream stream, CancellationToken cancellationToken)
    {
        using var sr = new StreamReader(stream);

        while (!sr.EndOfStream && !cancellationToken.IsCancellationRequested)
        {
            var line = await sr.ReadLineAsync(cancellationToken);
            if (line == null)
                continue;
            // Process the line as needed
            logger.LogDebug("Processing line: {Line}", line);
            var policy = CreatePolicy(line);
            policies.Add(policy);
        }
    }

    private static Policy CreatePolicy(string line)
    {
        var parts = line.Split('\t');
        if (parts.Length < 2)
        {
            throw new FormatException($"Invalid policy line format: {line}");
        }

        if (!int.TryParse(parts[0], out var id))
        {
            throw new FormatException($"Invalid ID in policy line: {line}");
        }
        var policyName = parts[1].Trim();
        if (string.IsNullOrEmpty(policyName))
        {
            throw new FormatException($"Empty policy name in line: {line}");
        }

        return new Policy
        {
            ID = id,
            PolicyName = policyName
        };
    }
}
