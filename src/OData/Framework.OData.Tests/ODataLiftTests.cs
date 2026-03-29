using Framework.BLL.Domain.Persistent.Visitors;

using Framework.OData.QueryLanguage.StandardExpressionBuilder;
using Framework.OData.Tests.LiftTestData;
using Framework.OData.Typed;

using NUnit.Framework;

namespace Framework.OData.Tests;

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

        var parsedTypedExpr = StandardExpressionBuilder.Default.ToTyped<TestIntObjContainer>(parsedExpr);

        var typesSelectOperation = parsedTypedExpr.Visit(ExpandPathVisitor.Value);

        // Assert

        var testResult = typesSelectOperation.Process(testData).ToList();

        return;
    }
}
