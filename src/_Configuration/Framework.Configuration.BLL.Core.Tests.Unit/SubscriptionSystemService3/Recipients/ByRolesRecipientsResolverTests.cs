using AutoFixture;
using AutoFixture.Idioms;

using FluentAssertions;

using Framework.Authorization.Domain;
using Framework.Authorization.Notification;
using Framework.Configuration.BLL.SubscriptionSystemService3;
using Framework.Configuration.BLL.SubscriptionSystemService3.Lambdas;
using Framework.Configuration.BLL.SubscriptionSystemService3.Recipients;
using Framework.Configuration.Core;
using Framework.Configuration.Domain;
using Framework.DomainDriven;
using Framework.SecuritySystem;
using Framework.UnitTesting;
using NUnit.Framework;
using NSubstitute;

namespace Framework.Configuration.BLL.Core.Tests.Unit.SubscriptionSystemService3.Recipients;

[TestFixture]
public sealed class ByRolesRecipientsResolverTests : TestFixtureBase
{
    private SecurityItemSourceLambdaProcessor<ITestBLLContext> securityItemSourceLambdaProcessor;
    private DynamicSourceLambdaProcessor<ITestBLLContext> dynamicSourceLambdaProcessor;
    private ConfigurationContextFacade configurationContextFacade;

    [SetUp]
    public void SetUp()
    {
        //var subscriptionDynamicSourceLegacyLambdaProcessorMock = this.Fixture.RegisterStub<>()

        //this.parserFactory.GetBySubscriptionDynamicSourceLegacy<string>().Returns(new SubscriptionDynamicSourceLegacyLambdaProcessor<string>())

        this.securityItemSourceLambdaProcessor = this.Fixture.RegisterStub<SecurityItemSourceLambdaProcessor<ITestBLLContext>>();
        this.dynamicSourceLambdaProcessor = this.Fixture.RegisterStub<DynamicSourceLambdaProcessor<ITestBLLContext>>();
        this.configurationContextFacade = this.Fixture.RegisterStub<ConfigurationContextFacade>();

        var lambdaProcessorFactory = this.Fixture.RegisterStub<LambdaProcessorFactory<ITestBLLContext>>();

        lambdaProcessorFactory
                .Create<SecurityItemSourceLambdaProcessor<ITestBLLContext>>()
                .Returns(this.securityItemSourceLambdaProcessor);

        lambdaProcessorFactory
                .Create<DynamicSourceLambdaProcessor<ITestBLLContext>>()
                .Returns(this.dynamicSourceLambdaProcessor);
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
                               .With(s => s.SecurityItems, new List<SubscriptionSecurityItem>())
                               .With(s => s.SubBusinessRoles, new List<SubBusinessRole>())
                               .Create();

        ((List<SubscriptionSecurityItem>)subscription.SecurityItems).Add(new SubscriptionSecurityItem(subscription));

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
        var businessRoleIds = new[] { businessRole.SecurityRole };

        var versions = this.Fixture.Create<DomainObjectVersions<string>>();
        var fid = new FilterItemIdentity(typeof(object), Guid.NewGuid());
        var securityContextType = this.Fixture.Create<SecurityContextType>();
        var securityType = typeof(object);

        var principals = new[] { this.Fixture.Create<Principal>() };
        var employees = new RecipientCollection(new[] { this.Fixture.Create<Recipient>() });

        var subscription = this.Fixture
                               .Build<Subscription>()
                               .With(s => s.DynamicSource, new SubscriptionLambda())
                               .With(s => s.DynamicSourceExpandType, NotificationExpandType.All)
                               .With(s => s.SecurityItems, new List<SubscriptionSecurityItem>())
                               .With(s => s.SubBusinessRoles, new List<SubBusinessRole>())
                               .Create();

        ((List<SubBusinessRole>)subscription.SubBusinessRoles).Add(businessRole);

        this.configurationContextFacade
            .GetNotificationPrincipals(
                                       Arg.Is<SecurityRole[]>(v => v.SequenceEqual(businessRoleIds)),
                                       Arg.Is<IEnumerable<NotificationFilterGroup>>(v => v != null))
            .Returns(principals);

        this.dynamicSourceLambdaProcessor
            .Invoke(subscription, versions)
            .Returns(new[] { fid });

        this.configurationContextFacade
            .GetSecurityType(securityContextType)
            .Returns(securityType);

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
        var businessRoleIds = new[] { businessRole.SecurityRole };

        var principals = new[] { this.Fixture.Create<Principal>() };
        var employees = new RecipientCollection(new[] { this.Fixture.Create<Recipient>() });

        var subscription = this.Fixture
                               .Build<Subscription>()
                               .With(s => s.DynamicSource, default(SubscriptionLambda))
                               .With(s => s.DynamicSourceExpandType)
                               .With(s => s.SecurityItems, new List<SubscriptionSecurityItem>())
                               .With(s => s.SubBusinessRoles, new List<SubBusinessRole>())
                               .Create();

        ((List<SubBusinessRole>)subscription.SubBusinessRoles).Add(businessRole);

        this.configurationContextFacade
            .GetNotificationPrincipals(Arg.Is<SecurityRole[]>(v => v.SequenceEqual(businessRoleIds)))
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

        var subscription = this.Fixture
                               .Build<Subscription>()
                               .With(s => s.DynamicSource, default(SubscriptionLambda))
                               .With(s => s.DynamicSourceExpandType)
                               .With(s => s.SecurityItems, new List<SubscriptionSecurityItem>())
                               .With(s => s.SubBusinessRoles, new List<SubBusinessRole>())
                               .Create();

        var securityItem = this.Fixture
                               .Build<SubscriptionSecurityItem>()
                               .With(item => item.Source, new SubscriptionLambda() { AuthDomainType = typeof(IdentityObject) })
                               .With(item => item.Subscription, subscription)
                               .Create();

        var businessRole = this.Fixture.Build<SubBusinessRole>().With(item => item.Subscription, subscription).Create();
        var businessRoleIds = new[] { businessRole.SecurityRole };

        var identityObject = this.Fixture.Create<IdentityObject>();

        var principals = new[] { this.Fixture.Create<Principal>() };
        var employees = new RecipientCollection(new[] { this.Fixture.Create<Recipient>() });


        ((List<SubBusinessRole>)subscription.SubBusinessRoles).Add(businessRole);
        ((List<SubscriptionSecurityItem>)subscription.SecurityItems).Add(securityItem);

        this.configurationContextFacade
            .GetNotificationPrincipals(
                                       Arg.Is<SecurityRole[]>(v => v.SequenceEqual(businessRoleIds)),
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
        var businessRoleIds = new[] { businessRole.SecurityRole };

        var identityObject = this.Fixture.Create<IdentityObject>();

        var principals = new[] { this.Fixture.Create<Principal>() };
        var employees = new RecipientCollection(new[] { this.Fixture.Create<Recipient>() });

        var subscription = this.Fixture
                               .Build<Subscription>()
                               .With(s => s.DynamicSource, default(SubscriptionLambda))
                               .With(s => s.DynamicSourceExpandType)
                               .With(s => s.SecurityItems, new List<SubscriptionSecurityItem>())
                               .With(s => s.SubBusinessRoles, new List<SubBusinessRole>())
                               .Create();

        ((List<SubBusinessRole>)subscription.SubBusinessRoles).Add(businessRole);
        ((List<SubscriptionSecurityItem>)subscription.SecurityItems).Add(securityItem);

        //this.configurationContextFacade
        //    .GetSecurityType(securityItem.Id)
        //    .Returns(default(Type))
        //.Repeat.Never()
        ;

        this.configurationContextFacade
            .GetNotificationPrincipals(
                                       Arg.Is<SecurityRole[]>(v => v.SequenceEqual(businessRoleIds)),
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
