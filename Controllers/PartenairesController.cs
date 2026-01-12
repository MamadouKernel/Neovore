using Microsoft.AspNetCore.Mvc;
using Neovore.Web.Application.Services;

namespace Neovore.Web.Controllers;

public class PartenairesController : Controller
{
    private readonly PartenairesService _service;

    public PartenairesController(PartenairesService service)
    {
        _service = service;
    }

    [HttpGet("/partenaires")]
    public async Task<IActionResult> Index()
    {
        var partenaires = await _service.GetPubliesAsync();
        return View(partenaires);
    }
}

