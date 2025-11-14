namespace Projekt_razvoj.Storitve;

public sealed class PriljubljeniStoritev
{
    // uporabnikId -> mnozica dogodkov
    private readonly Dictionary<string, HashSet<Guid>> _priljubljeni = new(StringComparer.Ordinal);

    public void Dodaj(string uporabnikId, Guid dogodekId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(uporabnikId);
        if (!_priljubljeni.TryGetValue(uporabnikId, out var set))
        {
            set = [];
            _priljubljeni[uporabnikId] = set;
        }
        set.Add(dogodekId);
    }

    public bool Odstrani(string uporabnikId, Guid dogodekId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(uporabnikId);
        return _priljubljeni.TryGetValue(uporabnikId, out var set) && set.Remove(dogodekId);
    }

    public bool Vsebuje(string uporabnikId, Guid dogodekId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(uporabnikId);
        return _priljubljeni.TryGetValue(uporabnikId, out var set) && set.Contains(dogodekId);
    }

    public IReadOnlyCollection<Guid> PridobiVse(string uporabnikId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(uporabnikId);
        return _priljubljeni.TryGetValue(uporabnikId, out var set) ? set.ToArray() : Array.Empty<Guid>();
    }
}