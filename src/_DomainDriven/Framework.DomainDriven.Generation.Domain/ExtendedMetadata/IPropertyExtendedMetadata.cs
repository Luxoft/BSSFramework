using System.Reflection;

namespace Framework.DomainDriven.Generation.Domain.ExtendedMetadata;

public interface IPropertyExtendedMetadata : IAttributesExtendedMetadata
{
    PropertyInfo Property { get; }
}
