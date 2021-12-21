using Framework.Core;

namespace Framework.Workflow
{
    public interface IRestrictedStateInstance
    {
        string Name { get; }

        IRestrictedWofkflowInstanceCollection<IRestrictedWofkflowInstance> SubWorkflows { get; }
    }

    public interface IRestrictedWofkflowInstanceCollection<out TRestrictedWofkflowInstance> : INamedCollection<TRestrictedWofkflowInstance>
        where TRestrictedWofkflowInstance : IRestrictedWofkflowInstance
    {
        bool AllStates(string stateName);

        bool AnyStates(string stateName);

        bool AnyAborted { get; }
    }
}