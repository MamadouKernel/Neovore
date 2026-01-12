using Microsoft.EntityFrameworkCore;
using Neovore.Web.Domain.Entities;
using Neovore.Web.Infrastructure.Repositories;

namespace Neovore.Web.Application.Services;

public class CatalogueService
{
    private readonly IGenericRepository<CategorieProduit> _catRepo;
    private readonly IGenericRepository<Produit> _prodRepo;

    public CatalogueService(IGenericRepository<CategorieProduit> catRepo, IGenericRepository<Produit> prodRepo)
    {
        _catRepo = catRepo;
        _prodRepo = prodRepo;
    }

    public Task<List<CategorieProduit>> GetCategoriesAsync()
        => _catRepo.Query().Where(x => x.EstActive).OrderBy(x => x.Ordre).ToListAsync();

    public Task<List<Produit>> GetProduitsAsync(int? categorieId)
    {
        var q = _prodRepo.Query()
            .Include(p => p.Categorie)
            .Include(p => p.Medias)
            .Where(p => p.EstPublie);

        if (categorieId.HasValue) q = q.Where(p => p.CategorieProduitId == categorieId.Value);

        return q.OrderBy(p => p.Nom).ToListAsync();
    }

    public Task<Produit?> GetProduitBySlugAsync(string slug)
        => _prodRepo.Query()
            .Include(p => p.Categorie)
            .Include(p => p.Medias.OrderBy(m => m.Ordre))
            .Include(p => p.Commentaires)
            .FirstOrDefaultAsync(p => p.Slug == slug);
}
