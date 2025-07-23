namespace BuildingBlocks.Application.Abstractions;

public class Result<T>
{
    public T? Value { get; }
    public string? Error { get; }
    public bool IsSuccess => Error is null;

    protected Result(T? value) => Value = value;
    protected Result(string? error) => Error = error;

    public static Result<T> Success(T value) => new(value);
    public static Result<T> Failure(string error) => new(error);
}
