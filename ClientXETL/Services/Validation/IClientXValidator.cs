namespace ClientXETL.Services.Validation;

public interface IClientXValidator
{
    Task<IReadOnlyCollection<FailedValidationResult>> ValidateAsync(CancellationToken cancellationToken);
}
