namespace ClientXETL.Services.Validation;
 
public class RangeValidationRule<TModel, TProperty>(Func<TModel, TProperty> propertySelector, TProperty min, TProperty max)
    : PropertyValidationRuleBase<TModel, TProperty>(propertySelector)
    where TProperty : IComparable<TProperty>
    where TModel : class
{
    protected override IEnumerable<FailedValidationResult> Validate(TModel? model, TProperty? property)
    {
        if (property?.CompareTo(min) < 0 || property?.CompareTo(max) > 0)
        {
            yield return new FailedValidationResult($"{model?.GetType().Name} value {property} is out of range [{min}, {max}]");
        }
    }
}
