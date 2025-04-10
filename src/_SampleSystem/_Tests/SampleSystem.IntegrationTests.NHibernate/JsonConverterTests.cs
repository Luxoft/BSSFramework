﻿using System.Text.Json;

using Framework.Core;
using Framework.DomainDriven.WebApiNetCore.JsonConverter;

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
        var testPeriod = this.TimeProvider.GetCurrentMonth();

        var options = new JsonSerializerOptions { Converters = { new UtcDateTimeJsonConverter(), new PeriodJsonConverter() } };

        //Act
        var jsonText = JsonSerializer.Serialize(testPeriod, options);
        var restored = JsonSerializer.Deserialize<Period>(jsonText, options);

        //Assert
        testPeriod.Should().Be(restored);
    }

    [TestMethod]
    public void PeriodWithNullConverted_ResultCorrected()
    {
        //Arrange
        var testPeriod = new Period(this.TimeProvider.GetToday());

        var options = new JsonSerializerOptions { Converters = { new UtcDateTimeJsonConverter(), new PeriodJsonConverter() } };

        //Act
        var jsonText = JsonSerializer.Serialize(testPeriod, options);
        var restored = JsonSerializer.Deserialize<Period>(jsonText, options);

        //Assert
        testPeriod.Should().Be(restored);
    }
}
