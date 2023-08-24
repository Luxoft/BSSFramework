using System.Reflection;

using Framework.Validation;

namespace Framework.DomainDriven.DALExceptions;

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
