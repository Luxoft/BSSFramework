using System.Text.Json;

using Framework.Core;
using Framework.Infrastructure.JsonConverter;

using SampleSystem.IntegrationTests.__Support.TestData;

namespace SampleSystem.IntegrationTests;

public class JsonConverterTests : TestBase
{
    [Fact]
    public void DateTimeConverted_ResultCorrected()
    {
        //Arrange
        var now = this.TimeProvider.GetLocalNow();

        var options = new JsonSerializerOptions { Converters = { new UtcDateTimeJsonConverter() } };

        //Act
        var jsonText = JsonSerializer.Serialize(now, options);
        var restored = JsonSerializer.Deserialize<DateTime>(jsonText, options);

        //Assert
        Assert.Equal(now, restored);
    }


    [Fact]
    public void PeriodConverted_ResultCorrected()
    {
        //Arrange
        var testPeriod = this.TimeProvider.GetCurrentMonth();

        var options = new JsonSerializerOptions { Converters = { new UtcDateTimeJsonConverter(), new PeriodJsonConverter() } };

        //Act
        var jsonText = JsonSerializer.Serialize(testPeriod, options);
        var restored = JsonSerializer.Deserialize<Period>(jsonText, options);

        //Assert
        Assert.Equal(testPeriod, restored);
    }

    [Fact]
    public void PeriodWithNullConverted_ResultCorrected()
    {
        //Arrange
        var testPeriod = new Period(this.TimeProvider.GetToday());

        var options = new JsonSerializerOptions { Converters = { new UtcDateTimeJsonConverter(), new PeriodJsonConverter() } };

        //Act
        var jsonText = JsonSerializer.Serialize(testPeriod, options);
        var restored = JsonSerializer.Deserialize<Period>(jsonText, options);

        //Assert
        Assert.Equal(testPeriod, restored);
    }
}
