using System.Reflection;

namespace Framework.Application.DALExceptions;

public class DalValidationIdentitySource : IDalValidationIdentitySource
{
    public string GetTypeValidationName(Type type)
    {
        return type.Name;
    }

    public string GetPropertyValidationName(PropertyInfo property)
    {
        return property.Name;
    }
}
