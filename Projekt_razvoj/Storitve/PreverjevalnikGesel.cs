using System.Text.RegularExpressions;

namespace Projekt_razvoj.Storitve;

public sealed class PreverjevalnikGesel
{
    private static readonly Regex EpostaRegex =
        new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.CultureInvariant);

    public bool JeEpostaVeljavna(string? eposta) =>
        !string.IsNullOrWhiteSpace(eposta) && EpostaRegex.IsMatch(eposta);

    // Mocno geslo: najmanj 8 znakov, vsaj 1 velika, 1 mala, 1 stevilka, 1 poseben znak
    public bool JeGesloMocno(string? geslo)
    {
        if (string.IsNullOrEmpty(geslo) || geslo.Length < 8) return false;

        bool imaVeliko = false, imaMalo = false, imaStevko = false, imaPoseben = false;
        foreach (var ch in geslo)
        {
            if (char.IsUpper(ch)) imaVeliko = true;
            else if (char.IsLower(ch)) imaMalo = true;
            else if (char.IsDigit(ch)) imaStevko = true;
            else imaPoseben = true;
        }
        return imaVeliko && imaMalo && imaStevko && imaPoseben;
    }
}