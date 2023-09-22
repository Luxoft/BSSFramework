using System.Reflection;

namespace Framework.Projection.Environment;

public interface IPropertyExtendedMetadata : IAttributesExtendedMetadata
{
    PropertyInfo Property { get; }
}
