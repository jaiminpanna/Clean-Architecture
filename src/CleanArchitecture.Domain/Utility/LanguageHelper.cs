using Microsoft.Extensions.Localization;
using System.Globalization;

namespace CleanArchitecture.Domain.Utility
{
    public sealed class LanguageHelper(IStringLocalizer localizer)
    {
        public string GetLocalizedValue(string resourceKey, string languageId)
        {
            var culture = CultureInfo.InvariantCulture;

            if (!string.IsNullOrWhiteSpace(languageId) && languageId.Contains(
                "nb-NO", StringComparison.OrdinalIgnoreCase))
            {
                culture = new CultureInfo("no");
            }

            var originalCulture = CultureInfo.CurrentUICulture;

            try
            {
                CultureInfo.CurrentUICulture = culture;
                return localizer[resourceKey];
            }
            finally
            {
                CultureInfo.CurrentUICulture = originalCulture;
            }
        }
    }
}
