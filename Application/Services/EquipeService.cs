using Microsoft.EntityFrameworkCore;
using Neovore.Web.Domain.Entities;
using Neovore.Web.Infrastructure.Repositories;

namespace Neovore.Web.Application.Services;

public class EquipeService
{
    private readonly IGenericRepository<MembreEquipe> _repo;

    public EquipeService(IGenericRepository<MembreEquipe> repo)
    {
        _repo = repo;
    }

    public Task<List<MembreEquipe>> GetPubliesAsync()
        => _repo.Query().Where(m => m.EstPublie).OrderBy(m => m.Ordre).ThenBy(m => m.Nom).ToListAsync();

    public Task<List<MembreEquipe>> GetAllAsync()
        => _repo.Query().OrderBy(m => m.Ordre).ThenBy(m => m.Nom).ToListAsync();
}

