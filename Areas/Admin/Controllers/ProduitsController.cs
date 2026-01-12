using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Neovore.Web.Application.Services;
using Neovore.Web.Domain.Entities;
using Neovore.Web.Infrastructure.Repositories;
using Neovore.Web.Infrastructure.Storage;

namespace Neovore.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Policy = "EditorOrAdmin")]
public class ProduitsController : Controller
{
    private readonly IGenericRepository<Produit> _repo;
    private readonly IGenericRepository<CategorieProduit> _catRepo;
    private readonly IGenericRepository<Media> _mediaRepo;
    private readonly IFileStorage _fileStorage;
    private readonly CatalogueService _catalogueService;

    public ProduitsController(
        IGenericRepository<Produit> repo,
        IGenericRepository<CategorieProduit> catRepo,
        IGenericRepository<Media> mediaRepo,
        IFileStorage fileStorage,
        CatalogueService catalogueService)
    {
        _repo = repo;
        _catRepo = catRepo;
        _mediaRepo = mediaRepo;
        _fileStorage = fileStorage;
        _catalogueService = catalogueService;
    }

    public async Task<IActionResult> Index()
    {
        var produits = await _repo.Query()
            .Include(p => p.Categorie)
            .Include(p => p.Medias)
            .OrderBy(p => p.Nom)
            .ToListAsync();
        return View(produits);
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Categories = await _catalogueService.GetCategoriesAsync();
        return View(new Produit());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Produit model, IFormFile? imageFile, CancellationToken ct)
    {
        ViewBag.Categories = await _catalogueService.GetCategoriesAsync();

        if (string.IsNullOrWhiteSpace(model.Slug))
            model.Slug = SlugHelper.ToSlug(model.Nom);

        if (!ModelState.IsValid) return View(model);

        await _repo.AddAsync(model);
        await _repo.SaveChangesAsync();

        // Upload image si fournie
        if (imageFile != null && imageFile.Length > 0)
        {
            var imageUrl = await _fileStorage.SaveImageAsync(imageFile, "produits", ct);
            var media = new Media
            {
                Url = imageUrl,
                Alt = model.Nom,
                Ordre = 0,
                ProduitId = model.Id
            };
            await _mediaRepo.AddAsync(media);
            await _mediaRepo.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var item = await _repo.Query()
            .Include(p => p.Categorie)
            .Include(p => p.Medias)
            .FirstOrDefaultAsync(p => p.Id == id);
        if (item is null) return NotFound();

        ViewBag.Categories = await _catalogueService.GetCategoriesAsync();
        return View(item);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Produit model, IFormFile? imageFile, CancellationToken ct)
    {
        ViewBag.Categories = await _catalogueService.GetCategoriesAsync();

        if (string.IsNullOrWhiteSpace(model.Slug))
            model.Slug = SlugHelper.ToSlug(model.Nom);

        if (!ModelState.IsValid)
        {
            var item = await _repo.Query()
                .Include(p => p.Medias)
                .FirstOrDefaultAsync(p => p.Id == model.Id);
            if (item != null)
            {
                model.Medias = item.Medias;
            }
            return View(model);
        }

        // Upload nouvelle image si fournie
        if (imageFile != null && imageFile.Length > 0)
        {
            var imageUrl = await _fileStorage.SaveImageAsync(imageFile, "produits", ct);
            var media = new Media
            {
                Url = imageUrl,
                Alt = model.Nom,
                Ordre = 0,
                ProduitId = model.Id
            };
            await _mediaRepo.AddAsync(media);
        }

        await _repo.UpdateAsync(model);
        await _repo.SaveChangesAsync();
        if (imageFile != null && imageFile.Length > 0)
        {
            await _mediaRepo.SaveChangesAsync();
        }

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

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteMedia(int id)
    {
        var media = await _mediaRepo.GetByIdAsync(id);
        if (media is null) return NotFound();
        await _mediaRepo.DeleteAsync(media);
        await _mediaRepo.SaveChangesAsync();
        return RedirectToAction(nameof(Edit), new { id = media.ProduitId });
    }
}
