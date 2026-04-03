using System.Reflection;

namespace Framework.Database.DALExceptions;

public class DalValidationIdentitySource : IDalValidationIdentitySource
{
    public string GetTypeValidationName(Type type) => type.Name;

    public string GetPropertyValidationName(PropertyInfo property) => property.Name;
}
