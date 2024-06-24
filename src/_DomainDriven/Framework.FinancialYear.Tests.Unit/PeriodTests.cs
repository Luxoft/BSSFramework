using FluentAssertions;

using Framework.FinancialYear;

using NUnit.Framework;

namespace Framework.Core.Tests.Unit;

// MethodUnderTest_Scenario_Behavior
[TestFixture]
public class PeriodTests
{
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
        var service = new FinancialYearCalculator(new FinancialYearServiceSettings());

        // Act
        var financialYear = service.GetFinancialYear(year, month);

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
        var service = new FinancialYearCalculator(new FinancialYearServiceSettings());

        // Act
        Action action = () => service.GetFinancialYear(2017, month);

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
        var service = new FinancialYearCalculator(new FinancialYearServiceSettings());

        // Act
        Action action = () => service.GetFinancialYear(year, month);

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }
}
