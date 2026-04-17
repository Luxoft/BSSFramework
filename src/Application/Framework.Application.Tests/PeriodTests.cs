using Framework.Application.FinancialYear;

namespace Framework.Application.Tests;

// MethodUnderTest_Scenario_Behavior
public class PeriodTests
{
    /// <summary>
    /// IADFRAME-796 Ошибка в методе GetFinancialYear
    /// </summary>
    [Theory]
    [InlineData(2017, 1, 2017)]
    [InlineData(2017, 3, 2017)]
    [InlineData(2017, 4, 2018)]
    [InlineData(2017, 12, 2018)]
    [InlineData(1, 12, 2)]
    [InlineData(1, 3, 1)]
    [InlineData(9999, 3, 9999)]
    public void GetFinancialYear_DifferentYearMonthCombinations_ResultAsExpected(int year, int month, int expectedFinYear)
    {
        // Arrange
        var service = new FinancialYearCalculator(new FinancialYearServiceSettings());

        // Act
        var financialYear = service.GetFinancialYear(year, month);

        // Assert
        Assert.Equal(expectedFinYear, financialYear);
    }

    /// <summary>
    /// IADFRAME-796 Ошибка в методе GetFinancialYear
    /// </summary>
    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(13)]
    public void GetFinancialYear_MonthOutOfRange_ThrowArgumentOutOfRangeException(int month)
    {
        // Arrange
        var service = new FinancialYearCalculator(new FinancialYearServiceSettings());

        // Act
        Action action = () => service.GetFinancialYear(2017, month);

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(action);
    }

    /// <summary>
    /// IADFRAME-796 Ошибка в методе GetFinancialYear
    /// </summary>
    [Theory]
    [InlineData(-1, 12)]
    [InlineData(0, 12)]
    [InlineData(10000, 1)]
    [InlineData(9999, 4)]
    public void GetFinancialYear_YearOutOfRange_ThrowArgumentOutOfRangeException(int year, int month)
    {
        // Arrange
        var service = new FinancialYearCalculator(new FinancialYearServiceSettings());

        // Act
        Action action = () => service.GetFinancialYear(year, month);

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(action);
    }
}
