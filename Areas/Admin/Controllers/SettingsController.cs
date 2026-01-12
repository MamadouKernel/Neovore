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

    public SettingsController(SettingsService service, IFileStorage fileStorage)
    {
        _service = service;
        _fileStorage = fileStorage;
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
            var existingSettings = await _service.GetAsync();
            if (existingSettings != null && string.IsNullOrEmpty(model.LogoUrl))
            {
                model.LogoUrl = existingSettings.LogoUrl;
            }
        }

        await _service.SaveAsync(model);
        ViewBag.Message = "Paramètres sauvegardés.";
        return View(model);
    }
}
