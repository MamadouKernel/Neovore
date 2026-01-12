using Microsoft.AspNetCore.Mvc;
using Neovore.Web.Application.Services;
using Neovore.Web.ViewModels;

namespace Neovore.Web.Controllers;

public class ProduitsController : Controller
{
    private readonly CatalogueService _catalogue;
    private readonly CommentairesService _commentairesService;

    public ProduitsController(CatalogueService catalogue, CommentairesService commentairesService)
    {
        _catalogue = catalogue;
        _commentairesService = commentairesService;
    }

    public async Task<IActionResult> Index(int? categorieId)
    {
        ViewBag.Categories = await _catalogue.GetCategoriesAsync();
        var produits = await _catalogue.GetProduitsAsync(categorieId);
        return View(produits);
    }

    [HttpGet("/produits/{slug}")]
    public async Task<IActionResult> Details(string slug)
    {
        var produit = await _catalogue.GetProduitBySlugAsync(slug);
        if (produit is null) return NotFound();
        
        var commentaires = produit.CommentairesActives 
            ? await _commentairesService.GetCommentairesApprouvesAsync(null, produit.Id, null)
            : new List<Domain.Entities.Commentaire>();
        
        ViewData["CommentairesPartial"] = new CommentairePartialVm
        {
            ProduitId = produit.Id,
            CommentairesActives = produit.CommentairesActives,
            Commentaires = commentaires
        };
        
        return View(produit);
    }
}
