using System.Linq.Expressions;

using CommonFramework;

using FluentAssertions;

using Framework.Core;
using Framework.DomainDriven._Visitors;

using Xunit;

namespace SampleSystem.UnitTests;

public class ExtractConstTests
{
    [Fact]
    public void NullValueChainExpression_GetDeepMemberConstValue_NotThrowException()
    {
        // Arrange
        Parameters p = null;
        Expression<Func<Obj, bool>> testExpr = obj => p.Period.StartDate > obj.Period.StartDate;

        Action action = () => testExpr.UpdateBase(OverrideHashSetVisitor<Guid>.Value);

        action.Should().NotThrow();
    }

    public record Parameters(Period Period);

    public record Obj(Period Period);
}
