using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Neovore.Web.Application.Services;
using Neovore.Web.Domain.Entities;
using Neovore.Web.Infrastructure.Repositories;

namespace Neovore.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Policy = "EditorOrAdmin")]
public class CategoriesController : Controller
{
    private readonly IGenericRepository<CategorieProduit> _repo;

    public CategoriesController(IGenericRepository<CategorieProduit> repo)
    {
        _repo = repo;
    }

    public async Task<IActionResult> Index()
    {
        var categories = await _repo.Query()
            .OrderBy(c => c.Ordre)
            .ThenBy(c => c.Nom)
            .ToListAsync();
        return View(categories);
    }

    public IActionResult Create()
    {
        return View(new CategorieProduit());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CategorieProduit model)
    {
        if (string.IsNullOrWhiteSpace(model.Slug))
            model.Slug = SlugHelper.ToSlug(model.Nom);

        if (!ModelState.IsValid) return View(model);

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
    public async Task<IActionResult> Edit(CategorieProduit model)
    {
        if (string.IsNullOrWhiteSpace(model.Slug))
            model.Slug = SlugHelper.ToSlug(model.Nom);

        if (!ModelState.IsValid) return View(model);

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

