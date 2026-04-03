using System.Reflection;

namespace Framework.Projection.ExtendedMetadata;

public interface IPropertyExtendedMetadata : IAttributesExtendedMetadata
{
    PropertyInfo Property { get; }
}
