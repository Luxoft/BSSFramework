using NUnit.Framework;

namespace Framework.Core.Tests.Unit;

[TestFixture]
public class MonthFormatterTest
{
    [TestCase(2015, 1, ExpectedResult = "Январе 2015 г.")]
    [TestCase(2025, 5, ExpectedResult = "Мае 2025 г.")]
    [TestCase(200, 12, ExpectedResult = "Декабре 200 г.")]
    [TestCase(3015, 3, ExpectedResult = "Марте 3015 г.")]
    public string GetInMonthAndYearStrRus_AnyCorrectDate_StringInPrepositional(int year, int month)
    {
        // Arrange
        var dateTime = new DateTime(year, month, 15);

        // Act
        var actualResult = dateTime.GetInMonthAndYearStrRus();

        // Assert
        return actualResult;
    }

    [TestCase(2015, 1, ExpectedResult = "Январь 2015 г.")]
    [TestCase(2025, 5, ExpectedResult = "Май 2025 г.")]
    [TestCase(200, 12, ExpectedResult = "Декабрь 200 г.")]
    [TestCase(3015, 3, ExpectedResult = "Март 3015 г.")]
    public string GetMonthAndYearStrRus_AnyCorrectDate_StringInNominative(int year, int month)
    {
        // Arrange
        var dateTime = new DateTime(year, month, 15);

        // Act
        var actualResult = dateTime.GetMonthAndYearStrRus();

        // Assert
        return actualResult;
    }

    [TestCase(2015, 1, ExpectedResult = "January 2015")]
    [TestCase(2025, 5, ExpectedResult = "May 2025")]
    [TestCase(200, 12, ExpectedResult = "December 200")]
    [TestCase(3015, 3, ExpectedResult = "March 3015")]
    public string GetMonthAndYearStr_AnyCorrectDate_StringInNominative(int year, int month)
    {
        // Arrange
        var dateTime = new DateTime(year, month, 15);

        // Act
        var actualResult = dateTime.GetMonthAndYearStr();

        // Assert
        return actualResult;
    }

    [TestCase(2015, 1, ExpectedResult = "January")]
    [TestCase(2025, 5, ExpectedResult = "May")]
    [TestCase(200, 12, ExpectedResult = "December")]
    [TestCase(3015, 3, ExpectedResult = "March")]
    public string GetMonthStr_AnyCorrectDate_StringInNominative(int year, int month)
    {
        // Arrange
        var dateTime = new DateTime(year, month, 15);

        // Act
        var actualResult = dateTime.GetMonthStr();

        // Assert
        return actualResult;
    }

    [TestCase(2015, 1, ExpectedResult = "I '15")]
    [TestCase(2025, 5, ExpectedResult = "V '25")]
    [TestCase(3015, 4, ExpectedResult = "IV '15")]
    [TestCase(3015, 6, ExpectedResult = "VI '15")]
    [TestCase(210, 10, ExpectedResult = "X '10")]
    [TestCase(200, 11, ExpectedResult = "XI '00")]
    [TestCase(2015, 9, ExpectedResult = "IX '15")]
    public string GetMonthAndYearStrRoman_AnyCorrectDate_StringWithRomanNumerals(int year, int month)
    {
        // Arrange
        var dateTime = new DateTime(year, month, 15);

        // Act
        var actualResult = dateTime.GetMonthAndYearStrRoman();

        // Assert
        return actualResult;
    }

    [TestCase(2015, 1, ExpectedResult = "I '15")]
    [TestCase(2025, 5, ExpectedResult = "V '25")]
    [TestCase(3015, 4, ExpectedResult = "IV '15")]
    [TestCase(3015, 6, ExpectedResult = "VI '15")]
    [TestCase(210, 10, ExpectedResult = "I '53")]
    [TestCase(200, 11, ExpectedResult = "I '53")]
    [TestCase(2015, 9, ExpectedResult = "IX '15")]
    public string GetMonthAndYearStrRoman_AnyCorrectPeriodWithinOneMonth_StringWithRomanNumerals(int year, int month)
    {
        // Arrange
        var period = new Period(new DateTime(year, month, 10), new DateTime(year, month, 20));

        // Act
        var actualResult = MonthFormatter.GetMonthAndYearStrRoman(period);

        // Assert
        return actualResult;
    }

    [TestCase(2015, 1, 2015, 2, ExpectedResult = "I -II '15")]
    [TestCase(2015, 4, 2015, 9, ExpectedResult = "IV -IX '15")]
    [TestCase(2015, 8, 2015, 12, ExpectedResult = "VIII -XII '15")]
    [TestCase(2014, 1, 2016, 12, ExpectedResult = "I -XII '14")]
    [TestCase(214, 5, 216, 6, ExpectedResult = "I '53")]
    [TestCase(3015, 5, 3015, 6, ExpectedResult = "V -VI '15")]
    public string GetMonthAndYearStrRoman_AnyCorrectPeriodNotInOneMonth_StringWithRomanNumerals(int startYear, int startMonth, int endYear, int endMonth)
    {
        // Arrange
        var period = new Period(new DateTime(startYear, startMonth, 10), new DateTime(endYear, endMonth, 20));

        // Act
        var actualResult = MonthFormatter.GetMonthAndYearStrRoman(period);

        // Assert
        return actualResult;
    }
}
