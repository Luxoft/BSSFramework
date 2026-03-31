using System.Reflection;

using SecuritySystem;

namespace Framework.CodeGeneration.BLLCoreGenerator.Configuration;

public static class PropertyInfoExtensions
{
    public static SecurityRule? TryGetSecurityRule(this PropertyInfo property) =>

        property.GetValue(null) switch
        {
            SecurityOperation securityOperation => securityOperation,
            SecurityRole securityRole => securityRole,
            SecurityRule securityRule => securityRule,
            _ => null
        };
}
