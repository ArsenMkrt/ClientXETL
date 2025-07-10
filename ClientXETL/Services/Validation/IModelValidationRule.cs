namespace ClientXETL.Services.Validation;

public interface IModelValidationRule<TModel>
{
    IEnumerable<FailedValidationResult> Validate(TModel model);
}
