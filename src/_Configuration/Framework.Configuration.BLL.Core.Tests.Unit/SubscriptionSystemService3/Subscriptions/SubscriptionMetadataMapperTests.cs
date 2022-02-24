using System;
using System.Linq;

using AutoFixture;
using AutoFixture.Idioms;

using FluentAssertions;
using Framework.Configuration.BLL.Core.Tests.Unit.SubscriptionSystemService3.Subscriptions.Metadata;
using Framework.Configuration.BLL.SubscriptionSystemService3;
using Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;
using Framework.Configuration.Domain;
using Framework.Configuration.SubscriptionModeling;
using Framework.UnitTesting;
using NUnit.Framework;
using NSubstitute;

namespace Framework.Configuration.BLL.Core.Tests.Unit.SubscriptionSystemService3.Subscriptions
{
    [TestFixture]
    public sealed class SubscriptionMetadataMapperTests : TestFixtureBase
    {
        private SubscriptionMetadataMapper mapper;
        private ConfigurationContextFacade configurationContextFacade;

        [SetUp]
        public void SetUp()
        {
            this.configurationContextFacade = this.Fixture.RegisterStub<ConfigurationContextFacade>();
            this.mapper = this.Fixture.Create<SubscriptionMetadataMapper>();
        }

        [Test]
        public void PublicSurface_NullArguments_ArgumentNullException()
        {
            // Arrange
            var assertion = new GuardClauseAssertion(this.Fixture);

            // Act

            // Assert
            assertion.Verify(typeof(SubscriptionMetadataMapper));
        }

        [Test]
        public void Map_SubscriptionTemplate_Subscription()
        {
            // Arrange
            var metadata = new ObjectChangingSubscription();
            var domainType = this.Fixture.Create<DomainType>();

            this.configurationContextFacade
                .GetDomainType(metadata.DomainObjectType)
                .Returns(domainType);

            // Act
            var subscription = this.mapper.Map(metadata);

            // Assert
            subscription.Should().NotBeNull();
            subscription.SenderName.Should().Be(metadata.SenderName);
            subscription.SenderEmail.Should().Be(metadata.SenderEmail);
            subscription.SendIndividualLetters.Should().Be(metadata.SendIndividualLetters);
            subscription.ExcludeCurrentUser.Should().Be(metadata.ExcludeCurrentUser);
            subscription.IncludeAttachments.Should().Be(metadata.IncludeAttachments);
            subscription.AllowEmptyListOfRecipients.Should().Be(metadata.AllowEmptyListOfRecipients);
            subscription.RazorMessageTemplateType.Should().Be(metadata.MessageTemplateType);
            subscription.RecepientsMode.Should().Be(metadata.RecepientsSelectorMode);
            subscription.SecurityItems.Should().HaveCount(1);
            subscription.MetadataSourceType.Should().Be(metadata.GetType());

            subscription.SubBusinessRoles.Should().HaveCount(metadata.SubBusinessRoleIds.Count());
            subscription.SubBusinessRoles.First().BusinessRoleId.Should().Be(metadata.SubBusinessRoleIds.First());

            subscription.SecurityItems.Should().HaveCount(1);
            CheckLambdaMapping(metadata.SecurityItemSourceLambdas.Single(), subscription.SecurityItems.First().Source);

            subscription.SecurityItems.First().ExpandType
                .Should().Be(metadata.SecurityItemSourceLambdas.Single().ExpandType);

            subscription.SecurityItems.First().Source.AuthDomainType
                .Should().Be(metadata.SecurityItemSourceLambdas.Single().AuthDomainType);

            CheckLambdaMapping(metadata.ConditionLambda, subscription.Condition);
            CheckLambdaMapping(metadata.GenerationLambda, subscription.Generation);
            CheckLambdaMapping(metadata.CopyGenerationLambda, subscription.CopyGeneration);
        }

        [Test]
        public void DomainObjectChangeType_Any_RequiredModePrevNull_RequiredModeNextNull()
        {
            // Arrange

            // Act

            // Assert
            this.CheckDomainObjectChangeType(DomainObjectChangeType.Any, null, null);
        }

        [Test]
        public void DomainObjectChangeType_Create_RequiredModePrevFalse_RequiredModeNextTrue()
        {
            // Arrange

            // Act

            // Assert
            this.CheckDomainObjectChangeType(DomainObjectChangeType.Create, false, true);
        }

        [Test]
        public void DomainObjectChangeType_Update_RequiredModePrevTrue_RequiredModeNextTrue()
        {
            // Arrange

            // Act

            // Assert
            this.CheckDomainObjectChangeType(DomainObjectChangeType.Update, true, true);
        }

        [Test]
        public void DomainObjectChangeType_Delete_RequiredModePrevTrue_RequiredModeNextFalse()
        {
            // Arrange

            // Act

            // Assert
            this.CheckDomainObjectChangeType(DomainObjectChangeType.Delete, true, false);
        }

        [Test]
        public void DomainObjectChangeType_CreateOrUpdate_RequiredModePrevNull_RequiredModeNextTrue()
        {
            // Arrange

            // Act

            // Assert
            this.CheckDomainObjectChangeType(DomainObjectChangeType.CreateOrUpdate, null, true);
        }

        [Test]
        public void DomainObjectChangeType_UpdateOrDelete_RequiredModePrevTrue_RequiredModeNextNull()
        {
            // Arrange

            // Act

            // Assert
            this.CheckDomainObjectChangeType(DomainObjectChangeType.UpdateOrDelete, true, null);
        }

        private static void CheckLambdaMapping(ILambdaMetadata expected, SubscriptionLambda actual)
        {
            actual.FuncValue.Should().Be(expected.Lambda);
            actual.MetadataSourceType.Should().Be(expected.GetType());
        }

        private void CheckDomainObjectChangeType(
            DomainObjectChangeType requirement,
            bool? prevExpectation,
            bool? nextExpectation)
        {
            // Arrange
            var metadata = new ObjectChangingSubscription();

            ((ILambdaMetadataBase)metadata.ConditionLambda)
                .SetDomainObjectChangeType(requirement);

            this.configurationContextFacade
                .GetDomainType(Arg.Any<Type>())
                .Returns(default(DomainType));

            // Act
            var subscription = this.mapper.Map(metadata);

            // Assert
            subscription.Condition.RequiredModePrev.Should().Be(prevExpectation);
            subscription.Condition.RequiredModeNext.Should().Be(nextExpectation);
        }
    }
}
