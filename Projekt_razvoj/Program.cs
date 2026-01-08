using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

// Dinamièen port, èe URL ni podan v konfiguraciji ali okolju
if (string.IsNullOrWhiteSpace(builder.Configuration["ASPNETCORE_URLS"]) &&
    string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("ASPNETCORE_URLS")))
{
    builder.WebHost.UseUrls("http://127.0.0.1:0");
}

// Registracija Razor Pages
builder.Services.AddRazorPages();

// Configure form options for file uploads
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 10 * 1024 * 1024; // 10 MB
    options.ValueLengthLimit = int.MaxValue;
    options.MultipartHeadersLengthLimit = int.MaxValue;
});

// Authentication & Authorization (cookie-based)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Uporabnik/Prijava";
        options.AccessDeniedPath = "/Uporabnik/Prijava";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        options.SlidingExpiration = true;
        options.Cookie.IsEssential = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        options.Cookie.MaxAge = null;
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("OrganizatorOnly", policy => policy.RequireRole("Organizator"));
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});

// Aplikacijske storitve
builder.Services.AddSingleton<Projekt_razvoj.Storitve.PreverjevalnikGesel>();
builder.Services.AddSingleton<Projekt_razvoj.Storitve.IskanjeDogodkovStoritev>();
builder.Services.AddSingleton<Projekt_razvoj.Storitve.PriljubljeniStoritev>();
builder.Services.AddSingleton<Projekt_razvoj.Storitve.IDogodkiRepository, Projekt_razvoj.Storitve.DogodkiRepository>();
builder.Services.AddSingleton<Projekt_razvoj.Storitve.UporabnikiStoritev>();
builder.Services.AddSingleton<Projekt_razvoj.Storitve.OceneStoritev>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
}

// GLOBAL EXCEPTION HANDLER
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "UNHANDLED EXCEPTION: {Message}", ex.Message);
        logger.LogError("Stack Trace: {StackTrace}", ex.StackTrace);
        throw;
    }
});

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.Run();
