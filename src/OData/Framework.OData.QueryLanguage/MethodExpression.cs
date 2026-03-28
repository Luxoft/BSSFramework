using System.Collections.Immutable;

using CommonFramework;

namespace Framework.OData.QueryLanguage;

public record MethodExpression(Expression Source, MethodExpressionType Type, ImmutableArray<Expression?> Arguments) : Expression
{
    public override string ToString() => $"{this.Source}.{this.Type.ToFormatString()}({this.Arguments.Join(", ")})";

    public virtual bool Equals(MethodExpression? other) =>
        object.ReferenceEquals(this, other)
        || (other is not null
            && this.Type == other.Type
            && this.Source == other.Source
            && this.Arguments.SequenceEqual(other.Arguments));

    public override int GetHashCode() => base.GetHashCode() ^ this.Type.GetHashCode();
}
