using System.Linq.Expressions;

using Framework.DomainDriven._Visitors;

namespace Framework.Authorization.SecuritySystem;

public class AuthorizationExpressionVisitorContainerItem : IExpressionVisitorContainerItem
{
    public IEnumerable<ExpressionVisitor> GetVisitors()
    {
        yield return new AuthorizationPermissionExpressionVisitor();
    }
}
