namespace ClientXETL.Services.Validation;

public class EnumDefinedValidationRule<TModel, TEnum>(Func<TModel, TEnum> propertySelector)
    : PropertyValidationRuleBase<TModel, TEnum>(propertySelector)
    where TEnum : struct, Enum
{
    protected override IEnumerable<FailedValidationResult> Validate(TModel? model, TEnum property)
    {
        if (!Enum.IsDefined(typeof(TEnum), property))
        {
            yield return new FailedValidationResult($"Invalid value for {typeof(TEnum).Name}: {property}");
        }
    }
}
