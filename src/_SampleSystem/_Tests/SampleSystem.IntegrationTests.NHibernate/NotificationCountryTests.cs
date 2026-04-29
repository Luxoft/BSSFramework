using Framework.Application;
using Framework.Configuration.Domain;
using Framework.Database;

using SampleSystem.Domain.Directories;
using SampleSystem.Generated.DTO;
using SampleSystem.IntegrationTests._Environment.TestData;

namespace SampleSystem.IntegrationTests;

public class NotificationCountryTests(IServiceProvider rootServiceProvider) : TestBase(rootServiceProvider)
{
    [Fact]
    public void CreateAndUpdateCountry_SingleModificationExists()
    {
        // Arrange

        // Act
        this.ClearModifications();

        var countryId = this.Evaluate(
            DBSessionMode.Write,
            context =>
            {
                var bll = context.Logics.Country;

                var country = new Country
                              {
                                  Code = Guid.NewGuid().ToString(), NameNative = Guid.NewGuid().ToString(), Culture = Guid.NewGuid().ToString(), Name = Guid.NewGuid().ToString()
                              };

                bll.Save(country);

                country.Name = Guid.NewGuid().ToString();

                bll.Save(country);

                return country.Id;
            });

        // Assert
        Assert.Single(this.GetModifications());
        Assert.Equal(1, this.GetModifications().Count(mod => mod.ModificationType == ModificationType.Save && mod.Identity == countryId && mod.TypeInfoDescription.Name == nameof(Country)));
    }

    [Fact]
    public void CreateAndRemoveCountry_ModificationNotExists()
    {
        // Arrange

        // Act
        this.ClearModifications();

        this.Evaluate(
            DBSessionMode.Write,
            context =>
            {
                var bll = context.Logics.Country;

                var country = new Country
                              {
                                  Code = Guid.NewGuid().ToString(), NameNative = Guid.NewGuid().ToString(), Culture = Guid.NewGuid().ToString(), Name = Guid.NewGuid().ToString()
                              };

                bll.Save(country);

                bll.Remove(country);
            });

        // Assert
        Assert.Empty(this.GetModifications());
    }

    [Fact]
    public void RemoveCountry_RemoveModificationExists()
    {
        // Arrange
        var countryController = this.MainWebApi.Country;
        var countryId = this.Evaluate(
            DBSessionMode.Write,
            context =>
            {
                var bll = context.Logics.Country;

                var country = new Country
                              {
                                  Code = Guid.NewGuid().ToString(), NameNative = Guid.NewGuid().ToString(), Culture = Guid.NewGuid().ToString(), Name = Guid.NewGuid().ToString()
                              };

                bll.Save(country);

                return country.Id;
            });

        // Act
        this.ClearModifications();

        countryController.Evaluate(c => c.RemoveCountry(new CountryIdentityDTO { Id = countryId }));

        // Assert
        Assert.Single(this.GetModifications());
        Assert.Equal(1, this.GetModifications().Count(mod => mod.ModificationType == ModificationType.Remove && mod.Identity == countryId && mod.TypeInfoDescription.Name == nameof(Country)));
    }


    [Fact]
    public void EmulateFailureCountryModification_RaisedException()
    {
        // Arrange
        var domainObjectId = Guid.NewGuid();
        var revision = 123;

        this.ClearModifications();
        this.Evaluate(
            DBSessionMode.Write,
            context =>
            {
                var fakeModification = new DomainObjectModification()
                                       {
                                           DomainType = context.Configuration.GetDomainType(typeof(Country)),
                                           Type = ModificationType.Save,
                                           Revision = revision,
                                           DomainObjectId = domainObjectId
                                       };

                context.Configuration.Logics.DomainObjectModification.Save(fakeModification);
            });

        var configController = this.GetConfigurationControllerEvaluator(DefaultConstants.NOTIFICATION_ADMIN);

        // Act
        var ex = Record.Exception(() => configController.Evaluate(c => c.ProcessModifications(1000)));

        // Assert
        var argumentException = Assert.IsType<ArgumentException>(ex);
        Assert.Equal("Both arguments (previous and current) can't be null", argumentException.Message);
    }
}
