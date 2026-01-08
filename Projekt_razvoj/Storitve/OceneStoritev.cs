using Projekt_razvoj.Modeli;
using System.Collections.Concurrent;

namespace Projekt_razvoj.Storitve;

public sealed class OceneStoritev
{
    private readonly ConcurrentDictionary<Guid, Ocena> _ocene = new();

    public Ocena DodajOceno(Guid dogodekId, string uporabnikEmail, int zvezdice, string komentar)
    {
        // Preveri, èe je uporabnik že ocenil ta dogodek
        var obstojeca = _ocene.Values.FirstOrDefault(o => 
            o.DogodekId == dogodekId && 
            string.Equals(o.UporabnikEmail, uporabnikEmail, StringComparison.OrdinalIgnoreCase));

        if (obstojeca is not null)
        {
            // Posodobi obstojeèo oceno
            _ocene.TryRemove(obstojeca.Id, out _);
        }

        var novaOcena = new Ocena
        {
            DogodekId = dogodekId,
            UporabnikEmail = uporabnikEmail,
            Zvezdice = Math.Clamp(zvezdice, 1, 5),
            Komentar = komentar ?? string.Empty
        };

        _ocene[novaOcena.Id] = novaOcena;
        return novaOcena;
    }

    public IEnumerable<Ocena> PridobiOceneDogodka(Guid dogodekId)
    {
        return _ocene.Values
            .Where(o => o.DogodekId == dogodekId)
            .OrderByDescending(o => o.DatumOcene);
    }

    public double? IzracunajPovprecnoOceno(Guid dogodekId)
    {
        var ocene = _ocene.Values.Where(o => o.DogodekId == dogodekId).ToList();
        return ocene.Any() ? ocene.Average(o => o.Zvezdice) : null;
    }

    public int PridobiStOcen(Guid dogodekId)
    {
        return _ocene.Values.Count(o => o.DogodekId == dogodekId);
    }

    public bool JeUporabnikOcenil(Guid dogodekId, string uporabnikEmail)
    {
        return _ocene.Values.Any(o => 
            o.DogodekId == dogodekId && 
            string.Equals(o.UporabnikEmail, uporabnikEmail, StringComparison.OrdinalIgnoreCase));
    }

    public Ocena? PridobiOcenoUporabnika(Guid dogodekId, string uporabnikEmail)
    {
        return _ocene.Values.FirstOrDefault(o => 
            o.DogodekId == dogodekId && 
            string.Equals(o.UporabnikEmail, uporabnikEmail, StringComparison.OrdinalIgnoreCase));
    }
}