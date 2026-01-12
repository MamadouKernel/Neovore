using Microsoft.AspNetCore.Mvc;
using Neovore.Web.Application.Services;
using Neovore.Web.Domain.Entities;
using Neovore.Web.ViewModels;

namespace Neovore.Web.Controllers;

public class CommentairesController : Controller
{
    private readonly CommentairesService _service;
    private readonly ServicesService _servicesService;
    private readonly CatalogueService _catalogueService;
    private readonly RealisationsService _realisationsService;

    public CommentairesController(
        CommentairesService service,
        ServicesService servicesService,
        CatalogueService catalogueService,
        RealisationsService realisationsService)
    {
        _service = service;
        _servicesService = servicesService;
        _catalogueService = catalogueService;
        _realisationsService = realisationsService;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Creer(CommentaireVm vm)
    {
        // Honeypot anti-spam
        if (!string.IsNullOrWhiteSpace(vm.Website))
        {
            return RedirectToAction("Index", "Home");
        }

        if (!ModelState.IsValid)
        {
            // Rediriger vers la page d'origine avec erreur
            if (vm.ServiceOffertId.HasValue)
            {
                var service = await GetServiceByIdAsync(vm.ServiceOffertId.Value);
                if (service != null) return RedirectToAction("Details", "Services", new { slug = service.Slug });
            }
            if (vm.ProduitId.HasValue)
            {
                var produit = await GetProduitByIdAsync(vm.ProduitId.Value);
                if (produit != null) return RedirectToAction("Details", "Produits", new { slug = produit.Slug });
            }
            if (vm.RealisationId.HasValue)
            {
                var real = await GetRealisationByIdAsync(vm.RealisationId.Value);
                if (real != null) return RedirectToAction("Details", "Realisations", new { slug = real.Slug });
            }
            return RedirectToAction("Index", "Home");
        }

        var commentaire = new Commentaire
        {
            Nom = vm.Nom,
            Email = vm.Email,
            Message = vm.Message,
            Note = vm.Note,
            ServiceOffertId = vm.ServiceOffertId,
            ProduitId = vm.ProduitId,
            RealisationId = vm.RealisationId
        };

        await _service.CreerCommentaireAsync(commentaire);

        TempData["CommentaireEnAttente"] = "Votre commentaire a été soumis et sera publié après validation par l'administrateur.";

        // Rediriger vers la page d'origine
        if (vm.ServiceOffertId.HasValue)
        {
            var service = await GetServiceByIdAsync(vm.ServiceOffertId.Value);
            if (service != null) return RedirectToAction("Details", "Services", new { slug = service.Slug });
        }
        if (vm.ProduitId.HasValue)
        {
            var produit = await GetProduitByIdAsync(vm.ProduitId.Value);
            if (produit != null) return RedirectToAction("Details", "Produits", new { slug = produit.Slug });
        }
        if (vm.RealisationId.HasValue)
        {
            var real = await GetRealisationByIdAsync(vm.RealisationId.Value);
            if (real != null) return RedirectToAction("Details", "Realisations", new { slug = real.Slug });
        }

        return RedirectToAction("Index", "Home");
    }

    private async Task<ServiceOffert?> GetServiceByIdAsync(int id)
    {
        var services = await _servicesService.GetPubliesAsync();
        return services.FirstOrDefault(s => s.Id == id);
    }

    private async Task<Produit?> GetProduitByIdAsync(int id)
    {
        var produits = await _catalogueService.GetProduitsAsync(null);
        return produits.FirstOrDefault(p => p.Id == id);
    }

    private async Task<Realisation?> GetRealisationByIdAsync(int id)
    {
        var realisations = await _realisationsService.GetPubliesAsync();
        return realisations.FirstOrDefault(r => r.Id == id);
    }
}
