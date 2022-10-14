using System.Text.Json;

namespace App.Extensions
{
    public static class StringExtensions
    {
        public static string JsonPrettify(this string json)
        {
            using var jsonDocument = JsonDocument.Parse(json);
            var jsonOptions = new JsonSerializerOptions { WriteIndented = true };
            return JsonSerializer.Serialize(jsonDocument, jsonOptions);
        }
    }
}
