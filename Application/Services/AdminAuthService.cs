using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Neovore.Web.Domain.Entities;
using Neovore.Web.Infrastructure.Repositories;

namespace Neovore.Web.Application.Services;

public class AdminAuthService
{
    private readonly IGenericRepository<AdminUser> _repo;

    public AdminAuthService(IGenericRepository<AdminUser> repo)
    {
        _repo = repo;
    }

    public Task<AdminUser?> FindByUsernameAsync(string username)
        => _repo.Query().FirstOrDefaultAsync(u => u.Username == username && u.IsActive);

    public bool VerifyPassword(AdminUser user, string password)
        => BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
}
