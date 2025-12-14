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

    [BindProperty] public string Naslov { get; set; } = string.Empty;
    [BindProperty] public string Vrsta { get; set; } = string.Empty;
    [BindProperty] public string Lokacija { get; set; } = string.Empty;
    [BindProperty] public DateTime Zacetek { get; set; } = DateTime.Now;
    [BindProperty] public decimal? Cena { get; set; }
    [BindProperty] public int Priljubljenost { get; set; }

    // Dropdown možnosti za "Vrsta"
    public IReadOnlyList<string> MozneVrste { get; } = new[] { "koncert", "razstava", "gledalisce" };

    public string? ErrorMessage { get; private set; }

    public void OnGet() { }

    public IActionResult OnPost()
    {
        if (string.IsNullOrWhiteSpace(Naslov) || string.IsNullOrWhiteSpace(Vrsta) || string.IsNullOrWhiteSpace(Lokacija))
            return Page();

        // Preveri izbrano vrsto
        if (!MozneVrste.Contains(Vrsta, StringComparer.OrdinalIgnoreCase))
        {
            ErrorMessage = "Izberite veljavno vrsto dogodka.";
            return Page();
        }

        // Omejitev sliderja (varnostna)
        var priljubljenost = Math.Clamp(Priljubljenost, 1, 100);

        var d = new Dogodek
        {
            Naslov = Naslov,
            Vrsta = Vrsta,
            Lokacija = Lokacija,
            Zacetek = Zacetek,
            Cena = Cena,
            Priljubljenost = Priljubljenost
        };

        _repo.Dodaj(d);
        return RedirectToPage("Seznam");
    }
}