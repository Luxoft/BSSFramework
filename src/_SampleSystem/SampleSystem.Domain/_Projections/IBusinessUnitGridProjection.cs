using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.Persistent;
using Framework.Projection.Contract;
using Framework.Security;

namespace SampleSystem.Domain;

[BLLProjectionViewRole]
[ProjectionContract(typeof(BusinessUnit))]
public interface IBusinessUnitGridProjection : IDefaultIdentityObject, IVisualIdentityObject, IPeriodObject
{
    IBusinessUnitTypeVisualProjection BusinessUnitType { get; }
}
