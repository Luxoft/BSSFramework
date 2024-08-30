using System.Linq.Expressions;
using System.Reflection;

using Framework.Authorization.Domain;
using Framework.Core;
using Framework.SecuritySystem;
using Framework.SecuritySystem.ExternalSystem;

namespace Framework.Authorization.SecuritySystem;

public class AuthorizationPermissionExpressionVisitor : ExpressionVisitor
{
    private static readonly MethodInfo ConvertPermissionMethod =
        new Func<Permission, ISecurityContextSource, IPermission>(ConvertPermissionExtensions.ConvertPermission).Method;

    private static readonly MethodInfo GetRestrictionsMethod =
        typeof(IPermission).GetMethod(nameof(IPermission.GetRestrictions), true);

    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
        if (node.Method == GetRestrictionsMethod)
        {

        }

        if (node.Arguments.Count > 0 && node.Arguments[0].Type == typeof(IQueryable<IPermission>))
        {
            var x = ExpandConstVisitor.Value.Visit(node);
        }

        if (node.Method == ConvertPermissionMethod)
        {
            var x = ExpandConstVisitor.Value.Visit(node);
        }

        return base.VisitMethodCall(node);
    }
}
