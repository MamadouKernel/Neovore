using Microsoft.AspNetCore.Hosting;

namespace Neovore.Web.Infrastructure.Storage;

public interface IFileStorage
{
    Task<string> SaveImageAsync(IFormFile file, string subFolder, CancellationToken ct);
    Task<string> SaveVideoAsync(IFormFile file, string subFolder, CancellationToken ct);
    Task<string> SaveSubtitleAsync(IFormFile file, string subFolder, CancellationToken ct);
}

public class LocalFileStorage : IFileStorage
{
    private readonly IWebHostEnvironment _env;

    public LocalFileStorage(IWebHostEnvironment env)
    {
        _env = env;
    }

    public async Task<string> SaveImageAsync(IFormFile file, string subFolder, CancellationToken ct)
    {
        if (file.Length == 0) throw new InvalidOperationException("Fichier vide.");

        var uploadsRoot = Path.Combine(_env.WebRootPath, "uploads", subFolder);
        Directory.CreateDirectory(uploadsRoot);

        var ext = Path.GetExtension(file.FileName);
        var safeExt = string.IsNullOrWhiteSpace(ext) ? ".jpg" : ext.ToLowerInvariant();
        var fileName = $"{Guid.NewGuid():N}{safeExt}";
        var fullPath = Path.Combine(uploadsRoot, fileName);

        await using var stream = new FileStream(fullPath, FileMode.Create);
        await file.CopyToAsync(stream, ct);

        // URL public
        return $"/uploads/{subFolder}/{fileName}";
    }

    public async Task<string> SaveVideoAsync(IFormFile file, string subFolder, CancellationToken ct)
    {
        if (file.Length == 0) throw new InvalidOperationException("Fichier vide.");

        var uploadsRoot = Path.Combine(_env.WebRootPath, "uploads", subFolder, "videos");
        Directory.CreateDirectory(uploadsRoot);

        var ext = Path.GetExtension(file.FileName);
        var safeExt = string.IsNullOrWhiteSpace(ext) ? ".mp4" : ext.ToLowerInvariant();
        // Validation des formats vidéo
        var allowedExts = new[] { ".mp4", ".webm", ".ogg" };
        if (!allowedExts.Contains(safeExt))
            throw new InvalidOperationException($"Format vidéo non supporté. Formats acceptés: {string.Join(", ", allowedExts)}");

        var fileName = $"{Guid.NewGuid():N}{safeExt}";
        var fullPath = Path.Combine(uploadsRoot, fileName);

        await using var stream = new FileStream(fullPath, FileMode.Create);
        await file.CopyToAsync(stream, ct);

        return $"/uploads/{subFolder}/videos/{fileName}";
    }

    public async Task<string> SaveSubtitleAsync(IFormFile file, string subFolder, CancellationToken ct)
    {
        if (file.Length == 0) throw new InvalidOperationException("Fichier vide.");

        var uploadsRoot = Path.Combine(_env.WebRootPath, "uploads", subFolder, "subtitles");
        Directory.CreateDirectory(uploadsRoot);

        var ext = Path.GetExtension(file.FileName);
        var safeExt = string.IsNullOrWhiteSpace(ext) ? ".vtt" : ext.ToLowerInvariant();
        // Validation des formats de sous-titres
        var allowedExts = new[] { ".vtt", ".srt" };
        if (!allowedExts.Contains(safeExt))
            throw new InvalidOperationException($"Format de sous-titres non supporté. Formats acceptés: {string.Join(", ", allowedExts)}");

        var fileName = $"{Guid.NewGuid():N}{safeExt}";
        var fullPath = Path.Combine(uploadsRoot, fileName);

        await using var stream = new FileStream(fullPath, FileMode.Create);
        await file.CopyToAsync(stream, ct);

        return $"/uploads/{subFolder}/subtitles/{fileName}";
    }
}
