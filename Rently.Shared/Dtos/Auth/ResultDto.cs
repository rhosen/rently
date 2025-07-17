using System.Net;

namespace Rently.Shared.Dtos.Auth
{
    public class ResultDto<T>
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public T? Data { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public static ResultDto<T> Success(T data) =>
            new ResultDto<T> { IsSuccess = true, Data = data, StatusCode = HttpStatusCode.OK };

        public static ResultDto<T> Failure(string errorMessage, HttpStatusCode statusCode = HttpStatusCode.BadRequest) =>
            new ResultDto<T> { IsSuccess = false, ErrorMessage = errorMessage, StatusCode = statusCode };
    }
}
