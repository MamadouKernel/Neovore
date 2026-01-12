using Microsoft.EntityFrameworkCore;
using Neovore.Web.Domain.Entities;
using Neovore.Web.Infrastructure.Repositories;

namespace Neovore.Web.Application.Services;

public class RealisationsService
{
    private readonly IGenericRepository<Realisation> _repo;

    public RealisationsService(IGenericRepository<Realisation> repo)
    {
        _repo = repo;
    }

    public Task<List<Realisation>> GetPubliesAsync()
        => _repo.Query().Include(r => r.Medias).Where(r => r.EstPublie).OrderByDescending(r => r.CreatedAtUtc).ToListAsync();

    public Task<Realisation?> GetBySlugAsync(string slug)
        => _repo.Query()
            .Include(r => r.Medias.OrderBy(m => m.Ordre))
            .Include(r => r.Commentaires)
            .FirstOrDefaultAsync(r => r.Slug == slug);
}
