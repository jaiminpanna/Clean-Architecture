using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace CleanArchitecture.Domain.Utility
{
    public static class ExtractionHelper
    {
        public static string ExtractValidationOfRoleManagement(string message, ILogger logger)
        {
            try
            {
                var json = message.Substring(message.IndexOf('{'));
                using var doc = JsonDocument.Parse(json);

                var errors = doc.RootElement.GetProperty("errors");

                foreach (var prop in errors.EnumerateObject())
                {
                    foreach (var item in prop.Value.EnumerateArray())
                        return item.GetString();
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Failed to extract validation message from response: {Message}", message);
                return message;
            }

            return message;
        }
    }
}
