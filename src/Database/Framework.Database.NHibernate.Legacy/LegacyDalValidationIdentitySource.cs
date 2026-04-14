using System.Reflection;

using Framework.ExtendedMetadata;
using Framework.Validation;

namespace Framework.Database.NHibernate;

public class LegacyDalValidationIdentitySource(IMetadataProxyProvider metadataProxyProvider) : IDalValidationIdentitySource
{
    public string GetTypeValidationName(Type type) => metadataProxyProvider.Wrap(type).GetValidationName();

    public string GetPropertyValidationName(PropertyInfo property) => property.GetValidationName();
}
