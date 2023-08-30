using System.CodeDom;

namespace Framework.CodeDom;

public class CodeNameofExpression : CodeExpression
{
    public CodeNameofExpression()
    {
    }

    public CodeNameofExpression(string value)
    {
        this.Value = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string Value { get; set; }
}
