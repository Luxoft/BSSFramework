using System.Reflection;

using FluentAssertions;

using Framework.Configuration.BLL.Core.Tests.Unit.SubscriptionSystemService3.Subscriptions.Metadata;
using Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;

using NUnit.Framework;

namespace Framework.Configuration.BLL.Core.Tests.Unit.SubscriptionSystemService3.Subscriptions;

[TestFixture]
public sealed class SubscriptionMetadataFinderTests
{
    /// <summary>
    /// Тест проверяет, что при наличии в сборке с моделями подписок
    /// абстрактных типов и типов без конструкторов по умолчанию,
    /// не производится приводящая к исключению попытка создать экземпляры этих типов.
    /// Для текущей сборки это типы <see cref="AbstractSubscription"/>
    /// и <see cref="NonDefaultCtorSubscription"/>.
    /// </summary>
    [Test]
    public void Find_AssemblyWithAbstractAndNonDefultCtorSubscriptions_NoException()
    {
        // Arrange
        var finder = new TestSubscriptionMetadataFinder();

        // Act
        Action call = () => finder.Find();

        // Assert
        call.Should().NotThrow<MemberAccessException>();
        call.Should().NotThrow<NullReferenceException>();
    }

    [Test]
    public void Find_Call_SubscriptionFound()
    {
        // Arrange
        var finder = new TestSubscriptionMetadataFinder();

        // Act
        var subscription = finder.Find().SingleOrDefault();

        // Assert
        subscription.Should().NotBeNull();
        subscription.Should().BeOfType<ObjectChangingSubscription>();
    }

    private class TestSubscriptionMetadataFinder : SubscriptionMetadataFinder
    {
        protected override Assembly[] GetSubscriptionMetadataAssemblies()
        {
            return new[] { this.GetType().Assembly };
        }
    }
}
