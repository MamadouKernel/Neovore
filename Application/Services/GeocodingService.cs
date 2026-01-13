using System.Text.Json;

namespace Neovore.Web.Application.Services;

public interface IGeocodingService
{
    Task<(decimal? Latitude, decimal? Longitude)> GeocodeAsync(string address);
    Task<(decimal? Latitude, decimal? Longitude)> ExtractCoordinatesFromGoogleMapsUrlAsync(string googleMapsUrl);
}

public class GeocodingService : IGeocodingService
{
    private readonly HttpClient _httpClient;

    public GeocodingService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "Neovore.Web/1.0");
    }

    public async Task<(decimal? Latitude, decimal? Longitude)> GeocodeAsync(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
            return (null, null);

        try
        {
            // Utiliser l'API Nominatim d'OpenStreetMap (gratuit, pas besoin de clé API)
            var encodedAddress = Uri.EscapeDataString(address);
            var url = $"https://nominatim.openstreetmap.org/search?format=json&q={encodedAddress}&limit=1";

            var response = await _httpClient.GetStringAsync(url);
            var results = JsonSerializer.Deserialize<JsonElement[]>(response);

            if (results != null && results.Length > 0)
            {
                var firstResult = results[0];
                
                if (firstResult.TryGetProperty("lat", out var latElement) &&
                    firstResult.TryGetProperty("lon", out var lonElement))
                {
                    if (decimal.TryParse(latElement.GetString(), System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out var latitude) &&
                        decimal.TryParse(lonElement.GetString(), System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out var longitude))
                    {
                        return (latitude, longitude);
                    }
                }
            }
        }
        catch
        {
            // En cas d'erreur, retourner null
            // L'admin pourra toujours saisir les coordonnées manuellement si nécessaire
        }

        return (null, null);
    }

    public async Task<(decimal? Latitude, decimal? Longitude)> ExtractCoordinatesFromGoogleMapsUrlAsync(string googleMapsUrl)
    {
        if (string.IsNullOrWhiteSpace(googleMapsUrl))
            return (null, null);

        try
        {
            // Formater l'URL pour gérer les différents formats
            var url = googleMapsUrl.Trim();

            // Si c'est un lien court (goo.gl ou maps.app.goo.gl), on doit d'abord le résoudre
            if (url.Contains("goo.gl") || url.Contains("maps.app.goo.gl"))
            {
                try
                {
                    // Résoudre le lien court pour obtenir l'URL complète
                    // Utiliser AllowAutoRedirect = false pour capturer la redirection
                    using var handler = new System.Net.Http.HttpClientHandler { AllowAutoRedirect = false };
                    using var tempClient = new HttpClient(handler);
                    tempClient.DefaultRequestHeaders.Add("User-Agent", "Neovore.Web/1.0");
                    
                    var response = await tempClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, url));
                    if (response.StatusCode == System.Net.HttpStatusCode.Redirect || 
                        response.StatusCode == System.Net.HttpStatusCode.MovedPermanently ||
                        response.StatusCode == System.Net.HttpStatusCode.TemporaryRedirect)
                    {
                        var location = response.Headers.Location?.ToString();
                        if (!string.IsNullOrEmpty(location))
                        {
                            url = location;
                        }
                    }
                }
                catch
                {
                    // Si la résolution échoue, continuer avec l'URL originale
                }
            }

            // Extraire les coordonnées depuis différents formats de liens Google Maps

            // Format 1: https://www.google.com/maps/@latitude,longitude,zoom
            var match1 = System.Text.RegularExpressions.Regex.Match(url, @"@(-?\d+\.?\d*),(-?\d+\.?\d*),");
            if (match1.Success)
            {
                if (decimal.TryParse(match1.Groups[1].Value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out var lat) &&
                    decimal.TryParse(match1.Groups[2].Value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out var lon))
                {
                    return (lat, lon);
                }
            }

            // Format 2: https://www.google.com/maps/place/.../@latitude,longitude,zoom
            var match2 = System.Text.RegularExpressions.Regex.Match(url, @"/place/[^/@]+/@(-?\d+\.?\d*),(-?\d+\.?\d*),");
            if (match2.Success)
            {
                if (decimal.TryParse(match2.Groups[1].Value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out var lat) &&
                    decimal.TryParse(match2.Groups[2].Value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out var lon))
                {
                    return (lat, lon);
                }
            }

            // Format 3: https://maps.google.com/?q=latitude,longitude
            var match3 = System.Text.RegularExpressions.Regex.Match(url, @"[?&]q=(-?\d+\.?\d*),(-?\d+\.?\d*)");
            if (match3.Success)
            {
                if (decimal.TryParse(match3.Groups[1].Value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out var lat) &&
                    decimal.TryParse(match3.Groups[2].Value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out var lon))
                {
                    return (lat, lon);
                }
            }

            // Si aucun format ne correspond, essayer de récupérer la page et chercher les coordonnées dans le contenu
            try
            {
                var htmlContent = await _httpClient.GetStringAsync(url);
                
                // Chercher les coordonnées dans les meta tags ou le JSON embarqué
                var metaMatch = System.Text.RegularExpressions.Regex.Match(htmlContent, @"""lat""\s*:\s*(-?\d+\.?\d*).*?""lng""\s*:\s*(-?\d+\.?\d*)", System.Text.RegularExpressions.RegexOptions.Singleline);
                if (metaMatch.Success)
                {
                    if (decimal.TryParse(metaMatch.Groups[1].Value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out var lat) &&
                        decimal.TryParse(metaMatch.Groups[2].Value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out var lon))
                    {
                        return (lat, lon);
                    }
                }
            }
            catch
            {
                // Si l'extraction depuis le HTML échoue, on retourne null
            }
        }
        catch
        {
            // En cas d'erreur, retourner null
        }

        return (null, null);
    }
}

