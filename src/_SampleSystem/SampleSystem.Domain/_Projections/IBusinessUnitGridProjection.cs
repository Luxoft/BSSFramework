using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.Persistent;
using Framework.Projection.Contract;

namespace SampleSystem.Domain;

[BLLProjectionViewRole]
[ProjectionContract(typeof(BusinessUnit))]
public interface IBusinessUnitGridProjection : IIdentityObject<Guid>, IVisualIdentityObject, IPeriodObject
{
    IBusinessUnitTypeVisualProjection BusinessUnitType { get; }
}
