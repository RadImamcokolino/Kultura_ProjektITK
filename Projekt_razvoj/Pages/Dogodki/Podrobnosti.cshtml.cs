using Microsoft.AspNetCore.Mvc.RazorPages;
using Projekt_razvoj.Modeli;
using Projekt_razvoj.Storitve;

namespace Projekt_razvoj.Pages.Dogodki;

public class PodrobnostiModel : PageModel
{
    private readonly IDogodkiRepository _repo;
    public PodrobnostiModel(IDogodkiRepository repo) => _repo = repo;

    public Dogodek? Dogodek { get; private set; }

    public void OnGet(Guid id) => Dogodek = _repo.Pridobi(id);
}