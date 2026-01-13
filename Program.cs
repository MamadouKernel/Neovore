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
builder.Services.AddHttpClient<IGeocodingService, GeocodingService>();
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

    // Ajouter les colonnes manquantes si elles n'existent pas (fix pour migrations manquantes)
    try
    {
        await db.Database.ExecuteSqlRawAsync(@"
            DO $$ 
            BEGIN 
                -- LogoUrl
                IF NOT EXISTS (
                    SELECT 1 FROM information_schema.columns 
                    WHERE table_name = 'SiteSettings' AND column_name = 'LogoUrl'
                ) THEN
                    ALTER TABLE ""SiteSettings"" ADD COLUMN ""LogoUrl"" text NULL;
                END IF;
                
                -- Couleurs
                IF NOT EXISTS (
                    SELECT 1 FROM information_schema.columns 
                    WHERE table_name = 'SiteSettings' AND column_name = 'CouleurPrimaire'
                ) THEN
                    ALTER TABLE ""SiteSettings"" ADD COLUMN ""CouleurPrimaire"" text NOT NULL DEFAULT '#FF6B35';
                END IF;
                
                IF NOT EXISTS (
                    SELECT 1 FROM information_schema.columns 
                    WHERE table_name = 'SiteSettings' AND column_name = 'CouleurSecondaire'
                ) THEN
                    ALTER TABLE ""SiteSettings"" ADD COLUMN ""CouleurSecondaire"" text NOT NULL DEFAULT '#E63946';
                END IF;
                
                IF NOT EXISTS (
                    SELECT 1 FROM information_schema.columns 
                    WHERE table_name = 'SiteSettings' AND column_name = 'CouleurAccent'
                ) THEN
                    ALTER TABLE ""SiteSettings"" ADD COLUMN ""CouleurAccent"" text NOT NULL DEFAULT '#1E88E5';
                END IF;
                
                IF NOT EXISTS (
                    SELECT 1 FROM information_schema.columns 
                    WHERE table_name = 'SiteSettings' AND column_name = 'CouleurOr'
                ) THEN
                    ALTER TABLE ""SiteSettings"" ADD COLUMN ""CouleurOr"" text NOT NULL DEFAULT '#dbb438';
                END IF;
                
                -- ModeSombre
                IF NOT EXISTS (
                    SELECT 1 FROM information_schema.columns 
                    WHERE table_name = 'SiteSettings' AND column_name = 'ModeSombre'
                ) THEN
                    ALTER TABLE ""SiteSettings"" ADD COLUMN ""ModeSombre"" boolean NOT NULL DEFAULT false;
                END IF;
                
                -- GoogleMapsUrl
                IF NOT EXISTS (
                    SELECT 1 FROM information_schema.columns 
                    WHERE table_name = 'SiteSettings' AND column_name = 'GoogleMapsUrl'
                ) THEN
                    ALTER TABLE ""SiteSettings"" ADD COLUMN ""GoogleMapsUrl"" text NULL;
                END IF;
                
                -- GoogleMapsIframe
                IF NOT EXISTS (
                    SELECT 1 FROM information_schema.columns 
                    WHERE table_name = 'SiteSettings' AND column_name = 'GoogleMapsIframe'
                ) THEN
                    ALTER TABLE ""SiteSettings"" ADD COLUMN ""GoogleMapsIframe"" text NULL;
                END IF;
                
                -- Géolocalisation
                IF NOT EXISTS (
                    SELECT 1 FROM information_schema.columns 
                    WHERE table_name = 'SiteSettings' AND column_name = 'AfficherCarte'
                ) THEN
                    ALTER TABLE ""SiteSettings"" ADD COLUMN ""AfficherCarte"" boolean NOT NULL DEFAULT false;
                END IF;
                
                IF NOT EXISTS (
                    SELECT 1 FROM information_schema.columns 
                    WHERE table_name = 'SiteSettings' AND column_name = 'Latitude'
                ) THEN
                    ALTER TABLE ""SiteSettings"" ADD COLUMN ""Latitude"" numeric NULL;
                END IF;
                
                IF NOT EXISTS (
                    SELECT 1 FROM information_schema.columns 
                    WHERE table_name = 'SiteSettings' AND column_name = 'Longitude'
                ) THEN
                    ALTER TABLE ""SiteSettings"" ADD COLUMN ""Longitude"" numeric NULL;
                END IF;
            END $$;
        ");
    }
    catch
    {
        // Ignorer si les colonnes existent déjà ou en cas d'erreur
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
