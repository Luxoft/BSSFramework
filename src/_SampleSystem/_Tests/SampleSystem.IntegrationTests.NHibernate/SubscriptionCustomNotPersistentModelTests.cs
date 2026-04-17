using Framework.Application;
using Framework.Database;

using SampleSystem.Domain.Directories;
using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.Subscriptions.Metadata.DomainChangedByRecipients.NotPersistentCustomModel;

namespace SampleSystem.IntegrationTests;

public class SubscriptionCustomNotPersistentModelTests : TestBase
{
    [Fact]
    public void CustomNotPersistentNotificationModel_Always_ShouldNotThrowException()
    {
        // Arrange
        var countryId = this.Evaluate(DBSessionMode.Write, context =>
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
        this.Evaluate(DBSessionMode.Write, context =>
                                           {
                                               var bll = context.Logics.Country;

                                               var country = bll.GetById(countryId, true);

                                               country.Name = $"{country.Name} renamed";

                                               bll.Save(country);
                                           });

        this.GetConfigurationControllerEvaluator(DefaultConstants.NOTIFICATION_ADMIN).Evaluate(c => c.ProcessModifications(1000));

        var notifications = this.GetNotifications();

        // Assert
        Assert.Contains(notifications, x => x.TechnicalInformation.MessageTemplateCode == typeof(_DomainChangedByRecipients_NotPersistentCustomModel_MessageTemplate_cshtml).FullName);
    }
}
