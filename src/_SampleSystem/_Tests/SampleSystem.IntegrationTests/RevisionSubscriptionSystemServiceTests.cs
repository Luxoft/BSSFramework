using System;
using System.Linq;
using FluentAssertions;
using Framework.Configuration.Domain;
using Framework.Configuration.Generated.DTO;
using Framework.DomainDriven.DAL.Revisions;
using Framework.Notification.DTO;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SampleSystem.Domain;
using SampleSystem.IntegrationTests.__Support.ServiceEnvironment.IntegrationTests;
using SampleSystem.IntegrationTests.__Support.TestData;

namespace SampleSystem.IntegrationTests
{
    [TestClass]
    public sealed class RevisionSubscriptionSystemServiceTests : TestBase
    {
        private const string TestRecipientTo = "subscription_tester@luxoft.com";

        private const string TestRecipientCc = "subscription_tester_cc@luxoft.com";

        [TestInitialize]
        public void SetUp()
        {
            this.GetNotifications().Clear();
        }

        [TestMethod]
        public void RunSubscriptionInTestMode_NotificationShouldBeSent()
        {
            // Arrange
            var employee = this.CreateEmployee();
            var subscription = this.CreateSubscription();

            var dto = new TestSubscriptionModelStrictDTO();
            dto.Subscription = subscription.ToIdentityDTO();
            dto.DomainObjectId = employee.Id;

            // Act
            this.GetConfigurationController().TestSubscription(dto);

            // Assert
            this.GetNotifications().Should().HaveCount(1);
        }

        [TestMethod]
        public void RunSubscriptionInTestMode_BothToAndCcShouldBeInTargets()
        {
            // Arrange
            var employee = this.CreateEmployee();
            var subscription = this.CreateSubscription();

            var dto = new TestSubscriptionModelStrictDTO();
            dto.Subscription = subscription.ToIdentityDTO();
            dto.DomainObjectId = employee.Id;

            // Act
            this.GetConfigurationController().TestSubscription(dto);

            // Assert
            var targets = this.GetNotifications().First().Targets;
            targets.Should().HaveCount(2);
            targets.Where(x => x.Type == NotificationTargetTypes.To).Select(x => x.Name).Should().BeEquivalentTo(new[] { TestRecipientTo });
            targets.Where(x => x.Type == NotificationTargetTypes.Copy).Select(x => x.Name).Should().BeEquivalentTo(new[] { TestRecipientCc });
        }

        [TestMethod]
        public void GetRecipientsUntyped_ResultShouldContainsCorrectRecipientAndCorrectSubscription()
        {
            // Arrange
            var employee = this.CreateEmployee();
            var subscription = this.CreateSubscription();

            // Act
            var result = this.DataHelper.GetRecipientsUntyped(typeof(Employee), employee, employee, subscription.Code);

            // Assert
            result.Recipients.Should().BeEquivalentTo(TestRecipientTo);
            result.Subscription.Should().Be(subscription);
        }

        [TestMethod]
        public void GetRecipientsUntyped_NullPrefNullNext_ResultShouldContainsCorrectRecipientAndCorrectSubscription()
        {
            // Arrange
            var subscription = this.CreateSubscription();

            // Act
            var result = this.DataHelper.GetRecipientsUntyped(typeof(Employee), null, new Employee(), subscription.Code);

            // Assert
            result.Recipients.Should().BeEquivalentTo(TestRecipientTo);
            result.Subscription.Should().Be(subscription);
        }

        [TestMethod]
        public void ProcessChangedObjectUntyped_NotificationShouldBeSent()
        {
            // Arrange
            var employee = this.CreateEmployee();
            this.CreateSubscription();

            // Act
            this.DataHelper.ProcessChangedObjectUntyped(typeof(Employee), employee, employee);

            var expectedNotification = this.GetNotifications()
                .SingleOrDefault(n => n.From == this.Environment.NotificationContext.Sender.Address);

            // Assert
            expectedNotification.Should().NotBeNull();
        }

        [TestMethod]
        public void ProcessChangedObjectUntyped_NullPrevNullNext_NotificationShouldBeSent()
        {
            // Arrange
            this.CreateSubscription();

            // Act
            this.DataHelper.ProcessChangedObjectUntyped(typeof(Employee), null, new Employee());

            var expectedNotification = this.GetNotifications().SingleOrDefault(n => n.From == this.Environment.NotificationContext.Sender.Address);

            // Assert
            expectedNotification.Should().NotBeNull();
        }

        [TestMethod]
        public void TwoSubscriptionsForOneTemplate_OneNotificationForBothShouldBeSent()
        {
            // Arrange
            var employee = this.CreateEmployee();
            this.CreateSubscription("S000");
            this.CreateSubscription("S001");

            // Act
            this.DataHelper.ProcessChangedObjectUntyped(typeof(Employee), employee, employee);

            var expectedNotifications = this.GetNotifications()
                .Where(n => n.From == this.Environment.NotificationContext.Sender.Address);

            // Assert
            expectedNotifications.Should().HaveCount(1);
        }

        [TestMethod]
        public void ProcessChangedObjectUntyped_NoErrors_ResultShouldBeEmpty()
        {
            // Arrange
            var employee = this.CreateEmployee();
            this.CreateSubscription();

            // Act
            var results = this.DataHelper.ProcessChangedObjectUntyped(typeof(Employee), employee, employee);

            // Assert
            results.Should().HaveCount(0);
        }

        [TestMethod]
        public void GetObjectModifications_ResultShouldBeSetToEventQueue()
        {
            // Arrange

            // Act
            this.CreateEmployee();

            // Assert
            this.GetModifications().Should().HaveCount(1);
        }

        [TestMethod]
        public void ProcessChangedObjectInfo_NotificationShouldBeSent()
        {
            // Arrange
            this.CreateEmployee();

            var dto = this.GetModifications()
                .Single(omi => omi.TypeInfoDescription.Name == "Employee");

            var typeInfoDescription = new TypeInfoDescription();
            typeInfoDescription.Name = dto.TypeInfoDescription.Name;
            typeInfoDescription.NameSpace = dto.TypeInfoDescription.NameSpace;

            var change = new ObjectModificationInfo<Guid>(
                dto.Identity,
                typeInfoDescription,
                dto.ModificationType,
                dto.Revision);

            this.CreateSubscription();

            // Act
            this.DataHelper.ProcessChangedObjectInfo(change);

            var expectedNotification = this.GetNotifications()
                .SingleOrDefault(n => n.From == this.Environment.NotificationContext.Sender.Address);

            // Assert
            expectedNotification.Should().NotBeNull();
        }

        private Employee CreateEmployee()
        {
            var employee = this.DataHelper.SaveEmployee("John Doe");
            return employee;
        }

        private Subscription CreateSubscription(string code = null)
        {
            var messageTemplate = this.DataHelper.SaveMessageTemplate("T000");

            var domainType = this.DataHelper.GetDomainType(typeof(Employee));

            var conditionLambda = this.DataHelper.SaveSubscriptionLambda(
                "CL000",
                SubscriptionLambdaType.Condition,
                domainType,
                "(prev, next) => true",
                false);

            var generationLambda = this.DataHelper.SaveSubscriptionLambda(
                "GL000",
                SubscriptionLambdaType.Generation,
                domainType,
                $"(p, c) => new List<NotificationMessageGenerationInfo>() {{new NotificationMessageGenerationInfo(\"{TestRecipientTo}\", c, p)}};",
                false);

            var copyGenerationLambda = this.DataHelper.SaveSubscriptionLambda(
                "GL001",
                SubscriptionLambdaType.Generation,
                domainType,
                $"(p, c) => new List<NotificationMessageGenerationInfo>() {{new NotificationMessageGenerationInfo(\"{TestRecipientCc}\", c, p)}};",
                false);

            var subscription = this.DataHelper.SaveSubscription(
                code ?? "S000",
                true,
                domainType,
                messageTemplate,
                conditionLambda,
                generationLambda,
                copyGenerationLambda);

            return subscription;
        }
    }
}
