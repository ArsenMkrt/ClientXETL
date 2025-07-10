namespace ClientXETL.Services.Validation;

public class NotNullOrEmptyValidationRule<TModel>(Func<TModel, string> propertySelector) 
    : PropertyValidationRuleBase<TModel, string>(propertySelector)
{
    protected override IEnumerable<FailedValidationResult> Validate(TModel? model, string? property)
    {
        if (string.IsNullOrEmpty(property))
        {
            yield return new FailedValidationResult("Property cannot be null or empty");
        }
    }
}
