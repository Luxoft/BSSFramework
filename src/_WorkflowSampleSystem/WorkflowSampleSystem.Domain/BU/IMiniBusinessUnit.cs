using Framework.Core;
using Framework.DomainDriven;
using Framework.Persistent;
using Framework.Projection.Contract;

namespace WorkflowSampleSystem.Domain
{
    [ProjectionContract(typeof(BusinessUnit))]
    public interface IMiniBusinessUnit : IDefaultIdentityObject, IVisualIdentityObject, IPeriodObject
    {

    }
}