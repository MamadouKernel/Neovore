using Microsoft.AspNetCore.Mvc;
using Neovore.Web.Application.Services;
using Neovore.Web.ViewModels;

namespace Neovore.Web.Controllers;

public class RealisationsController : Controller
{
    private readonly RealisationsService _service;
    private readonly CommentairesService _commentairesService;

    public RealisationsController(RealisationsService service, CommentairesService commentairesService)
    {
        _service = service;
        _commentairesService = commentairesService;
    }

    public async Task<IActionResult> Index()
    {
        var items = await _service.GetPubliesAsync();
        return View(items);
    }

    [HttpGet("/realisations/{slug}")]
    public async Task<IActionResult> Details(string slug)
    {
        var item = await _service.GetBySlugAsync(slug);
        if (item is null) return NotFound();
        
        var commentaires = item.CommentairesActives 
            ? await _commentairesService.GetCommentairesApprouvesAsync(null, null, item.Id)
            : new List<Domain.Entities.Commentaire>();
        
        ViewData["CommentairesPartial"] = new CommentairePartialVm
        {
            RealisationId = item.Id,
            CommentairesActives = item.CommentairesActives,
            Commentaires = commentaires
        };
        
        return View(item);
    }
}
