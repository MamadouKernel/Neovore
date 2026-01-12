using Microsoft.EntityFrameworkCore;
using Neovore.Web.Domain.Entities;
using Neovore.Web.Infrastructure.Repositories;

namespace Neovore.Web.Application.Services;

public class SettingsService
{
    private readonly IGenericRepository<SiteSettings> _repo;

    public SettingsService(IGenericRepository<SiteSettings> repo)
    {
        _repo = repo;
    }

    public async Task<SiteSettings?> GetAsync()
    {
        try
        {
            return await _repo.Query().OrderByDescending(x => x.Id).FirstOrDefaultAsync();
        }
        catch
        {
            // Retourne null si les colonnes n'existent pas encore (avant migration)
            return null;
        }
    }

    public async Task SaveAsync(SiteSettings settings)
    {
        if (settings.Id == 0) await _repo.AddAsync(settings);
        else await _repo.UpdateAsync(settings);

        await _repo.SaveChangesAsync();
    }
}
