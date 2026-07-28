using System.Linq.Expressions;

using Framework.Core.Visitors;
using Framework.Database;

using SampleSystem.Domain.Projections;

namespace SampleSystem.ServiceEnvironment;

public class CalculatedProjectPropertyVisitorContainer : ExpressionVisitorAggregator
{
    protected override IEnumerable<ExpressionVisitor> GetVisitors()
    {
        yield return new OverridePropertyVisitor<TestEmployee, string>(e => e.PositionNameOrRoleName, TestEmployee.GetPositionNameOrRoleNameExpr);
    }
}
