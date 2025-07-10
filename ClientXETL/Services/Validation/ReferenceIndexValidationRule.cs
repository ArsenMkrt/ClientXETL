using ClientXETL.Services.SearchIndexes;

namespace ClientXETL.Services.Validation;

public class ReferenceIndexValidationRule<TModel, TRefModel, TProperty>(Func<TModel, TProperty> keySelector, SearchIndex<TProperty, TRefModel> index)
    : PropertyValidationRuleBase<TModel, TProperty>(keySelector)
    where TProperty : notnull
    where TRefModel : class
{
    protected override IEnumerable<FailedValidationResult> Validate(TModel? model, TProperty? property)
    {
        if (model is null || property is null)
        {
            yield break;
        }

        if (!index.ContainsKey(property))
        {
            yield return new FailedValidationResult($"Reference with ID {property} does not exist.");
        }
    }
}