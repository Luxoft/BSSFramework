using System;
using System.Collections.Generic;
using System.Linq;

using AutoFixture;
using AutoFixture.Idioms;

using FluentAssertions;
using Framework.Configuration.BLL.SubscriptionSystemService3;
using Framework.Configuration.BLL.SubscriptionSystemService3.Services;
using Framework.Configuration.Core;
using Framework.Configuration.Domain;
using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.DAL.Revisions;
using Framework.UnitTesting;
using NUnit.Framework;
using NSubstitute;

namespace Framework.Configuration.BLL.Core.Tests.Unit.SubscriptionSystemService3
{
    public sealed class RevisionSubscriptionSystemServiceTests : TestFixtureBase
    {
        private SubscriptionNotificationService<ITestBLLContext> notificationService;
        private RevisionService<IdentityObject> revisionService;
        private ConfigurationContextFacade configurationContextFacade;

        [SetUp]
        public void SetUp()
        {
            this.notificationService = this.Fixture.RegisterStub<SubscriptionNotificationService<ITestBLLContext>>();
            this.revisionService = this.Fixture.RegisterStub<RevisionService<IdentityObject>>();
            this.configurationContextFacade = this.Fixture.RegisterDynamicMock<ConfigurationContextFacade>();

            var servicesFactory = this.Fixture.RegisterStub<SubscriptionServicesFactory<ITestBLLContext, IdentityObject>>();
            servicesFactory.CreateNotificationService().Returns(this.notificationService);
            servicesFactory.CreateRevisionService<IdentityObject >().Returns(this.revisionService);
            servicesFactory.CreateConfigurationContextFacade().Returns(this.configurationContextFacade);
        }

        [Test]
        public void PublicSurface_NullArguments_ArgumentNullException()
        {
            // Arrange
            var assertion = new GuardClauseAssertion(this.Fixture, new NullReferenceBehaviorExpectation());
            var targetType = typeof(RevisionSubscriptionSystemService<ITestBLLContext, IdentityObject >);

            // Act

            // Assert
            assertion.Verify(targetType.GetConstructors());
            assertion.Verify(
                targetType.GetMethods()
                .Where(mi => mi.Name != "GetRecipientsUntyped" && mi.Name != "ProcessChangedObjectUntyped"));
        }

        [Test]
        public void ProcessByObjectModifications_Call_NonEmptyTryResultCollection()
        {
            // Arrange
            var modifications = this.Fixture.Create<ObjectModificationInfo<Guid>>();
            var versions = this.Fixture.Create<DomainObjectVersions<IdentityObject>>();
            var expectedResult = this.CreateStub<IList<ITryResult<Subscription>>>();

            this.configurationContextFacade
                .GetDomainObjectType(modifications.TypeInfo)
                .Returns(typeof(IdentityObject));

            this.revisionService
                .GetDomainObjectVersions(modifications.Identity, modifications.Revision)
                .Returns(versions);

            this.notificationService
                .NotifyDomainObjectChanged(versions)
                .Returns(expectedResult);

            // Act
            var sut = this.Fixture.Create<RevisionSubscriptionSystemService<ITestBLLContext, IdentityObject >>();
            var actualResult = sut.Process(modifications);

            // Assert
            actualResult.Should().BeSameAs(expectedResult);
        }

        [Test]
        public void ProcessByObjectModifications_NoActiveSubscribtions_EmptyTryResultCollection()
        {
            // Arrange
            var modifications = this.Fixture.Create<ObjectModificationInfo<Guid>>();

            this.configurationContextFacade
                .GetDomainObjectType(modifications.TypeInfo)
                .Returns(typeof(IdentityObject));

            this.revisionService
                .GetDomainObjectVersions(modifications.Identity, modifications.Revision)
                .Returns(default(DomainObjectVersions<IdentityObject>));

            // Act
            var sut = this.Fixture.Create<RevisionSubscriptionSystemService<ITestBLLContext, IdentityObject >>();
            var result = sut.Process(modifications);

            // Assert
            result.Should().HaveCount(0);
        }

        [Test]
        public void GetObjectModifications_Call_NonEmptyObjectModificationCollection()
        {
            // Arrange
            var changes = this.Fixture.Create<DALChanges>();
            var expectedResult = this.Fixture.CreateMany<ObjectModificationInfo<Guid>>();

            this.revisionService
                .GetObjectModifications(changes)
                .Returns(expectedResult);

            // Act
            var sut = this.Fixture.Create<RevisionSubscriptionSystemService<ITestBLLContext, IdentityObject>>();
            var actualResult = sut.GetObjectModifications(changes);

            // Assert
            actualResult.Should().BeSameAs(expectedResult);
        }
    }
}
