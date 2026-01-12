using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Neovore.Web.Application.Services;
using Neovore.Web.Domain.Entities;
using Neovore.Web.Infrastructure.Repositories;

namespace Neovore.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Policy = "EditorOrAdmin")]
public class CommentairesController : Controller
{
    private readonly IGenericRepository<Commentaire> _repo;

    public CommentairesController(IGenericRepository<Commentaire> repo)
    {
        _repo = repo;
    }

    public async Task<IActionResult> Index()
    {
        var commentaires = await _repo.Query()
            .OrderByDescending(c => c.CreatedAtUtc)
            .ToListAsync();
        return View(commentaires);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Approuver(int id)
    {
        var commentaire = await _repo.GetByIdAsync(id);
        if (commentaire is null) return NotFound();

        commentaire.EstApprouve = true;
        commentaire.EstRejete = false;
        await _repo.UpdateAsync(commentaire);
        await _repo.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Rejeter(int id)
    {
        var commentaire = await _repo.GetByIdAsync(id);
        if (commentaire is null) return NotFound();

        commentaire.EstApprouve = false;
        commentaire.EstRejete = true;
        await _repo.UpdateAsync(commentaire);
        await _repo.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Supprimer(int id)
    {
        var commentaire = await _repo.GetByIdAsync(id);
        if (commentaire is null) return NotFound();

        await _repo.DeleteAsync(commentaire);
        await _repo.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}

