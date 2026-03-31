using Framework.BLL.Domain.Attributes;
using Framework.BLL.Domain.Persistent.Attributes;
using Framework.Persistent;
using Framework.Security;
using SecuritySystem;

namespace SampleSystem.Domain;

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
