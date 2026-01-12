using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Neovore.Web.Domain.Entities;
using Neovore.Web.Infrastructure.Data;

namespace Neovore.Web.Application.Services;

public class AnalyticsService
{
    private readonly AppDbContext _db;

    public AnalyticsService(AppDbContext db)
    {
        _db = db;
    }

    public async Task EnregistrerVisiteAsync(string? ipAddress, string? userAgent, string url, string? referrer, string? controller, string? action, bool estUnique = false)
    {
        var visite = new Visite
        {
            IpAddress = ipAddress,
            UserAgent = userAgent,
            Url = url,
            Referrer = referrer,
            Controller = controller,
            Action = action,
            DateVisite = DateTime.UtcNow,
            EstUnique = estUnique
        };

        _db.Visites.Add(visite);
        await _db.SaveChangesAsync();
    }

    public async Task EnregistrerRechercheAsync(string terme, string? ipAddress, int? resultatsCount = null)
    {
        var recherche = new Recherche
        {
            Terme = terme,
            IpAddress = ipAddress,
            DateRecherche = DateTime.UtcNow,
            ResultatsCount = resultatsCount
        };

        _db.Recherches.Add(recherche);
        await _db.SaveChangesAsync();
    }

    // Statistiques globales - récupération directe depuis la base de données
    public async Task<AnalyticsStatsVm> GetStatsAsync(DateTime? dateDebut = null, DateTime? dateFin = null)
    {
        // S'assurer que les dates sont en UTC
        var debut = dateDebut.HasValue 
            ? (dateDebut.Value.Kind == DateTimeKind.Utc ? dateDebut.Value : dateDebut.Value.ToUniversalTime())
            : DateTime.UtcNow.AddDays(-30);
        
        var fin = dateFin.HasValue
            ? (dateFin.Value.Kind == DateTimeKind.Utc ? dateFin.Value : dateFin.Value.ToUniversalTime())
            : DateTime.UtcNow;

        // S'assurer que les dates sont vraiment en UTC
        if (debut.Kind != DateTimeKind.Utc) debut = DateTime.SpecifyKind(debut, DateTimeKind.Utc);
        if (fin.Kind != DateTimeKind.Utc) fin = DateTime.SpecifyKind(fin, DateTimeKind.Utc);

        // Récupération directe depuis la base de données via AppDbContext
        var visitesQuery = _db.Visites.Where(v => v.DateVisite >= debut && v.DateVisite <= fin);
        var recherchesQuery = _db.Recherches.Where(r => r.DateRecherche >= debut && r.DateRecherche <= fin);

        var stats = new AnalyticsStatsVm
        {
            TotalVisites = await visitesQuery.CountAsync(),
            VisitesUniques = await visitesQuery.Where(v => v.EstUnique).CountAsync(),
            TotalRecherches = await recherchesQuery.CountAsync(),
            PagesPopulaires = await visitesQuery
                .Where(v => !string.IsNullOrEmpty(v.Url))
                .GroupBy(v => v.Url)
                .Select(g => new PageVisiteVm { Url = g.Key!, Count = g.Count() })
                .OrderByDescending(p => p.Count)
                .Take(10)
                .ToListAsync(),
            RecherchesPopulaires = await recherchesQuery
                .GroupBy(r => r.Terme)
                .Select(g => new RecherchePopulaireVm { Terme = g.Key, Count = g.Count() })
                .OrderByDescending(r => r.Count)
                .Take(10)
                .ToListAsync(),
            VisitesParJour = (await visitesQuery
                .Select(v => v.DateVisite)
                .ToListAsync())
                .GroupBy(v => v.Date)
                .Select(g => new VisiteParJourVm { Date = g.Key, Count = g.Count() })
                .OrderBy(v => v.Date)
                .ToList()
        };

        return stats;
    }
}

// ViewModels pour les stats
public class AnalyticsStatsVm
{
    public int TotalVisites { get; set; }
    public int VisitesUniques { get; set; }
    public int TotalRecherches { get; set; }
    public List<PageVisiteVm> PagesPopulaires { get; set; } = new();
    public List<RecherchePopulaireVm> RecherchesPopulaires { get; set; } = new();
    public List<VisiteParJourVm> VisitesParJour { get; set; } = new();
}

public class PageVisiteVm
{
    public string Url { get; set; } = "";
    public int Count { get; set; }
}

public class RecherchePopulaireVm
{
    public string Terme { get; set; } = "";
    public int Count { get; set; }
}

public class VisiteParJourVm
{
    public DateTime Date { get; set; }
    public int Count { get; set; }
}


