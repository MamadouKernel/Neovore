using System.ComponentModel.DataAnnotations;

namespace Neovore.Web.ViewModels;

public class DemandeDevisVm
{
    [Required] public string Nom { get; set; } = "";
    [Required] public string Societe { get; set; } = "";
    [Required] public string Telephone { get; set; } = "";
    [Required, EmailAddress] public string Email { get; set; } = "";
    [Required] public string Secteur { get; set; } = "";
    [Required] public string TypeBesoin { get; set; } = "";
    [Required] public string Message { get; set; } = "";

    // honeypot anti-spam
    public string? Website { get; set; }
}
