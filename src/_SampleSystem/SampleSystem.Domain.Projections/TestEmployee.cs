using System.Linq.Expressions;

namespace SampleSystem.Domain.Projections;

public partial class TestEmployee
{
    public static readonly Expression<Func<TestEmployee, string>> GetPositionNameOrRoleNameExpr = e => e.PositionName ?? e.RoleName;

    public static readonly Func<TestEmployee, string> GetPositionNameOrRoleNameFunc = GetPositionNameOrRoleNameExpr.Compile();

    public override string PositionNameOrRoleName => GetPositionNameOrRoleNameFunc(this);
}
