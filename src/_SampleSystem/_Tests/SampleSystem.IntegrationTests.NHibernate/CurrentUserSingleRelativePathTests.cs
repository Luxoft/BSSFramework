using Framework.Application;
using Framework.Database;
using Framework.BLL;

using Anch.SecuritySystem;
using Anch.SecuritySystem.UserSource;

using Microsoft.Extensions.DependencyInjection;

using SampleSystem.Domain;
using SampleSystem.Domain.Employee;
using SampleSystem.IntegrationTests._Environment.TestData;

namespace SampleSystem.IntegrationTests;

public class CurrentUserSingleRelativePathTests(IServiceProvider rootServiceProvider) : TestBase(rootServiceProvider)
{
    private Guid[] testObjectIdents;

    protected override async ValueTask InitializeAsync(CancellationToken ct) =>
        this.testObjectIdents = this.Evaluate(
            DBSessionMode.Write,
            ctx =>
            {
                var bll = ctx.Logics.Default.Create<TestRelativeEmployeeObject>();

                var currentEmployee = ctx.ServiceProvider.GetRequiredService<ICurrentUserSource<Employee>>().CurrentUser;

                var testObj1 = new TestRelativeEmployeeObject { EmployeeRef1 = currentEmployee };
                var testObj2 = new TestRelativeEmployeeObject { EmployeeRef2 = currentEmployee };
                bll.Save([testObj1, testObj2]);

                return new[] { testObj1.Id, testObj2.Id };
            });

    [Theory]
    [MemberData(nameof(TestRelativeEmployeeObject_FilterByPrimaryEmployeeRef_EmployeeBySecondaryRefMissed_Source))]
    public void TestRelativeEmployeeObject_FilterByEmployeeRef1_EmployeeRef2Missed(string propName, int expectedIndex)
    {
        // Arrange
        var testSecurityRule = new DomainSecurityRule.CurrentUserSecurityRule { RelativePathKey = propName };

        // Act
        var loadedObjects = this.Evaluate(
            DBSessionMode.Read,
            ctx => ctx.Logics
                      .Default
                      .Create<TestRelativeEmployeeObject>(testSecurityRule)
                      .GetSecureQueryable()
                      .Select(obj => obj.Id).ToList());

        // Assert
        Xunit.Assert.Equal([this.testObjectIdents[expectedIndex]], loadedObjects);
    }

    public static IEnumerable<object[]> TestRelativeEmployeeObject_FilterByPrimaryEmployeeRef_EmployeeBySecondaryRefMissed_Source() =>
    [
        [nameof(TestRelativeEmployeeObject.EmployeeRef1), 0],
        [nameof(TestRelativeEmployeeObject.EmployeeRef2), 1],
    ];
}
