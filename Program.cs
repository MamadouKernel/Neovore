using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Neovore.Web.Application.Services;
using Neovore.Web.Infrastructure.Data;
using Neovore.Web.Infrastructure.Email;
using Neovore.Web.Infrastructure.Middleware;
using Neovore.Web.Infrastructure.Repositories;
using Neovore.Web.Infrastructure.Storage;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();

// Session (pour le tracking des visites uniques)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(24);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Database (PostgreSQL)
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Auth (cookie) - dashboard admin
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Admin/Account/Login";
        options.AccessDeniedPath = "/Admin/Account/AccessDenied";
        options.Cookie.Name = "Neovore.Auth";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("EditorOrAdmin", policy => policy.RequireRole("Admin", "Editeur"));
});

// Infrastructure
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IFileStorage, LocalFileStorage>();
builder.Services.AddScoped<IEmailSender, SmtpEmailSender>();

// Repos
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Application services
builder.Services.AddScoped<SiteSeedService>();
builder.Services.AddScoped<DevisService>();
builder.Services.AddScoped<ContactService>();
builder.Services.AddScoped<CatalogueService>();
builder.Services.AddScoped<RealisationsService>();
builder.Services.AddScoped<ServicesService>();
builder.Services.AddScoped<AdminAuthService>();
builder.Services.AddScoped<SettingsService>();
        builder.Services.AddScoped<CommentairesService>();
        builder.Services.AddScoped<PartenairesService>();
        builder.Services.AddScoped<EquipeService>();
        builder.Services.AddScoped<AnalyticsService>();

var app = builder.Build();

// Migrate + seed (V1 simple)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    // Ajouter la colonne LogoUrl si elle n'existe pas (fix pour migration manquante)
    try
    {
        await db.Database.ExecuteSqlRawAsync(@"
            DO $$ 
            BEGIN 
                IF NOT EXISTS (
                    SELECT 1 FROM information_schema.columns 
                    WHERE table_name = 'SiteSettings' AND column_name = 'LogoUrl'
                ) THEN
                    ALTER TABLE ""SiteSettings"" ADD COLUMN ""LogoUrl"" text NULL;
                END IF;
            END $$;
        ");
    }
    catch
    {
        // Ignorer si la colonne existe déjà ou en cas d'erreur
    }

    var seeder = scope.ServiceProvider.GetRequiredService<SiteSeedService>();
    await seeder.SeedAsync();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession(); // Nécessaire pour le tracking des visites uniques
app.UseRouting();

app.UseMiddleware<AnalyticsMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

// Areas (Admin)
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

// Default routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
