using System.Collections.ObjectModel;
using System.Runtime.Serialization;

using Framework.Core;

using SExpressions = System.Linq.Expressions;

namespace Framework.QueryLanguage;

[DataContract]
public class LambdaExpression : Expression
{
    public LambdaExpression(SExpressions.LambdaExpression expression)
    {
        if (expression == null) throw new ArgumentNullException(nameof(expression));

        this.Body = Create(expression.Body);
        this.Parameters = expression.Parameters.ToReadOnlyCollection(p => new ParameterExpression(p.Name));
    }

    public LambdaExpression(Expression body, params ParameterExpression[] parameters)
            : this(body, (IEnumerable<ParameterExpression>)parameters)
    {

    }

    public LambdaExpression(Expression body, IEnumerable<ParameterExpression> parameters)
    {
        if (body == null) throw new ArgumentNullException(nameof(body));
        if (parameters == null) throw new ArgumentNullException(nameof(parameters));

        this.Body = body;
        this.Parameters = parameters.CheckNotNull().ToReadOnlyCollection();
    }


    [DataMember]
    public Expression Body { get; private set; }

    [DataMember]
    public ReadOnlyCollection<ParameterExpression> Parameters { get; private set; }


    public Type ExtractTargetType<TDomainObject>()
    {
        return this.ExtractPropertyPath(this.Body).Reverse().Aggregate(typeof(TDomainObject), (currentType, property) => currentType.GetMemberType(property.PropertyName, true));
    }

    private IEnumerable<PropertyExpression> ExtractPropertyPath(Expression currentNode)
    {
        if (currentNode == null) throw new ArgumentNullException(nameof(currentNode));

        if (currentNode is ParameterExpression)
        {
            if ((currentNode as ParameterExpression) != this.Parameters.Single())
            {
                throw new Exception("invalid startup element");
            }
            else
            {
                yield break;
            }
        }
        else if (currentNode is PropertyExpression)
        {
            var currentProperty = currentNode as PropertyExpression;

            yield return currentProperty;

            foreach (var baseNode in this.ExtractPropertyPath(currentProperty.Source))
            {
                yield return baseNode;
            }
        }
        else
        {
            throw new Exception("invalid expression");
        }
    }

    public override string ToString()
    {
        return $"({this.Parameters.Join(", ")}) => {this.Body}";
    }

    public override int GetHashCode()
    {
        return base.GetHashCode() ^ this.Parameters.Count;
    }


    protected override bool InternalEquals(Expression other)
    {
        return (other as LambdaExpression).Maybe(otherLambdaExpression =>
                                                         this.Parameters.SequenceEqual(otherLambdaExpression.Parameters)
                                                         && this.Body == otherLambdaExpression.Body);
    }
}
