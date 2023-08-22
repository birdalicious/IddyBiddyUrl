namespace UrlShortenerService
{
    public class Result<T, TError>
    {
        public T? Value { get; set; }
        public TError? Error { get; set; }
        public bool IsSuccess => Error is null;

        private Result(T value)
        {
            Value = value;
        }

        private Result(TError error)
        {
            Error = error;
        }

        public TResult Match<TResult>(Func<T, TResult> success, Func<TError, TResult> failure)
        {
            return IsSuccess
                ? success(Value!)
                : failure(Error!);
        }

        public static implicit operator Result<T, TError>(T value) => new Result<T, TError>(value);
        public static implicit operator Result<T, TError>(TError error) => new Result<T, TError>(error);
    }
}
