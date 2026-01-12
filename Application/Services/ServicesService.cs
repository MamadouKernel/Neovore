using Microsoft.EntityFrameworkCore;
using Neovore.Web.Domain.Entities;
using Neovore.Web.Infrastructure.Repositories;

namespace Neovore.Web.Application.Services;

public class ServicesService
{
    private readonly IGenericRepository<ServiceOffert> _repo;

    public ServicesService(IGenericRepository<ServiceOffert> repo)
    {
        _repo = repo;
    }

    public Task<List<ServiceOffert>> GetPubliesAsync()
        => _repo.Query().Where(x => x.EstPublie).OrderBy(x => x.Ordre).ToListAsync();

    public Task<ServiceOffert?> GetBySlugAsync(string slug)
        => _repo.Query().FirstOrDefaultAsync(x => x.Slug == slug);
}
