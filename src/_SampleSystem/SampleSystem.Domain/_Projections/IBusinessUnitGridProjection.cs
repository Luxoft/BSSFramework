using Framework.Application.Domain;
using Framework.BLL.Domain.ServiceRole;
using Framework.Core;
using Framework.Projection;

namespace SampleSystem.Domain;

[BLLProjectionViewRole]
[ProjectionContract(typeof(BusinessUnit))]
public interface IBusinessUnitGridProjection : IIdentityObject<Guid>, IVisualIdentityObject, IPeriodObject
{
    IBusinessUnitTypeVisualProjection BusinessUnitType { get; }
}
