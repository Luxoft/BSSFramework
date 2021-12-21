using System.CodeDom;

namespace Framework.CodeDom
{
    public class CodeForeachStatement : CodeStatement
    {
        private CodeStatementCollection statements = new CodeStatementCollection();

        private CodeParameterDeclarationExpression iterator = new CodeParameterDeclarationExpression();

        public CodeStatementCollection Statements
        {
            get { return this.statements; }
            set { this.statements = value ?? new CodeStatementCollection(); }
        }

        public CodeParameterDeclarationExpression Iterator
        {
            get { return this.iterator; }
            set { this.iterator = value ?? new CodeParameterDeclarationExpression(); }
        }

        public CodeExpression Source { get; set; }
    }
}
