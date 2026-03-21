using System.Reflection;

namespace Framework.Application.DALExceptions;

public interface IDalValidationIdentitySource
{
    string GetTypeValidationName(Type type);

    string GetPropertyValidationName(PropertyInfo property);
}
