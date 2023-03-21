using System;
using System.CodeDom;

using JetBrains.Annotations;

namespace Framework.CodeDom;

public class CodeNameofExpression : CodeExpression
{
    public CodeNameofExpression()
    {
    }

    public CodeNameofExpression([NotNull] string value)
    {
        this.Value = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string Value { get; set; }
}
