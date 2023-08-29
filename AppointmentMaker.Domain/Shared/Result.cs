namespace AppointmentMaker.Domain.Shared;

public class Result
{
    public Result(bool isSuccess, Error error)
    {
        if(isSuccess && error != Error.None) 
        {
            throw new InvalidOperationException();
        }
        if(!isSuccess && error == Error.None) 
        {
            throw new InvalidOperationException();
        }

        Error = error;
        IsSuccess = isSuccess;
    }

    public bool IsSuccess { get; set; }
    public bool IsFailure => !IsSuccess;

    public Error Error { get; set; }

    public static Result Success() => new(true, Error.None);

    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);

    public static Result Failure(Error error) => new(false, error);

    public static Result<TValue> Failure<TValue>(Error error) => new Result<TValue>(default, false, error);
}
