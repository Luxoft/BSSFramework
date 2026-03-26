using System.CodeDom;

using Framework.CodeDom;

namespace Framework.DomainDriven.ServiceModelGenerator;

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
