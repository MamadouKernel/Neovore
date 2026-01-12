using Microsoft.AspNetCore.Mvc;
using Neovore.Web.Application.Services;
using Neovore.Web.Domain.Entities;
using Neovore.Web.ViewModels;

namespace Neovore.Web.Controllers;

public class DevisController : Controller
{
    private readonly DevisService _service;

    public DevisController(DevisService service)
    {
        _service = service;
    }

    [HttpGet("/devis")]
    public IActionResult Index() => View(new DemandeDevisVm());

    [HttpPost("/devis")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(DemandeDevisVm vm)
    {
        // honeypot
        if (!string.IsNullOrWhiteSpace(vm.Website))
        {
            return RedirectToAction("Success");
        }

        if (!ModelState.IsValid) return View(vm);

        var entity = new DemandeDevis
        {
            Nom = vm.Nom,
            Societe = vm.Societe,
            Telephone = vm.Telephone,
            Email = vm.Email,
            Secteur = vm.Secteur,
            TypeBesoin = vm.TypeBesoin,
            Message = vm.Message,
            Statut = "Nouveau"
        };

        await _service.CreerAsync(entity);
        return RedirectToAction("Success");
    }

    [HttpGet("/devis/success")]
    public IActionResult Success() => View();
}
