namespace ClientXETL.Services.Validation;

public interface IClientXDataValidatorService
{
    Task ValidateAsync(CancellationToken cancellationToken);
}
