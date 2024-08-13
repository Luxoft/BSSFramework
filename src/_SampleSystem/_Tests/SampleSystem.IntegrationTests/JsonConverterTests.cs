using System.Text.Json;

using FluentAssertions;

using Framework.Core;
using Framework.DomainDriven.WebApiNetCore.JsonConverter;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.IntegrationTests.__Support.TestData;

namespace SampleSystem.IntegrationTests;

[TestClass]
public class JsonConverterTests : TestBase
{
    [TestMethod]
    public void DateTimeConverted_ResultCorrected()
    {
        //Arrange
        var now = this.TimeProvider.GetLocalNow();

        var options = new JsonSerializerOptions { Converters = { new UtcDateTimeJsonConverter() } };

        //Act
        var jsonText = JsonSerializer.Serialize(now, options);
        var restored = JsonSerializer.Deserialize<DateTime>(jsonText, options);

        //Assert
        now.Should().Be(restored);
    }


    [TestMethod]
    public void PeriodConverted_ResultCorrected()
    {
        //Arrange
        var currentMonth = this.TimeProvider.GetCurrentMonth();

        var options = new JsonSerializerOptions { Converters = { new UtcDateTimeJsonConverter(), new PeriodJsonConverter() } };

        //Act
        var jsonText = JsonSerializer.Serialize(currentMonth, options);
        var restored = JsonSerializer.Deserialize<Period>(jsonText, options);

        //Assert
        currentMonth.Should().Be(restored);
    }
}
