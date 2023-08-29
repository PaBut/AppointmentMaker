
using FluentValidation.Results;

namespace AppointmentMaker.Domain.Shared;

public class Error
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new("Error.NullValue", "The specified result value is null");

    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public string Code { get; set; }
    public string Message { get; set; }

    public static implicit operator string(Error error) => error.Code;

    public static Error FromValidationResult(ValidationResult validationResult)
    {
        string description = string.Join('|', validationResult.Errors.Select(e => e.ErrorMessage));
        string errorCodes = string.Join('|', validationResult.Errors.Select(e => e.ErrorCode));
        return new Error(errorCodes, description);
    }
}
