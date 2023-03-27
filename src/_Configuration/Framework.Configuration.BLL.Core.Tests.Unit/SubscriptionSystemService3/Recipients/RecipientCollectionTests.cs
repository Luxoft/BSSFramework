using AutoFixture;

using FluentAssertions;
using Framework.Configuration.BLL.SubscriptionSystemService3.Recipients;
using Framework.UnitTesting;
using NUnit.Framework;

namespace Framework.Configuration.BLL.Core.Tests.Unit.SubscriptionSystemService3.Recipients;

[TestFixture]
public sealed class RecipientCollectionTests : TestFixtureBase
{
    [Test]
    public void GetEnumerator_Call_ResultNotNull()
    {
        // Arrange
        var collection = new RecipientCollection();

        // Act

        // Assert
        Assert.IsNotNull(collection.GetEnumerator());
    }

    [Test]
    public void Add_Recipient_ItemAdded()
    {
        // Arrange
        var recipient = this.Fixture.Create<Recipient>();

        // Act
        var collection = new RecipientCollection(new[] { recipient });

        // Assert
        collection.Should().HaveCount(1);
    }

    [Test]
    public void Add_RecipientEMailNull_ItemNotAdded()
    {
        // Arrange
        var recipient = new Recipient("login", null);

        // Act
        var collection = new RecipientCollection(new[] { recipient });

        // Assert
        collection.Should().HaveCount(0);
    }

    [Test]
    public void Add_RecipientEMailEmpty_ItemNotAdded()
    {
        // Arrange
        var recipient = new Recipient("login", string.Empty);

        // Act
        var collection = new RecipientCollection(new[] { recipient });

        // Assert
        collection.Should().HaveCount(0);
    }

    [Test]
    public void Merge_Union_CorrectMergeResult()
    {
        // Arrange
        var recipient1 = new Recipient("ivanov", "ivanov@ya.ru");
        var recipient2 = new Recipient("petrov", "petrov@ya.ru");

        var left = new RecipientCollection(new[] {recipient1});
        var right = new RecipientCollection(new[] {recipient2});
        var expected = new RecipientCollection(new[] { recipient1, recipient2});

        // Act
        var mergeResult = left.Merge(right, RecepientsMergeMode.Union);

        // Assert
        mergeResult.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Merge_Intersect_CorrectMergeResult()
    {
        // Arrange
        var recipient1 = new Recipient("ivanov", "ivanov@ya.ru");
        var recipient2 = new Recipient("petrov", "petrov@ya.ru");

        var left = new RecipientCollection(new[] { recipient1 });
        var right = new RecipientCollection(new[] { recipient1, recipient2 });
        var expected = new RecipientCollection(new[] { recipient1 });

        // Act
        var mergeResult = left.Merge(right, RecepientsMergeMode.Intersect);

        // Assert
        mergeResult.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Merge_LeftExceptRight_CorrectMergeResult()
    {
        // Arrange
        var recipient1 = new Recipient("ivanov", "ivanov@ya.ru");
        var recipient2 = new Recipient("petrov", "petrov@ya.ru");

        var left = new RecipientCollection(new[] { recipient1 });
        var right = new RecipientCollection(new[] { recipient2 });
        var expected = new RecipientCollection(new[] { recipient1 });

        // Act
        var mergeResult = left.Merge(right, RecepientsMergeMode.LeftExceptRight);

        // Assert
        mergeResult.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Merge_RightExceptRight_CorrectMergeResult()
    {
        // Arrange
        var recipient1 = new Recipient("ivanov", "ivanov@ya.ru");
        var recipient2 = new Recipient("petrov", "petrov@ya.ru");

        var left = new RecipientCollection(new[] { recipient1 });
        var right = new RecipientCollection(new[] { recipient2 });
        var expected = new RecipientCollection(new[] { recipient2 });

        // Act
        var mergeResult = left.Merge(right, RecepientsMergeMode.RightExceptLeft);

        // Assert
        mergeResult.Should().BeEquivalentTo(expected);
    }
}
