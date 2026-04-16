using SampleSystem.Domain;
using SampleSystem.Generated.DTO;

namespace SampleSystem.IntegrationTests;

public class DefaultFieldsTests
{
    [Fact]
    public void GetDefaultValueFromAttr_CompareWithDTO_DefaultValuesEquals()
    {
        // Arrange
        var dto = new TestDefaultFieldsMappingObjStrictDTO();

        // Act

        // Assert
        Assert.NotEqual(default(int), dto.IntVal);
        Assert.NotEqual(default(string), dto.StrVal);
        Assert.NotEqual(default(DayOfWeek), dto.EnumVal);

        Assert.Equal(TestDefaultFieldsMappingObj.IntDefaultVal, dto.IntVal);
        Assert.Equal(TestDefaultFieldsMappingObj.StringDefaultVal, dto.StrVal);
        Assert.Equal(TestDefaultFieldsMappingObj.EnumDefaultVal, dto.EnumVal);
    }
}
