namespace WP.Web.Models
{
    public static class Extenstions
    {
        public static string GetClientIP(this HttpContext context)
        {
            return context.Connection.RemoteIpAddress?.ToString() ?? "";

        }
    }
}
