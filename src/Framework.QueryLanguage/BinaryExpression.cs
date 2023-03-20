using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

using SExpressions = System.Linq.Expressions;

using Framework.Core;

namespace Framework.QueryLanguage;

[DataContract]
public class BinaryExpression : Expression
{
    public BinaryExpression(SExpressions.BinaryExpression binaryExpression)
    {
        if (binaryExpression == null) throw new ArgumentNullException(nameof(binaryExpression));

        this.Left = Expression.Create(binaryExpression.Left);
        this.Right = Expression.Create(binaryExpression.Right);
        this.Operation = binaryExpression.NodeType.ToBinaryOperation();
    }

    public BinaryExpression(Expression left, BinaryOperation operation, Expression right)
    {
        if (left == null) throw new ArgumentNullException(nameof(left));
        if (right == null) throw new ArgumentNullException(nameof(right));

        this.Left = left;
        this.Operation = operation;
        this.Right = right;
    }


    [DataMember]
    public Expression Left { get; private set; }

    [DataMember]
    public BinaryOperation Operation { get; private set; }

    [DataMember]
    public Expression Right { get; private set; }




    public override string ToString()
    {
        return $"({this.Left} {this.Operation.ToFormatString()} {this.Right})";
    }

    public override int GetHashCode()
    {
        return base.GetHashCode() ^ this.Operation.GetHashCode();
    }

    protected override bool InternalEquals(Expression other)
    {
        return (other as BinaryExpression).Maybe(otherBinaryExpression =>

                                                         this.Operation == otherBinaryExpression.Operation
                                                         && this.Left      == otherBinaryExpression.Left
                                                         && this.Right     == otherBinaryExpression.Right);
    }
}
