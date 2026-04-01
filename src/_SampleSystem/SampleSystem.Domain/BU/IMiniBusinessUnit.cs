using Framework.Application.Domain;
using Framework.Core;
using Framework.Projection;

namespace SampleSystem.Domain;

[ProjectionContract(typeof(BusinessUnit))]
public interface IMiniBusinessUnit : IIdentityObject<Guid>, IVisualIdentityObject, IPeriodObject;
