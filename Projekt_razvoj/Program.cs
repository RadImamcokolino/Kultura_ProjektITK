using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

// --------------------
// KESTREL – samo za Production (Docker / Azure)
// --------------------
if (!builder.Environment.IsDevelopment())
{
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ListenAnyIP(8080);
    });
}

// --------------------
// SERVICES
// --------------------

builder.Services.AddRazorPages();

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 10 * 1024 * 1024;
    options.ValueLengthLimit = int.MaxValue;
    options.MultipartHeadersLengthLimit = int.MaxValue;
});

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
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("OrganizatorOnly", p => p.RequireRole("Organizator"));
    options.AddPolicy("AdminOnly", p => p.RequireRole("Admin"));
});

builder.Services.AddSingleton<Projekt_razvoj.Storitve.PreverjevalnikGesel>();
builder.Services.AddSingleton<Projekt_razvoj.Storitve.IskanjeDogodkovStoritev>();
builder.Services.AddSingleton<Projekt_razvoj.Storitve.PriljubljeniStoritev>();
builder.Services.AddSingleton<Projekt_razvoj.Storitve.IDogodkiRepository, Projekt_razvoj.Storitve.DogodkiRepository>();
builder.Services.AddSingleton<Projekt_razvoj.Storitve.UporabnikiStoritev>();
builder.Services.AddSingleton<Projekt_razvoj.Storitve.OceneStoritev>();

var app = builder.Build();

// --------------------
// MIDDLEWARE
// --------------------

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();

app.Run();
