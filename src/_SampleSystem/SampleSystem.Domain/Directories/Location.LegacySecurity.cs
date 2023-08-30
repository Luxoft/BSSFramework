using Framework.Security;
using Framework.SecuritySystem;

namespace SampleSystem.Domain;

[SecurityNode]
public interface ILocationSecurityElement<out TLocation>
        where TLocation : ISecurityContext
{
    TLocation Location { get; }
}
