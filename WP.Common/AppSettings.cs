namespace WP.Common
{
    public class AppSettings
    {
        public Email Email { get; set; }
        public Jwt Jwt { get; set; }
        public Logging Logging { get; set; }
        public SecuritySettings SecuritySettings { get; set; }
    }

  

    public class Email
    {
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string From { get; set; }
    }

    public class Jwt
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpireMinutes { get; set; }
    }

    public class Logging
    {
        public LogLevel LogLevel { get; set; }
    }

    public class LogLevel
    {
        public string Default { get; set; }
        public string MicrosoftAspNetCore { get; set; }
        public string MicrosoftHostingLifetime { get; set; }
    }

    public class SecuritySettings
    {
        public int FailedLoginAttemptWindowMinutes { get; set; }
    }

}
