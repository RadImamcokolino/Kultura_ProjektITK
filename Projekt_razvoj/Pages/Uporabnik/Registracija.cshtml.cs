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
    public string? Sporocilo { get; private set; }

    public void OnGet() { }

    public void OnPost()
    {
        if (!_preverjevalnik.JeEpostaVeljavna(Email))
            Sporocilo = "Neveljavna e-pošta.";
        else if (!_preverjevalnik.JeGesloMocno(Geslo))
            Sporocilo = "Geslo ni dovolj moèno.";
        else
            Sporocilo = "Uspešna (demo) registracija.";
    }
}