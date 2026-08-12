using Framework.Core;

namespace Framework.BLL.Domain.Persistent.Attributes;

public interface IPathAttribute
{
    string Path { get; }

    public PropertyPath GetPropertyPath(Type domainType) => new(domainType, this.Path);
}
