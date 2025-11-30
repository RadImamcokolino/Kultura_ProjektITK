using Projekt_razvoj.Modeli;
using System.Collections.Concurrent;

namespace Projekt_razvoj.Storitve;

public interface IDogodkiRepository
{
    IEnumerable<Dogodek> PridobiVse();
    Dogodek? Pridobi(Guid id);
    Dogodek Dodaj(Dogodek d);
    bool Obstaja(Guid id);
}

public sealed class DogodkiRepository : IDogodkiRepository
{
    private readonly ConcurrentDictionary<Guid, Dogodek> _dogodki = new();

    public DogodkiRepository()
    {
        // Seed
        Dodaj(new Dogodek { Naslov = "Rock Night", Vrsta = "koncert", Lokacija = "Ljubljana", Zacetek = DateTime.Today.AddDays(2).AddHours(20), Cena = 25, Priljubljenost = 80 });
        Dodaj(new Dogodek { Naslov = "Free Museum", Vrsta = "razstava", Lokacija = "Maribor", Zacetek = DateTime.Today.AddDays(1).AddHours(10), Cena = 0, Priljubljenost = 50 });
        Dodaj(new Dogodek { Naslov = "Street Music", Vrsta = "koncert", Lokacija = "Maribor", Zacetek = DateTime.Today.AddDays(3).AddHours(18), Cena = null, Priljubljenost = 70 });
    }

    public IEnumerable<Dogodek> PridobiVse() => _dogodki.Values.OrderBy(d => d.Zacetek);
    public Dogodek? Pridobi(Guid id) => _dogodki.TryGetValue(id, out var d) ? d : null;
    public bool Obstaja(Guid id) => _dogodki.ContainsKey(id);

    public Dogodek Dodaj(Dogodek d)
    {
        _dogodki[d.Id] = d;
        return d;
    }
}