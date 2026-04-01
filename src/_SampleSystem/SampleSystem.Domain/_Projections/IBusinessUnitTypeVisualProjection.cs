using Framework.Application.Domain;
using Framework.BLL.Domain.ServiceRole;
using Framework.Projection;

namespace SampleSystem.Domain;

[BLLProjectionViewRole]
[ProjectionContract(typeof(BusinessUnitType))]
public interface IBusinessUnitTypeVisualProjection : IIdentityObject<Guid>, IVisualIdentityObject;
