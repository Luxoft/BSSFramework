using FluentAssertions;

using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace Framework.Core.Tests.Unit;

[TestFixture]
public class EnumerableExtensionsTests
{
    [Test]
    public void TakeWhile_RequestInclusive_FindedValue()
    {
        // Arrange
        var container = new[] {1, 2, 3, 4 };

        // Act
        var result = container.TakeWhile(v => v < 3, true);

        // Assert
        result.Should().BeEquivalentTo(new[] {1, 2, 3});
    }

    [Test]
    public void SafeSum_IntSelectorEmpty_ZeroResult()
    {
        // Arrange
        var container = new List<EnumerableTestClass>().AsQueryable();

        // Act
        var result = container.SafeSum(x => x.IntegerProperty);

        // Assert
        result.Should().Be(0);
    }

    [Test]
    public void SafeSum_IntSelectorManyElements_PropertiesValueSumResult()
    {
        // Arrange
        var container =
                new List<EnumerableTestClass>(new[] { new EnumerableTestClass { IntegerProperty = 2 }, new EnumerableTestClass { IntegerProperty = 1 } }).AsQueryable();

        // Act
        var result = container.SafeSum(x => x.IntegerProperty);

        // Assert
        result.Should().Be(3);
    }

    [Test]
    public void SafeSum_IntSelectorSingleElement_PropertyValueResult()
    {
        // Arrange
        var container = new List<EnumerableTestClass>(new[] { new EnumerableTestClass { IntegerProperty = 2 } }).AsQueryable();

        // Act
        var result = container.SafeSum(x => x.IntegerProperty);

        // Assert
        result.Should().Be(2);
    }

    [Test]
    public void SafeSum_LongSelectorEmpty_ZeroResult()
    {
        // Arrange
        var container = new List<EnumerableTestClass>().AsQueryable();

        // Act
        var result = container.SafeSum(x => x.LongProperty);

        // Assert
        result.Should().Be(0L);
    }

    [Test]
    public void SafeSum_LongSelectorManyElements_PropertiesValueSumResult()
    {
        // Arrange
        var container =
                new List<EnumerableTestClass>(new[] { new EnumerableTestClass { LongProperty = 2L }, new EnumerableTestClass { LongProperty = 1L } }).AsQueryable();

        // Act
        var result = container.SafeSum(x => x.LongProperty);

        // Assert
        result.Should().Be(3L);
    }

    [Test]
    public void SafeSum_LongSelectorSingleElement_PropertyValueResult()
    {
        // Arrange
        var container = new List<EnumerableTestClass>(new[] { new EnumerableTestClass { LongProperty = 2L } }).AsQueryable();

        // Act
        var result = container.SafeSum(x => x.LongProperty);

        // Assert
        result.Should().Be(2L);
    }

    [Test]
    public void SafeSum_FloatSelectorEmpty_ZeroResult()
    {
        // Arrange
        var container = new List<EnumerableTestClass>().AsQueryable();

        // Act
        var result = container.SafeSum(x => x.FloatProperty);

        // Assert
        result.Should().Be(0.0F);
    }

    [Test]
    public void SafeSum_FloatSelectorManyElements_PropertiesValueSumResult()
    {
        // Arrange
        var container =
                new List<EnumerableTestClass>(new[] { new EnumerableTestClass { FloatProperty = 2.0F }, new EnumerableTestClass { FloatProperty = 1.0F } }).AsQueryable();

        // Act
        var result = container.SafeSum(x => x.FloatProperty);

        // Assert
        result.Should().Be(3.0F);
    }

    [Test]
    public void SafeSum_FloatSelectorSingleElement_PropertyValueResult()
    {
        // Arrange
        var container = new List<EnumerableTestClass>(new[] { new EnumerableTestClass { FloatProperty = 2.0F } }).AsQueryable();

        // Act
        var result = container.SafeSum(x => x.FloatProperty);

        // Assert
        result.Should().Be(2.0F);
    }

    [Test]
    public void SafeSum_DoubleSelectorEmpty_ZeroResult()
    {
        // Arrange
        var container = new List<EnumerableTestClass>().AsQueryable();

        // Act
        var result = container.SafeSum(x => x.DoubleProperty);

        // Assert
        result.Should().Be(0.0);
    }

    [Test]
    public void SafeSum_DoubleSelectorManyElements_PropertiesValueSumResult()
    {
        // Arrange
        var container =
                new List<EnumerableTestClass>(new[] { new EnumerableTestClass { DoubleProperty = 2.0 }, new EnumerableTestClass { DoubleProperty = 1.0 } }).AsQueryable();

        // Act
        var result = container.SafeSum(x => x.DoubleProperty);

        // Assert
        result.Should().Be(3.0);
    }

    [Test]
    public void SafeSum_DoubleSelectorSingleElement_PropertyValueResult()
    {
        // Arrange
        var container = new List<EnumerableTestClass>(new[] { new EnumerableTestClass { DoubleProperty = 2.0 } }).AsQueryable();

        // Act
        var result = container.SafeSum(x => x.DoubleProperty);

        // Assert
        result.Should().Be(2.0);
    }

    [Test]
    public void SafeSum_DecimalSelectorEmpty_ZeroResult()
    {
        // Arrange
        var container = new List<EnumerableTestClass>().AsQueryable();

        // Act
        var result = container.SafeSum(x => x.DecimalProperty);

        // Assert
        result.Should().Be(0.0m);
    }

    [Test]
    public void SafeSum_DecimalSelectorManyElements_PropertiesValueSumResult()
    {
        // Arrange
        var container =
                new List<EnumerableTestClass>(new[] { new EnumerableTestClass { DecimalProperty = 2.0m }, new EnumerableTestClass { DecimalProperty = 1.0m } }).AsQueryable();

        // Act
        var result = container.SafeSum(x => x.DecimalProperty);

        // Assert
        result.Should().Be(3.0m);
    }

    [Test]
    public void SafeSum_DecimalSelectorSingleElement_PropertyValueResult()
    {
        // Arrange
        var container = new List<EnumerableTestClass>(new[] { new EnumerableTestClass { DecimalProperty = 2.0m } }).AsQueryable();

        // Act
        var result = container.SafeSum(x => x.DecimalProperty);

        // Assert
        result.Should().Be(2.0m);
    }

    [Test]
    public void DistinctTest()
    {
        // Arrange
        var list = new List<string> { "1", "2", null, "3" };

        // Act
        var everythingIsFineHere = list.Distinct().ToList();

        var nullRefError = list.Distinct(z => z).ToList();

        // Assert
        ClassicAssert.AreEqual(everythingIsFineHere.Count, nullRefError.Count);
    }
}
