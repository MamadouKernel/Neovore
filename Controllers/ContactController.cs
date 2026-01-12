using Microsoft.AspNetCore.Mvc;
using Neovore.Web.Application.Services;
using Neovore.Web.Domain.Entities;
using Neovore.Web.ViewModels;

namespace Neovore.Web.Controllers;

public class ContactController : Controller
{
    private readonly ContactService _service;

    public ContactController(ContactService service)
    {
        _service = service;
    }

    [HttpGet("/contact")]
    public IActionResult Index() => View(new ContactVm());

    [HttpPost("/contact")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(ContactVm vm)
    {
        if (!string.IsNullOrWhiteSpace(vm.Website))
        {
            return RedirectToAction("Success");
        }

        if (!ModelState.IsValid) return View(vm);

        var entity = new ContactMessage
        {
            Nom = vm.Nom,
            Email = vm.Email,
            Telephone = vm.Telephone,
            Sujet = vm.Sujet,
            Message = vm.Message,
            Statut = "Nouveau"
        };

        await _service.CreerAsync(entity);
        return RedirectToAction("Success");
    }

    [HttpGet("/contact/success")]
    public IActionResult Success() => View();
}
