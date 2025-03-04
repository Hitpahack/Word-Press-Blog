using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace WP.Web.Models
{
   
    public class JwtSessionMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtSessionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                var token = context.Session?.GetString("JWToken");

                if (!string.IsNullOrEmpty(token))
                {
                    context.Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token).ToString();
                }

            }
            catch (Exception)
            {
                
            }
            await _next(context);
        }
    }

}
