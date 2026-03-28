using System.Reflection;

using Framework.Validation;

namespace Framework.Database.NHibernate;

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
