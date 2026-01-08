using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Projekt_razvoj.Modeli;
using Projekt_razvoj.Storitve;
using System.ComponentModel.DataAnnotations;

namespace Projekt_razvoj.Pages.Dogodki;

public class PodrobnostiModel : PageModel
{
    private readonly IDogodkiRepository _repo;
    private readonly OceneStoritev _ocene;

    public PodrobnostiModel(IDogodkiRepository repo, OceneStoritev ocene)
    {
        _repo = repo;
        _ocene = ocene;
    }

    public Dogodek? Dogodek { get; private set; }
    public IEnumerable<Ocena> Ocene { get; private set; } = Enumerable.Empty<Ocena>();
    public double? PovprecnaOcena { get; private set; }
    public int StOcen { get; private set; }
    public bool JeOcenil { get; private set; }

    [BindProperty]
    [Range(1, 5, ErrorMessage = "Izberite oceno od 1 do 5 zvezdic")]
    public int Zvezdice { get; set; }

    [BindProperty]
    [StringLength(500, ErrorMessage = "Komentar lahko vsebuje najveè 500 znakov")]
    public string Komentar { get; set; } = string.Empty;

    public void OnGet(Guid id)
    {
        Dogodek = _repo.Pridobi(id);
        if (Dogodek is not null)
        {
            NaloziOcene(id);
        }
    }

    public IActionResult OnPostDodajOceno(Guid id)
    {
        Dogodek = _repo.Pridobi(id);
        if (Dogodek is null)
            return NotFound();

        if (!ModelState.IsValid)
        {
            NaloziOcene(id);
            return Page();
        }

        var uporabnik = User?.Identity?.Name ?? "demoUser";
        _ocene.DodajOceno(id, uporabnik, Zvezdice, Komentar);

        return RedirectToPage(new { id });
    }

    private void NaloziOcene(Guid dogodekId)
    {
        Ocene = _ocene.PridobiOceneDogodka(dogodekId);
        PovprecnaOcena = _ocene.IzracunajPovprecnoOceno(dogodekId);
        StOcen = _ocene.PridobiStOcen(dogodekId);

        var uporabnik = User?.Identity?.Name ?? "demoUser";
        JeOcenil = _ocene.JeUporabnikOcenil(dogodekId, uporabnik);

        if (JeOcenil)
        {
            var obstojeca = _ocene.PridobiOcenoUporabnika(dogodekId, uporabnik);
            if (obstojeca is not null)
            {
                Zvezdice = obstojeca.Zvezdice;
                Komentar = obstojeca.Komentar;
            }
        }
    }
}