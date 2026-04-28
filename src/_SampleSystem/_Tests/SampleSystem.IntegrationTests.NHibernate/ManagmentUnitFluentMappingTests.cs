using Framework.Application;
using Framework.Core;
using Framework.Database;

using SampleSystem.Domain.NhFluentMapping;
using SampleSystem.IntegrationTests._Environment.TestData;

namespace SampleSystem.IntegrationTests;

public class ManagmentUnitFluentMappingTests(IServiceProvider rootServiceProvider) : TestBase(rootServiceProvider)
{
    [Fact]
    public void CheckBusinessUnitSecondaryAccess_HasAccess()
    {
        // Arrange
        var employeeId = this.DataHelper.SaveEmployee();

        this.Evaluate(
                      DBSessionMode.Write,
                      c =>
                      {
                          var employee = c.Logics.Employee.GetById(employeeId.Id)!;
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
        var r = this.Evaluate(
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
        var item = Assert.Single(r);
        Assert.Equal(employeeId.Id, item.EmployeeId);
    }
}
