using AutoFixture.Idioms;

using Framework.Configuration.BLL.SubscriptionSystemService3.Recipients;
using Framework.UnitTesting;
using NUnit.Framework;

namespace Framework.Configuration.BLL.Core.Tests.Unit.SubscriptionSystemService3.Recipients
{
    [TestFixture]
    public sealed class RecipientsBagTests : TestFixtureBase
    {
        [Test]
        public void PublicSurface_NullArguments_ArgumentNullException()
        {
            // Arrange
            var assertion = new GuardClauseAssertion(this.Fixture);

            // Act

            // Assert
            assertion.Verify(typeof(RecipientsBag));
        }

        [Test]
        public void Ctor_NotNullArguments_PropertiesInitialized()
        {
            // Arrange
            var assertion = new ConstructorInitializedMemberAssertion(this.Fixture);

            // Act

            // Assert
            assertion.Verify(typeof(RecipientsBag));
        }
    }
}
