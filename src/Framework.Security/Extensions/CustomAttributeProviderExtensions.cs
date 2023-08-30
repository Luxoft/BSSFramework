using System.Reflection;

using Framework.Core;

namespace Framework.Security;

public static class CustomAttributeProviderExtensions
{
    public static SecurityOperationAttribute GetSecurityOperationAttribute(this Enum value, bool handleDisabled = true)
    {
        if (value == null) throw new ArgumentNullException(nameof(value));

        return handleDisabled && value.IsDefaultEnumValue()
                       ? new SecurityOperationAttribute("Disabled", false, new Guid().ToString())
                       : value.ToFieldInfo().GetSecurityOperationAttribute();
    }

    public static SecurityOperationAttribute GetSecurityOperationAttribute(this FieldInfo fieldInfo)
    {
        if (fieldInfo == null) throw new ArgumentNullException(nameof(fieldInfo));

        return fieldInfo.GetCustomAttribute<SecurityOperationAttribute>()
                        .FromMaybe(() => $"SecurityOperationAttribute for field \"{fieldInfo.Name}\" not found");
    }
}
