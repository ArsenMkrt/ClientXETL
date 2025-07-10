namespace ClientXETL.Services.Validation;

public class GreaterThanValidationRule<TModel, TProperty>(Func<TModel, TProperty> propertySelector, TProperty minValue)
    : PropertyValidationRuleBase<TModel, TProperty>(propertySelector)
    where TProperty : IComparable<TProperty>
{
    protected override IEnumerable<FailedValidationResult> Validate(TModel? model, TProperty? property)
    {
        if (property?.CompareTo(minValue) <= 0)
        {
            yield return new FailedValidationResult($"Property must be greater than {minValue}");
        }
    }
}
