using Anch.Testing.Xunit;

using Framework.BLL;
using Framework.Database;

using SampleSystem.Domain.Directories;
using SampleSystem.IntegrationTests._Environment.TestData;
using SampleSystem.Subscriptions.Metadata.DomainChangedByRecipients.NotPersistentCustomModel;

namespace SampleSystem.IntegrationTests;

public class SubscriptionCustomNotPersistentModelTests(IServiceProvider rootServiceProvider) : TestBase(rootServiceProvider)
{
    [AnchFact]
    public async Task CustomNotPersistentNotificationModel_Always_ShouldNotThrowException(CancellationToken ct)
    {
        // Arrange
        var countryId = this.Evaluate(
            DBSessionMode.Write,
            context =>
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
                    context.Logics.Location.Save(new Location { Country = country, Name = Guid.NewGuid().ToString(), Code = i + 1, CloseDate = 15 });
                }

                return country.Id;
            });

        this.ClearModifications();

        // Act
        this.Evaluate(
            DBSessionMode.Write,
            context =>
            {
                var bll = context.Logics.Country;

                var country = bll.GetById(countryId, true);

                country!.Name = $"{country!.Name} renamed";

                bll.Save(country);
            });

        await this.ProcessModificationsAsync(ct);

        var notifications = this.GetNotifications();

        // Assert
        Assert.Contains(
            notifications,
            x => x.TechnicalInformation.MessageTemplateCode == typeof(_DomainChangedByRecipients_NotPersistentCustomModel_MessageTemplate_cshtml).FullName);
    }
}
