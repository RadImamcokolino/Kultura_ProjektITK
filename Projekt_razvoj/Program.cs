using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Registracija Razor Pages
builder.Services.AddRazorPages();

// Authentication & Authorization (cookie-based)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Uporabnik/Registracija";
        options.AccessDeniedPath = "/Uporabnik/Registracija";
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("OrganizatorOnly", policy => policy.RequireRole("Organizator"));
});

// Aplikacijske storitve
builder.Services.AddSingleton<Projekt_razvoj.Storitve.PreverjevalnikGesel>();
builder.Services.AddSingleton<Projekt_razvoj.Storitve.IskanjeDogodkovStoritev>();
builder.Services.AddSingleton<Projekt_razvoj.Storitve.PriljubljeniStoritev>();
builder.Services.AddSingleton<Projekt_razvoj.Storitve.IDogodkiRepository, Projekt_razvoj.Storitve.DogodkiRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

// Authentication then Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
