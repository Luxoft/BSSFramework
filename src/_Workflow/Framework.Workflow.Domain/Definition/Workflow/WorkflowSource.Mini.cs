using System;
using Framework.DomainDriven;

using Framework.Persistent;
using Framework.Projection.Contract;

namespace Framework.Workflow.Domain.Definition
{
    public partial class WorkflowSource : IMiniWorkflowSource
    {
        IVisualDomainType ITypeObject<IVisualDomainType>.Type => this.Type;
    }

    [ProjectionContract(typeof(WorkflowSource))]
    public interface IMiniWorkflowSource : IDefaultIdentityObject, IVisualIdentityObject, IDescriptionObject, ITypeObject<IVisualDomainType>
    {
    }
}