using Neovore.Web.Domain.Entities;
using Neovore.Web.Infrastructure.Repositories;

namespace Neovore.Web.Application.Services;

public class ContactService
{
    private readonly IGenericRepository<ContactMessage> _repo;

    public ContactService(IGenericRepository<ContactMessage> repo)
    {
        _repo = repo;
    }

    public async Task CreerAsync(ContactMessage msg)
    {
        await _repo.AddAsync(msg);
        await _repo.SaveChangesAsync();
    }
}
