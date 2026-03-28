using System.CodeDom;

namespace Framework.CodeDom.Extend;

public class CodeForeachStatement : CodeStatement
{
    private CodeStatementCollection statements = new CodeStatementCollection();

    private CodeParameterDeclarationExpression iterator = new CodeParameterDeclarationExpression();

    public CodeStatementCollection Statements
    {
        get => this.statements;
        set => this.statements = value ?? new CodeStatementCollection();
    }

    public CodeParameterDeclarationExpression Iterator
    {
        get => this.iterator;
        set => this.iterator = value ?? new CodeParameterDeclarationExpression();
    }

    public CodeExpression Source { get; set; }
}
