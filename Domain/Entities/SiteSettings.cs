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
    
    // Couleurs personnalisables du site
    public string CouleurPrimaire { get; set; } = "#FF6B35";      // Orange principal
    public string CouleurSecondaire { get; set; } = "#E63946";    // Rouge
    public string CouleurAccent { get; set; } = "#1E88E5";        // Bleu
    public string CouleurOr { get; set; } = "#dbb438";            // Or (selon charte graphique)
    
    // Mode d'affichage
    public bool ModeSombre { get; set; } = false;                 // Mode sombre activé ou non
    
    // Géolocalisation pour la carte
    public string? GoogleMapsIframe { get; set; }                 // Code iframe Google Maps (optionnel - méthode recommandée)
    public string? GoogleMapsUrl { get; set; }                    // Lien Google Maps (optionnel - pour extraction coordonnées)
    public decimal? Latitude { get; set; }                        // Latitude (extraite depuis le lien Google Maps ou géocodée depuis l'adresse)
    public decimal? Longitude { get; set; }                       // Longitude (extraite depuis le lien Google Maps ou géocodée depuis l'adresse)
    public bool AfficherCarte { get; set; } = false;              // Afficher la carte sur le site
}
