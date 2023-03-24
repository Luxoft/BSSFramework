using System;

namespace Framework.StateMachine;

public class ActionDescription : ActionDescription<State>
{
    public ActionDescription(Type eventType, ActionWrapper actionWrapper, State targetState) : base(eventType, actionWrapper, targetState)
    {
    }
}
public class ActionDescription<TState>
{
    private readonly Type eventType;
    private readonly ActionWrapper actionWrapper;
    private readonly TState targetState;

    public ActionDescription(Type eventType, ActionWrapper actionWrapper, TState targetState)
    {
        this.eventType = eventType;
        this.actionWrapper = actionWrapper;
        this.targetState = targetState;
    }

    public Type EventType
    {
        get { return this.eventType; }
    }

    public ActionWrapper ActionWrapper
    {
        get { return this.actionWrapper; }
    }

    public TState TargetState
    {
        get { return this.targetState; }
    }
}
