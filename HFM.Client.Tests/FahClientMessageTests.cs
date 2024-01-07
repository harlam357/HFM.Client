using NUnit.Framework;

namespace HFM.Client;

[TestFixture]
public class FahClientMessageTests
{
    [Test]
    public void FahClientMessageIdentifier_AreEqual()
    {
        var id1 = new FahClientMessageIdentifier("foo", new DateTime(2019, 1, 1));
        var id2 = new FahClientMessageIdentifier("foo", new DateTime(2019, 1, 1));
        Assert.IsTrue(id1.Equals(id2));
    }

    [Test]
    public void FahClientMessageIdentifier_AreEqualObjects()
    {
        object id1 = new FahClientMessageIdentifier("foo", new DateTime(2019, 1, 1));
        object id2 = new FahClientMessageIdentifier("foo", new DateTime(2019, 1, 1));
        Assert.IsTrue(id1.Equals(id2));
    }

    [Test]
    public void FahClientMessageIdentifier_AreEqualHashCodes()
    {
        var id1 = new FahClientMessageIdentifier("foo", new DateTime(2019, 1, 1));
        var id2 = new FahClientMessageIdentifier("foo", new DateTime(2019, 1, 1));
        Assert.IsTrue(id1.GetHashCode().Equals(id2.GetHashCode()));
    }

    [Test]
    public void FahClientMessageIdentifier_AreNotEqual()
    {
        var id1 = new FahClientMessageIdentifier("foo", new DateTime(2019, 1, 1));
        var id2 = new FahClientMessageIdentifier("bar", new DateTime(2019, 1, 1));
        Assert.IsFalse(id1.Equals(id2));
    }

    [Test]
    public void FahClientMessageIdentifier_AreNotEqualObjects()
    {
        object id1 = new FahClientMessageIdentifier("foo", new DateTime(2019, 1, 1));
        object id2 = new FahClientMessageIdentifier("foo", new DateTime(2020, 1, 1));
        Assert.IsFalse(id1.Equals(id2));
    }

    [Test]
    public void FahClientMessageIdentifier_AreNotEqualHashCodes()
    {
        var id1 = new FahClientMessageIdentifier("foo", new DateTime(2019, 1, 1));
        var id2 = new FahClientMessageIdentifier("bar", new DateTime(2019, 1, 1));
        Assert.IsFalse(id1.GetHashCode().Equals(id2.GetHashCode()));
    }

    [Test]
    public void FahClientMessageIdentifier_ObjectDoesNotEqualNull()
    {
        object id1 = new FahClientMessageIdentifier("foo", new DateTime(2019, 1, 1));
        object id2 = null;
        Assert.IsFalse(id1.Equals(id2));
    }

    [Test]
    public void FahClientMessageIdentifier_AreNotSameObjectTypes()
    {
        object id1 = new FahClientMessageIdentifier("foo", new DateTime(2019, 1, 1));
        object id2 = "foo";
        Assert.IsFalse(id1.Equals(id2));
    }
}
