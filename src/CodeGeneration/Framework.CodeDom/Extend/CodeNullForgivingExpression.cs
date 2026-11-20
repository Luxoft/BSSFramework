using System.CodeDom;

namespace Framework.CodeDom.Extend;

/// <summary>
/// Null-forgiving operator (<c>expr!</c>), rendered only when nullable annotations are enabled
/// </summary>
public class CodeNullForgivingExpression : CodeExpression
{
    public CodeNullForgivingExpression()
    {
    }

    public CodeNullForgivingExpression(CodeExpression expression) => this.Expression = expression ?? throw new ArgumentNullException(nameof(expression));

    public CodeExpression Expression { get; set; } = null!;
}
