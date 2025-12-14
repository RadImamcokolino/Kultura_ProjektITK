using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Projekt_razvoj.Modeli;
using Projekt_razvoj.Storitve;

namespace Projekt_razvoj.Pages.Dogodki;

[Authorize(Roles = "Organizator")]
public class DodajModel : PageModel
{
    private readonly IDogodkiRepository _repo;

    public DodajModel(IDogodkiRepository repo) => _repo = repo;

    // Seznam podprtih vrst dogodkov za dropdown
    public IReadOnlyList<string> Vrste { get; private set; } = new[] { "koncert", "razstava", "gledalisce" };

    [BindProperty] public string Naslov { get; set; } = string.Empty;
    [BindProperty] public string Vrsta { get; set; } = string.Empty;
    [BindProperty] public string Lokacija { get; set; } = string.Empty;
    [BindProperty] public DateTime Zacetek { get; set; } = DateTime.Now;
    [BindProperty] public decimal? Cena { get; set; }
    [BindProperty] public int Priljubljenost { get; set; } = 50; // privzeto sredina

    public void OnGet()
    {
        // niè posebnega; Vrste so že nastavljene
    }

    public IActionResult OnPost()
    {
        if (string.IsNullOrWhiteSpace(Naslov) || string.IsNullOrWhiteSpace(Vrsta) || string.IsNullOrWhiteSpace(Lokacija))
            return Page();

        var d = new Dogodek
        {
            Naslov = Naslov.Trim(),
            Vrsta = Vrsta.Trim(),
            Lokacija = Lokacija.Trim(),
            Zacetek = Zacetek,
            Cena = Cena,
            Priljubljenost = Priljubljenost
        };

        _repo.Dodaj(d);
        return RedirectToPage("Seznam");
    }
}