using System;
using FluentAssertions;
using Framework.Configuration.SubscriptionModeling.Tests.Unit.Metadata;
using NUnit.Framework;

namespace Framework.Configuration.SubscriptionModeling.Tests.Unit;

[TestFixture]
public sealed class SubscriptionMetadataTests
{
    [Test]
    public void Validate_NullSenderName_Exception()
    {
        // Arrange
        var subscription = new TestSubscription();
        subscription.SetSenderName(null);

        // Act
        Action call = () => subscription.Validate();

        // Assert
        call.Should().Throw<SubscriptionModelingException>()
            .WithMessage("Property 'SenderName' of subscription 'Framework.Configuration.SubscriptionModeling.Tests.Unit.Metadata.TestSubscription' could not be null or whitespace.");
    }

    [Test]
    public void Validate_EmptySenderName_Exception()
    {
        // Arrange
        var subscription = new TestSubscription();
        subscription.SetSenderName(string.Empty);

        // Act
        Action call = () => subscription.Validate();

        // Assert
        call.Should().Throw<SubscriptionModelingException>()
            .WithMessage("Property 'SenderName' of subscription 'Framework.Configuration.SubscriptionModeling.Tests.Unit.Metadata.TestSubscription' could not be null or whitespace.");
    }

    [Test]
    public void Validate_WhiteSpaceSenderName_Exception()
    {
        // Arrange
        var subscription = new TestSubscription();
        subscription.SetSenderName(" ");

        // Act
        Action call = () => subscription.Validate();

        // Assert
        call.Should().Throw<SubscriptionModelingException>()
            .WithMessage("Property 'SenderName' of subscription 'Framework.Configuration.SubscriptionModeling.Tests.Unit.Metadata.TestSubscription' could not be null or whitespace.");
    }

    [Test]
    public void Validate_NullSenderEmail_Exception()
    {
        // Arrange
        var subscription = new TestSubscription();
        subscription.SetSenderEmail(null);

        // Act
        Action call = () => subscription.Validate();

        // Assert
        call.Should().Throw<SubscriptionModelingException>()
            .WithMessage("Property 'SenderEmail' of subscription 'Framework.Configuration.SubscriptionModeling.Tests.Unit.Metadata.TestSubscription' could not be null or whitespace.");
    }

    [Test]
    public void Validate_EmptySenderEmail_Exception()
    {
        // Arrange
        var subscription = new TestSubscription();
        subscription.SetSenderEmail(string.Empty);

        // Act
        Action call = () => subscription.Validate();

        // Assert
        call.Should().Throw<SubscriptionModelingException>()
            .WithMessage("Property 'SenderEmail' of subscription 'Framework.Configuration.SubscriptionModeling.Tests.Unit.Metadata.TestSubscription' could not be null or whitespace.");
    }

    [Test]
    public void Validate_WhiteSpaceSenderEmail_Exception()
    {
        // Arrange
        var subscription = new TestSubscription();
        subscription.SetSenderEmail(" ");

        // Act
        Action call = () => subscription.Validate();

        // Assert
        call.Should().Throw<SubscriptionModelingException>()
            .WithMessage("Property 'SenderEmail' of subscription 'Framework.Configuration.SubscriptionModeling.Tests.Unit.Metadata.TestSubscription' could not be null or whitespace.");
    }

    [Test]
    public void Validate_NullConditionLambda_Exception()
    {
        // Arrange
        var subscription = new TestSubscription();
        subscription.SetConditionLambda(null);

        // Act
        Action call = () => subscription.Validate();

        // Assert
        call.Should().Throw<SubscriptionModelingException>()
            .WithMessage("Property 'ConditionLambda' of subscription 'Framework.Configuration.SubscriptionModeling.Tests.Unit.Metadata.TestSubscription' must be specified.");
    }

    [Test]
    public void Validate_ConditionLambdaWithNullFunc_Exception()
    {
        // Arrange
        var lambda = new ConditionLambda();
        lambda.SetFunc(null);

        var subscription = new TestSubscription();
        subscription.SetConditionLambda(lambda);

        // Act
        Action call = () => subscription.Validate();

        // Assert
        call.Should().Throw<SubscriptionModelingException>()
            .WithMessage("Property Lambda for type Framework.Configuration.SubscriptionModeling.Tests.Unit.Metadata.ConditionLambda must be specified.");
    }

    [Test]
    public void Validate_GenerationLambdaWithNullFunc_Exception()
    {
        // Arrange
        var lambda = new GenerationLambda();
        lambda.SetFunc(null);

        var subscription = new TestSubscription();
        subscription.SetGenerationLambda(lambda);

        // Act
        Action call = () => subscription.Validate();

        // Assert
        call.Should().Throw<SubscriptionModelingException>()
            .WithMessage("Property Lambda for type Framework.Configuration.SubscriptionModeling.Tests.Unit.Metadata.GenerationLambda must be specified.");
    }

    [Test]
    public void Validate_CopyGenerationLambdaWithNullFunc_Exception()
    {
        // Arrange
        var lambda = new CopyGenerationLambda();
        lambda.SetFunc(null);

        var subscription = new TestSubscription();
        subscription.SetCopyGenerationLambda(lambda);

        // Act
        Action call = () => subscription.Validate();

        // Assert
        call.Should().Throw<SubscriptionModelingException>()
            .WithMessage("Property Lambda for type Framework.Configuration.SubscriptionModeling.Tests.Unit.Metadata.CopyGenerationLambda must be specified.");
    }

    [Test]
    public void Validate_CopyGenerationLambdaNotSpecified_Exception()
    {
        // Arrange
        var subscription = new TestSubscription();
        subscription.SetCopyGenerationLambda(null);

        // Act
        Action call = () => subscription.Validate();

        // Assert
        call.Should().NotThrow<SubscriptionModelingException>();
    }

    [Test]
    public void Validate_SecurityItemSourceLambdaWithNullFunc_Exception()
    {
        // Arrange
        var lambda = new SecurityItemSourceSourceLambda();
        lambda.SetFunc(null);

        var subscription = new TestSubscription();
        subscription.SetSecurityItemSourceLambdas(lambda);

        // Act
        Action call = () => subscription.Validate();

        // Assert
        call.Should().Throw<SubscriptionModelingException>()
            .WithMessage("Property Lambda for type Framework.Configuration.SubscriptionModeling.Tests.Unit.Metadata.SecurityItemSourceSourceLambda must be specified.");
    }

    [Test]
    public void Validate_SecurityItemSourceLambdaNotSpecified_Exception()
    {
        // Arrange
        var subscription = new TestSubscription();
        subscription.SetSecurityItemSourceLambdas(null);

        // Act
        Action call = () => subscription.Validate();

        // Assert
        call.Should().NotThrow<SubscriptionModelingException>();
    }
}
