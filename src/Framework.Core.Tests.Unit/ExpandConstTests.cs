using System;
using System.Linq;
using System.Linq.Expressions;

using FluentAssertions;

using Framework.Core.ExpressionComparers;

using NUnit.Framework;

namespace Framework.Core.Tests.Unit;

[TestFixture]
public class ExpandConstTests
{
    [Test]
    public void ExpandTestFunc_ExprEquals()
    {
        // Arrange
        var baseTestExpr = GetTestFunc(123);

        Expression<Func<int, int>> expectedTestExpr = x => x + 123;

        // Act
        var expandedTestExpr = baseTestExpr.ExpandConst();

        // Assert
        var equals = ExpressionComparer.Value.Equals(expandedTestExpr, expectedTestExpr);
        equals.Should().Be(true);
    }

    [Test]
    public void ExpandDeepTestFunc_ExprEquals()
    {
        // Arrange
        var baseTestExpr = GetDeepTestFunc(123);

        Expression<Func<IQueryable<int>, IQueryable<int>>> expectedTestExpr = q => q.Where(x => x + 123 > 0);

        // Act
        var expandedTestExpr = baseTestExpr.ExpandConst();

        // Assert
        var equals = ExpressionComparer.Value.Equals(expandedTestExpr, expectedTestExpr);
        equals.Should().Be(true);
    }

    private static Expression<Func<int, int>> GetTestFunc(int y)
    {
        return x => x + y;
    }

    private static Expression<Func<IQueryable<int>, IQueryable<int>>> GetDeepTestFunc(int y)
    {
        return q => q.Where(x => x + y > 0);
    }
}
