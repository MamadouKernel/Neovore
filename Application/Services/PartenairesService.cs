using Microsoft.EntityFrameworkCore;
using Neovore.Web.Domain.Entities;
using Neovore.Web.Infrastructure.Repositories;

namespace Neovore.Web.Application.Services;

public class PartenairesService
{
    private readonly IGenericRepository<Partenaire> _repo;

    public PartenairesService(IGenericRepository<Partenaire> repo)
    {
        _repo = repo;
    }

    public Task<List<Partenaire>> GetPubliesAsync()
        => _repo.Query().Where(p => p.EstPublie).OrderBy(p => p.Ordre).ThenBy(p => p.Nom).ToListAsync();

    public Task<List<Partenaire>> GetAllAsync()
        => _repo.Query().OrderBy(p => p.Ordre).ThenBy(p => p.Nom).ToListAsync();
}

