using System.Reflection;

namespace Framework.ExtendedMetadata;

public interface IWrappingObject : ICustomAttributeProvider
{
    bool CanWrap { get; }
}
