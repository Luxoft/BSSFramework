using System.CodeDom;

namespace Framework.CodeDom;

public class CodeLambdaExpression : CodeExpression
{
    private CodeStatementCollection _statements;

    private CodeParameterDeclarationExpressionCollection _parameters;


    public CodeLambdaExpression()
    {
        this.Statements = new CodeStatementCollection();
        this.Parameters = new CodeParameterDeclarationExpressionCollection();
    }


    public CodeStatementCollection Statements
    {
        get { return this._statements; }
        set { this._statements = value ?? new CodeStatementCollection(); }
    }

    public CodeParameterDeclarationExpressionCollection Parameters
    {
        get { return this._parameters; }
        set { this._parameters = value ?? new CodeParameterDeclarationExpressionCollection(); }
    }
}
