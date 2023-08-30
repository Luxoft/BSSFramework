using AutoFixture;
using AutoFixture.Idioms;

using FluentAssertions;

using Framework.Configuration.BLL.SubscriptionSystemService3.Recipients;
using Framework.Configuration.Core;
using Framework.Configuration.Domain;
using Framework.UnitTesting;
using NUnit.Framework;
using NSubstitute;

namespace Framework.Configuration.BLL.Core.Tests.Unit.SubscriptionSystemService3.Recipients;

[TestFixture]
public sealed class RecipientsResolverTests : TestFixtureBase
{
    private ByRolesRecipientsResolver<ITestBLLContext> rolesResolver;
    private GenerationRecipientsResolver<ITestBLLContext> generationResolver;

    [SetUp]
    public void SetUp()
    {
        this.rolesResolver  = this.Fixture.RegisterStub<ByRolesRecipientsResolver<ITestBLLContext>>();
        this.generationResolver = this.Fixture.RegisterStub<GenerationRecipientsResolver<ITestBLLContext>>();
    }

    [Test]
    public void PublicSurface_NullArguments_ArgumentNullException()
    {
        // Arrange
        var assertion = new GuardClauseAssertion(this.Fixture);

        // Act

        // Assert
        assertion.Verify(typeof(RecipientsResolver<ITestBLLContext>));
    }

    [Test]
    public void Resolve_Call_RecipientsBag()
    {
        // Arrange
        var recipient1 = new Recipient("ivanov", "ivanov@ya.ru");
        var recipient2 = new Recipient("petrov", "petrov@ya.ru");
        var recipient3 = new Recipient("sidorov", "sidorov@ya.ru");
        var replyTo = new Recipient("replayTo", "replayTo@ya.ru");

        var recipientsBag = new RecipientsBag(
                                              new RecipientCollection(new []{recipient2}),
                                              new RecipientCollection(new []{recipient3}),
                                              new RecipientCollection(new[] { replyTo }));

        var generationResolverResult = new RecipientsResolverResult(
                                                                    recipientsBag,
                                                                    this.Fixture.Create<DomainObjectVersions<object>>());

        var subscription = this.Fixture
                               .Build<Subscription>()
                               .With(s => s.RecepientsMode, RecepientsSelectorMode.Union)
                               .Create();

        var versions = this.Fixture.Create<DomainObjectVersions<string>>();

        this.rolesResolver
            .Resolve(subscription, versions)
            .Returns(new RecipientCollection(new[] {recipient1}));

        this.generationResolver
            .Resolve(subscription, versions)
            .Returns(new[] { generationResolverResult });

        // Act
        var resolver = this.Fixture.Create<RecipientsResolver<ITestBLLContext>>();
        var resolverResult = resolver.Resolve(subscription, versions).Single();
        var resultBag = resolverResult.RecipientsBag;

        // Assert
        resultBag.To.Should().BeEquivalentTo(new RecipientCollection(new[] {recipient1, recipient2}));
        resultBag.Cc.Should().BeEquivalentTo(new RecipientCollection(new[] {recipient3}));
        resultBag.ReplyTo.Should().BeEquivalentTo(new RecipientCollection(new[] { replyTo }));
    }

    [Test]
    public void Resolve_ExcludeCurrentUser_UsersExcludedFromRecipientsBag()
    {
        // Arrange
        var recipient1 = new Recipient("ivanov", "ivanov@ya.ru");
        var recipient2 = new Recipient("petrov", "petrov@ya.ru");
        var recipient3 = new Recipient("sidorov", "sidorov@ya.ru");
        var recipient4 = new Recipient("kuznetcov", "kuznetcov@ya.ru");
        var replayTo = new Recipient("replayTo", "replayTo@ya.ru");

        var recipientsBag = new RecipientsBag(
                                              new RecipientCollection(new[] { recipient1, recipient2 }),
                                              new RecipientCollection(new[] { recipient3, recipient4 }),
                                              new RecipientCollection(new[] { replayTo}));

        var subscription = this.Fixture
                               .Build<Subscription>()
                               .With(s => s.ExcludeCurrentUser, true)
                               .With(s => s.RecepientsMode, RecepientsSelectorMode.Union)
                               .Create();

        var emailContainer1 = this.CreateStub<ICurrentUserEmailContainer>();
        emailContainer1.CurrentUserEmail.Returns("petrov@ya.ru");

        var emailContainer2 = this.CreateStub<ICurrentUserEmailContainer>();
        emailContainer2.CurrentUserEmail.Returns("kuznetcov@ya.ru");

        var versions = new DomainObjectVersions<ICurrentUserEmailContainer>(emailContainer1, emailContainer2);
        var untypedVersions = new DomainObjectVersions<object>(versions.Previous, versions.Current);

        var generationResolverResult = new RecipientsResolverResult(recipientsBag, untypedVersions);

        this.rolesResolver
            .Resolve(subscription, versions)
            .Returns(new RecipientCollection(new[] { recipient1 }));

        this.generationResolver
            .Resolve(subscription, versions)
            .Returns(new[] { generationResolverResult });

        var resolver = this.Fixture.Create<RecipientsResolver<ITestBLLContext>>();

        // Act

        var resolverResults = resolver.Resolve(subscription, versions);
        var actualBag = resolverResults.First().RecipientsBag;

        // Assert
        actualBag.To.Should().BeEquivalentTo(new RecipientCollection(new[] { recipient1 }));
        actualBag.ReplyTo.Should().BeEquivalentTo(new RecipientCollection(new[] { replayTo }));
    }

    [Test]
    public void Resolve_NoGenerationRecipients_ResultWithRolesRecipients()
    {
        // Arrange
        var recipient1 = new Recipient("ivanov", "ivanov@ya.ru");

        var subscription = this.Fixture
                               .Build<Subscription>()
                               .With(s => s.RecepientsMode, RecepientsSelectorMode.Union)
                               .Create();

        var versions = this.Fixture.Create<DomainObjectVersions<string>>();

        this.rolesResolver
            .Resolve(subscription, versions)
            .Returns(new RecipientCollection(new[] { recipient1 }));

        this.generationResolver
            .Resolve(subscription, versions)
            .Returns(Enumerable.Empty<RecipientsResolverResult>());

        // Act
        var resolver = this.Fixture.Create<RecipientsResolver<ITestBLLContext>>();
        var resolverResult = resolver.Resolve(subscription, versions).Single();
        var resultBag = resolverResult.RecipientsBag;

        // Assert
        resultBag.To.Should().BeEquivalentTo(new RecipientCollection(new[] { recipient1 }));
        resultBag.Cc.Should().BeEmpty();
    }
}
