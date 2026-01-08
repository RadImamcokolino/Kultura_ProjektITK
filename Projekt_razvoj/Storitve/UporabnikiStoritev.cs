using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Projekt_razvoj.Storitve;

public sealed class UporabnikiStoritev
{
    private readonly Dictionary<string, Uporabnik> _uporabniki = new(StringComparer.OrdinalIgnoreCase);
    private readonly HashSet<string> _pendingOrganizator = new(StringComparer.OrdinalIgnoreCase);

    public UporabnikiStoritev()
    {
        // Seed a default admin (demo)
        var adminEmail = "admin@kultura.local";
        var admin = new Uporabnik { Email = adminEmail, Role = "Admin", PasswordHash = Hash("Admin123!") };
        _uporabniki[adminEmail] = admin;
    }

    public Uporabnik RegistrirajAliPrijavi(string email, string roleRequested)
    {
        if (!_uporabniki.TryGetValue(email, out var u))
        {
            u = new Uporabnik { Email = email, Role = "Uporabnik" };
            _uporabniki[email] = u;
        }

        if (string.Equals(roleRequested, "Organizator", StringComparison.OrdinalIgnoreCase)
            && u.Role is not "Organizator" and not "Admin")
        {
            _pendingOrganizator.Add(email);
        }

        return u;
    }

    // DEMO password hashing
    public void NastaviGeslo(string email, string geslo)
    {
        var u = RegistrirajAliPrijavi(email, "Uporabnik");
        u.PasswordHash = Hash(geslo);
    }

    public bool PreveriGeslo(string email, string geslo)
    {
        return _uporabniki.TryGetValue(email, out var u) && u.PasswordHash is not null && u.PasswordHash == Hash(geslo);
    }

    private static string Hash(string input)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(bytes);
    }

    public IReadOnlyCollection<string> PridobiPendingOrganizatorje() => _pendingOrganizator.ToArray();

    public bool OdobriOrganizatorja(string email)
    {
        if (_pendingOrganizator.Remove(email) && _uporabniki.TryGetValue(email, out var u))
        {
            u.Role = "Organizator";
            return true;
        }
        return false;
    }

    public bool RazveljaviOrganizatorja(string email)
    {
        if (_uporabniki.TryGetValue(email, out var u))
        {
            if (u.Role == "Organizator")
            {
                u.Role = "Uporabnik";
                return true;
            }
        }
        return false;
    }

    public IReadOnlyCollection<string> PridobiOrganizatorje() =>
        _uporabniki.Values.Where(u => string.Equals(u.Role, "Organizator", StringComparison.OrdinalIgnoreCase))
                          .Select(u => u.Email)
                          .ToArray();

    public void DodeliAdmin(string email)
    {
        if (!_uporabniki.TryGetValue(email, out var u))
        {
            u = new Uporabnik { Email = email };
            _uporabniki[email] = u;
        }
        u.Role = "Admin";
        _pendingOrganizator.Remove(email);
    }

    public Uporabnik? Najdi(string email) => _uporabniki.TryGetValue(email, out var u) ? u : null;

    // NEW: Update user profile
    public bool PosodobiProfil(string email, string? lokacija, List<string>? interesi, string? fotografija)
    {
        if (_uporabniki.TryGetValue(email, out var u))
        {
            u.Lokacija = lokacija;
            u.Interesi = interesi ?? new List<string>();
            u.Fotografija = fotografija;
            return true;
        }
        return false;
    }

    public ClaimsPrincipal UstvariPrincipal(Uporabnik u, string? overrideRole = null)
    {
        var role = overrideRole ?? u.Role ?? "Uporabnik";
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, u.Email),
            new Claim(ClaimTypes.Role, role)
        };
        var identity = new ClaimsIdentity(claims, "Cookies");
        return new ClaimsPrincipal(identity);
    }
}

public sealed class Uporabnik
{
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = "Uporabnik"; // Uporabnik | Organizator | Admin
    public string? PasswordHash { get; set; }
    public string? Lokacija { get; set; }
    public List<string> Interesi { get; set; } = new();
    public string? Fotografija { get; set; } // pot do fotografije
}