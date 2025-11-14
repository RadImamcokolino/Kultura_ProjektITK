using Microsoft.VisualStudio.TestTools.UnitTesting;
using Projekt_razvoj.Storitve;

namespace Projekt_razvoj.Tests.Storitve;

[TestClass]
public class PreverjevalnikGeselTests
{
    [TestMethod]
    public void Test_JeEpostaVeljavna_Veljavne()
    {
        var s = new PreverjevalnikGesel();
        Assert.IsTrue(s.JeEpostaVeljavna("uporabnik@example.com"));
        Assert.IsTrue(s.JeEpostaVeljavna("ime.priimek@domena.si"));
    }

    [TestMethod]
    public void Test_JeEpostaVeljavna_Neveljavne()
    {
        var s = new PreverjevalnikGesel();
        Assert.IsFalse(s.JeEpostaVeljavna("narobe"));
        Assert.IsFalse(s.JeEpostaVeljavna("user@"));
        Assert.IsFalse(s.JeEpostaVeljavna("@host.si"));
    }

    [TestMethod]
    public void Test_JeGesloMocno_Pravilno()
    {
        var s = new PreverjevalnikGesel();
        Assert.IsTrue(s.JeGesloMocno("Abcdef1!"));
    }

    [TestMethod]
    public void Test_JeGesloMocno_Prekrtko()
    {
        var s = new PreverjevalnikGesel();
        Assert.IsFalse(s.JeGesloMocno("A1!a"));
    }

    [TestMethod]
    public void Test_JeGesloMocno_ManjkaVelikaCrka()
    {
        var s = new PreverjevalnikGesel();
        Assert.IsFalse(s.JeGesloMocno("abcdef1!"));
    }
}