using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Projekt_razvoj.Storitve;

namespace Projekt_razvoj.Pages.Uporabnik;

public class RegistracijaModel : PageModel
{
    private readonly PreverjevalnikGesel _preverjevalnik;
    public RegistracijaModel(PreverjevalnikGesel preverjevalnik) => _preverjevalnik = preverjevalnik;

    [BindProperty] public string Email { get; set; } = string.Empty;
    [BindProperty] public string Geslo { get; set; } = string.Empty;
    [BindProperty] public string Role { get; set; } = "Uporabnik"; // "Uporabnik" or "Organizator"
    public string? Sporocilo { get; private set; }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        // Basic validation (demo)
        if (!_preverjevalnik.JeEpostaVeljavna(Email) || !_preverjevalnik.JeGesloMocno(Geslo))
        {
            Sporocilo = "Neveljavna e-pošta ali šibko geslo.";
            return Page();
        }

        // Create claims and sign-in (demo: no persistent user store)
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, Email),
            new Claim(ClaimTypes.Role, Role)
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        Sporocilo = $"Uspešna (demo) prijava kot {Role}.";
        return Page();
    }

    public async Task<IActionResult> OnPostLogoutAsync()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToPage();
    }
}