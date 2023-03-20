using Framework.DomainDriven.BLL;
using Framework.Persistent;
using Framework.Projection.Contract;

namespace SampleSystem.Domain;

[BLLProjectionViewRole]
[ProjectionContract(typeof(BusinessUnitType))]
public interface IBusinessUnitTypeVisualProjection : IDefaultIdentityObject, IVisualIdentityObject
{
}
