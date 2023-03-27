using System.Globalization;

using FluentAssertions;

using NUnit.Framework;

namespace Framework.Core.Tests.Unit;

// MethodUnderTest_Scenario_Behavior
[TestFixture]
public class PeriodTests
{
    [SetUp]
    public void Setup()
    {
        System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("ru-ru");
    }

    [Test]
    public void IsIntersectTest()
    {
        // Arrange
        var period1 = new Period(new DateTime(2014, 8, 1), new DateTime(2014, 8, 3));
        var period2 = new Period(new DateTime(2014, 8, 4), new DateTime(2014, 8, 10));

        // Act
        var result = period1.IsIntersected(period2);

        // Assert
        Assert.IsFalse(result);
    }

    [Test]
    public void Parse_CorrectPeriodString_Ok()
    {
        // Arrange
        var str = "2014-01-01@2014-05-04";
        var expected = new Period(new DateTime(2014, 1, 1), new DateTime(2014, 5, 4));

        // Act
        var result = Period.Parse(str);

        // Assert
        Assert.AreEqual(expected, result);
    }

    [Test]
    public void Parse_IncorrectPeriodString_Exception()
    {
        // Arrange
        var str = "2014-01-01@2014-05-04@";
        var expected = new Period(new DateTime(2014, 1, 1), new DateTime(2014, 5, 14));

        // Act
        object TestDelegate() => Period.Parse(str);

        // Assert
        Assert.That(TestDelegate, Throws.TypeOf<ArgumentException>());
    }

    [Test]
    public void Parse_CorrectDateString_Ok()
    {
        // Arrange
        var str = "2014-01-01";
        var expected = new Period(new DateTime(2014, 1, 1));

        // Act
        var result = Period.Parse(str);

        // Assert
        Assert.AreEqual(expected, result);
    }

    [Test]
    public void Parse_EmptyDateString_EmptyPeriod()
    {
        // Arrange
        var str = string.Empty;

        // Act
        var result = Period.Parse(str);

        // Assert
        Assert.AreEqual(Period.Empty, result);
    }

    [TestCase(1, 2015, 1, 2015, ExpectedResult = "Январе 2015 г.")]
    [TestCase(1, 2015, 2, 2015, ExpectedResult = "Январе 2015 г.")]
    [TestCase(2, 2015, 2, 2015, ExpectedResult = "Феврале 2015 г.")]
    [TestCase(5, 2014, 5, 2016, ExpectedResult = "Мае 2014 г.")]
    public string GetInMonthAndYearStrRus_CorrectPeriod_NameMonthInPrepositional(int startMonth, int startYear, int endMonth, int endYear)
    {
        // Arrange
        var period = new Period(new DateTime(startYear, startMonth, 15), new DateTime(endYear, endMonth, 15));

        // Act
        var actualResult = period.GetInMonthAndYearStrRus();

        // Assert
        return actualResult;
    }

    [TestCase(1, 2015, 1, 2015, ExpectedResult = "Январь 2015 г.")]
    [TestCase(1, 2015, 2, 2015, ExpectedResult = "Январь 2015 г. - Февраль 2015 г.")]
    [TestCase(2, 2015, 2, 2015, ExpectedResult = "Февраль 2015 г.")]
    [TestCase(5, 2014, 5, 2016, ExpectedResult = "Май 2014 г. - Май 2016 г.")]
    public string GetMonthAndYearStrRus_CorrectPeriod_NameMonthInNominative(int startMonth, int startYear, int endMonth, int endYear)
    {
        // Arrange
        var period = new Period(new DateTime(startYear, startMonth, 15), new DateTime(endYear, endMonth, 15));

        // Act
        var actualResult = period.GetMonthAndYearStrRus();

        // Assert
        return actualResult;
    }

    [TestCase(1, 2015, 1, 2015, ExpectedResult = "January 2015")]
    [TestCase(1, 2015, 2, 2015, ExpectedResult = "January 2015 - February 2015")]
    [TestCase(2, 2015, 2, 2015, ExpectedResult = "February 2015")]
    [TestCase(5, 2014, 5, 2016, ExpectedResult = "May 2014 - May 2016")]
    public string GetMonthAndYearStr_CorrectPeriod_NameMonthInNominative(int startMonth, int startYear, int endMonth, int endYear)
    {
        // Arrange
        var period = new Period(new DateTime(startYear, startMonth, 15), new DateTime(endYear, endMonth, 15));

        // Act
        var actualResult = period.GetMonthAndYearStr();

        // Assert
        return actualResult;
    }

    [TestCase(1, 2015, 1, 2015, ExpectedResult = "I '15")]
    [TestCase(1, 2015, 2, 2015, ExpectedResult = "I -II '15")]
    [TestCase(2, 2015, 2, 2015, ExpectedResult = "II '15")]
    [TestCase(5, 2014, 5, 2016, ExpectedResult = "V -V '14")]
    [TestCase(8, 2014, 12, 2016, ExpectedResult = "VIII -XII '14")]
    public string GetMonthAndYearStrRoman_CorrectPeriod_NameMonthInNominative(int startMonth, int startYear, int endMonth, int endYear)
    {
        // Arrange
        var period = new Period(new DateTime(startYear, startMonth, 15), new DateTime(endYear, endMonth, 15));

        // Act
        var actualResult = period.GetMonthAndYearStrRoman();

        // Assert
        return actualResult;
    }

    [TestCase(1, 2015, 3, ExpectedResult = 1)]
    [TestCase(1, 2015, 4, ExpectedResult = 1)]
    [TestCase(1, 2015, 5, ExpectedResult = 2)]
    [TestCase(1, 2015, 6, ExpectedResult = 2)]
    [TestCase(2, 2015, 15, ExpectedResult = 7)]
    [TestCase(2, 2015, 28, ExpectedResult = 9)]
    [TestCase(12, 2015, 31, ExpectedResult = 53)]
    public int GetWeekNumber_CorrectPeriod_ReturnNumberWeekOnStartDate(int startMonth, int startYear, int startDay)
    {
        // Arrange
        var period = new Period(new DateTime(startYear, startMonth, startDay), new DateTime(startYear + 1, startMonth, 1));

        // Act
        var actualResult = period.GetWeekNumber();

        // Assert
        return actualResult;
    }

    [TestCase(12, 2015, 31, "ru-RU", ExpectedResult = 53)]
    [TestCase(12, 2015, 31, "en-US", ExpectedResult = 53)]
    [TestCase(12, 2015, 31, "en-GB", ExpectedResult = 53)]
    [TestCase(12, 2015, 31, "th-TH", ExpectedResult = 53)]
    public int GetWeekNumber_CorrectPeriodWithDiffrentCulture_ReturnNumberWeekOnStartDate(int startMonth, int startYear, int startDay, string isoCultureName)
    {
        // Arrange
        var period = new Period(new DateTime(startYear, startMonth, startDay), new DateTime(startYear + 1, startMonth, 1));

        // Act
        var actualResult = period.GetWeekNumber(isoCultureName);

        // Assert
        return actualResult;
    }

    [TestCase(1, 2015, 1, 2015, ExpectedResult = "15.01-15.01")]
    [TestCase(1, 2015, 2, 2015, ExpectedResult = "15.01-15.02")]
    [TestCase(2, 2015, 2, 2015, ExpectedResult = "15.02-15.02")]
    [TestCase(5, 2014, 5, 2016, ExpectedResult = "15.05.2014-15.05.2016")]
    public string ToExtraShortString_CorrectPeriod_ResultInDefaultCulture(int startMonth, int startYear, int endMonth, int endYear)
    {
        // Arrange
        var period = new Period(new DateTime(startYear, startMonth, 15), new DateTime(endYear, endMonth, 15));

        // Act
        var actualResult = period.ToExtraShortString();

        // Assert
        return actualResult;
    }

    [TestCase(1, 2015, ExpectedResult = "15.01.2015-∞")]
    [TestCase(5, 2014, ExpectedResult = "15.05.2014-∞")]
    public string ToExtraShortString_CorrectPeriodWithoutEndDate_ResultInDefaultCulture(int startMonth, int startYear)
    {
        // Arrange
        var period = new Period(new DateTime(startYear, startMonth, 15));

        // Act
        var actualResult = period.ToExtraShortString();

        // Assert
        return actualResult;
    }

    [TestCase(5, 2015, 5, 2015, "ru-RU", ExpectedResult = "15.05-15.05")]
    [TestCase(5, 2015, 5, 2015, "en-US", ExpectedResult = "5/15-5/15")]
    [TestCase(5, 2015, 5, 2015, "ro-RO", ExpectedResult = "15.05-15.05")]
    [TestCase(5, 2015, 5, 2015, "fr-CH", ExpectedResult = "15.05-15.05")]
    [TestCase(5, 2015, 5, 2015, "pl-PL", ExpectedResult = "15.05-15.05")]
    [TestCase(5, 2015, 5, 2015, "xh-ZA", ExpectedResult = "05-15-05-15")]

    [TestCase(5, 2014, 5, 2016, "ru-RU", ExpectedResult = "15.05.2014-15.05.2016")]
    [TestCase(5, 2014, 5, 2016, "en-US", ExpectedResult = "5/15/2014-5/15/2016")]
    [TestCase(5, 2014, 5, 2016, "ro-RO", ExpectedResult = "15.05.2014-15.05.2016")]
    [TestCase(5, 2014, 5, 2016, "fr-CH", ExpectedResult = "15.05.2014-15.05.2016")]
    [TestCase(5, 2014, 5, 2016, "pl-PL", ExpectedResult = "15.05.2014-15.05.2016")]
    [TestCase(5, 2014, 5, 2016, "xh-ZA", ExpectedResult = "2014-05-15-2016-05-15")]
    public string ToExtraShortString_CorrectPeriodWithCulture_ResultInSelectedCulture(int startMonth, int startYear, int endMonth, int endYear, string isoCultureName)
    {
        // Arrange
        var period = new Period(new DateTime(startYear, startMonth, 15), new DateTime(endYear, endMonth, 15));

        // Act
        var actualResult = period.ToExtraShortString(new CultureInfo(isoCultureName));

        // Assert
        return actualResult;
    }

    [TestCase(5, 2015, "ru-RU", ExpectedResult = "15.05.2015-∞")]
    [TestCase(5, 2015, "en-US", ExpectedResult = "5/15/2015-∞")]
    [TestCase(5, 2015, "pl-PL", ExpectedResult = "15.05.2015-∞")]
    [TestCase(5, 2015, "xh-ZA", ExpectedResult = "2015-05-15-∞")]
    public string ToExtraShortString_CorrectPeriodWithCultureAndWithoutEndDate_ResultInSelectedCulture(int startMonth, int startYear, string isoCultureName)
    {
        // Arrange
        var period = new Period(new DateTime(startYear, startMonth, 15));

        // Act
        var actualResult = period.ToExtraShortString(new CultureInfo(isoCultureName));

        // Assert
        return actualResult;
    }

    [TestCase(15, 5, 2015, 15, 5, 2015, ExpectedResult = "15 May (2015)")]
    [TestCase(15, 5, 2015, 15, 6, 2015, ExpectedResult = "15.05-15.06 (2015)")]
    [TestCase(1, 5, 2015, 31, 5, 2015, ExpectedResult = "May (2015)")]
    [TestCase(1, 5, 2015, 15, 5, 2016, ExpectedResult = "01.05.2015-15.05.2016")]
    [TestCase(1, 1, 2015, 30, 1, 2015, ExpectedResult = "01-30 January (2015)")]
    public string ToString_PeriodWithoutCulture_ResultInIadCulture(int startDay, int startMonth, int startYear, int endDay, int endMonth, int endYear)
    {
        // Arrange
        var period = new Period(new DateTime(startYear, startMonth, startDay), new DateTime(endYear, endMonth, endDay));

        // Act
        var actualResult = period.ToString();

        // Assert
        return actualResult;
    }

    [TestCase(15, 5, 2015, ExpectedResult = "15.05.2015-∞")]
    [TestCase(1, 1, 2015, ExpectedResult = "01.01.2015-∞")]
    public string ToString_PeriodWithoutCultureAndWithoutEndDate_ResultInIadCulture(int startDay, int startMonth, int startYear)
    {
        // Arrange
        var period = new Period(new DateTime(startYear, startMonth, startDay));

        // Act
        var actualResult = period.ToString();

        // Assert
        return actualResult;
    }

    /// <summary>
    /// IADFRAME-796 Ошибка в методе GetFinancialYear
    /// </summary>
    [TestCase(2017, 1, 2017)]
    [TestCase(2017, 3, 2017)]
    [TestCase(2017, 4, 2018)]
    [TestCase(2017, 12, 2018)]
    [TestCase(1, 12, 2)]
    [TestCase(1, 3, 1)]
    [TestCase(9999, 3, 9999)]
    public void GetFinancialYear_DifferentYearMonthCombinations_ResultAsExpected(int year, int month, int expectedFinYear)
    {
        // Arrange

        // Act
        var financialYear = Period.GetFinancialYear(year, month);

        // Assert
        financialYear.Should().Be(expectedFinYear);
    }

    /// <summary>
    /// IADFRAME-796 Ошибка в методе GetFinancialYear
    /// </summary>
    [TestCase(-1)]
    [TestCase(0)]
    [TestCase(13)]
    public void GetFinancialYear_MonthOutOfRange_ThrowArgumentOutOfRangeException(int month)
    {
        // Arrange

        // Act
        Action action = () => Period.GetFinancialYear(2017, month);

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    /// <summary>
    /// IADFRAME-796 Ошибка в методе GetFinancialYear
    /// </summary>
    [TestCase(-1, 12)]
    [TestCase(0, 12)]
    [TestCase(10000, 1)]
    [TestCase(9999, 4)]
    public void GetFinancialYear_YearOutOfRange_ThrowArgumentOutOfRangeException(int year, int month)
    {
        // Arrange

        // Act
        Action action = () => Period.GetFinancialYear(year, month);

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [TestCase(2020, 05, 2020, 08, 3)]
    [TestCase(2020, 12, 2021, 02, 2)]
    [TestCase(2020, 01, 2020, 01, 0)]
    [TestCase(2020, 01, 2019, 05, null)]
    public void TotalMonths_Period_ResultAsExpected(int startDateYear, int startDateMonth, int endDateYear, int endDateMonth, int? expectedResult)
    {
        // Arrange
        var period = new Period(new DateTime(startDateYear, startDateMonth, 1), new DateTime(endDateYear, endDateMonth, 1));

        // Act
        var totalMonths = period.TotalMonths();

        // Assert
        totalMonths.Should().Be(expectedResult);
    }

    [Test]
    public void TotalMonths_PeriodWithNullEndDate_ReturnNull()
    {
        // Arrange
        var period = new Period(DateTime.Today);

        // Act
        var totalMonths = period.TotalMonths();

        // Assert
        totalMonths.Should().BeNull();
    }
}
