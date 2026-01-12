using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Neovore.Web.Domain.Entities;
using Neovore.Web.Infrastructure.Repositories;
using Neovore.Web.Infrastructure.Storage;

namespace Neovore.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Policy = "EditorOrAdmin")]
public class PartenairesController : Controller
{
    private readonly IGenericRepository<Partenaire> _repo;
    private readonly IFileStorage _fileStorage;

    public PartenairesController(IGenericRepository<Partenaire> repo, IFileStorage fileStorage)
    {
        _repo = repo;
        _fileStorage = fileStorage;
    }

    public async Task<IActionResult> Index()
    {
        var partenaires = await _repo.Query()
            .OrderBy(p => p.Ordre)
            .ThenBy(p => p.Nom)
            .ToListAsync();
        return View(partenaires);
    }

    public IActionResult Create()
    {
        return View(new Partenaire());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Partenaire model, IFormFile? logoFile, CancellationToken ct)
    {
        if (!ModelState.IsValid) return View(model);

        if (logoFile != null && logoFile.Length > 0)
        {
            model.LogoUrl = await _fileStorage.SaveImageAsync(logoFile, "partenaires", ct);
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
    public async Task<IActionResult> Edit(Partenaire model, IFormFile? logoFile, CancellationToken ct)
    {
        if (!ModelState.IsValid) return View(model);

        if (logoFile != null && logoFile.Length > 0)
        {
            model.LogoUrl = await _fileStorage.SaveImageAsync(logoFile, "partenaires", ct);
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

