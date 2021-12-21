using System;

using Framework.Persistent;
using Framework.Projection.Contract;

namespace Framework.Workflow.Domain.Definition
{
    public partial class StateBase : IMiniStateBase
    {
    }

    [ProjectionContract(typeof(StateBase))]
    public interface IMiniStateBase : IDefaultIdentityObject, IVisualIdentityObject
    {
    }
}