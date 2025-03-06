using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WP.Core.Middleware
{
    public class GlobalDateTimeConverter : JsonConverter<DateTime?>
    {
        private const string DateFormat = "yyyy-MM-dd HH:mm:ss"; // Customize the format

        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                string? dateStr = reader.GetString();
                if (DateTime.TryParse(dateStr, out var date))
                {
                    return date == DateTime.MinValue ? null : date;
                }
            }
            return null; // Return null if invalid or zero-date
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
                writer.WriteStringValue(value.Value.ToString(DateFormat));
            else
                writer.WriteNullValue();
        }
    }

}
