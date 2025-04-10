using System.Linq.Expressions;

using Automation.ServiceEnvironment;

using Framework.Core;
using Framework.SecuritySystem;
using Framework.SecuritySystem.UserSource;

using SampleSystem.Domain;
using SampleSystem.Domain.Ad;
using SampleSystem.IntegrationTests.__Support.TestData;

namespace SampleSystem.IntegrationTests;

public class MySecProvider<TDomainObject>(
    IRelativeDomainPathInfo<TDomainObject, Banner> relativeDomainPathInfo,
    ICurrentUserSource<Employee> currentUserSource,
    bool accessFlag) : SecurityProvider<TDomainObject>
{
    public override Expression<Func<TDomainObject, bool>> SecurityFilter { get; } =

        from banner in relativeDomainPathInfo.Path

        select banner.Accesses.Any(ba => ba.AccessFlag == accessFlag && ba.Group.Members.Select(m => m.Employee).Contains(currentUserSource.CurrentUser));

    public override SecurityAccessorData GetAccessorData(TDomainObject domainObject) => throw new NotImplementedException();
}

// () -> Full Access
// (true) -> Single Access
// (false) -> All Except single

// (true, false)

[TestClass]
public class AdTests : TestBase
{
    [TestMethod]
    public async Task TestTest()
    {
        SecurityRule grandAccess =
            new DomainSecurityRule.RelativeConditionSecurityRule(new RelativeConditionInfo<Banner>(banner => !banner.Accesses.Any()));

        var accessGranted = new DomainSecurityRule.ProviderFactorySecurityRule(new MySecProvider(true));

        var accessDenied = new DomainSecurityRule.ProviderFactorySecurityRule(new MySecProvider(false));

        var totalRule = grandAccess.





        // Arrange
        AdGroup
        var group = this.EvaluateWrite(
            ctx =>
            {
                var
            })

        var banner1 = this.EvaluateWrite(ctx =>
                                         {
                                             var banner = new banne

                                             return ctx.Logics;
                                         })



        // Act
        var ident = await asyncControllerEvaluator.EvaluateAsync(c => c.AsyncSaveLocation(saveDto, default));

        var loadedLocationList = await asyncControllerEvaluator.EvaluateAsync(c => c.AsyncGetLocations(default));

        // Assert
        var location = loadedLocationList.SingleOrDefault(bu => bu.Name == saveDto.Name && bu.Identity == ident);

        location.Should().NotBeNull();
    }
}
