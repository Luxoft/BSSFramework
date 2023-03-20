using System;
using System.CodeDom;

using Framework.CodeDom;

using JetBrains.Annotations;

namespace Framework.DomainDriven.ServiceModelGenerator;

public static class CodeExpressionExtensions
{
    public static CodeExpression GetContext([NotNull] this CodeExpression evaluateDataExpr)
    {
        if (evaluateDataExpr == null) throw new ArgumentNullException(nameof(evaluateDataExpr));

        return evaluateDataExpr.ToPropertyReference("Context");
    }

    public static CodeExpression GetMappingService([NotNull] this CodeExpression evaluateDataExpr)
    {
        if (evaluateDataExpr == null) throw new ArgumentNullException(nameof(evaluateDataExpr));

        return evaluateDataExpr.ToPropertyReference("MappingService");
    }
}
