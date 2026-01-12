using Microsoft.EntityFrameworkCore;
using Neovore.Web.Domain.Entities;
using Neovore.Web.Infrastructure.Repositories;

namespace Neovore.Web.Application.Services;

public class CommentairesService
{
    private readonly IGenericRepository<Commentaire> _repo;
    private readonly IGenericRepository<ServiceOffert> _serviceRepo;
    private readonly IGenericRepository<Produit> _produitRepo;
    private readonly IGenericRepository<Realisation> _realRepo;

    public CommentairesService(
        IGenericRepository<Commentaire> repo,
        IGenericRepository<ServiceOffert> serviceRepo,
        IGenericRepository<Produit> produitRepo,
        IGenericRepository<Realisation> realRepo)
    {
        _repo = repo;
        _serviceRepo = serviceRepo;
        _produitRepo = produitRepo;
        _realRepo = realRepo;
    }

    public async Task CreerCommentaireAsync(Commentaire commentaire)
    {
        // Vérifier que les commentaires sont activés
        bool commentairesActives = false;
        if (commentaire.ServiceOffertId.HasValue)
        {
            var service = await _serviceRepo.GetByIdAsync(commentaire.ServiceOffertId.Value);
            commentairesActives = service?.CommentairesActives ?? false;
        }
        else if (commentaire.ProduitId.HasValue)
        {
            var produit = await _produitRepo.GetByIdAsync(commentaire.ProduitId.Value);
            commentairesActives = produit?.CommentairesActives ?? false;
        }
        else if (commentaire.RealisationId.HasValue)
        {
            var real = await _realRepo.GetByIdAsync(commentaire.RealisationId.Value);
            commentairesActives = real?.CommentairesActives ?? false;
        }

        if (!commentairesActives) return;

        commentaire.EstApprouve = false;
        commentaire.EstRejete = false;
        await _repo.AddAsync(commentaire);
        await _repo.SaveChangesAsync();
    }

    public Task<List<Commentaire>> GetCommentairesApprouvesAsync(int? serviceId, int? produitId, int? realisationId)
    {
        var q = _repo.Query().Where(c => c.EstApprouve && !c.EstRejete);
        
        if (serviceId.HasValue) q = q.Where(c => c.ServiceOffertId == serviceId);
        if (produitId.HasValue) q = q.Where(c => c.ProduitId == produitId);
        if (realisationId.HasValue) q = q.Where(c => c.RealisationId == realisationId);

        return q.OrderByDescending(c => c.CreatedAtUtc).ToListAsync();
    }

    public Task<List<Commentaire>> GetAllCommentairesAsync()
        => _repo.Query().OrderByDescending(c => c.CreatedAtUtc).ToListAsync();
}

