using System.Reflection;

namespace Framework.DomainDriven.DALExceptions;

public interface IDalValidationIdentitySource
{
    string GetTypeValidationName(Type type);

    string GetPropertyValidationName(PropertyInfo property);
}
