using Microsoft.AspNetCore.Mvc;

namespace Neovore.Web.Controllers;

public class LegalController : Controller
{
    [HttpGet("/mentions-legales")]
    public IActionResult MentionsLegales()
    {
        ViewData["Title"] = "Mentions légales - NÉOVORE";
        return View();
    }

    [HttpGet("/politique-de-confidentialite")]
    public IActionResult PolitiqueConfidentialite()
    {
        ViewData["Title"] = "Politique de confidentialité - NÉOVORE";
        return View();
    }
}

