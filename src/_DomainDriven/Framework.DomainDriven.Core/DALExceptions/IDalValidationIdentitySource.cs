using System.Reflection;

namespace Framework.DomainDriven.DALExceptions;

public interface IDalValidationIdentitySource
{
    string GetTypeValidationName(Type type);

    string GetPropertyValidationName(PropertyInfo property);
}


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
