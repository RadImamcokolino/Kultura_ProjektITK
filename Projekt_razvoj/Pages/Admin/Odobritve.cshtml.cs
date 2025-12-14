using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Projekt_razvoj.Storitve;

namespace Projekt_razvoj.Pages.Admin;

[Authorize(Roles = "Admin")]
public class OdobritveModel : PageModel
{
    private readonly UporabnikiStoritev _users;
    public OdobritveModel(UporabnikiStoritev users) => _users = users;

    public IReadOnlyCollection<string> Pending { get; private set; } = Array.Empty<string>();
    public IReadOnlyCollection<string> Organizatorji { get; private set; } = Array.Empty<string>();

    public void OnGet()
    {
        Pending = _users.PridobiPendingOrganizatorje();
        Organizatorji = _users.PridobiOrganizatorje();
    }

    public IActionResult OnPostOdobri(string email)
    {
        if (!string.IsNullOrWhiteSpace(email))
        {
            _users.OdobriOrganizatorja(email);
        }
        return RedirectToPage();
    }

    public IActionResult OnPostRazveljavi(string email)
    {
        if (!string.IsNullOrWhiteSpace(email))
        {
            _users.RazveljaviOrganizatorja(email);
        }
        return RedirectToPage();
    }
}