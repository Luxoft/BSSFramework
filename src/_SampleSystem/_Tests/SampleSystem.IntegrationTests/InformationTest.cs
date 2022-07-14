using System;
using System.Linq;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.Domain;
using SampleSystem.IntegrationTests.__Support.TestData;

using Framework.Configuration.Generated.DTO;
using Framework.DomainDriven.BLL;

using SampleSystem.Generated.DTO;
using SampleSystem.IntegrationTests.__Support.ServiceEnvironment;

namespace SampleSystem.IntegrationTests
{
    [TestClass]
    public class InformationTest : TestBase
    {
        [TestMethod]
        public void CreateAndRemoveInformation_ContainsIntegrationEvents()
        {
            // Arrange
            this.ClearIntegrationEvents();

            // Act
            var id = this.Environment.GetContextEvaluator().Evaluate(DBSessionMode.Write, context =>
            {
                var newObj = new Information() { Name = "ololo" };

                context.Logics.Information.Save(newObj);

                return newObj.Id;
            });

            this.Environment.GetContextEvaluator().Evaluate(DBSessionMode.Write, context =>
            {
                var obj = context.Logics.Information.GetById(id, true);

                context.Logics.Information.Remove(obj);
            });

            // Assert
            this.GetIntegrationEvents<InformationSaveEventDTO>().Should().ContainSingle(dto => dto.Information.Id == id);
            this.GetIntegrationEvents<InformationRemoveEventDTO>().Should().ContainSingle(dto => dto.Information.Id == id);
        }
    }
}
