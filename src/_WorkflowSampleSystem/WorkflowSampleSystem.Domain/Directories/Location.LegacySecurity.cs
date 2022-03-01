using System;

using Framework.Security;
using Framework.SecuritySystem;

namespace WorkflowSampleSystem.Domain
{
    [SecurityNode]
    public interface ILocationSecurityElement<out TLocation>
        where TLocation : ISecurityContext
    {
        TLocation Location { get; }
    }
}
