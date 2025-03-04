using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WP.DTOs
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public int StatusCode { get; set; }
        public ApiResponse(bool success, string message, T data, int statusCode)
        {
            Success = success;
            Message = message;
            Data = data;
            StatusCode = statusCode;
        }
    }
    public class SuccessApiResponse<T> : ApiResponse<T> 
    {
        public SuccessApiResponse(T data, string message = "success") : base(true, message, data, 200)
        {

        }
    }
    public class FailedApiResponse<T> : ApiResponse<T> 
    {
        public FailedApiResponse(string message) : base(false, message, default(T), 500)
        {

        }
    }
}
