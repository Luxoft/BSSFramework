using System.Reflection;

namespace Framework.DomainDriven.Generation.Domain.ExtendedMetadata;

public interface IDomainTypeRootExtendedMetadata
{
    ICustomAttributeProvider GetType(Type type);

    ICustomAttributeProvider GetProperty(PropertyInfo property);
}
