using Framework.Core;
using Framework.QueryLanguage;
using NUnit.Framework;

using Framework.Persistent;

namespace Framework.OData.Tests.Unit;

[TestFixture]
public class ODataLiftTests
{
    [Test]
    public void NullableIntWithIntersect_Executed()
    {
        // Arrange

        var value = 100;

        var testQuery = $"$filter={value} ge {nameof(TestIntObjContainer.Int)}";

        var testData = new[] { new TestIntObjContainer { InnerObj = new TestIntObj { Int = value - 1 } }, new TestIntObjContainer { InnerObj = null } }.AsQueryable();

        // Act
        var parsedExpr = SelectOperation.Parse(testQuery);

        var parsedTypedExpr = StandartExpressionBuilder.Default.ToTyped<TestIntObjContainer>(parsedExpr);

        var typesSelectOperation = parsedTypedExpr.Visit(ExpandPathVisitor.Value);

        // Assert

        var testResult = typesSelectOperation.Process(testData).ToList();

        return;
    }

    [Test]
    public void NullablePeriodWithIntersect_Executed()
    {
        // Arrange

        var today = DateTime.Today;

        var testQuery = $"$filter=isIntersectedP({nameof(TestPeriodObjContainer.Period)}, period(datetime'{today}'))";

        var testData = new[] { new TestPeriodObjContainer { InnerObj = new TestPeriodObj { Period = new Period(today.SubtractDay(), today.AddDay()) } }, new TestPeriodObjContainer { InnerObj = null } }.AsQueryable();

        // Act
        var parsedExpr = SelectOperation.Parse(testQuery);

        var parsedTypedExpr = StandartExpressionBuilder.Default.ToTyped<TestPeriodObjContainer>(parsedExpr);

        var typesSelectOperation = parsedTypedExpr.Visit(ExpandPathVisitor.Value);

        // Assert

        var testResult = typesSelectOperation.Process(testData).ToList();

        return;
    }

    [Test]
    public void NullablePeriodWithContains_Executed()
    {
        // Arrange

        var today = DateTime.Today;

        var testQuery = $"$filter=containsP({nameof(TestPeriodObjContainer.Period)}, datetime'{today}')";

        var testData = new[] { new TestPeriodObjContainer { InnerObj = new TestPeriodObj { Period = new Period(today.SubtractDay(), today.AddDay()) } }, new TestPeriodObjContainer { InnerObj = null } }.AsQueryable();

        // Act
        var parsedExpr = SelectOperation.Parse(testQuery);

        var parsedTypedExpr = StandartExpressionBuilder.Default.ToTyped<TestPeriodObjContainer>(parsedExpr);

        var typesSelectOperation = parsedTypedExpr.Visit(ExpandPathVisitor.Value);

        // Assert

        var testResult = typesSelectOperation.Process(testData).ToList();

        return;
    }
}
