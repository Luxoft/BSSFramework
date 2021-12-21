using System.Linq;

using AutoFixture;

using FluentAssertions;
using Framework.Configuration.BLL.SubscriptionSystemService3.Templates;
using Framework.UnitTesting;
using NUnit.Framework;

namespace Framework.Configuration.BLL.Core.Tests.Unit.SubscriptionSystemService3.Templates
{
    [TestFixture]
    public sealed class ExcessTemplatesFilterTests : TestFixtureBase
    {
        [Test]
        public void FilterTemplates_ToRecipients_SendIndividualLetters_TemplateForEachRecipient()
        {
            // Arrange
            var templates = MessageTemplateBuilder.With()
                .Template("code 1")
                .ToRecipients("ivanov, petrov")
                .SendIndividualLetters()
                .End()
                .Build();

            var filter = this.Fixture.Create<ExcessTemplatesFilter>();

            // Act
            var results = filter.FilterTemplates(templates).ToList();

            // Assert
            results.Should().HaveCount(1);
        }

        [Test]
        public void FilterTemplates_ToRecipients_DontSendIndividualLetters_OneTemplateForAllRecipients()
        {
            // Arrange
            var templates = MessageTemplateBuilder.With()
                .Template("code 1")
                .ToRecipients("ivanov, petrov")
                .End()
                .Build();

            var filter = this.Fixture.Create<ExcessTemplatesFilter>();

            // Act
            var results = filter.FilterTemplates(templates).ToList();

            // Assert
            results.Should().HaveCount(1);
        }

        [Test]
        public void FilterTemplates_ToRecipients_TemplatesWithDifferentCodes_OneTemplateForEachCode()
        {
            // Arrange
            var templates = MessageTemplateBuilder.With()
                .Template("code 1")
                .End()

                .Template("code 2")
                .End()
                .Build();

            var filter = this.Fixture.Create<ExcessTemplatesFilter>();

            // Act
            var results = filter.FilterTemplates(templates).ToList();

            // Assert
            results.Should().HaveCount(2);
        }

        [Test]
        public void FilterTemplates_ToRecipients_TemplatesWithDifferentCodesIndividualLetters_OneTemplateForEachRecipientInCodeGroup()
        {
            // Arrange
            var templates = MessageTemplateBuilder.With()
                .Template("code 1")
                .ToRecipients("ivanov", "petrov")
                .SendIndividualLetters()
                .End()

                .Template("code 2")
                .ToRecipients("sidorov", "kuznetcov")
                .SendIndividualLetters()
                .End()
                .Build();

            var filter = this.Fixture.Create<ExcessTemplatesFilter>();

            // Act
            var results = filter.FilterTemplates(templates).ToList();

            // Assert
            results.Should().HaveCount(4);
        }

        [Test]
        public void FilterTemplates_ToRecipients_TemplatesWithDifferentCodesSemiIndividualLetters_MixedResult()
        {
            // Arrange
            var templates = MessageTemplateBuilder.With()
                .Template("code 1")

                .ToRecipients("ivanov", "petrov")
                .SendIndividualLetters()
                .End()

                .Template("code 2")
                .ToRecipients("sidorov", "kuznetcov")
                .End()
                .Build();

            var filter = this.Fixture.Create<ExcessTemplatesFilter>();

            // Act
            var results = filter.FilterTemplates(templates).ToList();

            // Assert
            results.Should().HaveCount(3);
        }

        [Test]
        public void FilterTemplates_CcRecipients_OneTemplateForAllRecipients()
        {
            // Arrange
            var templates = MessageTemplateBuilder.With()
                .Template("code 1")
                .CcRecipients("ivanov, petrov")
                .End()
                .Build();

            var filter = this.Fixture.Create<ExcessTemplatesFilter>();

            // Act
            var results = filter.FilterTemplates(templates).ToList();

            // Assert
            results.Should().HaveCount(1);
        }

        [Test]
        public void FilterTemplates_CcRecipients_TemplatesWithDifferentCodes_OneTemplateForEachCodeGroup()
        {
            // Arrange
            var templates = MessageTemplateBuilder.With()
                .Template("code 1")
                .End()
                .Template("code 2")
                .End()
                .Build();

            var filter = this.Fixture.Create<ExcessTemplatesFilter>();

            // Act
            var results = filter.FilterTemplates(templates).ToList();

            // Assert
            results.Should().HaveCount(2);
        }

        [Test]
        public void FilterTemplates_CcRecipients_CorrectCommonTemplateForAll()
        {
            // Arrange
            var templates = MessageTemplateBuilder.With()
                .Template("code 1")
                .CcRecipients("ivanov")
                .ToReplayTo("replay1")
                .End()

                .Template("code 1")
                .CcRecipients("petrov", "kuznetcov")
                .ToReplayTo("replay2")
                .IncludeAttachments()
                .End()
                .Build();

            var filter = this.Fixture.Create<ExcessTemplatesFilter>();

            // Act
            var results = filter.FilterTemplates(templates).ToList();
            var actualTemplate = results.Single();

            // Assert
            actualTemplate.MessageTemplateCode.Should().Be("code 1");
            actualTemplate.ReplyTo.Should().BeEquivalentTo("replay1", "replay2");
            actualTemplate.CopyReceivers.Should().BeEquivalentTo("ivanov", "petrov", "kuznetcov");
            actualTemplate.Subscription.IncludeAttachments.Should().BeTrue();
        }


        [Test]
        public void FilterTemplates_SourceTemplateDataCorrectCopiedToResultTemplate()
        {
            // Arrange
            var templates = MessageTemplateBuilder.With()
                .Template("code 1")
                .ToRecipients("ivanov")
                .CcRecipients("petrov")
                .SendIndividualLetters()
                .IncludeAttachments()
                .AllowEmptyListOfRecipients()
                .End()
                .Build()
                .ToList();

            var filter = this.Fixture.Create<ExcessTemplatesFilter>();
            var expectedTemplate = templates.First();

            // Act
            var results = filter.FilterTemplates(templates).ToList();
            var actualTemplate = results.Single();

            // Assert
            actualTemplate.MessageTemplateCode.Should().Be(expectedTemplate.MessageTemplateCode);
            actualTemplate.ContextObject.Should().Be(expectedTemplate.ContextObject);
            actualTemplate.ContextObjectType.Should().Be(expectedTemplate.ContextObjectType);
            actualTemplate.Receivers.Should().BeEquivalentTo(expectedTemplate.Receivers);
            actualTemplate.CopyReceivers.Should().BeEquivalentTo(expectedTemplate.CopyReceivers);
            actualTemplate.Subscription.Should().Be(expectedTemplate.Subscription);
            actualTemplate.SendWithEmptyListOfRecipients.Should().Be(expectedTemplate.SendWithEmptyListOfRecipients);
        }

        [Test]
        public void FilterTemplates_TwoInputTemplatesWithSameCodeAndDifferentContextObjects_TwoResultTemplates()
        {
            // Arrange
            var templates = MessageTemplateBuilder.With()
                .Template("code 1")
                .ContextObject("1")
                .End()

                .Template("code 1")
                .ContextObject("2")
                .End().Build();

            var filter = this.Fixture.Create<ExcessTemplatesFilter>();

            // Act
            var results = filter.FilterTemplates(templates);

            // Assert
            results.Should().HaveCount(2);
        }

        [Test]
        public void FilterTemplates_CcAndToRecipients_CombinedResult()
        {
            // Arrange
            var templates = MessageTemplateBuilder.With()
                .Template("code 1")
                .ToRecipients("ivanov", "sidorov")
                .SendIndividualLetters()
                .End()

                .Template("code 1")
                .ToRecipients("ivanov", "sidorov")
                .CcRecipients("petrov", "kuznetcov")
                .End()

                .Template("code 2")
                .ToRecipients("ivanov", "sidorov")
                .End()
                .Build();

            var filter = this.Fixture.Create<ExcessTemplatesFilter>();

            // Act
            var results = filter.FilterTemplates(templates).ToList();

            // Assert
            results.Should().HaveCount(4);
        }

        [Test]
        public void FilterTemplates_RecipientInToAndInCc_RecipientShouldBeOnlyInTo()
        {
            // Arrange
            var templates = MessageTemplateBuilder.With()
                .Template("code 1")
                .ToRecipients("ivanov", "sidorov")
                .End()

                .Template("code 1")
                .CcRecipients("sidorov", "petrov")
                .End()

                .Build();

            var filter = this.Fixture.Create<ExcessTemplatesFilter>();

            // Act
            var results = filter.FilterTemplates(templates).ToList();
            var actualTemplate = results.Last();

            // Assert
            actualTemplate.CopyReceivers.Should().BeEquivalentTo(new[] { "petrov" });
        }
    }
}
