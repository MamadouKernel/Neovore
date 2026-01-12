namespace Neovore.Web.Domain.Entities;

public class SiteSettings : BaseEntity
{
    public string NomEntreprise { get; set; } = "NÉOVORE";
    public string? LogoUrl { get; set; }
    public string EmailContact { get; set; } = "";
    public string TelephoneContact { get; set; } = "";
    public string Adresse { get; set; } = "";
    public string? FacebookUrl { get; set; }
    public string? LinkedInUrl { get; set; }
    public string? TwitterUrl { get; set; }
    public string? InstagramUrl { get; set; }
    public string? YouTubeUrl { get; set; }
    public string? TikTokUrl { get; set; }
    public string SeoTitleDefaut { get; set; } = "NÉOVORE - Solutions écologiques & industrielles";
    public string SeoDescriptionDefaut { get; set; } = "Installation électrique, groupes électrogènes, cuisine haut de gamme, froid industriel et audit énergétique.";
    
    // Statistiques pour la page d'accueil
    public string AnneesExperience { get; set; } = "+10";
    public string ProjetsRealises { get; set; } = "500+";
    public string SatisfactionClient { get; set; } = "100%";
}
