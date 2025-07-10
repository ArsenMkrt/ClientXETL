namespace ClientXETL.Services.Validation;

public abstract class PropertyValidationRuleBase<TModel, TProperty>(Func<TModel, TProperty> propertySelector)
    : IModelValidationRule<TModel>
{
    public IEnumerable<FailedValidationResult> Validate(TModel model)
    {
        return Validate(model, propertySelector(model));
    }

    protected abstract IEnumerable<FailedValidationResult> Validate(TModel? model, TProperty? property);
}
