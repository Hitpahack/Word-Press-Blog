using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WP.Core
{
    public static class PasswordHelper
    {
        private static readonly Random Random = new();
        public static string GeneratePassword(int length = 12)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()";
            return new string(Enumerable.Repeat(validChars, length)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
        }
        public static string GetPasswordStrength(string password)
        {
            if (password.Length < 6)
                return "Weak";
            if (password.Length < 10)
                return "Medium";
            return "Strong";
        }
    }
}
