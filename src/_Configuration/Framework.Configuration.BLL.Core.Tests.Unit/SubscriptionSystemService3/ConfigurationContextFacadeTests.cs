using AutoFixture;
using AutoFixture.Idioms;

using FluentAssertions;
using Framework.Authorization.BLL;
using Framework.Authorization.Domain;
using Framework.Authorization.Notification;
using Framework.Configuration.BLL.SubscriptionSystemService3;
using Framework.Configuration.BLL.SubscriptionSystemService3.Recipients;
using Framework.Configuration.Domain;
using Framework.Core;
using Framework.DomainDriven.DAL.Revisions;
using Framework.Notification;
using Framework.UnitTesting;
using NUnit.Framework;

using NSubstitute;

namespace Framework.Configuration.BLL.Core.Tests.Unit.SubscriptionSystemService3;

[TestFixture]
public sealed class ConfigurationContextFacadeTests : TestFixtureBase
{
    private IConfigurationBLLContext context;
    private IEmployeeSource employeeSource;
    private INotificationPrincipalExtractor notificationPrincipalExtractor;
    private IAuthorizationBLLContext authorizationContext;
    private ITypeResolver<DomainType> domainTypeResolver;
    private IDomainTypeBLL domainTypeBll;
    private IEntityTypeBLL securityContextTypeBll;
    private ICodeFirstSubscriptionBLL codeFirstSubscriptionBLL;

    [SetUp]
    public void SetUp()
    {
        this.employeeSource = this.CreateStub<IEmployeeSource>();
        this.notificationPrincipalExtractor = this.CreateStub<INotificationPrincipalExtractor>();
        this.domainTypeResolver = this.CreateStub<ITypeResolver<DomainType>>();
        this.domainTypeBll = this.CreateStub<IDomainTypeBLL>();
        this.securityContextTypeBll = this.CreateStub<IEntityTypeBLL>();
        this.codeFirstSubscriptionBLL = this.CreateStub<ICodeFirstSubscriptionBLL>();

        var configurationLogics = this.CreateStub<IConfigurationBLLFactoryContainer>();
        configurationLogics.DomainType.Returns(this.domainTypeBll);
        configurationLogics.CodeFirstSubscription.Returns(this.codeFirstSubscriptionBLL);

        var authorizationLogics = this.CreateStub<IAuthorizationBLLFactoryContainer>();
        authorizationLogics.EntityType.Returns(this.securityContextTypeBll);

        this.authorizationContext = this.CreateStub<IAuthorizationBLLContext>();
        this.authorizationContext.Logics.Returns(authorizationLogics);
        this.authorizationContext.NotificationPrincipalExtractor.Returns(this.notificationPrincipalExtractor);

        this.context = this.Fixture.RegisterStub<IConfigurationBLLContext>();
        this.context.EmployeeSource.Returns(this.employeeSource);
        this.context.Logics.Returns(configurationLogics);
        this.context.Authorization.Returns(this.authorizationContext);
        this.context.ComplexDomainTypeResolver.Returns(this.domainTypeResolver);
    }

    [Test]
    public void PublicSurface_NullArguments_ArgumentNullException()
    {
        // Arrange
        var assertion = new GuardClauseAssertion(this.Fixture, new NullReferenceBehaviorExpectation());

        // Act

        // Assert
        assertion.Verify(typeof(ConfigurationContextFacade).GetConstructors());
        assertion.Verify(typeof(ConfigurationContextFacade).GetMethods().Where(m => m.Name != "CreateLogger"));
    }

    [Test]
    public void ConvertPrincipals_Call_Employee()
    {
        // Arrange
        var employees = new RecipientCollection(new[] { new Recipient("ivanov", "ivanov@ya.ru") });
        var principals = new[] { new Principal { Name = "ivanov" }, new Principal { Name = "petrov" } };

        this.employeeSource.GetQueryable().Returns(employees.AsQueryable());

        // Act
        var configurationContextFacade = this.Fixture.Create<ConfigurationContextFacade>();
        var employee = configurationContextFacade.ConvertPrincipals(principals).Single();

        // Assert
        employee.Login.Should().Be("ivanov");
        employee.Email.Should().Be("ivanov@ya.ru");
    }

    [Test]
    public void GetNotificationPrincipals_Call_CollectionOfPrincipals()
    {
        // Arrange
        var idents = this.Fixture.CreateMany<Guid>().ToArray();
        var principals = this.Fixture.CreateMany<Principal>();

        this.notificationPrincipalExtractor
            .GetNotificationPrincipalsByRoles(idents, Array.Empty<NotificationFilterGroup>())
            .Returns(principals);

        // Act
        var configurationContextFacade = this.Fixture.Create<ConfigurationContextFacade>();
        var result = configurationContextFacade.GetNotificationPrincipals(idents);

        // Assert
        result.Should().BeEquivalentTo(principals);
    }

    [Test]
    public void GetNotificationPrincipals2_Call_CollectionOfPrincipals()
    {
        // Arrange
        var idents = this.Fixture.CreateMany<Guid>().ToArray();
        var notificationFilterGroups = this.Fixture.CreateMany<NotificationFilterGroup>();
        var principals = this.Fixture.CreateMany<Principal>();

        this.notificationPrincipalExtractor
            .GetNotificationPrincipalsByRoles(idents, notificationFilterGroups)
            .Returns(principals);

        // Act
        var configurationContextFacade = this.Fixture.Create<ConfigurationContextFacade>();
        var result = configurationContextFacade.GetNotificationPrincipals(idents, notificationFilterGroups);

        // Assert
        result.Should().BeEquivalentTo(principals);
    }

    [Test]
    public void GetEntityType_Call_FoundEntityType()
    {
        //Arrange
        var domainTypeName = this.Fixture.Create<string>();
        var securityContextType = this.Fixture.Create<SecurityContextType>();

        this.authorizationContext
            .GetEntityType(domainTypeName)
            .Returns(securityContextType);

        // Act
        var configurationContextFacade = this.Fixture.Create<ConfigurationContextFacade>();
        var result = configurationContextFacade.GetEntityType(domainTypeName);

        // Assert
        result.Should().Be(securityContextType);
    }

    [Test]
    public void GetDomainType_Call_FoundDomainType()
    {
        // Arrange
        var domainObjectType = typeof(object);
        var domainType = this.Fixture.Create<DomainType>();

        this.context
            .GetDomainType(domainObjectType, true)
            .Returns(domainType);

        // Act
        var configurationContextFacade = this.Fixture.Create<ConfigurationContextFacade>();
        var result = configurationContextFacade.GetDomainType(domainObjectType);

        // Assert
        result.Should().Be(domainType);
    }

    [Test]
    public void GetDomainObjectTypeByDomainType_Call_FoundType()
    {
        // Arrange
        var domainObjectType = typeof(object);
        var domainType = this.Fixture.Create<DomainType>();

        this.domainTypeResolver
            .Resolve(domainType)
            .Returns(domainObjectType);

        // Act
        var configurationContextFacade = this.Fixture.Create<ConfigurationContextFacade>();
        var result = configurationContextFacade.GetDomainObjectType(domainType);

        // Assert
        result.Should().Be(typeof(object));
    }

    [Test]
    public void GetDomainObjectTypeByTypeInfoDescription_Call_FoundType()
    {
        // Arrange
        var domainObjectType = typeof(object);
        var typeInfoDescription = this.Fixture.Create<TypeInfoDescription>();
        var domainType = this.Fixture.Create<DomainType>();

        this.context
            .GetDomainType(typeInfoDescription)
            .Returns(domainType);

        this.domainTypeResolver
            .Resolve(domainType)
            .Returns(domainObjectType);

        // Act
        var configurationContextFacade = this.Fixture.Create<ConfigurationContextFacade>();
        var result = configurationContextFacade.GetDomainObjectType(typeInfoDescription);

        // Assert
        result.Should().Be(domainObjectType);
    }

    [Test]
    public void GetDomainObjectType_TypeNotFound_Exception()
    {
        // Arrange
        this.domainTypeResolver
            .Resolve(Arg.Any<DomainType>())
            .Returns(default(Type));

        // Act
        var configurationContextFacade = this.Fixture.Create<ConfigurationContextFacade>();
        Action call = () => configurationContextFacade.GetDomainObjectType(this.Fixture.Create<DomainType>());

        // Assert
        call.Should().Throw<SubscriptionServicesException>();
    }


    [Test]
    public void GetActiveCodeFirstSubscriptionCodes_Call_CollectionOfCodes()
    {
        // Arrange
        var expectedCollection = this.Fixture.CreateMany<string>();

        var configurationContextFacade = this.Fixture.Create<ConfigurationContextFacade>();

        this.codeFirstSubscriptionBLL
            .GetActiveCodeFirstSubscriptionCodes()
            .Returns(expectedCollection);

        // Act
        var actualCollection = configurationContextFacade.GetActiveCodeFirstSubscriptionCodes();

        // Assert
        actualCollection.Should().BeEquivalentTo(expectedCollection);
    }
}
