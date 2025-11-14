using Microsoft.VisualStudio.TestTools.UnitTesting;
using Projekt_razvoj.Storitve;

namespace Projekt_razvoj.Tests.Storitve;

[TestClass]
public class PriljubljeniStoritevTests
{
    [TestMethod]
    public void Test_DodajInVsebuje()
    {
        var s = new PriljubljeniStoritev();
        var user = "u1";
        var dogodek = Guid.NewGuid();

        s.Dodaj(user, dogodek);
        Assert.IsTrue(s.Vsebuje(user, dogodek));
    }

    [TestMethod]
    public void Test_Dodaj_NePodvaja()
    {
        var s = new PriljubljeniStoritev();
        var user = "u1";
        var dogodek = Guid.NewGuid();

        s.Dodaj(user, dogodek);
        s.Dodaj(user, dogodek);
        Assert.AreEqual(1, s.PridobiVse(user).Count);
    }

    [TestMethod]
    public void Test_Odstrani()
    {
        var s = new PriljubljeniStoritev();
        var user = "u1";
        var dogodek = Guid.NewGuid();
        s.Dodaj(user, dogodek);
        Assert.IsTrue(s.Odstrani(user, dogodek));
        Assert.IsFalse(s.Vsebuje(user, dogodek));
    }

    [TestMethod]
    public void Test_PridobiVse_PrazenUporabnik()
    {
        var s = new PriljubljeniStoritev();
        var vsi = s.PridobiVse("neobstaja");
        Assert.AreEqual(0, vsi.Count);
    }

    [TestMethod]
    public void Test_Dodaj_PrazenUporabnikId_Vrze()
    {
        var s = new PriljubljeniStoritev();
        Assert.ThrowsException<ArgumentException>(() => s.Dodaj("", Guid.NewGuid()));
    }
}