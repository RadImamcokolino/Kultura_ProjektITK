using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Projekt_razvoj.Modeli;
using Projekt_razvoj.Storitve;

namespace Projekt_razvoj.Pages.Dogodki;

public class DodajModel : PageModel
{
    private readonly IDogodkiRepository _repo;
    public DodajModel(IDogodkiRepository repo) => _repo = repo;

    // Dropdown možnosti za "Vrsta"
    public IReadOnlyList<string> MozneVrste { get; } = new[] { "koncert", "razstava", "gledalisce" };

    [BindProperty] public Dogodek Input { get; set; } = new()
    {
        Priljubljenost = 50 // sredina sliderja kot privzeto
    };

    public string? ErrorMessage { get; private set; }

    public void OnGet() { }

    public IActionResult OnPost()
    {
        if (string.IsNullOrWhiteSpace(Input.Naslov) || string.IsNullOrWhiteSpace(Input.Vrsta))
        {
            ErrorMessage = "Naslov in vrsta sta obvezna.";
            return Page();
        }

        // Preveri izbrano vrsto
        if (!MozneVrste.Contains(Input.Vrsta, StringComparer.OrdinalIgnoreCase))
        {
            ErrorMessage = "Izberite veljavno vrsto dogodka.";
            return Page();
        }

        // Omejitev sliderja (varnostna)
        var priljubljenost = Math.Clamp(Input.Priljubljenost, 1, 100);

        // Ustvari novo instanco (brez 'with') in skopiraj vrednosti
        var novo = new Dogodek
        {
            Id = Guid.NewGuid(),
            Naslov = Input.Naslov,
            Vrsta = Input.Vrsta,
            Lokacija = Input.Lokacija,
            Zacetek = Input.Zacetek,
            Cena = Input.Cena,
            Priljubljenost = priljubljenost
        };

        _repo.Dodaj(novo);
        return RedirectToPage("Podrobnosti", new { id = novo.Id });
    }
}