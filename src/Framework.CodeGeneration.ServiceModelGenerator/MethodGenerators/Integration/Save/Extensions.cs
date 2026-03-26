using System.CodeDom;
using System.Reflection;

using Framework.Persistent;

namespace Framework.DomainDriven.ServiceModelGenerator;

internal static class Extensions
{
    internal static CodeBinaryOperatorType ToCodeBinaryOperator(this PropertyInfo integrationVersionProperty)
    {
        return integrationVersionProperty.GetCustomAttribute<IntegrationVersionAttribute>().IntegrationPolicy == ApplyIntegrationPolicy.IgnoreLessVersion
                       ? CodeBinaryOperatorType.LessThanOrEqual
                       : CodeBinaryOperatorType.LessThan;
    }
}
