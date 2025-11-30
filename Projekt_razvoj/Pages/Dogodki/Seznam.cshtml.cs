using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Projekt_razvoj.Modeli;
using Projekt_razvoj.Storitve;

namespace Projekt_razvoj.Pages.Dogodki;

public class SeznamModel : PageModel
{
    private readonly IDogodkiRepository _repo;
    private readonly IskanjeDogodkovStoritev _iskanje;
    private readonly PriljubljeniStoritev _priljubljeni;

    public SeznamModel(IDogodkiRepository repo, IskanjeDogodkovStoritev iskanje, PriljubljeniStoritev priljubljeni)
    {
        _repo = repo;
        _iskanje = iskanje;
        _priljubljeni = priljubljeni;
    }

    public IEnumerable<Dogodek> Dogodki { get; private set; } = [];
    [BindProperty(SupportsGet = true)] public string? Lokacija { get; set; }
    [BindProperty(SupportsGet = true)] public string? Vrsta { get; set; }
    [BindProperty(SupportsGet = true)] public bool Brezplacni { get; set; }

    public void OnGet()
    {
        var all = _repo.PridobiVse();
        if (!string.IsNullOrWhiteSpace(Lokacija))
            all = _iskanje.FiltrirajPoLokaciji(all, Lokacija);
        if (!string.IsNullOrWhiteSpace(Vrsta))
            all = _iskanje.FiltrirajPoVrsti(all, Vrsta);
        if (Brezplacni)
            all = _iskanje.FiltrirajBrezplacne(all);
        Dogodki = _iskanje.UrediPoDatumu(all);
    }

    public IActionResult OnPostPriljubljen(Guid id)
    {
        var d = _repo.Pridobi(id);
        if (d is null) return NotFound();
        _priljubljeni.Dodaj("demoUser", id); // demo user
        return RedirectToPage(new { Lokacija, Vrsta, Brezplacni });
    }
}