using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WP.DTOs
{
    public class ResponseDto<T> where T : class
    {
        public ResponseDto(bool success = false, string message = "something wrong", T data = null , int statuscode = 500)
        {
            Success = success;
            Message = message;
            Data = data;
            StatusCode = statuscode;
        }
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public int StatusCode { get; set; }
    }
    public class SuccessResponseDto<T>: ResponseDto<T> where T : class
    {
        public SuccessResponseDto(string message, T data):base(true,message, data, 200)
        {
            
        }
    }
    public class FailedResponseDto<T> : ResponseDto<T> where T : class
    {
        public FailedResponseDto(string message) : base(false, message)
        {

        }
    }
}
