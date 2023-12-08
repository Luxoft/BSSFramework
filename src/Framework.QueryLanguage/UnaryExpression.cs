using System.Runtime.Serialization;
using Framework.Core;

namespace Framework.QueryLanguage;

[DataContract]
public class UnaryExpression : Expression
{
    public UnaryExpression(UnaryOperation operation, Expression operand)
    {
        if (operand == null) throw new ArgumentNullException(nameof(operand));

        this.Operation = operation;
        this.Operand = operand;
    }


    [DataMember]
    public UnaryOperation Operation { get; private set; }

    [DataMember]
    public Expression Operand { get; private set; }



    public override string ToString()
    {
        return $"({this.Operation.ToFormatString()}{this.Operand})";
    }


    public override int GetHashCode()
    {
        return base.GetHashCode() ^ this.Operation.GetHashCode();
    }

    protected override bool InternalEquals(Expression other)
    {
        return (other as UnaryExpression).Maybe(otherUnaryExpression =>
                                                        this.Operation == otherUnaryExpression.Operation
                                                        && this.Operand == otherUnaryExpression.Operand);
    }
}
