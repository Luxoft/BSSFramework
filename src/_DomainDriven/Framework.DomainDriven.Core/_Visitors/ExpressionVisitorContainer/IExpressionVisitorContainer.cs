using System.Linq.Expressions;

namespace Framework.DomainDriven._Visitors;

public interface IExpressionVisitorContainer
{
    ExpressionVisitor Visitor { get; }
}

