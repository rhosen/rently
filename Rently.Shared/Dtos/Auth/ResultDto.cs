namespace Rently.Shared.Dtos.Auth
{
    public class ResultDto<T>
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public T? Data { get; set; }

        public static ResultDto<T> Success(T data) =>
            new ResultDto<T> { IsSuccess = true, Data = data };

        public static ResultDto<T> Failure(string errorMessage) =>
            new ResultDto<T> { IsSuccess = false, ErrorMessage = errorMessage };
    }
}
