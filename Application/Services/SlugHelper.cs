using System.Text;
using System.Text.RegularExpressions;

namespace Neovore.Web.Application.Services;

public static class SlugHelper
{
    public static string ToSlug(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return "";
        value = value.Trim().ToLowerInvariant();

        // remove accents (simple)
        var normalized = value.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();
        foreach (var c in normalized)
        {
            var uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
            if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                sb.Append(c);
        }

        var cleaned = sb.ToString().Normalize(NormalizationForm.FormC);
        cleaned = Regex.Replace(cleaned, @"[^a-z0-9\s-]", "");
        cleaned = Regex.Replace(cleaned, @"\s+", "-").Trim('-');
        cleaned = Regex.Replace(cleaned, @"-+", "-");
        return cleaned;
    }
}
