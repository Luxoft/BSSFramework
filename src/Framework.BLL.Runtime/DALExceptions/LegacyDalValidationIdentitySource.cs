using System.Reflection;

using Framework.Application.DALExceptions;

namespace Framework.BLL.DALExceptions;

public class LegacyDalValidationIdentitySource : IDalValidationIdentitySource
{
    public string GetTypeValidationName(Type type)
    {
        return type.GetValidationName();
    }

    public string GetPropertyValidationName(PropertyInfo property)
    {
        return property.GetValidationName();
    }
}
