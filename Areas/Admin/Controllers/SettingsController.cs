using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Neovore.Web.Application.Services;
using Neovore.Web.Domain.Entities;
using Neovore.Web.Infrastructure.Storage;

namespace Neovore.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Policy = "AdminOnly")]
public class SettingsController : Controller
{
    private readonly SettingsService _service;
    private readonly IFileStorage _fileStorage;
    private readonly IGeocodingService _geocodingService;

    public SettingsController(SettingsService service, IFileStorage fileStorage, IGeocodingService geocodingService)
    {
        _service = service;
        _fileStorage = fileStorage;
        _geocodingService = geocodingService;
    }

    public async Task<IActionResult> Index()
    {
        var settings = await _service.GetAsync() ?? new SiteSettings();
        return View(settings);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(SiteSettings model, IFormFile? logoFile)
    {
        if (!ModelState.IsValid) return View(model);

        // Récupérer les paramètres existants pour toutes les opérations
        var existingSettings = await _service.GetAsync();

        // Gérer l'upload du logo si un fichier est fourni
        if (logoFile != null && logoFile.Length > 0)
        {
            try
            {
                var logoUrl = await _fileStorage.SaveImageAsync(logoFile, "logo", CancellationToken.None);
                model.LogoUrl = logoUrl;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("logoFile", $"Erreur lors de l'upload du logo : {ex.Message}");
                return View(model);
            }
        }
        else
        {
            // Conserver l'URL du logo existante si aucun nouveau fichier n'est fourni
            if (existingSettings != null && string.IsNullOrEmpty(model.LogoUrl))
            {
                model.LogoUrl = existingSettings.LogoUrl;
            }
        }

        // Si un iframe est fourni, c'est la méthode principale
        // Les coordonnées GPS ne sont plus nécessaires si on a un iframe
        if (model.AfficherCarte && !string.IsNullOrWhiteSpace(model.GoogleMapsIframe))
        {
            // Si l'iframe a changé, on peut essayer d'extraire les coordonnées depuis l'iframe pour référence
            // Mais ce n'est pas obligatoire car l'iframe contient déjà la carte
        }
        // Extraction des coordonnées GPS si la carte est activée et pas d'iframe
        else if (model.AfficherCarte)
        {
            // Priorité 1 : Extraire depuis le lien Google Maps si fourni
            if (!string.IsNullOrWhiteSpace(model.GoogleMapsUrl))
            {
                // Vérifier si le lien a changé ou si les coordonnées n'existent pas
                if (existingSettings == null || 
                    existingSettings.GoogleMapsUrl != model.GoogleMapsUrl || 
                    !existingSettings.Latitude.HasValue || 
                    !existingSettings.Longitude.HasValue)
                {
                    try
                    {
                        var (latitude, longitude) = await _geocodingService.ExtractCoordinatesFromGoogleMapsUrlAsync(model.GoogleMapsUrl);
                        if (latitude.HasValue && longitude.HasValue)
                        {
                            model.Latitude = latitude;
                            model.Longitude = longitude;
                        }
                        else
                        {
                            // Si l'extraction échoue, conserver les coordonnées existantes ou afficher un avertissement
                            if (existingSettings != null && existingSettings.Latitude.HasValue && existingSettings.Longitude.HasValue)
                            {
                                model.Latitude = existingSettings.Latitude;
                                model.Longitude = existingSettings.Longitude;
                                ModelState.AddModelError("GoogleMapsUrl", "Impossible d'extraire les coordonnées depuis le lien Google Maps. Les coordonnées existantes ont été conservées.");
                            }
                            else
                            {
                                ModelState.AddModelError("GoogleMapsUrl", "Impossible d'extraire les coordonnées depuis ce lien Google Maps. Veuillez vérifier le lien ou utiliser l'adresse pour le géocodage automatique.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // En cas d'erreur, conserver les coordonnées existantes
                        if (existingSettings != null && existingSettings.Latitude.HasValue && existingSettings.Longitude.HasValue)
                        {
                            model.Latitude = existingSettings.Latitude;
                            model.Longitude = existingSettings.Longitude;
                            ModelState.AddModelError("GoogleMapsUrl", $"Erreur lors de l'extraction des coordonnées : {ex.Message}. Les coordonnées existantes ont été conservées.");
                        }
                    }
                }
                else
                {
                    // Conserver les coordonnées existantes si le lien n'a pas changé
                    model.Latitude = existingSettings.Latitude;
                    model.Longitude = existingSettings.Longitude;
                }
            }
            // Priorité 2 : Géocoder depuis l'adresse si pas de lien Google Maps
            else if (!string.IsNullOrWhiteSpace(model.Adresse))
            {
                // Vérifier si l'adresse est en fait un lien Google Maps (erreur de l'utilisateur)
                if (model.Adresse.Contains("maps.app.goo.gl") || 
                    model.Adresse.Contains("goo.gl") || 
                    model.Adresse.Contains("google.com/maps") ||
                    model.Adresse.StartsWith("http"))
                {
                    // L'utilisateur a mis un lien dans le champ adresse, lui suggérer de le mettre dans le bon champ
                    ModelState.AddModelError("Adresse", "Ce champ doit contenir une adresse textuelle, pas un lien Google Maps. Si vous avez un lien Google Maps, veuillez le mettre dans le champ 'Lien Google Maps' ci-dessous et laissez ce champ vide ou mettez l'adresse textuelle.");
                    
                    // Essayer quand même d'extraire les coordonnées du lien si possible
                    try
                    {
                        var (latitude, longitude) = await _geocodingService.ExtractCoordinatesFromGoogleMapsUrlAsync(model.Adresse);
                        if (latitude.HasValue && longitude.HasValue)
                        {
                            model.Latitude = latitude;
                            model.Longitude = longitude;
                            model.GoogleMapsUrl = model.Adresse; // Copier dans le bon champ
                            ModelState.AddModelError("", $"Les coordonnées ont été extraites du lien trouvé dans le champ 'Adresse'. Pour éviter ce message, veuillez mettre le lien dans le champ 'Lien Google Maps' et mettre l'adresse textuelle dans le champ 'Adresse'.");
                        }
                    }
                    catch { }
                    
                    // Conserver les coordonnées existantes en attendant
                    if (existingSettings != null && existingSettings.Latitude.HasValue && existingSettings.Longitude.HasValue)
                    {
                        model.Latitude = existingSettings.Latitude;
                        model.Longitude = existingSettings.Longitude;
                    }
                }
                else
                {
                    // C'est bien une adresse textuelle, procéder au géocodage
                    // Vérifier si l'adresse a changé ou si les coordonnées n'existent pas
                    if (existingSettings == null || 
                        existingSettings.Adresse != model.Adresse || 
                        !existingSettings.Latitude.HasValue || 
                        !existingSettings.Longitude.HasValue)
                    {
                        try
                        {
                            var (latitude, longitude) = await _geocodingService.GeocodeAsync(model.Adresse);
                            if (latitude.HasValue && longitude.HasValue)
                            {
                                model.Latitude = latitude;
                                model.Longitude = longitude;
                            }
                            else
                            {
                                // Si le géocodage échoue, conserver les coordonnées existantes ou afficher un avertissement
                                if (existingSettings != null && existingSettings.Latitude.HasValue && existingSettings.Longitude.HasValue)
                                {
                                    model.Latitude = existingSettings.Latitude;
                                    model.Longitude = existingSettings.Longitude;
                                    ModelState.AddModelError("Adresse", "Impossible de géocoder cette adresse. Pour Abidjan, précisez la rue (avec code si applicable comme 'RUE K84'), le quartier et la commune (ex: 'RUE K84, Angré, Cocody, Abidjan' plutôt que juste 'Abidjan'). Les coordonnées existantes ont été conservées. Si vous avez un lien Google Maps, utilisez le champ 'Lien Google Maps' ci-dessous (méthode recommandée).");
                                }
                                else
                                {
                                    ModelState.AddModelError("Adresse", "Impossible de trouver les coordonnées GPS pour cette adresse. Pour Abidjan, précisez la rue (avec code si applicable comme 'RUE K84'), le quartier et la commune (ex: 'RUE K84, Angré, Cocody, Abidjan'). Veuillez fournir un lien Google Maps dans le champ dédié (méthode recommandée) ou désactiver la carte.");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            // En cas d'erreur de géocodage, conserver les coordonnées existantes
                            if (existingSettings != null && existingSettings.Latitude.HasValue && existingSettings.Longitude.HasValue)
                            {
                                model.Latitude = existingSettings.Latitude;
                                model.Longitude = existingSettings.Longitude;
                                ModelState.AddModelError("Adresse", $"Erreur lors du géocodage : {ex.Message}. Les coordonnées existantes ont été conservées.");
                            }
                        }
                    }
                    else
                    {
                        // Conserver les coordonnées existantes si l'adresse n'a pas changé
                        model.Latitude = existingSettings.Latitude;
                        model.Longitude = existingSettings.Longitude;
                    }
                }
            }
            else
            {
                // Pas de lien Google Maps ni d'adresse, conserver les coordonnées existantes ou afficher une erreur
                if (existingSettings != null && existingSettings.Latitude.HasValue && existingSettings.Longitude.HasValue)
                {
                    model.Latitude = existingSettings.Latitude;
                    model.Longitude = existingSettings.Longitude;
                }
                else
                {
                    ModelState.AddModelError("AfficherCarte", "Pour afficher la carte, veuillez fournir soit un lien Google Maps, soit une adresse complète.");
                }
            }
        }
        else if (!model.AfficherCarte)
        {
            // Conserver les coordonnées existantes si la carte est désactivée
            if (existingSettings != null)
            {
                model.Latitude = existingSettings.Latitude;
                model.Longitude = existingSettings.Longitude;
                model.GoogleMapsUrl = existingSettings.GoogleMapsUrl;
            }
        }

        if (!ModelState.IsValid) return View(model);

        await _service.SaveAsync(model);
        ViewBag.Message = "Paramètres sauvegardés.";
        return View(model);
    }
}
