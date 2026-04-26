using Framework.BLL.Domain.Attributes;

using Anch.SecuritySystem;

namespace SampleSystem.Domain.Directories;

[SecurityNode]
public interface ILocationSecurityElement<out TLocation>
        where TLocation : ISecurityContext
{
    TLocation Location { get; }
}
