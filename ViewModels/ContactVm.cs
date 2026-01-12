using System.ComponentModel.DataAnnotations;

namespace Neovore.Web.ViewModels;

public class ContactVm
{
    [Required] public string Nom { get; set; } = "";
    [Required, EmailAddress] public string Email { get; set; } = "";
    public string Telephone { get; set; } = "";
    [Required] public string Sujet { get; set; } = "";
    [Required] public string Message { get; set; } = "";

    // honeypot
    public string? Website { get; set; }
}
