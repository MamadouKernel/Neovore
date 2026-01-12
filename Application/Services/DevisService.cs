using Neovore.Web.Domain.Entities;
using Neovore.Web.Infrastructure.Repositories;

namespace Neovore.Web.Application.Services;

public class DevisService
{
    private readonly IGenericRepository<DemandeDevis> _repo;

    public DevisService(IGenericRepository<DemandeDevis> repo)
    {
        _repo = repo;
    }

    public async Task CreerAsync(DemandeDevis demande)
    {
        await _repo.AddAsync(demande);
        await _repo.SaveChangesAsync();
    }
}
