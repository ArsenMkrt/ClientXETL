using Microsoft.Extensions.Logging;

namespace ClientXETL.Services.Validation;

public class ClientXDataValidatorService(IEnumerable<IClientXValidator> validators, ILogger<ClientXDataValidatorService> logger)
    : IClientXDataValidatorService
{
    public async Task ValidateAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting data validation...");
        bool hasFail = false;
        foreach (var validator in validators)
        {
            var messages = await validator.ValidateAsync(cancellationToken);
            if (messages.Count > 0)
            {
                hasFail = true;
                logger.LogError("Validation failed for {ValidatorName} with {Count} errors.", validator.GetType().Name, messages.Count);
                foreach (var message in messages)
                {
                    logger.LogError("Validation error: {Message}", message);
                }
            }
        }

        if (hasFail)
        {
            logger.LogError("Data validation failed. Please check the logs for details.");
            throw new InvalidOperationException("Data validation failed. See logs for details.");
        }
    }
}
