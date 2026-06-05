using Anch.SecuritySystem;

using Framework.BLL.Domain.Attributes;

namespace SampleSystem.Domain.Directories;

[SecurityNode]
public interface ILocationSecurityElement<out TLocation>
        where TLocation : ISecurityContext
{
    TLocation Location { get; }
}

