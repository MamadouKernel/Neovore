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
public class RealisationsController : Controller
{
    private readonly IGenericRepository<Realisation> _repo;
    private readonly IGenericRepository<Media> _mediaRepo;
    private readonly IFileStorage _fileStorage;

    public RealisationsController(
        IGenericRepository<Realisation> repo,
        IGenericRepository<Media> mediaRepo,
        IFileStorage fileStorage)
    {
        _repo = repo;
        _mediaRepo = mediaRepo;
        _fileStorage = fileStorage;
    }

    public async Task<IActionResult> Index()
    {
        var realisations = await _repo.Query()
            .Include(r => r.Medias)
            .OrderByDescending(r => r.CreatedAtUtc)
            .ToListAsync();
        return View(realisations);
    }

    public IActionResult Create()
    {
        return View(new Realisation());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Realisation model, List<IFormFile>? imageFiles, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(model.Slug))
            model.Slug = SlugHelper.ToSlug(model.Titre);

        if (!ModelState.IsValid) return View(model);

        await _repo.AddAsync(model);
        await _repo.SaveChangesAsync();

        // Upload images si fournies
        if (imageFiles != null && imageFiles.Any(f => f.Length > 0))
        {
            int ordre = 0;
            foreach (var file in imageFiles.Where(f => f.Length > 0))
            {
                var imageUrl = await _fileStorage.SaveImageAsync(file, "realisations", ct);
                var media = new Media
                {
                    Url = imageUrl,
                    Alt = model.Titre,
                    Ordre = ordre++,
                    RealisationId = model.Id
                };
                await _mediaRepo.AddAsync(media);
            }
            await _mediaRepo.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var item = await _repo.Query()
            .Include(r => r.Medias.OrderBy(m => m.Ordre))
            .FirstOrDefaultAsync(r => r.Id == id);
        if (item is null) return NotFound();

        return View(item);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Realisation model, List<IFormFile>? imageFiles, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(model.Slug))
            model.Slug = SlugHelper.ToSlug(model.Titre);

        if (!ModelState.IsValid)
        {
            var item = await _repo.Query()
                .Include(r => r.Medias.OrderBy(m => m.Ordre))
                .FirstOrDefaultAsync(r => r.Id == model.Id);
            if (item != null)
            {
                model.Medias = item.Medias;
            }
            return View(model);
        }

        // Upload nouvelles images si fournies
        if (imageFiles != null && imageFiles.Any(f => f.Length > 0))
        {
            var existingMedias = await _mediaRepo.Query()
                .Where(m => m.RealisationId == model.Id)
                .ToListAsync();
            int maxOrdre = existingMedias.Any() ? existingMedias.Max(m => m.Ordre) + 1 : 0;

            foreach (var file in imageFiles.Where(f => f.Length > 0))
            {
                var imageUrl = await _fileStorage.SaveImageAsync(file, "realisations", ct);
                var media = new Media
                {
                    Url = imageUrl,
                    Alt = model.Titre,
                    Ordre = maxOrdre++,
                    RealisationId = model.Id
                };
                await _mediaRepo.AddAsync(media);
            }
            await _mediaRepo.SaveChangesAsync();
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

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteMedia(int id)
    {
        var media = await _mediaRepo.GetByIdAsync(id);
        if (media is null) return NotFound();
        var realisationId = media.RealisationId;
        await _mediaRepo.DeleteAsync(media);
        await _mediaRepo.SaveChangesAsync();
        return RedirectToAction(nameof(Edit), new { id = realisationId });
    }
}
