using System.ComponentModel.DataAnnotations;

namespace Neovore.Web.Areas.Admin.ViewModels;

public class LoginVm
{
    [Required]
    public string Username { get; set; } = "";

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = "";

    public string? ReturnUrl { get; set; }
}
