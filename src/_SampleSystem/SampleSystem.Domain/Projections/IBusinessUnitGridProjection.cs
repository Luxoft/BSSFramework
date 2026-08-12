using Framework.Application.Domain;
using Framework.BLL.Domain.ServiceRole;
using Framework.Core;
using Framework.Projection;

using SampleSystem.Domain.BU;

namespace SampleSystem.Domain.Projections;

[BLLProjectionViewRole]
[ProjectionContract(typeof(BusinessUnit))]
public interface IBusinessUnitGridProjection : IIdentityObject<Guid>, IVisualIdentityObject, IPeriodObject
{
    IBusinessUnitTypeVisualProjection BusinessUnitType { get; }
}
