using System.Text;

using NUnit.Framework;

namespace HFM.Client.Internal;

[TestFixture]
public class StringBuilderExtensionsTests
{
    [Test]
    public void StringBuilder_IndexOf_ThrowsArgumentNullExceptionWhenSourceIsNull()
    {
        // Arrange
        StringBuilder? source = null;
        string value = "";
        int startIndex = 0;
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => source.IndexOf(value, startIndex));
    }

    [Test]
    public void StringBuilder_IndexOf_ThrowsArgumentNullExceptionWhenValueIsNull()
    {
        // Arrange
        StringBuilder source = new StringBuilder();
        string? value = null;
        int startIndex = 0;
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => source.IndexOf(value, startIndex));
    }

    [Test]
    public void StringBuilder_IndexOf_ThrowsArgumentOutOfRangeExceptionWhenStartIndexIsLessThanZero()
    {
        // Arrange
        StringBuilder source = new StringBuilder();
        string value = "";
        int startIndex = -1;
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => source.IndexOf(value, startIndex));
    }

    [Test]
    public void StringBuilder_IndexOf_ThrowsArgumentOutOfRangeExceptionWhenStartIndexIsGreaterThanSourceLength()
    {
        // Arrange
        StringBuilder source = new StringBuilder("foo");
        string value = "";
        int startIndex = 4;
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => source.IndexOf(value, startIndex));
    }

    [Test]
    public void StringBuilder_IndexOf_ReturnsStartIndexWhenValueIsEmptyString()
    {
        // Arrange
        StringBuilder source = new StringBuilder("foo");
        string value = "";
        int startIndex = 3;
        // Act
        int index = source.IndexOf(value, startIndex);
        // Assert
        Assert.AreEqual(startIndex, index);
    }

    [Test]
    public void StringBuilder_IndexOf_ReturnsNegativeOneWhenValueIsNotEmptyStringAndStartIndexEqualsSourceLength()
    {
        // Arrange
        StringBuilder source = new StringBuilder("foo");
        string value = "a";
        int startIndex = 3;
        // Act
        int index = source.IndexOf(value, startIndex);
        // Assert
        Assert.AreEqual(-1, index);
    }

    [Test]
    public void StringBuilder_IndexOf_ReturnsIndexWhenValueExistsInSource()
    {
        // Arrange
        StringBuilder source = new StringBuilder("foobarfizzbizz");
        string value = "fizz";
        int startIndex = 0;
        // Act
        int index = source.IndexOf(value, startIndex);
        // Assert
        Assert.AreEqual(6, index);
    }

    [Test]
    public void StringBuilder_Trim_ThrowsArgumentNullExceptionWhenSourceIsNull()
    {
        // Arrange
        StringBuilder? source = null;
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => source.Trim());
    }

    [Test]
    public void StringBuilder_Trim_ThrowsArgumentNullExceptionWhenTrimCharsIsNull()
    {
        // Arrange
        StringBuilder source = new StringBuilder();
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => source.Trim(null));
    }

    [Test]
    public void StringBuilder_Trim_ReturnsTheSameInstance()
    {
        // Arrange
        StringBuilder source = new StringBuilder("\"foo\"");
        // Act
        var result = source.Trim('\"');
        // Assert
        Assert.AreSame(source, result);
    }

    [Test]
    public void StringBuilder_Trim_RemovesSingleCharacterFromStartAndEnd()
    {
        // Arrange
        StringBuilder source = new StringBuilder("\"foo\"");
        // Act
        source.Trim('\"');
        // Assert
        Assert.AreEqual(3, source.Length);
        Assert.AreEqual("foo", source.ToString());
    }

    [Test]
    public void StringBuilder_Trim_RemovesMultipleCharactersFromStartAndEnd()
    {
        // Arrange
        StringBuilder source = new StringBuilder("\"\"foo\"\"");
        // Act
        source.Trim('\"');
        // Assert
        Assert.AreEqual(3, source.Length);
        Assert.AreEqual("foo", source.ToString());
    }

    [Test]
    public void StringBuilder_Trim_RemovesMultipleTrimCharactersFromStartAndEnd()
    {
        // Arrange
        StringBuilder source = new StringBuilder("\"*foo*\"");
        // Act
        source.Trim('\"', '*');
        // Assert
        Assert.AreEqual(3, source.Length);
        Assert.AreEqual("foo", source.ToString());
    }

    [Test]
    public void StringBuilder_CopyTo_ThrowsArgumentNullExceptionWhenSourceIsNull()
    {
        // Arrange
        StringBuilder? source = null;
        int sourceIndex = 0;
        StringBuilder destination = new StringBuilder();
        int destinationIndex = 0;
        int count = 0;
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => source.CopyTo(sourceIndex, destination, destinationIndex, count));
    }

    [Test]
    public void StringBuilder_CopyTo_ThrowsArgumentNullExceptionWhenDestinationIsNull()
    {
        // Arrange
        StringBuilder source = new StringBuilder();
        int sourceIndex = 0;
        StringBuilder? destination = null;
        int destinationIndex = 0;
        int count = 0;
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => source.CopyTo(sourceIndex, destination, destinationIndex, count));
    }

    [Test]
    public void StringBuilder_CopyTo_ThrowsArgumentOutOfRangeExceptionWhenSourceIndexIsLessThanZero()
    {
        // Arrange
        StringBuilder source = new StringBuilder();
        int sourceIndex = -1;
        StringBuilder destination = new StringBuilder();
        int destinationIndex = 0;
        int count = 0;
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => source.CopyTo(sourceIndex, destination, destinationIndex, count));
    }

    [Test]
    public void StringBuilder_CopyTo_ThrowsArgumentOutOfRangeExceptionWhenDestinationIndexIsLessThanZero()
    {
        // Arrange
        StringBuilder source = new StringBuilder();
        int sourceIndex = 0;
        StringBuilder destination = new StringBuilder();
        int destinationIndex = -1;
        int count = 0;
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => source.CopyTo(sourceIndex, destination, destinationIndex, count));
    }

    [Test]
    public void StringBuilder_CopyTo_ThrowsArgumentOutOfRangeExceptionWhenCountIsLessThanZero()
    {
        // Arrange
        StringBuilder source = new StringBuilder();
        int sourceIndex = 0;
        StringBuilder destination = new StringBuilder();
        int destinationIndex = 0;
        int count = -1;
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => source.CopyTo(sourceIndex, destination, destinationIndex, count));
    }

    [Test]
    public void StringBuilder_CopyTo_CopiesNothingWhenSourceIndexIsSourceLengthAndCountIsZero()
    {
        // Arrange
        StringBuilder source = new StringBuilder("foo");
        int sourceIndex = 3;
        StringBuilder destination = new StringBuilder();
        int destinationIndex = 0;
        int count = 0;
        // Act
        source.CopyTo(sourceIndex, destination, destinationIndex, count);
        // Assert
        Assert.AreEqual(0, destination.Length);
    }

    [Test]
    public void StringBuilder_CopyTo_ThrowsArgumentOutOfRangeExceptionWhenSourceIndexIsGreaterThanSourceLength()
    {
        // Arrange
        StringBuilder source = new StringBuilder("foo");
        int sourceIndex = 4;
        StringBuilder destination = new StringBuilder();
        int destinationIndex = 0;
        int count = 0;
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => source.CopyTo(sourceIndex, destination, destinationIndex, count));
    }

    [Test]
    public void StringBuilder_CopyTo_ThrowsArgumentExceptionWhenSourceIndexPlusCountIsGreaterThanSourceLength()
    {
        // Arrange
        StringBuilder source = new StringBuilder("foo");
        int sourceIndex = 3;
        StringBuilder destination = new StringBuilder();
        int destinationIndex = 0;
        int count = 1;
        // Act & Assert
        Assert.Throws<ArgumentException>(() => source.CopyTo(sourceIndex, destination, destinationIndex, count));
    }

    [Test]
    public void StringBuilder_CopyTo_PadsDestinationWithSpacesWhenDestinationIndexIsGreaterThanDestinationLength()
    {
        // Arrange
        StringBuilder source = new StringBuilder("foo");
        int sourceIndex = 0;
        StringBuilder destination = new StringBuilder();
        int destinationIndex = 3;
        int count = source.Length;
        // Act
        source.CopyTo(sourceIndex, destination, destinationIndex, count);
        // Assert
        Assert.AreEqual(6, destination.Length);
        Assert.AreEqual("   foo", destination.ToString());
    }

    [Test]
    public void StringBuilder_CopyTo_OverwritesDestinationWhenDestinationIndexIsLessThanDestinationLength()
    {
        // Arrange
        StringBuilder source = new StringBuilder("bar");
        int sourceIndex = 0;
        StringBuilder destination = new StringBuilder("foofoo");
        int destinationIndex = 3;
        int count = source.Length;
        // Act
        source.CopyTo(sourceIndex, destination, destinationIndex, count);
        // Assert
        Assert.AreEqual(6, destination.Length);
        Assert.AreEqual("foobar", destination.ToString());
    }
}
