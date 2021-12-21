using System;
using System.Linq;

using AutoFixture;
using AutoFixture.Idioms;

using FluentAssertions;
using Framework.Configuration.BLL.SubscriptionSystemService3;
using Framework.Configuration.BLL.SubscriptionSystemService3.Services;
using Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;
using Framework.Configuration.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.DAL.Revisions;
using Framework.UnitTesting;
using NUnit.Framework;
using NSubstitute;

namespace Framework.Configuration.BLL.Core.Tests.Unit.SubscriptionSystemService3.Services
{
    [TestFixture]
    public class RevisionServiceTests : TestFixtureBase
    {
        private IRevisionBLL<IdentityObject, Guid> revisionBll;
        private SubscriptionResolver subscriptionResolver;

        [SetUp]
        public void SetUp()
        {
            this.revisionBll = this.Fixture.RegisterStub<IRevisionBLL<IdentityObject, Guid>>();
            this.subscriptionResolver = this.Fixture.RegisterDynamicMock<SubscriptionResolver>();
        }

        [Test]
        public void PublicSurface_NullArguments_ArgumentNullException()
        {
            // Arrange
            var assertion = new GuardClauseAssertion(this.Fixture, new NullReferenceBehaviorExpectation());

            // Act

            // Assert
            assertion.Verify(typeof(RevisionService<IdentityObject>));
        }

        [Test]
        public void GetDomainObjectVersions_NoActiveSubscriptions_Null()
        {
            // Arrange
            var domainObjectId = this.Fixture.Create<Guid>();
            var revisionNumber = this.Fixture.Create<long?>();

            this.subscriptionResolver
                .IsActiveSubscriptionForTypeExists(typeof(IdentityObject))
                .Returns(false);

            // Act
            var service = this.Fixture.Create<RevisionService<IdentityObject>>();
            var versions = service.GetDomainObjectVersions(domainObjectId, revisionNumber);

            // Assert
            versions.Should().BeNull();
        }

        [Test]
        public void GetDomainObjectVersions_NonNullRevisionNumber_ExpectedVersions()
        {
            // Arrange
            var domainObjectId = this.Fixture.Create<Guid>();
            var revisionNumber = this.Fixture.Create<long>();
            var prev = this.Fixture.Create<IdentityObject>();
            var next = this.Fixture.Create<IdentityObject>();
            var expected = new DomainObjectVersions<IdentityObject>(prev, next);

            this.subscriptionResolver
                .IsActiveSubscriptionForTypeExists(typeof(IdentityObject))
                .Returns(true);

            this.revisionBll
                .GetPreviousRevision(domainObjectId, revisionNumber)
                .Returns(revisionNumber - 1);

            this.revisionBll
                .GetObjectByRevision(domainObjectId, revisionNumber - 1)
                .Returns(prev);

            this.revisionBll
                .GetObjectByRevision(domainObjectId, revisionNumber)
                .Returns(next);

            // Act
            var service = this.Fixture.Create<RevisionService<IdentityObject>>();
            var actual = service.GetDomainObjectVersions(domainObjectId, revisionNumber);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetDomainObjectVersions_NullRevisionNumber_ExpectedVersions()
        {
            // Arrange
            var domainObjectId = this.Fixture.Create<Guid>();

            var revisions = new DomainObjectRevision<Guid>(Guid.Empty);

            var revision = new DomainObjectRevisionInfo<Guid>(
                revisions,
                AuditRevisionType.Added,
                "aa",
                DateTime.Now,
                3);

            var prev = this.Fixture.Create<IdentityObject>();
            var next = this.Fixture.Create<IdentityObject>();
            var expected = new DomainObjectVersions<IdentityObject>(prev, next);

            this.subscriptionResolver
                .IsActiveSubscriptionForTypeExists(typeof(IdentityObject))
                .Returns(true);

            this.revisionBll
                .GetObjectRevisions(domainObjectId)
                .Returns(revisions);

            this.revisionBll
                .GetPreviousRevision(domainObjectId, revision.RevisionNumber)
                .Returns(revision.RevisionNumber - 1);

            this.revisionBll
                .GetObjectByRevision(domainObjectId, revision.RevisionNumber - 1)
                .Returns(prev);

            this.revisionBll
                .GetObjectRevisions(domainObjectId)
                .Returns(revisions);

            this.revisionBll
                .GetObjectByRevision(domainObjectId, revision.RevisionNumber)
                .Returns(next);

            // Act
            var service = this.Fixture.Create<RevisionService<IdentityObject>>();
            var actual = service.GetDomainObjectVersions(domainObjectId, null);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetDomainObjectVersions_NullRevisionNumberAndNoPreviousRevision_ExpectedVersions()
        {
            // Arrange
            var domainObjectId = this.Fixture.Create<Guid>();

            var revisions = new DomainObjectRevision<Guid>(Guid.Empty);

            var revision = new DomainObjectRevisionInfo<Guid>(
                revisions,
                AuditRevisionType.Added,
                "aa",
                DateTime.Now,
                3);

            var next = this.Fixture.Create<IdentityObject>();
            var expected = new DomainObjectVersions<IdentityObject>(null, next);

            this.subscriptionResolver
                .IsActiveSubscriptionForTypeExists(typeof(IdentityObject))
                .Returns(true);

            this.revisionBll
                .GetObjectRevisions(domainObjectId)
                .Returns(revisions);

            this.revisionBll
                .GetPreviousRevision(domainObjectId, revision.RevisionNumber)
                .Returns(default(long?));

            this.revisionBll
                .GetObjectRevisions(domainObjectId)
                .Returns(revisions);

            this.revisionBll
                .GetObjectByRevision(domainObjectId, revision.RevisionNumber)
                .Returns(next);

            // Act
            var service = this.Fixture.Create<RevisionService<IdentityObject>>();
            var actual = service.GetDomainObjectVersions(domainObjectId, null);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetObjectModifications_Call_CollectionOfModificationInfos()
        {
            // Arrange
            var created = this.Fixture.Create<DALObject<IdentityObject>>();
            var updated = this.Fixture.Create<DALObject<IdentityObject>>();
            var removed = this.Fixture.Create<DALObject<IdentityObject>>();

            var changes = new DALChanges(new [] {created}, new[] { updated}, new[] { removed});
            var revisionNumber = this.Fixture.Create<long>();

            this.revisionBll
                .GetCurrentRevision()
                .Returns(revisionNumber);

            this.subscriptionResolver
                .IsActiveSubscriptionForTypeExists(typeof(IdentityObject))
                .Returns(true);

            // Act
            var service = this.Fixture.Create<RevisionService<IdentityObject>>();
            var modificationInfos = service.GetObjectModifications(changes).ToList();

            // Assert
            modificationInfos.Should().HaveCount(3);
            modificationInfos[0].Identity.Should().Be(created.Object.Id);
            modificationInfos[1].Identity.Should().Be(updated.Object.Id);
            modificationInfos[2].Identity.Should().Be(removed.Object.Id);
        }

        [Test]
        public void GetObjectModifications_ZeroRevision_EmptyResult()
        {
            // Arrange
            var created = this.Fixture.Create<DALObject<IdentityObject>>();
            var updated = this.Fixture.Create<DALObject<IdentityObject>>();
            var removed = this.Fixture.Create<DALObject<IdentityObject>>();

            var changes = new DALChanges(new[] { created }, new[] { updated }, new[] { removed });

            this.revisionBll
                .GetCurrentRevision()
                .Returns(0);

            // Act
            var service = this.Fixture.Create<RevisionService<IdentityObject>>();
            var modificationInfos = service.GetObjectModifications(changes).ToList();

            // Assert
            modificationInfos.Should().BeEmpty();
        }
    }
}
