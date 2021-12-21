using System;

using Framework.Projection.Contract;

namespace Framework.Workflow.Domain.Definition
{
    public partial class State : IMiniState
    {
    }

    [ProjectionContract(typeof(State))]
    public interface IMiniState : IMiniStateBase
    {
    }
}