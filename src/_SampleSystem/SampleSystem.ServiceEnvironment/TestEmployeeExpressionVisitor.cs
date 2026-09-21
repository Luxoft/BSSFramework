using Framework.Core.Visitors;

using SampleSystem.Domain.Projections;

namespace SampleSystem.ServiceEnvironment;

public class TestEmployeeExpressionVisitor() : OverridePropertyVisitor<TestEmployee, string>(e => e.PositionNameOrRoleName, TestEmployee.GetPositionNameOrRoleNameExpr);
