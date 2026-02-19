using Framework.DomainDriven;
using Framework.DomainDriven.Repository;
using Microsoft.Extensions.DependencyInjection;

using SampleSystem.Domain;
using SampleSystem.Generated.DTO;
using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.Security;

using SecuritySystem;

namespace SampleSystem.IntegrationTests;

[TestClass]
public class AuthPerformanceTest : TestBase
{
    private IReadOnlyCollection<BusinessUnitIdentityDTO?> fbuSource;

    private IReadOnlyCollection<ManagementUnitIdentityDTO?> mbuSource;

    private IReadOnlyCollection<LocationIdentityDTO?> locationSource;

    private IReadOnlyCollection<EmployeeIdentityDTO?> employeeSource;

    private static readonly string PrincipalName = "AuthPerformance";

    private static readonly int Size = 5;

    [TestInitialize]
    public void SetUp()
    {
        this.fbuSource = [null, .. Enumerable.Range(0, Size - 1).Select(_ => (BusinessUnitIdentityDTO?)this.DataHelper.SaveBusinessUnit())];

        this.mbuSource = [null, .. Enumerable.Range(0, Size - 1).Select(_ => (ManagementUnitIdentityDTO?)this.DataHelper.SaveManagementUnit())];

        this.locationSource = [null, .. Enumerable.Range(0, Size - 1).Select(_ => (LocationIdentityDTO?)this.DataHelper.SaveLocation())];

        this.employeeSource = [null, this.DataHelper.SaveEmployee()];

        this.AuthManager.For(PrincipalName).CreatePrincipal();

        this.GeneratePermission();
    }

    [TestMethod]
    public async Task LoadGenerateAuthPerformanceObjects_CountEquals()
    {
        // Arrange
        var authPerfCount = await this.GenerateAuthPerformanceObject();

        // Act
        var findCount = await this.RootServiceProvider.GetRequiredService<IServiceEvaluator<IRepositoryFactory<AuthPerformanceObject>>>().EvaluateAsync(
                            DBSessionMode.Write,
                            async service =>
                            {
                                var testObjRep = service.Create(SecurityRule.View);

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

                      select new SampleSystemTestPermission(SampleSystemSecurityRole.TestPerformance, fbu, mbu, location, employee).ToManagedPermissionData();

        this.AuthManager.For(PrincipalName).SetRole(request.ToArray());
    }

    private async Task<int> GenerateAuthPerformanceObject()
    {
        return await this.RootServiceProvider.GetRequiredService<IDBSessionEvaluator>().EvaluateAsync(
                   DBSessionMode.Write,
                   async sp =>
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
