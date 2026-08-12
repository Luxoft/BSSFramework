using Framework.Application.Domain;
using Framework.BLL.Domain.ServiceRole;
using Framework.Projection;

using SampleSystem.Domain.Directories;

namespace SampleSystem.Domain.Projections;

[BLLProjectionViewRole]
[ProjectionContract(typeof(BusinessUnitType))]
public interface IBusinessUnitTypeVisualProjection : IIdentityObject<Guid>, IVisualIdentityObject;
