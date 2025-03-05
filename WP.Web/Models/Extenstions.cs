using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace WP.Web.Models
{
    public static class Extenstions
    {
        public static string GetClientIP(this HttpContext context)
        {
            return context.Connection.RemoteIpAddress?.ToString() ?? "";

        }
        public static string GenerateStrongPassword(this int length)
        {
            const string upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowerCase = "abcdefghijklmnopqrstuvwxyz";
            const string digits = "0123456789";
            const string specialChars = "!@#$%^&*()-_=+<>?";

            if (length < 8)
                throw new ArgumentException("Password length must be at least 8 characters.");

            string allChars = upperCase + lowerCase + digits + specialChars;
            char[] password = new char[length];

            // Ensure at least one character from each category
            password[0] = GetRandomChar(upperCase);
            password[1] = GetRandomChar(lowerCase);
            password[2] = GetRandomChar(digits);
            password[3] = GetRandomChar(specialChars);

            // Fill remaining characters randomly
            for (int i = 4; i < length; i++)
            {
                password[i] = GetRandomChar(allChars);
            }

            // Shuffle password to avoid predictable patterns
            return new string(password.OrderBy(_ => RandomNumber()).ToArray());
        }
        private static char GetRandomChar(string input)
        {
            byte[] buffer = new byte[1];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(buffer);
            }
            return input[buffer[0] % input.Length];
        }
        private static int RandomNumber()
        {
            byte[] buffer = new byte[4];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(buffer);
            }
            return BitConverter.ToInt32(buffer, 0) & int.MaxValue; // Ensure positive number
        }
        public static string EvaluatePasswordStrength(this string password)
        {
            int score = 0;

            // Conditions for scoring password strength
            if (password.Length >= 8) score++;
            if (Regex.IsMatch(password, "[A-Z]")) score++; // Uppercase letter
            if (Regex.IsMatch(password, "[a-z]")) score++; // Lowercase letter
            if (Regex.IsMatch(password, "[0-9]")) score++; // Digit
            if (Regex.IsMatch(password, "[!@#$%^&*()_+\\-={}|:;'<>,.?/]")) score++; // Special character

            // Determine password strength
            return score switch
            {
                <= 2 => "Weak",    // Score 0-2
                3 or 4 => "Medium", // Score 3-4
                5 => "Strong",      // Score 5 (Meets all conditions)
                _ => "Unknown"
            };
        }
    }
}
