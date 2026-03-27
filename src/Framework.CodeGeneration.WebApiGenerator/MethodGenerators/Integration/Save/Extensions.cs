using System.CodeDom;
using System.Reflection;

using Framework.BLL.Domain.Attributes;

namespace Framework.CodeGeneration.WebApiGenerator.MethodGenerators.Integration.Save;

internal static class Extensions
{
    internal static CodeBinaryOperatorType ToCodeBinaryOperator(this PropertyInfo integrationVersionProperty)
    {
        return integrationVersionProperty.GetCustomAttribute<IntegrationVersionAttribute>().IntegrationPolicy == ApplyIntegrationPolicy.IgnoreLessVersion
                       ? CodeBinaryOperatorType.LessThanOrEqual
                       : CodeBinaryOperatorType.LessThan;
    }
}
