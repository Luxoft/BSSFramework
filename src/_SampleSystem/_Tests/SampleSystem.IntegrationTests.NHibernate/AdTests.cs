using Automation.ServiceEnvironment;

using Framework.SecuritySystem;

using SampleSystem.Domain.Ad;
using SampleSystem.IntegrationTests.__Support.TestData;

namespace SampleSystem.IntegrationTests;

[TestClass]
public class AdTests : TestBase
{
    [TestMethod]
    public async Task TestTest()
    {
        var grandAccess =
            new DomainSecurityRule.RelativeConditionSecurityRule(new RelativeConditionInfo<Banner>(banner => !banner.Accesses.Any()));

        var accessGrantedRule = new DomainSecurityRule.CurrentUserSecurityRule(BannerAccess.AccessGrantedKey);

        var accessDeniedRule = new DomainSecurityRule.CurrentUserSecurityRule(BannerAccess.AccessDeniedKey);

        var totalRule = grandAccess.Or(accessGrantedRule).Except(accessDeniedRule);


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
