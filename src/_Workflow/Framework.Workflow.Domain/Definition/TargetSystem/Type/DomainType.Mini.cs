using System;

using Framework.Persistent;
using Framework.Projection.Contract;

namespace Framework.Workflow.Domain.Definition
{
    public partial class DomainType : IVisualDomainType
    {
    }

    [ProjectionContract(typeof(DomainType))]
    public interface IVisualDomainType : IDefaultIdentityObject, IVisualIdentityObject
    {
    }
}