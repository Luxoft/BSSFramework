using System.Reflection;

namespace Framework.Projection.Environment;

public interface IDomainTypeRootExtendedMetadata
{
    ICustomAttributeProvider GetType(Type type);

    ICustomAttributeProvider GetProperty(PropertyInfo property);
}
