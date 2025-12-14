using Microsoft.AspNetCore.Mvc.RazorPages;
using Projekt_razvoj.Storitve;
using Projekt_razvoj.Modeli;

namespace Projekt_razvoj.Pages.Priljubljeni;

public class IndexModel : PageModel
{
    private readonly PriljubljeniStoritev _priljubljeni;
    private readonly IDogodkiRepository _repo;

    public IndexModel(PriljubljeniStoritev priljubljeni, IDogodkiRepository repo)
    {
        _priljubljeni = priljubljeni;
        _repo = repo;
    }

    public IEnumerable<Dogodek> Dogodki { get; private set; } = Enumerable.Empty<Dogodek>();

    public void OnGet()
    {
        // Use logged-in user name if available; fallback to demoUser
        var user = User?.Identity?.Name ?? "demoUser";

        var ids = _priljubljeni.PridobiVse(user);
        Dogodki = ids
            .Select(id => _repo.Pridobi(id))
            .Where(d => d is not null)!
            .Cast<Dogodek>();
    }
}