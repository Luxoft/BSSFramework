﻿using System;
using System.Linq;

using FluentAssertions;

using Framework.Configuration.Domain;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.DAL.Revisions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.Domain;
using SampleSystem.Generated.DTO;
using SampleSystem.IntegrationTests.__Support.ServiceEnvironment;
using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.WebApiCore.Controllers.Main;

namespace SampleSystem.IntegrationTests
{
    [TestClass]
    public class NotificationCountryTests : TestBase
    {
        [TestMethod]
        public void CreateAndUpdateCountry_SingleModificationExists()
        {
            // Arrange

            // Act
            this.ClearModifications();

            var countryId = this.GetContextEvaluator().Evaluate(DBSessionMode.Write, context =>
            {
                var bll = context.Logics.Country;

                var country = new Country
                {
                    Code = Guid.NewGuid().ToString(),
                    NameNative = Guid.NewGuid().ToString(),
                    Culture = Guid.NewGuid().ToString(),
                    Name = Guid.NewGuid().ToString()
                };

                bll.Save(country);

                country.Name = Guid.NewGuid().ToString();

                bll.Save(country);

                return country.Id;
            });

            // Assert
            this.GetModifications().Count.Should().Be(1);

            this.GetModifications().Count(mod => mod.ModificationType == ModificationType.Save && mod.Identity == countryId && mod.TypeInfoDescription.Name == nameof(Country)).Should().Be(1);
        }

        [TestMethod]
        public void CreateAndRemoveCountry_ModificationNotExists()
        {
            // Arrange

            // Act
            this.ClearModifications();

            this.GetContextEvaluator().Evaluate(DBSessionMode.Write, (context, session) =>
            {
                var bll = context.Logics.Country;

                var country = new Country
                {
                    Code = Guid.NewGuid().ToString(),
                    NameNative = Guid.NewGuid().ToString(),
                    Culture = Guid.NewGuid().ToString(),
                    Name = Guid.NewGuid().ToString()
                };

                bll.Save(country);

                bll.Remove(country);
            });

            // Assert
            this.GetModifications().Count.Should().Be(0);
        }

        [TestMethod]
        public void RemoveCountry_RemoveModificationExists()
        {
            // Arrange
            var countryController = this.MainWebApi.Country;
            var countryId = this.GetContextEvaluator().Evaluate(DBSessionMode.Write, context =>
            {
                var bll = context.Logics.Country;

                var country = new Country
                {
                    Code = Guid.NewGuid().ToString(),
                    NameNative = Guid.NewGuid().ToString(),
                    Culture = Guid.NewGuid().ToString(),
                    Name = Guid.NewGuid().ToString()
                };

                bll.Save(country);

                return country.Id;
            });

            // Act
            this.ClearModifications();

            countryController.Evaluate(c => c.RemoveCountry(new CountryIdentityDTO { Id = countryId }));

            // Assert
            this.GetModifications().Count.Should().Be(1);
            this.GetModifications().Count(mod => mod.ModificationType == ModificationType.Remove && mod.Identity == countryId && mod.TypeInfoDescription.Name == nameof(Country)).Should().Be(1);
        }


        [TestMethod]
        public void EmulateFailureCountryModification_RaisedException()
        {
            // Arrange
            var domainObjectId = Guid.NewGuid();
            var revision = 123;

            this.ClearModifications();
            this.GetContextEvaluator().Evaluate(DBSessionMode.Write, context =>
            {
                var fakeModification = new DomainObjectModification()
                {
                    DomainType = context.Configuration.GetDomainType(typeof(Country), true),
                    Type = ModificationType.Save,
                    Revision = revision,
                    DomainObjectId = domainObjectId
                };

                context.Configuration.Logics.DomainObjectModification.Save(fakeModification);
            });

            // Act
            var call = new Action(() => this.GetConfigurationControllerEvaluator(DefaultConstants.NOTIFICATION_ADMIN).Evaluate(c => c.ProcessModifications(1000)));

            // Assert
            call.Should().Throw<Exception>().WithMessage($"For DomainObject ({typeof(Country).Name}) [{domainObjectId}] both states (previous and current) can't be null. Revision: {revision}");
        }
    }
}
