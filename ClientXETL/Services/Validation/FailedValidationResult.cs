namespace ClientXETL.Services.Validation;

public class FailedValidationResult(string message)
{
    public string Message { get; } = message;
}
