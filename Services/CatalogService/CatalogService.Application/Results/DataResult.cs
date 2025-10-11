namespace CatalogService.Application.Results
{
    public class DataResult<T> : Result
    {
        public T? Data { get; }

        private DataResult(bool isSuccess, string message, T? data)
            : base(isSuccess, message)
        {
            Data = data;
        }

        public static DataResult<T> Success(T data, string message = "Success")
            => new(true, message, data);

        public static new DataResult<T> Failure(string message)
            => new(false, message, default);
    }
}
