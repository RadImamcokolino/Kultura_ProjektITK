namespace Projekt_razvoj.Modeli;

public sealed class Dogodek
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Naslov { get; init; } = string.Empty;
    public string Vrsta { get; init; } = string.Empty; // npr. "koncert", "razstava"
    public string Lokacija { get; init; } = string.Empty;
    public DateTime Zacetek { get; init; }
    public decimal? Cena { get; init; } // null ali 0 => brezplacno
    public int Priljubljenost { get; init; } // visje => bolj priljubljeno
}