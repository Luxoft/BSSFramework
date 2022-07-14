using System;
using System.Linq;

using FluentAssertions;

using Framework.Authorization.BLL;
using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven.BLL;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.Domain;
using SampleSystem.Generated.DTO;
using SampleSystem.IntegrationTests.__Support.ServiceEnvironment;
using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.IntegrationTests.__Support.Utils.Framework;
using SampleSystem.WebApiCore.Controllers.Main;
using SampleSystem.WebApiCore.Controllers.MainQuery;

namespace SampleSystem.IntegrationTests
{
    [TestClass]
    public class ManagmentUnitFluentMappingTests : TestBase
    {
        [TestInitialize]
        public void SetUp()
        {
        }

        [TestMethod]
        public void CheckBusinessUnitSecondaryAccess_HasAccess()
        {
            // Arrange
            var employeeId = this.DataHelper.SaveEmployee();

            this.Environment
                .GetContextEvaluator()
                .Evaluate(
                          DBSessionMode.Write,
                          (c) =>
                          {
                              var employee = c.Logics.Employee.GetById(employeeId.Id);
                              var mu = new ManagementUnitFluentMapping
                                       {
                                               Name = "test",
                                               Period = Period.Eternity,
                                               MuComponent = new MuComponent
                                                             {
                                                                     AuthorizedLuxoftSignatory = employee, LuxoftSignsFirst = true
                                                             }
                                       };
                              c.Logics.ManagementUnitFluentMapping.Save(mu);
                          });

            // Act
            var r = this.Environment
                        .GetContextEvaluator()
                        .Evaluate(
                                  DBSessionMode.Read,
                                  (c) =>
                                  {
                                      return c.Logics.ManagementUnitFluentMapping
                                              .GetUnsecureQueryable()
                                              .Where(x => x.MuComponent.AuthorizedLuxoftSignatory.Id == employeeId.Id)
                                              .Select(
                                                      x => new
                                                           {
                                                                   x.Id,
                                                                   x.MuComponent.LuxoftSignsFirst,
                                                                   EmployeeId = x.MuComponent.AuthorizedLuxoftSignatory.Id
                                                           })
                                              .ToList();
                                  });

            // Assert
            r.Should().HaveCount(1);
            r[0].EmployeeId.Should().Be(employeeId.Id);
        }
    }
}
