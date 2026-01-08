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
        // Seed - Koncerti
        Dodaj(new Dogodek { Naslov = "Rock Night", Vrsta = "koncert", Lokacija = "Ljubljana", Zacetek = DateTime.Today.AddDays(2).AddHours(20), Cena = 25, Priljubljenost = 80 });
        Dodaj(new Dogodek { Naslov = "Street Music", Vrsta = "koncert", Lokacija = "Maribor", Zacetek = DateTime.Today.AddDays(3).AddHours(18), Cena = null, Priljubljenost = 70 });
        Dodaj(new Dogodek { Naslov = "Jazz veèer", Vrsta = "koncert", Lokacija = "Ljubljana", Zacetek = DateTime.Today.AddDays(5).AddHours(21), Cena = 15, Priljubljenost = 65 });
        Dodaj(new Dogodek { Naslov = "Elektronska noè", Vrsta = "koncert", Lokacija = "Celje", Zacetek = DateTime.Today.AddDays(7).AddHours(22), Cena = 20, Priljubljenost = 85 });
        Dodaj(new Dogodek { Naslov = "Akustièni koncert", Vrsta = "koncert", Lokacija = "Koper", Zacetek = DateTime.Today.AddDays(4).AddHours(19), Cena = 12, Priljubljenost = 60 });
        
        // Razstave
        Dodaj(new Dogodek { Naslov = "Free Museum", Vrsta = "razstava", Lokacija = "Maribor", Zacetek = DateTime.Today.AddDays(1).AddHours(10), Cena = 0, Priljubljenost = 50 });
        Dodaj(new Dogodek { Naslov = "Moderna umetnost", Vrsta = "razstava", Lokacija = "Ljubljana", Zacetek = DateTime.Today.AddDays(1).AddHours(9), Cena = 8, Priljubljenost = 55 });
        Dodaj(new Dogodek { Naslov = "Fotografska razstava", Vrsta = "razstava", Lokacija = "Kranj", Zacetek = DateTime.Today.AddDays(2).AddHours(11), Cena = 5, Priljubljenost = 45 });
        Dodaj(new Dogodek { Naslov = "Skulpture v parku", Vrsta = "razstava", Lokacija = "Bled", Zacetek = DateTime.Today.AddDays(0).AddHours(10), Cena = 0, Priljubljenost = 75 });
        Dodaj(new Dogodek { Naslov = "Impresionisti", Vrsta = "razstava", Lokacija = "Ljubljana", Zacetek = DateTime.Today.AddDays(3).AddHours(10), Cena = 10, Priljubljenost = 90 });
        
        // Gledališèe
        Dodaj(new Dogodek { Naslov = "Hamlet", Vrsta = "gledalisce", Lokacija = "Ljubljana", Zacetek = DateTime.Today.AddDays(4).AddHours(20), Cena = 18, Priljubljenost = 88 });
        Dodaj(new Dogodek { Naslov = "Komedija pomeškov", Vrsta = "gledalisce", Lokacija = "Maribor", Zacetek = DateTime.Today.AddDays(6).AddHours(19), Cena = 15, Priljubljenost = 72 });
        Dodaj(new Dogodek { Naslov = "Otroška predstava", Vrsta = "gledalisce", Lokacija = "Celje", Zacetek = DateTime.Today.AddDays(2).AddHours(16), Cena = 8, Priljubljenost = 65 });
        Dodaj(new Dogodek { Naslov = "Muzikal - Cats", Vrsta = "gledalisce", Lokacija = "Ljubljana", Zacetek = DateTime.Today.AddDays(8).AddHours(20), Cena = 35, Priljubljenost = 95 });
        Dodaj(new Dogodek { Naslov = "Stand-up veèer", Vrsta = "gledalisce", Lokacija = "Koper", Zacetek = DateTime.Today.AddDays(5).AddHours(21), Cena = 12, Priljubljenost = 80 });
        
        // Dodatni dogodki
        Dodaj(new Dogodek { Naslov = "Filmski festival", Vrsta = "razstava", Lokacija = "Ljubljana", Zacetek = DateTime.Today.AddDays(10).AddHours(18), Cena = 0, Priljubljenost = 82 });
        Dodaj(new Dogodek { Naslov = "Ulièniplesni koncert", Vrsta = "koncert", Lokacija = "Ptuj", Zacetek = DateTime.Today.AddDays(6).AddHours(17), Cena = 0, Priljubljenost = 70 });
        Dodaj(new Dogodek { Naslov = "Kulturna noè", Vrsta = "razstava", Lokacija = "Nova Gorica", Zacetek = DateTime.Today.AddDays(12).AddHours(19), Cena = 0, Priljubljenost = 78 });
        Dodaj(new Dogodek { Naslov = "Opera veèer", Vrsta = "gledalisce", Lokacija = "Ljubljana", Zacetek = DateTime.Today.AddDays(9).AddHours(20), Cena = 45, Priljubljenost = 92 });
        Dodaj(new Dogodek { Naslov = "Baletna predstava", Vrsta = "gledalisce", Lokacija = "Maribor", Zacetek = DateTime.Today.AddDays(11).AddHours(19), Cena = 28, Priljubljenost = 87 });
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