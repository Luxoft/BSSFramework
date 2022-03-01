using System;
using System.Linq;

using FluentAssertions;

using Framework.Authorization.Domain;
using Framework.Configuration.Generated.DTO;
using Framework.Core;
using Framework.DomainDriven.BLL;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using WorkflowSampleSystem.Domain;
using WorkflowSampleSystem.Generated.DTO;
using WorkflowSampleSystem.IntegrationTests.__Support.TestData;

namespace WorkflowSampleSystem.IntegrationTests
{
    [TestClass]
    public class DefaultFieldsTests
    {
        [TestMethod]
        public void GetDefaultValueFromAttr_CompareWithDTO_DefaultValuesEquals()
        {
            // Arrange
            var dto = new TestDefaultFieldsMappingObjStrictDTO();

            // Act

            // Assert
            dto.IntVal.Should().NotBe(default(int));
            dto.StrVal.Should().NotBe(default(string));
            dto.EnumVal.Should().NotBe(default(DayOfWeek));

            dto.IntVal.Should().Be(TestDefaultFieldsMappingObj.IntDefaultVal);
            dto.StrVal.Should().Be(TestDefaultFieldsMappingObj.StringDefaultVal);
            dto.EnumVal.Should().Be(TestDefaultFieldsMappingObj.EnumDefaultVal);
        }
    }
}
