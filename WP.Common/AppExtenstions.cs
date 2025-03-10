using System.Text.RegularExpressions;

namespace WP.Common
{
    public static class AppExtenstions
    {
        public static string[] Roles = new string[] { "wpseo_editor", "wpseo_manager", "subscriber", "contributor", "author", "editor", "administrator" };

        public static string GetActualError(this Exception exception)
        {
            string message_ = string.Empty;
            if (exception != null)
            {
                while (exception.InnerException != null)
                    exception = exception.InnerException;

                if (exception.Message.Contains("The DELETE statement conflicted with the REFERENCE constraint"))
                {
                    string patern_ = "(\"dbo.)([A-Za-z0-9_.])+(\")";
                    Regex re = new Regex(patern_, RegexOptions.IgnoreCase);
                    Match m = re.Match(exception.Message);
                    if (re.IsMatch(exception.Message))
                    {
                        message_ = $@"This record is associate with {m.Value.Replace("dbo.", string.Empty)}";
                    }
                }
                else
                {
                    message_ = exception.Message;
                }
            }
            return message_;
        }
        public static IEnumerable<T> AsList<T>(this string commaSeparatedValues) where T : struct, Enum
        {
            // Split the comma-separated values into an array
            var values = commaSeparatedValues.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            // Try parsing each value into the enum, and filter out any invalid values
            foreach (var value in values)
            {
                if (Enum.TryParse(typeof(T), value.Trim(), true, out var result))
                {
                    yield return (T)result;  // Yield the valid enum values
                }
            }
        }
        public static IEnumerable<string> AsList(this string commaSeparatedValues)
        {
            if (string.IsNullOrEmpty(commaSeparatedValues))
                return default(IEnumerable<string>);

            return commaSeparatedValues.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).AsEnumerable();
        }
    }
    
}
