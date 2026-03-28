namespace Framework.QueryLanguage;

public record UnaryExpression(UnaryOperation Operation, Expression Operand) : Expression
{
    public override string ToString() => $"({this.Operation.ToFormatString()}{this.Operand})";

    public override int GetHashCode() => this.Operation.GetHashCode();

    public virtual bool Equals(UnaryExpression? other) =>
        object.ReferenceEquals(this, other)
        || (other is not null
            && this.Operation == other.Operation
            && this.Operand == other.Operand);
}
