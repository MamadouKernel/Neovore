using Microsoft.EntityFrameworkCore;
using Neovore.Web.Domain.Entities;

namespace Neovore.Web.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<ServiceOffert> Services => Set<ServiceOffert>();
    public DbSet<CategorieProduit> CategoriesProduits => Set<CategorieProduit>();
    public DbSet<Produit> Produits => Set<Produit>();
    public DbSet<Realisation> Realisations => Set<Realisation>();
    public DbSet<Media> Medias => Set<Media>();
    public DbSet<DemandeDevis> DemandesDevis => Set<DemandeDevis>();
    public DbSet<ContactMessage> ContactMessages => Set<ContactMessage>();
    public DbSet<SiteSettings> SiteSettings => Set<SiteSettings>();
    public DbSet<AdminUser> AdminUsers => Set<AdminUser>();
        public DbSet<Commentaire> Commentaires => Set<Commentaire>();
        public DbSet<Partenaire> Partenaires => Set<Partenaire>();
        public DbSet<MembreEquipe> MembresEquipe => Set<MembreEquipe>();
        public DbSet<Visite> Visites => Set<Visite>();
        public DbSet<Recherche> Recherches => Set<Recherche>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ServiceOffert>()
            .HasIndex(x => x.Slug)
            .IsUnique();

        modelBuilder.Entity<CategorieProduit>()
            .HasIndex(x => x.Slug)
            .IsUnique();

        modelBuilder.Entity<Produit>()
            .HasIndex(x => x.Slug)
            .IsUnique();

        modelBuilder.Entity<Realisation>()
            .HasIndex(x => x.Slug)
            .IsUnique();

        modelBuilder.Entity<Produit>()
            .HasOne(x => x.Categorie)
            .WithMany(c => c.Produits)
            .HasForeignKey(x => x.CategorieProduitId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Media>()
            .HasOne(m => m.Produit)
            .WithMany(p => p.Medias)
            .HasForeignKey(m => m.ProduitId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Media>()
            .HasOne(m => m.Realisation)
            .WithMany(r => r.Medias)
            .HasForeignKey(m => m.RealisationId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relations Commentaire
        modelBuilder.Entity<Commentaire>()
            .HasOne(c => c.ServiceOffert)
            .WithMany(s => s.Commentaires)
            .HasForeignKey(c => c.ServiceOffertId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Commentaire>()
            .HasOne(c => c.Produit)
            .WithMany(p => p.Commentaires)
            .HasForeignKey(c => c.ProduitId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Commentaire>()
            .HasOne(c => c.Realisation)
            .WithMany(r => r.Commentaires)
            .HasForeignKey(c => c.RealisationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
