using System;
using FluentAssertions;
using Framework.DomainDriven.BLL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkflowSampleSystem.Domain;
using WorkflowSampleSystem.IntegrationTests.__Support.TestData;

namespace WorkflowSampleSystem.IntegrationTests
{
    [TestClass]
    public class SubscriptionCustomNotPersistentModelTests : TestBase
    {
        [TestMethod]
        public void CustomNotPersistentNotificationModel_Always_ShouldNotThrowException()
        {
            // Arrange
            var countryId = this.Environment.GetContextEvaluator().Evaluate(DBSessionMode.Write, context =>
            {
                var country = new Country
                {
                    Code = Guid.NewGuid().ToString(),
                    NameNative = Guid.NewGuid().ToString(),
                    Culture = Guid.NewGuid().ToString(),
                    Name = Guid.NewGuid().ToString()
                };

                context.Logics.Country.Save(country);

                for (var i = 0; i < 5; i++)
                {
                    context.Logics.Location.Save(new Location
                    {
                        Country = country,
                        Name = Guid.NewGuid().ToString(),
                        Code = i + 1,
                        CloseDate = 15
                    });
                }

                return country.Id;
            });

            this.ClearModifications();

            // Act
            this.Environment.GetContextEvaluator().Evaluate(DBSessionMode.Write, context =>
            {
                var bll = context.Logics.Country;

                var country = bll.GetById(countryId, true);

                country.Name = $"{country.Name} renamed";

                bll.Save(country);
            });

            this.GetConfigurationController().ProcessModifications(1000);

            var notifications = this.GetNotifications();

            // Assert
            notifications.Should().Contain(x => x.TechnicalInformation.MessageTemplateCode == "ASP._DomainChangedByRecipients_NotPersistentCustomModel_MessageTemplate_cshtml");
        }
    }
}
