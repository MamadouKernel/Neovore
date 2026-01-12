using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Neovore.Web.Domain.Entities;
using Neovore.Web.Infrastructure.Repositories;
using Neovore.Web.Infrastructure.Storage;

namespace Neovore.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Policy = "EditorOrAdmin")]
public class EquipeController : Controller
{
    private readonly IGenericRepository<MembreEquipe> _repo;
    private readonly IFileStorage _fileStorage;

    public EquipeController(IGenericRepository<MembreEquipe> repo, IFileStorage fileStorage)
    {
        _repo = repo;
        _fileStorage = fileStorage;
    }

    public async Task<IActionResult> Index()
    {
        var membres = await _repo.Query()
            .OrderBy(m => m.Ordre)
            .ThenBy(m => m.Nom)
            .ToListAsync();
        return View(membres);
    }

    public IActionResult Create()
    {
        return View(new MembreEquipe());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MembreEquipe model, IFormFile? photoFile, CancellationToken ct)
    {
        if (!ModelState.IsValid) return View(model);

        if (photoFile != null && photoFile.Length > 0)
        {
            model.PhotoUrl = await _fileStorage.SaveImageAsync(photoFile, "equipe", ct);
        }

        await _repo.AddAsync(model);
        await _repo.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var item = await _repo.GetByIdAsync(id);
        if (item is null) return NotFound();
        return View(item);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(MembreEquipe model, IFormFile? photoFile, CancellationToken ct)
    {
        if (!ModelState.IsValid) return View(model);

        if (photoFile != null && photoFile.Length > 0)
        {
            model.PhotoUrl = await _fileStorage.SaveImageAsync(photoFile, "equipe", ct);
        }

        await _repo.UpdateAsync(model);
        await _repo.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _repo.GetByIdAsync(id);
        if (item is null) return NotFound();
        await _repo.DeleteAsync(item);
        await _repo.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}

