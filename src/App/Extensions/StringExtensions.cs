using System.Text.Json;

namespace App.Extensions
{
    public static class StringExtensions
    {
        public static bool IgnoreEquals(this string left, string right)
        {
            return string.Equals(left, right, StringComparison.OrdinalIgnoreCase);
        }
        
        public static string JsonPrettify(this string json)
        {
            using var jsonDocument = JsonDocument.Parse(json);
            var jsonOptions = new JsonSerializerOptions { WriteIndented = true };
            return JsonSerializer.Serialize(jsonDocument, jsonOptions);
        }
    }
}
