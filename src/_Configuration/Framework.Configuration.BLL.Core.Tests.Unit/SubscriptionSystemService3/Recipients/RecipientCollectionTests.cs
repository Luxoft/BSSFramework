using AutoFixture;

using FluentAssertions;
using Framework.Configuration.BLL.SubscriptionSystemService3.Recipients;
using Framework.UnitTesting;
using NUnit.Framework;
using NUnit.Framework.Legacy;

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
        ClassicAssert.IsNotNull(collection.GetEnumerator());
    }

    [Test]
    public void Add_Recipient_ItemAdded()
    {
        // Arrange
        var recipient = this.Fixture.Create<Recipient>();

        // Act
        var collection = new RecipientCollection([recipient]);

        // Assert
        collection.Should().HaveCount(1);
    }

    [Test]
    public void Add_RecipientEMailNull_ItemNotAdded()
    {
        // Arrange
        var recipient = new Recipient("login", null);

        // Act
        var collection = new RecipientCollection([recipient]);

        // Assert
        collection.Should().HaveCount(0);
    }

    [Test]
    public void Add_RecipientEMailEmpty_ItemNotAdded()
    {
        // Arrange
        var recipient = new Recipient("login", string.Empty);

        // Act
        var collection = new RecipientCollection([recipient]);

        // Assert
        collection.Should().HaveCount(0);
    }

    [Test]
    public void Merge_Union_CorrectMergeResult()
    {
        // Arrange
        var recipient1 = new Recipient("ivanov", "ivanov@ya.ru");
        var recipient2 = new Recipient("petrov", "petrov@ya.ru");

        var left = new RecipientCollection([recipient1]);
        var right = new RecipientCollection([recipient2]);
        var expected = new RecipientCollection([recipient1, recipient2]);

        // Act
        var mergeResult = left.Merge(right, RecipientsMergeMode.Union);

        // Assert
        mergeResult.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Merge_Intersect_CorrectMergeResult()
    {
        // Arrange
        var recipient1 = new Recipient("ivanov", "ivanov@ya.ru");
        var recipient2 = new Recipient("petrov", "petrov@ya.ru");

        var left = new RecipientCollection([recipient1]);
        var right = new RecipientCollection([recipient1, recipient2]);
        var expected = new RecipientCollection([recipient1]);

        // Act
        var mergeResult = left.Merge(right, RecipientsMergeMode.Intersect);

        // Assert
        mergeResult.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Merge_LeftExceptRight_CorrectMergeResult()
    {
        // Arrange
        var recipient1 = new Recipient("ivanov", "ivanov@ya.ru");
        var recipient2 = new Recipient("petrov", "petrov@ya.ru");

        var left = new RecipientCollection([recipient1]);
        var right = new RecipientCollection([recipient2]);
        var expected = new RecipientCollection([recipient1]);

        // Act
        var mergeResult = left.Merge(right, RecipientsMergeMode.LeftExceptRight);

        // Assert
        mergeResult.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Merge_RightExceptRight_CorrectMergeResult()
    {
        // Arrange
        var recipient1 = new Recipient("ivanov", "ivanov@ya.ru");
        var recipient2 = new Recipient("petrov", "petrov@ya.ru");

        var left = new RecipientCollection([recipient1]);
        var right = new RecipientCollection([recipient2]);
        var expected = new RecipientCollection([recipient2]);

        // Act
        var mergeResult = left.Merge(right, RecipientsMergeMode.RightExceptLeft);

        // Assert
        mergeResult.Should().BeEquivalentTo(expected);
    }
}
