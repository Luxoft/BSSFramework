using System.Reflection;

using Framework.Core;

namespace Framework.Projection.Environment;

public interface IDomainTypeRootExtendedMetadata
{
    ICustomAttributeProvider GetType(Type type);

    ICustomAttributeProvider GetProperty(PropertyInfo property);

    TAttribute GetCustomAttribute<TAttribute>(Type type)
        where TAttribute : Attribute => this.GetType(type).GetCustomAttribute<TAttribute>();

    bool HasAttribute<TAttribute>(Type type)
        where TAttribute : Attribute
        => this.GetType(type).HasAttribute<TAttribute>();

    bool HasAttribute<TAttribute>(Type type, Func<TAttribute, bool> predicate)
        where TAttribute : Attribute
        => this.GetType(type).GetCustomAttributes<TAttribute>().Any(predicate);
}
