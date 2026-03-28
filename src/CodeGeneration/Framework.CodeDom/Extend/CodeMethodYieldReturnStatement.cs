using System.CodeDom;

namespace Framework.CodeDom.Extend;

public class CodeMethodYieldReturnStatement : CodeStatement
{
    public CodeExpression Expression { get; set; }
}
