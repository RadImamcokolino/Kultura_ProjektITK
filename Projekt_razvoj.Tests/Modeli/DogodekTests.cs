using Microsoft.VisualStudio.TestTools.UnitTesting;
using Projekt_razvoj.Modeli;

namespace Projekt_razvoj.Tests.Modeli;

[TestClass]
public class DogodekTests
{
    [TestMethod]
    public void Test_PrivzeteVrednosti()
    {
        var d = new Dogodek();
        Assert.AreNotEqual(Guid.Empty, d.Id);
        Assert.AreEqual(string.Empty, d.Naslov);
        Assert.AreEqual(string.Empty, d.Vrsta);
        Assert.AreEqual(string.Empty, d.Lokacija);
        Assert.AreEqual(default(DateTime), d.Zacetek);
        Assert.IsNull(d.Cena);
        Assert.AreEqual(0, d.Priljubljenost);
    }

    [TestMethod]
    public void Test_Id_Unikaten()
    {
        var d1 = new Dogodek();
        var d2 = new Dogodek();
        Assert.AreNotEqual(d1.Id, d2.Id);
    }

    [TestMethod]
    public void Test_Cena_Brezplacno_KoNullAliNiè()
    {
        var dNull = new Dogodek { Cena = null };
        var dZero = new Dogodek { Cena = 0m };
        Assert.IsTrue(dNull.Cena is null || dNull.Cena == 0m);
        Assert.IsTrue(dZero.Cena is null || dZero.Cena == 0m);
    }

    [TestMethod]
    public void Test_Inicializacija_Polna()
    {
        var zacetek = new DateTime(2025, 12, 1, 20, 0, 0);
        var d = new Dogodek
        {
            Naslov = "Koncert",
            Vrsta = "koncert",
            Lokacija = "Ljubljana",
            Zacetek = zacetek,
            Cena = 15.5m,
            Priljubljenost = 77
        };
        Assert.AreEqual("Koncert", d.Naslov);
        Assert.AreEqual("koncert", d.Vrsta);
        Assert.AreEqual("Ljubljana", d.Lokacija);
        Assert.AreEqual(zacetek, d.Zacetek);
        Assert.AreEqual(15.5m, d.Cena);
        Assert.AreEqual(77, d.Priljubljenost);
    }
}