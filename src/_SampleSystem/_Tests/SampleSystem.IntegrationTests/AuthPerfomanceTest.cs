using Automation.ServiceEnvironment;

using FluentAssertions;

using Framework.DomainDriven;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.Domain;
using SampleSystem.Generated.DTO;
using SampleSystem.IntegrationTests.__Support.TestData;

namespace SampleSystem.IntegrationTests;

[TestClass]
public class AuthPerformanceTest : TestBase
{
    private IReadOnlyCollection<BusinessUnitIdentityDTO?> fbuSource;

    private IReadOnlyCollection<ManagementUnitIdentityDTO?> mbuSource;

    private IReadOnlyCollection<LocationIdentityDTO?> locationSource;

    private IReadOnlyCollection<EmployeeIdentityDTO?> employeeSource;

    private static readonly string PrincipalName = "AuthPerformance";

    private static readonly int Size = 6;

    [TestInitialize]
    public void SetUp()
    {
        this.fbuSource =
            new[] { default(BusinessUnitIdentityDTO?) }.Concat(Enumerable.Range(0, Size - 1).Select(_ => (BusinessUnitIdentityDTO?)this.DataHelper.SaveBusinessUnit())).ToList();

        this.mbuSource =
            new[] { default(ManagementUnitIdentityDTO?) }.Concat(Enumerable.Range(0, Size - 1).Select(_ => (ManagementUnitIdentityDTO?)this.DataHelper.SaveManagementUnit())).ToList();

        this.locationSource =
            new[] { default(LocationIdentityDTO?) }.Concat(Enumerable.Range(0, Size - 1).Select(_ => (LocationIdentityDTO?)this.DataHelper.SaveLocation())).ToList();

        this.employeeSource =
            new[] { default(EmployeeIdentityDTO?), (EmployeeIdentityDTO?)this.DataHelper.SaveEmployee() }.ToList();

        this.AuthHelper.SavePrincipal(PrincipalName, true);

        this.GeneratePermission();
    }

    [TestMethod]
    public async Task LoadGenerateAuthPerformanceObjects_CountEquals()
    {
        // Arrange
        var authPerfCount = await this.GenerateAuthPerformanceObject();

        // Act
        var findCount = await this.RootServiceProvider.GetRequiredService<IDBSessionEvaluator>().EvaluateAsync(
                            DBSessionMode.Write,
                            async (sp, _) =>
                            {
                                var testObjRep = sp.GetRequiredService<IRepositoryFactory<AuthPerformanceObject>>()
                                                   .Create(BLLSecurityMode.View);

                                return testObjRep.GetQueryable().Count();
                            });

        // Assert
        authPerfCount.Should().Be(findCount);
    }

    private void GeneratePermission()
    {
        var request = from fbu in this.fbuSource

                      from mbu in this.mbuSource

                      from location in this.locationSource

                      from employee in this.employeeSource

                      select new SampleSystemPermission(TestBusinessRole.Administrator, fbu, mbu, location, employee);

        this.AuthHelper.SetUserRole(PrincipalName, request.ToArray());
    }

    private async Task<int> GenerateAuthPerformanceObject()
    {
        return await this.RootServiceProvider.GetRequiredService<IDBSessionEvaluator>().EvaluateAsync(
                   DBSessionMode.Write,
                   async (sp, _) =>
                   {
                       var fbuRep = sp.GetRequiredService<IRepositoryFactory<BusinessUnit>>().Create();
                       var mbuRep = sp.GetRequiredService<IRepositoryFactory<ManagementUnit>>().Create();
                       var locRep = sp.GetRequiredService<IRepositoryFactory<Location>>().Create();
                       var empRep = sp.GetRequiredService<IRepositoryFactory<Employee>>().Create();
                       var testObjRep = sp.GetRequiredService<IRepositoryFactory<AuthPerformanceObject>>().Create();

                       int count = 0;
                       foreach (var fbu in this.fbuSource.Take(Size - 3))
                       {
                           foreach (var mbu in this.mbuSource.Take(Size - 3))
                           {
                               foreach (var loc in this.locationSource.Take(Size - 3))
                               {
                                   foreach (var emp in this.employeeSource)
                                   {
                                       var testObj = new AuthPerformanceObject
                                                     {
                                                         BusinessUnit = fbu == null ? null : await fbuRep.LoadAsync(fbu.Value.Id),
                                                         ManagementUnit = mbu == null ? null : await mbuRep.LoadAsync(mbu.Value.Id),
                                                         Location = loc == null ? null : await locRep.LoadAsync(loc.Value.Id),
                                                         Employee = emp == null ? null : await empRep.LoadAsync(emp.Value.Id),
                                                     };

                                       await testObjRep.SaveAsync(testObj);

                                       count++;
                                   }
                               }
                           }
                       }

                       return count;
                   });
    }
}
