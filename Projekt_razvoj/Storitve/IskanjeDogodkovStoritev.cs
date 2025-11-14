using Projekt_razvoj.Modeli;

namespace Projekt_razvoj.Storitve;

public sealed class IskanjeDogodkovStoritev
{
    public IEnumerable<Dogodek> FiltrirajPoLokaciji(IEnumerable<Dogodek> dogodki, string lokacija) =>
        dogodki.Where(d => string.Equals(d.Lokacija, lokacija, StringComparison.OrdinalIgnoreCase));

    public IEnumerable<Dogodek> FiltrirajPoVrsti(IEnumerable<Dogodek> dogodki, string vrsta) =>
        dogodki.Where(d => string.Equals(d.Vrsta, vrsta, StringComparison.OrdinalIgnoreCase));

    public IEnumerable<Dogodek> FiltrirajBrezplacne(IEnumerable<Dogodek> dogodki) =>
        dogodki.Where(d => d.Cena is null || d.Cena == 0m);

    public IEnumerable<Dogodek> FiltrirajPoDatumu(IEnumerable<Dogodek> dogodki, DateTime odVkljucno, DateTime doVkljucno) =>
        dogodki.Where(d => d.Zacetek >= odVkljucno && d.Zacetek <= doVkljucno);

    public IEnumerable<Dogodek> UrediPoPriljubljenosti(IEnumerable<Dogodek> dogodki, bool padajoce = true) =>
        padajoce ? dogodki.OrderByDescending(d => d.Priljubljenost) : dogodki.OrderBy(d => d.Priljubljenost);

    public IEnumerable<Dogodek> UrediPoDatumu(IEnumerable<Dogodek> dogodki, bool narascajoce = true) =>
        narascajoce ? dogodki.OrderBy(d => d.Zacetek) : dogodki.OrderByDescending(d => d.Zacetek);
}