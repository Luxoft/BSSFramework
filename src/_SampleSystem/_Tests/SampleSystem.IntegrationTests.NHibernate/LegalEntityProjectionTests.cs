using Anch.Core;

using Framework.AutomationCore.RootServiceProviderContainer;
using Framework.BLL;
using Framework.Core;

using SampleSystem.Domain.Directories;
using SampleSystem.Domain.Projections;
using SampleSystem.IntegrationTests.__Support.TestData;

namespace SampleSystem.IntegrationTests;

public class LegalEntityProjectionTests : TestBase
{
    [Fact]
    public void LegalEntityProjectionNestedMappingTest()
    {
        // Arrange
        var name = Guid.NewGuid().ToString();
        var nameEnglish = Guid.NewGuid().ToString();
        var code = Guid.NewGuid().ToString();

        var baseObjName = Guid.NewGuid().ToString();
        var nestedObjName = Guid.NewGuid().ToString();

        var period = new Period(new DateTime(2019, 6, 1), new DateTime(2019, 8, 1));

        var id = this.EvaluateWrite(context =>
                                    {
                                        var baseObj = new TestObjForNested { Name = baseObjName, Period = period };
                                        var nestedObj = new TestObjForNested { Name = nestedObjName, Period = period };

                                        context.Logics.Default.Create<TestObjForNested>().Save([baseObj, nestedObj]);

                                        var legalEntity = new CompanyLegalEntity { Name = name, NameEnglish = nameEnglish, Code = code, BaseObj = baseObj, CurrentObj = nestedObj };

                                        context.Logics.CompanyLegalEntity.Save(legalEntity);

                                        return legalEntity.Id;
                                    });

        // Act
        var result = this.EvaluateRead(context => context.Logics.Default.Create<CustomCompanyLegalEntity>().GetById(id, true).Pipe(customLegalEntity => new
                                           {
                                                   customLegalEntity.Name,
                                                   customLegalEntity.Code,
                                                   customLegalEntity.NameEnglish,
                                                   BaseObjName = customLegalEntity.BaseObj.Name,
                                                   CurrentObjName = customLegalEntity.CurrentObj.Name,
                                                   PeriodStartDate = customLegalEntity.CurrentObj.PeriodStartDateXXX,
                                                   customLegalEntity.AribaStatusDescription,
                                                   customLegalEntity.AribaStatusType,
                                           }));


        // Assert
        Assert.Equal(name, result.Name);
        Assert.Equal(nameEnglish, result.NameEnglish);
        Assert.Equal(code, result.Code);
        Assert.Equal(baseObjName, result.BaseObjName);
        Assert.Equal(nestedObjName, result.CurrentObjName);
        Assert.Equal(period.StartDate, result.PeriodStartDate);
    }
}
