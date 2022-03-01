using System;

using Framework.Persistent;
using Framework.Security;
using Framework.SecuritySystem;

namespace WorkflowSampleSystem.Domain
{
    [SecurityNode]
    public interface IBusinessUnitSecurityElement<out TBusinessUnit>
        where TBusinessUnit : ISecurityContext
    {
        TBusinessUnit BusinessUnit { get; }
    }

    public partial class BusinessUnit : IBusinessUnitSecurityElement<BusinessUnit>
    {
        [ExpandPath("")]
        BusinessUnit IBusinessUnitSecurityElement<BusinessUnit>.BusinessUnit => this;
    }
}
