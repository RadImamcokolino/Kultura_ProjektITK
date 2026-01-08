namespace Projekt_razvoj.Modeli;

public sealed class Ocena
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid DogodekId { get; init; }
    public string UporabnikEmail { get; init; } = string.Empty;
    public int Zvezdice { get; init; } // 1-5
    public string Komentar { get; init; } = string.Empty;
    public DateTime DatumOcene { get; init; } = DateTime.Now;
}