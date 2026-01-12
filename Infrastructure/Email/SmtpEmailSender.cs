using Microsoft.Extensions.Options;

namespace Neovore.Web.Infrastructure.Email;

public interface IEmailSender
{
    Task SendAsync(string to, string subject, string htmlBody, CancellationToken ct);
}

public class SmtpOptions
{
    public string Host { get; set; } = "";
    public int Port { get; set; } = 587;
    public string User { get; set; } = "";
    public string Password { get; set; } = "";
    public bool UseSsl { get; set; } = true;
}

public class SmtpEmailSender : IEmailSender
{
    private readonly IConfiguration _config;

    public SmtpEmailSender(IConfiguration config)
    {
        _config = config;
    }

    public Task SendAsync(string to, string subject, string htmlBody, CancellationToken ct)
    {
        // V1: Stub volontaire (évite de casser en dev)
        // Branche SMTP réel en prod (voir README).
        return Task.CompletedTask;
    }
}
