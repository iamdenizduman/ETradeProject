namespace CatalogService.Application.Results
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string Message { get; }

        protected Result(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }

        public static Result Success(string message = "Success")
            => new(true, message);

        public static Result Failure(string message)
            => new(false, message);
    }
}
