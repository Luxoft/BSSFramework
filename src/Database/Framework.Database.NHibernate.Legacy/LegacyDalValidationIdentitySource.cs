using System.Reflection;

using Framework.Validation;

namespace Framework.Database.NHibernate;

public class LegacyDalValidationIdentitySource : IDalValidationIdentitySource
{
    public string GetTypeValidationName(Type type) => type.GetValidationName();

    public string GetPropertyValidationName(PropertyInfo property) => property.GetValidationName();
}
