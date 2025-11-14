using Microsoft.VisualStudio.TestTools.UnitTesting;
using Projekt_razvoj.Modeli;
using Projekt_razvoj.Storitve;

namespace Projekt_razvoj.Tests.Storitve;

[TestClass]
public class IskanjeDogodkovStoritevTests
{
    private static List<Dogodek> Vzorec() => new()
    {
        new() { Naslov = "Rock Night", Vrsta = "koncert", Lokacija = "Ljubljana", Zacetek = new DateTime(2025,12,1,20,0,0), Cena = 25, Priljubljenost = 80 },
        new() { Naslov = "Free Museum", Vrsta = "razstava", Lokacija = "Maribor", Zacetek = new DateTime(2025,12,1,10,0,0), Cena = 0, Priljubljenost = 50 },
        new() { Naslov = "Drama Show", Vrsta = "gledalisce", Lokacija = "Ljubljana", Zacetek = new DateTime(2025,12,2,19,0,0), Cena = 15, Priljubljenost = 60 },
        new() { Naslov = "Street Music", Vrsta = "koncert", Lokacija = "Maribor", Zacetek = new DateTime(2025,12,3,18,0,0), Cena = null, Priljubljenost = 70 },
    };

    [TestMethod]
    public void Test_FiltrirajPoLokaciji()
    {
        var s = new IskanjeDogodkovStoritev();
        var r = s.FiltrirajPoLokaciji(Vzorec(), "Ljubljana").ToList();
        Assert.AreEqual(2, r.Count);
    }

    [TestMethod]
    public void Test_FiltrirajPoVrsti()
    {
        var s = new IskanjeDogodkovStoritev();
        var r = s.FiltrirajPoVrsti(Vzorec(), "koncert").ToList();
        Assert.AreEqual(2, r.Count);
    }

    [TestMethod]
    public void Test_FiltrirajBrezplacne()
    {
        var s = new IskanjeDogodkovStoritev();
        var r = s.FiltrirajBrezplacne(Vzorec()).ToList();
        Assert.AreEqual(2, r.Count);
    }

    [TestMethod]
    public void Test_UrediPoPriljubljenosti_Padajoce()
    {
        var s = new IskanjeDogodkovStoritev();
        var r = s.UrediPoPriljubljenosti(Vzorec()).ToList();
        for (int i = 1; i < r.Count; i++)
            Assert.IsTrue(r[i-1].Priljubljenost >= r[i].Priljubljenost);
    }

    [TestMethod]
    public void Test_UrediPoDatumu_Narascajoce()
    {
        var s = new IskanjeDogodkovStoritev();
        var r = s.UrediPoDatumu(Vzorec(), narašèajoce: true).ToList();
        for (int i = 1; i < r.Count; i++)
            Assert.IsTrue(r[i-1].Zacetek <= r[i].Zacetek);
    }
}