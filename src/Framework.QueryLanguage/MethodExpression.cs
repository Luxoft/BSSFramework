using System.Collections.ObjectModel;
using System.Runtime.Serialization;

using CommonFramework;

using Framework.Core;

namespace Framework.QueryLanguage;

[DataContract]
public class MethodExpression : Expression
{
    public MethodExpression(Expression source, MethodExpressionType type, params Expression[] arguments)
            : this(source, type, (IEnumerable<Expression>)arguments)
    {

    }

    public MethodExpression(Expression source, MethodExpressionType type, IEnumerable<Expression> arguments)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (arguments == null) throw new ArgumentNullException(nameof(arguments));

        this.Source = source;
        this.Type = type;
        this.Arguments = arguments.ToReadOnlyCollection();
    }


    [DataMember]
    public Expression Source { get; private set; }

    [DataMember]
    public MethodExpressionType Type { get; private set; }

    [DataMember]
    public ReadOnlyCollection<Expression> Arguments { get; private set; }


    public override string ToString()
    {
        return $"{this.Source}.{this.Type.ToFormatString()}({this.Arguments.Join(", ")})";
    }


    public override int GetHashCode()
    {
        return base.GetHashCode() ^ this.Type.GetHashCode();
    }

    protected override bool InternalEquals(Expression other)
    {
        return (other as MethodExpression).Maybe(otherMethodExpression =>

                                                         this.Type == otherMethodExpression.Type
                                                         && this.Source == otherMethodExpression.Source
                                                         && this.Arguments.SequenceEqual(otherMethodExpression.Arguments));
    }
}
