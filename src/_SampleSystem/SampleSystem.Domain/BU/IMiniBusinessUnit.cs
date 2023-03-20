using Framework.Core;
using Framework.DomainDriven;
using Framework.Persistent;
using Framework.Projection.Contract;

namespace SampleSystem.Domain;

[ProjectionContract(typeof(BusinessUnit))]
public interface IMiniBusinessUnit : IDefaultIdentityObject, IVisualIdentityObject, IPeriodObject
{

}
