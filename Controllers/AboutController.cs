using Microsoft.AspNetCore.Mvc;
using Neovore.Web.Application.Services;

namespace Neovore.Web.Controllers;

public class AboutController : Controller
{
    private readonly SettingsService _settings;
    private readonly EquipeService _equipeService;
    private readonly PartenairesService _partenairesService;

    public AboutController(
        SettingsService settings, 
        EquipeService equipeService,
        PartenairesService partenairesService)
    {
        _settings = settings;
        _equipeService = equipeService;
        _partenairesService = partenairesService;
    }

    [HttpGet("/a-propos")]
    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "À propos - NÉOVORE";
        
        var settings = await _settings.GetAsync();
        ViewBag.Settings = settings;
        
        // Récupérer l'équipe et les partenaires pour affichage
        var equipe = await _equipeService.GetPubliesAsync();
        var partenaires = await _partenairesService.GetPubliesAsync();
        
        ViewBag.Equipe = equipe;
        ViewBag.Partenaires = partenaires;
        
        return View();
    }
}
