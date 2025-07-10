using ClientXETL.Models;
using ClientXETL.Services.Storage;
using System.Collections.Immutable;

namespace ClientXETL.Services.Validation;

public class PolicyValidator(PolicyStorage storage)
    : IClientXValidator
{
    private readonly IModelValidationRule<Policy>[] validationRules = [
            new GreaterThanValidationRule<Policy, int>(p => p.ID, 0),
            new NotNullOrEmptyValidationRule<Policy>(p => p.PolicyName),
        ];

    public async Task<IReadOnlyCollection<FailedValidationResult>> ValidateAsync(CancellationToken cancellationToken)
    {
        var validationResults = from item in storage.Policies
                                from validation in validationRules
                                let result = validation.Validate(item)
                                select result;

        return await Task.FromResult(validationResults.SelectMany(x => x).ToImmutableArray());
    }
}
