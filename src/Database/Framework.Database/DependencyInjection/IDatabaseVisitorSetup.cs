using System.Linq.Expressions;

namespace Framework.Database.DependencyInjection;

public interface IDatabaseVisitorSetup
{
    IDatabaseVisitorSetup AddVisitor<TExpressionVisitor>()
        where TExpressionVisitor : ExpressionVisitor;
}
