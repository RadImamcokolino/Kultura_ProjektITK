using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Projekt_razvoj.Storitve;

namespace Projekt_razvoj.Pages.Uporabnik;

public class PrijavaModel : PageModel
{
    private readonly UporabnikiStoritev _users;
    private readonly PreverjevalnikGesel _preverjevalnik;

    public PrijavaModel(UporabnikiStoritev users, PreverjevalnikGesel preverjevalnik)
    {
        _users = users;
        _preverjevalnik = preverjevalnik;
    }

    [BindProperty] public string Email { get; set; } = string.Empty;
    [BindProperty] public string Geslo { get; set; } = string.Empty;
    public string? Sporocilo { get; private set; }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!_preverjevalnik.JeEpostaVeljavna(Email))
        {
            Sporocilo = "Neveljavna e-pošta.";
            return Page();
        }

        var u = _users.Najdi(Email);
        if (u is null || !_users.PreveriGeslo(Email, Geslo))
        {
            Sporocilo = "Napaèna e-pošta ali geslo.";
            return Page();
        }

        var principal = _users.UstvariPrincipal(u);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        return RedirectToPage("/Index");
    }

    public async Task<IActionResult> OnPostLogoutAsync()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToPage();
    }
}