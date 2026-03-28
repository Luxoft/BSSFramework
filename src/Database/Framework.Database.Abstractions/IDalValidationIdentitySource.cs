using System.Reflection;

namespace Framework.Database;

public interface IDalValidationIdentitySource
{
    string GetTypeValidationName(Type type);

    string GetPropertyValidationName(PropertyInfo property);
}
