using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using AutoFixture;
using AutoFixture.Idioms;

using FluentAssertions;
using Framework.Authorization.BLL;
using Framework.Authorization.Domain;
using Framework.Configuration.BLL.SubscriptionSystemService3;
using Framework.Configuration.BLL.SubscriptionSystemService3.Lambdas;
using Framework.Configuration.BLL.SubscriptionSystemService3.Recipients;
using Framework.Configuration.Core;
using Framework.Configuration.Domain;
using Framework.DomainDriven.BLL.Security;
using Framework.ExpressionParsers;
using Framework.Persistent;
using Framework.UnitTesting;
using NUnit.Framework;
using NSubstitute;

namespace Framework.Configuration.BLL.Core.Tests.Unit.SubscriptionSystemService3.Recipients
{
    [TestFixture]
    public sealed class ByRolesRecipientsResolverTests : TestFixtureBase
    {
        private SecurityItemSourceLambdaProcessor<ITestBLLContext> securityItemSourceLambdaProcessor;
        private ConfigurationContextFacade configurationContextFacade;

        [SetUp]
        public void SetUp()
        {
            this.securityItemSourceLambdaProcessor = this.Fixture.RegisterStub<SecurityItemSourceLambdaProcessor<ITestBLLContext>>();
            this.configurationContextFacade = this.Fixture.RegisterStub<ConfigurationContextFacade>();

            var lambdaProcessorFactory = this.Fixture.RegisterStub<LambdaProcessorFactory<ITestBLLContext>>();

            lambdaProcessorFactory
                .Create<SecurityItemSourceLambdaProcessor<ITestBLLContext>>()
                .Returns(this.securityItemSourceLambdaProcessor);
        }

        [Test]
        public void PublicSurface_NullArguments_ArgumentNullException()
        {
            // Arrange
            var assertion = new GuardClauseAssertion(this.Fixture);

            // Act

            // Assert
            assertion.Verify(typeof(ByRolesRecipientsResolver<ITestBLLContext>));
        }

        [Test]
        public void Resolve_InvalidSourceMode_EmptyResult()
        {
            // Arrange
            var versions = this.Fixture.Create<DomainObjectVersions<string>>();

            var subscription = this.Fixture
                .Build<Subscription>()
                .With(s => s.DynamicSource, new SubscriptionLambda())
                .With(s => s.DynamicSourceExpandType, NotificationExpandType.All)
                .Create();

            var si = new SubscriptionSecurityItem(subscription);

            subscription.SecurityItems = new[] { si };

            // Act
            var resolver = this.Fixture.Create<ByRolesRecipientsResolver<ITestBLLContext>>();
            var result = resolver.Resolve(subscription, versions);

            // Assert
            result.Should().HaveCount(0);
        }

        [Test]
        public void Resolve_DynamicSourceMode_Recipient()
        {
            // Arrange
            var businessRole = this.Fixture.Create<SubBusinessRole>();
            var buisnessRoleIds = new[] { businessRole.BusinessRoleId };

            var versions = this.Fixture.Create<DomainObjectVersions<string>>();
            var fid = new FilterItemIdentity("name", Guid.NewGuid());
            var entityType = this.Fixture.Create<EntityType>();

            var principals = new[] { this.Fixture.Create<Principal>() };
            var employees = new RecipientCollection(new[] { this.Fixture.Create<Recipient>() });

            var subscription = this.Fixture
                .Build<Subscription>()
                .With(s => s.DynamicSource, new SubscriptionLambda())
                .With(s => s.DynamicSourceExpandType, NotificationExpandType.All)
                .Create();

            ((List<SubBusinessRole>)subscription.SubBusinessRoles).Add(businessRole);

            this.configurationContextFacade
                .GetNotificationPrincipals(
                    Arg.Is<Guid[]>(v => v.SequenceEqual(buisnessRoleIds)),
                    Arg.Is<IEnumerable<NotificationFilterGroup>>(v => v != null))
                .Returns(principals);

            this.configurationContextFacade
                .GetEntityType(fid.EntityName.ToLowerInvariant())
                .Returns(entityType);

            this.configurationContextFacade
                .ConvertPrincipals(principals)
                .Returns(employees);

            // Act
            var resolver = this.Fixture.Create<ByRolesRecipientsResolver<ITestBLLContext>>();
            var recipient = resolver.Resolve(subscription, versions).Single();

            // Assert
            recipient.Login.Should().Be(employees.Single().Login);
            recipient.Email.Should().Be(employees.Single().Email);
        }

        [Test]
        public void Resolve_NonContextSourceMode_Recipient()
        {
            // Arrange
            var versions = this.Fixture.Create<DomainObjectVersions<string>>();

            var businessRole = this.Fixture.Create<SubBusinessRole>();
            var buisnessRoleIds = new[] { businessRole.BusinessRoleId };

            var principals = new[] { this.Fixture.Create<Principal>() };
            var employees = new RecipientCollection(new[] { this.Fixture.Create<Recipient>() });

            var subscription = this.Fixture
                .Build<Subscription>()
                .With(s => s.DynamicSource, default(SubscriptionLambda))
                .With(s => s.DynamicSourceExpandType)
                .Create();

            ((List<SubBusinessRole>)subscription.SubBusinessRoles).Add(businessRole);

            this.configurationContextFacade
                .GetNotificationPrincipals(Arg.Is<Guid[]>(v => v.SequenceEqual(buisnessRoleIds)))
                .Returns(principals);

            this.configurationContextFacade
                .ConvertPrincipals(principals)
                .Returns(employees);

            // Act
            var resolver = this.Fixture.Create<ByRolesRecipientsResolver<ITestBLLContext>>();
            var recipient = resolver.Resolve(subscription, versions).Single();

            // Assert
            recipient.Login.Should().Be(employees.Single().Login);
            recipient.Email.Should().Be(employees.Single().Email);
        }

        [Test]
        public void Resolve_TypedSourceMode_Recipient()
        {
            // Arrange
            var versions = this.Fixture.Create<DomainObjectVersions<string>>();

            var securityItem = this.Fixture
                .Build<SubscriptionSecurityItem>()
                .With(item => item.Source,  default(SubscriptionLambda))
                .Create();

            var businessRole = this.Fixture.Create<SubBusinessRole>();
            var buisnessRoleIds = new[] { businessRole.BusinessRoleId };

            var identityObject = this.Fixture.Create<IdentityObject>();

            var principals = new[] { this.Fixture.Create<Principal>() };
            var employees = new RecipientCollection(new[] { this.Fixture.Create<Recipient>() });

            var subscription = this.Fixture
                .Build<Subscription>()
                .With(s => s.DynamicSource, default(SubscriptionLambda))
                .With(s => s.DynamicSourceExpandType)
                .Create();

            ((List<SubBusinessRole>)subscription.SubBusinessRoles).Add(businessRole);
            ((List<SubscriptionSecurityItem>)subscription.SecurityItems).Add(securityItem);

            this.configurationContextFacade
                .GetNotificationPrincipals(
                    Arg.Is<Guid[]>(v => v.SequenceEqual(buisnessRoleIds)),
                    Arg.Is<IEnumerable<NotificationFilterGroup>>(v => v != null))
                .Returns(principals);

            this.securityItemSourceLambdaProcessor
                .Invoke<string, IdentityObject>(securityItem, versions)
                .Returns(new[] { identityObject });

            this.configurationContextFacade
                .ConvertPrincipals(principals)
                .Returns(employees);

            // Act
            var resolver = this.Fixture.Create<ByRolesRecipientsResolver<ITestBLLContext>>();
            var recipient = resolver.Resolve(subscription, versions).Single();

            // Assert
            recipient.Login.Should().Be(employees.Single().Login);
            recipient.Email.Should().Be(employees.Single().Email);
        }

        [Test]
        public void Resolve_TypedSourceMode_AuthDomainTypeSpecified_Recipient()
        {
            // Arrange
            var versions = this.Fixture.Create<DomainObjectVersions<string>>();
            var sourceLambda = new SubscriptionLambda { AuthDomainType = typeof(IdentityObject) };

            var securityItem = this.Fixture
                .Build<SubscriptionSecurityItem>().With(item => item.Source, sourceLambda)
                .Create();

            var businessRole = this.Fixture.Create<SubBusinessRole>();
            var buisnessRoleIds = new[] { businessRole.BusinessRoleId };

            var identityObject = this.Fixture.Create<IdentityObject>();

            var principals = new[] { this.Fixture.Create<Principal>() };
            var employees = new RecipientCollection(new[] { this.Fixture.Create<Recipient>() });

            var subscription = this.Fixture
                .Build<Subscription>()
                .With(s => s.DynamicSource, default(SubscriptionLambda))
                .With(s => s.DynamicSourceExpandType)
                .Create();

            ((List<SubBusinessRole>)subscription.SubBusinessRoles).Add(businessRole);
            ((List<SubscriptionSecurityItem>)subscription.SecurityItems).Add(securityItem);

            this.configurationContextFacade
                .GetNotificationPrincipals(
                    Arg.Is<Guid[]>(v => v.SequenceEqual(buisnessRoleIds)),
                    Arg.Is<IEnumerable<NotificationFilterGroup>>(z => z != null))
                .Returns(principals);

            this.securityItemSourceLambdaProcessor
                .Invoke<string, IdentityObject>(securityItem, versions)
                .Returns(new[] { identityObject });

            this.configurationContextFacade
                .ConvertPrincipals(principals)
                .Returns(employees);

            // Act
            var resolver = this.Fixture.Create<ByRolesRecipientsResolver<ITestBLLContext>>();
            var recipient = resolver.Resolve(subscription, versions).Single();

            // Assert
            recipient.Login.Should().Be(employees.Single().Login);
            recipient.Email.Should().Be(employees.Single().Email);
        }
    }
}
