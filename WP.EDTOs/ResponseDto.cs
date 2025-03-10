namespace WP.EDTOs
{

    public class ResponseDto<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public int StatusCode { get; set; }
        public ResponseDto(bool success, string message, T data, int statusCode)
        {
            Success = success;
            Message = message;
            Data = data;
            StatusCode = statusCode;
        }
    }
    public class SuccessResponseDto<T> : ResponseDto<T>
    {
        public SuccessResponseDto(T data, string message = "success") : base(true, message, data, 200)
        {

        }
    }
    public class FailedResponseDto<T> : ResponseDto<T>
    {
        public FailedResponseDto(string message) : base(false, message, default(T), 500)
        {

        }
    }
}
