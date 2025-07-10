using ClientXETL.Models;
using ClientXETL.Services.SearchIndexes;
using ClientXETL.Services.Storage;
using System.Collections.Immutable;

namespace ClientXETL.Services.Validation;

public class RiskValidator(RiskStorage storage, PolicyIdSearchIndex policyIdSearchIndex)
    : IClientXValidator
{
    private readonly IModelValidationRule<Risk>[] validationRules = [
            new GreaterThanValidationRule<Risk, int>(r => r.ID, 0),
            new NotNullOrEmptyValidationRule<Risk>(r => r.RiskName),
            new EnumDefinedValidationRule<Risk, Peril>(r => r.Peril),
            new GreaterThanValidationRule<Risk, int>(r => r.PolicyID, 0),
            new RangeValidationRule<Risk, double>(r => r.Latitude, -90.0, 90.0),
            new RangeValidationRule<Risk, double>(r => r.Longitude, -180.0, 180.0),
            new ReferenceIndexValidationRule<Risk, Policy, int>(r => r.PolicyID, policyIdSearchIndex)
        ];

    public async Task<IReadOnlyCollection<FailedValidationResult>> ValidateAsync(CancellationToken cancellationToken)
    {
        var validationResults = from item in storage.Risks
                                from validation in validationRules
                                let result = validation.Validate(item)
                                select result;
        return await Task.FromResult(validationResults.SelectMany(x => x).ToImmutableArray());
    }
}
