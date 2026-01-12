using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Neovore.Web.Application.Services;
using Neovore.Web.Domain.Entities;
using Neovore.Web.Infrastructure.Repositories;
using Neovore.Web.Infrastructure.Storage;

namespace Neovore.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Policy = "EditorOrAdmin")]
public class ServicesController : Controller
{
    private readonly IGenericRepository<ServiceOffert> _repo;
    private readonly IFileStorage _fileStorage;

    public ServicesController(IGenericRepository<ServiceOffert> repo, IFileStorage fileStorage)
    {
        _repo = repo;
        _fileStorage = fileStorage;
    }

    public async Task<IActionResult> Index()
        => View(await _repo.Query().OrderBy(x => x.Ordre).ToListAsync());

    public IActionResult Create() => View(new ServiceOffert());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ServiceOffert model, IFormFile? imageFile, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(model.Slug))
            model.Slug = SlugHelper.ToSlug(model.Titre);

        if (!ModelState.IsValid) return View(model);

        // Upload image si fournie
        if (imageFile != null && imageFile.Length > 0)
        {
            model.ImageUrl = await _fileStorage.SaveImageAsync(imageFile, "services", ct);
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
    public async Task<IActionResult> Edit(ServiceOffert model, IFormFile? imageFile, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(model.Slug))
            model.Slug = SlugHelper.ToSlug(model.Titre);

        if (!ModelState.IsValid)
        {
            var item = await _repo.GetByIdAsync(model.Id);
            if (item != null)
            {
                model.ImageUrl = item.ImageUrl;
            }
            return View(model);
        }

        // Upload nouvelle image si fournie
        if (imageFile != null && imageFile.Length > 0)
        {
            model.ImageUrl = await _fileStorage.SaveImageAsync(imageFile, "services", ct);
        }
        else
        {
            // Conserver l'image existante si pas de nouvelle image
            var existing = await _repo.GetByIdAsync(model.Id);
            if (existing != null)
            {
                model.ImageUrl = existing.ImageUrl;
            }
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
