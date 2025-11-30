using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Projekt_razvoj.Modeli;
using Projekt_razvoj.Storitve;

namespace Projekt_razvoj.Pages.Dogodki;

public class DodajModel : PageModel
{
    private readonly IDogodkiRepository _repo;
    public DodajModel(IDogodkiRepository repo) => _repo = repo;

    [BindProperty] public Dogodek Input { get; set; } = new();
    public string? ErrorMessage { get; private set; }

    public void OnGet() { }

    public IActionResult OnPost()
    {
        if (string.IsNullOrWhiteSpace(Input.Naslov) || string.IsNullOrWhiteSpace(Input.Vrsta))
        {
            ErrorMessage = "Naslov in vrsta sta obvezna.";
            return Page();
        }
        Input = Input with { Id = Guid.NewGuid() };
        _repo.Dodaj(Input);
        return RedirectToPage("Podrobnosti", new { id = Input.Id });
    }
}