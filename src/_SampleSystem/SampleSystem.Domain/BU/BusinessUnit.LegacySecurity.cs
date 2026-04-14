using Framework.BLL.Domain.Attributes;
using Framework.BLL.Domain.Persistent.Attributes;

using SecuritySystem;

namespace SampleSystem.Domain.BU;

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
