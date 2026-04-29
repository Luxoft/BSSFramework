using System.Linq.Expressions;

using Anch.Core;

using Framework.Core;
using Framework.Database.Visitors;

namespace SampleSystem.UnitTests;

public class ExtractConstTests
{
    [Fact]
    public void NullValueChainExpression_GetDeepMemberConstValue_NotThrowException()
    {
        // Arrange
        Parameters? p = null;
        Expression<Func<Obj, bool>> testExpr = obj => p!.Period.StartDate > obj.Period.StartDate;

        // Act
        var exception = Record.Exception(() => testExpr.UpdateBase(OverrideHashSetVisitor<Guid>.Value));

        // Assert
        Assert.Null(exception);
    }

    public record Parameters(Period Period);

    public record Obj(Period Period);
}
