using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Neovore.Web.Domain.Entities;
using Neovore.Web.Infrastructure.Data;

namespace Neovore.Web.Application.Services;

public class SiteSeedService
{
    private readonly AppDbContext _db;

    public SiteSeedService(AppDbContext db)
    {
        _db = db;
    }

    public async Task SeedAsync()
    {
        // Settings
        if (!await _db.SiteSettings.AnyAsync())
        {
            _db.SiteSettings.Add(new SiteSettings
            {
                NomEntreprise = "NÉOVORE",
                EmailContact = "contact@neovore.ci",
                TelephoneContact = "+225 00 00 00 00",
                Adresse = "Abidjan, Côte d'Ivoire",
                SeoTitleDefaut = "NÉOVORE - Solutions écologiques & industrielles",
                SeoDescriptionDefaut = "Installation électrique, groupes électrogènes, cuisine haut de gamme, froid industriel, audit énergétique.",
                AnneesExperience = "+10",
                ProjetsRealises = "500+",
                SatisfactionClient = "100%",
                CouleurPrimaire = "#FF6B35",
                CouleurSecondaire = "#E63946",
                CouleurAccent = "#1E88E5",
                CouleurOr = "#dbb438",
                ModeSombre = false
            });
        }

        // Admin user
        if (!await _db.AdminUsers.AnyAsync())
        {
            _db.AdminUsers.Add(new AdminUser
            {
                Username = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                Role = "Admin",
                IsActive = true
            });
        }

        // Demo content (minimal)
        if (!await _db.Services.AnyAsync())
        {
            _db.Services.AddRange(
                new ServiceOffert { Titre = "Installation électrique", Slug = "installation-electrique", Resume = "Conception et réalisation d’installations fiables.", ContenuHtml = "<p>Études, installation, mise en conformité.</p>", Ordre = 1 },
                new ServiceOffert { Titre = "Froid industriel", Slug = "froid-industriel", Resume = "Solutions de réfrigération pour professionnels.", ContenuHtml = "<p>Chambres froides, vitrines, maintenance.</p>", Ordre = 2 },
                new ServiceOffert { Titre = "Audit énergétique", Slug = "audit-energetique", Resume = "Optimisation de la consommation et réduction des coûts.", ContenuHtml = "<p>Analyse + recommandations concrètes.</p>", Ordre = 3 }
            );
        }

        if (!await _db.CategoriesProduits.AnyAsync())
        {
            var cat1 = new CategorieProduit { Nom = "Groupes électrogènes", Slug = "groupes-electrogenes", Ordre = 1 };
            var cat2 = new CategorieProduit { Nom = "Équipements frigorifiques", Slug = "equipements-frigorifiques", Ordre = 2 };
            _db.CategoriesProduits.AddRange(cat1, cat2);

            _db.Produits.AddRange(
                new Produit { Nom = "Groupe électrogène 20 KVA", Slug = "groupe-electrogene-20kva", Description = "Solution robuste pour sites sensibles.", Specifications = "Puissance: 20 KVA\nDémarrage: automatique", Categorie = cat1, Prix = null, EstPublie = true },
                new Produit { Nom = "Chambre froide positive", Slug = "chambre-froide-positive", Description = "Stockage alimentaire/médical.", Specifications = "Température: 0 à 4°C\nIsolation: panneaux sandwich", Categorie = cat2, Prix = null, EstPublie = true }
            );
        }

        if (!await _db.Realisations.AnyAsync())
        {
            _db.Realisations.Add(new Realisation
            {
                Titre = "Cuisine pro – Hôtel (exemple)",
                Slug = "cuisine-pro-hotel-exemple",
                Secteur = "Hôtellerie",
                Description = "Installation d’une cuisine haut de gamme et mise à niveau électrique.",
                EstPublie = true
            });
        }

        await _db.SaveChangesAsync();
    }
}
