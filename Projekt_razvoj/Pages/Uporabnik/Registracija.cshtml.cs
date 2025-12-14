using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Projekt_razvoj.Storitve;

namespace Projekt_razvoj.Pages.Uporabnik;

public class RegistracijaModel : PageModel
{
    private readonly PreverjevalnikGesel _preverjevalnik;
    private readonly UporabnikiStoritev _users;

    public RegistracijaModel(PreverjevalnikGesel preverjevalnik, UporabnikiStoritev users)
    {
        _preverjevalnik = preverjevalnik;
        _users = users;
    }

    [BindProperty] public string Email { get; set; } = string.Empty;
    [BindProperty] public string Geslo { get; set; } = string.Empty;
    [BindProperty] public string Role { get; set; } = "Uporabnik"; // "Uporabnik" or "Organizator"
    [BindProperty] public bool SamoRegistriraj { get; set; } = true;

    public string? Sporocilo { get; private set; }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!_preverjevalnik.JeEpostaVeljavna(Email) || !_preverjevalnik.JeGesloMocno(Geslo))
        {
            Sporocilo = "Neveljavna e-pošta ali šibko geslo.";
            return Page();
        }

        var u = _users.RegistrirajAliPrijavi(Email, Role);
        _users.NastaviGeslo(Email, Geslo);

        if (Role == "Organizator" && u.Role != "Organizator")
            Sporocilo = "Zahteva za organizatorja oddana. Admin mora odobriti.";
        else
            Sporocilo = $"Registracija uspešna. Trenutna vloga: {u.Role}.";

        if (SamoRegistriraj)
        {
            return RedirectToPage("/Uporabnik/Prijava");
        }
        else
        {
            var principal = _users.UstvariPrincipal(u);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            return RedirectToPage("/Index");
        }
    }

    public async Task<IActionResult> OnPostLogoutAsync()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToPage();
    }
}