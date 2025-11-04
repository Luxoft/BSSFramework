using Framework.Core;
using Framework.Persistent;
using Framework.Projection.Contract;

namespace SampleSystem.Domain;

[ProjectionContract(typeof(BusinessUnit))]
public interface IMiniBusinessUnit : IIdentityObject<Guid>, IVisualIdentityObject, IPeriodObject
{

}
