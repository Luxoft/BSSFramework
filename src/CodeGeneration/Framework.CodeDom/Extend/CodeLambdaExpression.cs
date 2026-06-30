using System.CodeDom;

namespace Framework.CodeDom.Extend;

public class CodeLambdaExpression : CodeExpression
{
    private CodeStatementCollection statements = new();

    private CodeParameterDeclarationExpressionCollection parameters = new();


    public CodeLambdaExpression()
    {
        this.Statements = new CodeStatementCollection();
        this.Parameters = new CodeParameterDeclarationExpressionCollection();
    }


    public CodeStatementCollection Statements
    {
        get => this.statements;
        set => this.statements = value ?? new CodeStatementCollection();
    }

    public CodeParameterDeclarationExpressionCollection Parameters
    {
        get => this.parameters;
        set => this.parameters = value ?? new CodeParameterDeclarationExpressionCollection();
    }
}

