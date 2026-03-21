using System.Collections.Immutable;

using SExpressions = System.Linq.Expressions;

namespace Framework.QueryLanguage;

public record LambdaExpression(Expression Body, ImmutableArray<ParameterExpression> Parameters) : Expression
{
    public LambdaExpression(SExpressions.LambdaExpression expression)
        : this(Create(expression.Body), [.. expression.Parameters.Select(p => new ParameterExpression(p.Name!))])
    {
    }

    public Type ExtractTargetType<TDomainObject>() =>
        this.ExtractPropertyPath(this.Body)
            .Reverse()
            .Aggregate(
                (SExpressions.Expression)SExpressions.Expression.Parameter(typeof(TDomainObject)),
                (currentExpr, property) =>
                    SExpressions.Expression.PropertyOrField(currentExpr, property.PropertyName))
            .Type;

    private IEnumerable<PropertyExpression> ExtractPropertyPath(Expression currentNode)
    {
        switch (currentNode)
        {
            case ParameterExpression parameterExpression when parameterExpression == this.Parameters.Single():
                yield break;

            case PropertyExpression propertyExpression:
            {
                yield return propertyExpression;

                foreach (var baseNode in this.ExtractPropertyPath(propertyExpression.Source))
                {
                    yield return baseNode;
                }

                break;
            }
            default:
                throw new Exception("invalid expression");
        }
    }

    public override string ToString() => $"({string.Join(", ", this.Parameters)}) => {this.Body}";

    public virtual bool Equals(LambdaExpression? other) =>
        object.ReferenceEquals(this, other)
        || (other is not null
            && this.Parameters.SequenceEqual(other.Parameters)
            && this.Body == other.Body);

    public override int GetHashCode() => base.GetHashCode() ^ this.Parameters.Length;
}
