using System;
using System.Linq;
using System.Linq.Expressions;

using FluentAssertions;

using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.OData;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.Domain;
using SampleSystem.Domain.Inline;
using SampleSystem.Domain.Projections;
using SampleSystem.IntegrationTests.__Support.TestData;

namespace SampleSystem.IntegrationTests
{
    [TestClass]
    public class LegalEntityProjectionTests : TestBase
    {
        [TestMethod]
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

                context.Logics.Default.Create<TestObjForNested>().Save(new[] { baseObj, nestedObj });

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
            result.Name.Should().BeEquivalentTo(name);
            result.NameEnglish.Should().BeEquivalentTo(nameEnglish);
            result.Code.Should().BeEquivalentTo(code);
            result.BaseObjName.Should().BeEquivalentTo(baseObjName);
            result.CurrentObjName.Should().BeEquivalentTo(nestedObjName);
            ((object)result.PeriodStartDate).Should().BeEquivalentTo((object)period.StartDate);
        }
    }
}
