using System.CodeDom;

namespace Framework.CodeDom.Extend;

public class CodeBinaryOperatorCollectionExpression : CodeExpression
{
    public CodeBinaryOperatorCollectionExpression()
    {

    }

    public CodeBinaryOperatorCollectionExpression(CodeBinaryOperatorType @operator, params CodeExpression[] expressions)
    {
        this.Operator = @operator;
        this.Expressions = new CodeExpressionCollection(expressions);
    }


    public CodeBinaryOperatorType Operator { get; set; }

    public CodeExpressionCollection Expressions { get; set; } = new();
}
