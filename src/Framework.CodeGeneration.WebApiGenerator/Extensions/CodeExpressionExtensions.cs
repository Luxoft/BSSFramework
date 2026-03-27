using System.CodeDom;

using Framework.CodeDom;
using Framework.CodeDom.Extensions;

namespace Framework.CodeGeneration.WebApiGenerator.Extensions;

public static class CodeExpressionExtensions
{
    public static CodeExpression GetContext(this CodeExpression evaluateDataExpr)
    {
        if (evaluateDataExpr == null) throw new ArgumentNullException(nameof(evaluateDataExpr));

        return evaluateDataExpr.ToPropertyReference("Context");
    }

    public static CodeExpression GetMappingService(this CodeExpression evaluateDataExpr)
    {
        if (evaluateDataExpr == null) throw new ArgumentNullException(nameof(evaluateDataExpr));

        return evaluateDataExpr.ToPropertyReference("MappingService");
    }
}
