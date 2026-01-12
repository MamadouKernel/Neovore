using Microsoft.AspNetCore.Mvc;
using Neovore.Web.Application.Services;

namespace Neovore.Web.Controllers;

public class HomeController : Controller
{
    private readonly ServicesService _services;
    private readonly CatalogueService _catalogue;
    private readonly RealisationsService _reals;
    private readonly SettingsService _settings;

    public HomeController(ServicesService services, CatalogueService catalogue, RealisationsService reals, SettingsService settings)
    {
        _services = services;
        _catalogue = catalogue;
        _reals = reals;
        _settings = settings;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.Services = await _services.GetPubliesAsync();
        ViewBag.Categories = await _catalogue.GetCategoriesAsync();
        ViewBag.Realisations = (await _reals.GetPubliesAsync()).Take(6).ToList();
        
        // Récupérer les produits avec leurs catégories
        var tousProduits = await _catalogue.GetProduitsAsync(null);
        ViewBag.Produits = tousProduits.Take(6).ToList();
        
        // Récupérer les statistiques depuis les settings
        try
        {
            var settings = await _settings.GetAsync();
            ViewBag.AnneeExperience = settings?.AnneesExperience ?? "+10";
            ViewBag.ProjetsRealises = settings?.ProjetsRealises ?? "500+";
            ViewBag.SatisfactionClient = settings?.SatisfactionClient ?? "100%";
        }
        catch
        {
            // Valeurs par défaut si les colonnes n'existent pas encore (avant migration)
            ViewBag.AnneeExperience = "+10";
            ViewBag.ProjetsRealises = "500+";
            ViewBag.SatisfactionClient = "100%";
        }
        
        return View();
    }
}
