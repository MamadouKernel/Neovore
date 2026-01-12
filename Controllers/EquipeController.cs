using Microsoft.AspNetCore.Mvc;
using Neovore.Web.Application.Services;

namespace Neovore.Web.Controllers;

public class EquipeController : Controller
{
    private readonly EquipeService _service;

    public EquipeController(EquipeService service)
    {
        _service = service;
    }

    [HttpGet("/equipe")]
    public async Task<IActionResult> Index()
    {
        var membres = await _service.GetPubliesAsync();
        return View(membres);
    }
}

