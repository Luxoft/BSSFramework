using Framework.Core;

namespace Framework.StateMachine;

public class StateMachine : StateMachine<State>
{
    public StateMachine(State initialState)
            : base(initialState)
    {
    }
}
public class StateMachine<TState>
{
    private TState _currentState;
    private readonly Dictionary<TState, IList<ActionDescription<TState>>> _stateToRegisterEventTypes;

    public StateMachine(TState initialState)
    {
        this._stateToRegisterEventTypes = new Dictionary<TState, IList<ActionDescription<TState>>>();
        this._currentState = initialState;
    }

    public TState CurrentState
    {
        get { return this._currentState; }
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T">EventType</typeparam>
    /// <param name="initialState"></param>
    /// <param name="postActions">Действие, выполняемые после перехода в targetState</param>
    /// <param name="action"></param>
    public void RegisterState<T>(TState initialState, Func<T, bool> action, TState targetState, params Action[] postActions)
    {
        IList<ActionDescription<TState>> eventTypes;
        if (!this._stateToRegisterEventTypes.TryGetValue(initialState, out eventTypes))
        {
            eventTypes = new List<ActionDescription<TState>>();
            this._stateToRegisterEventTypes.Add(initialState, eventTypes);
        }

        var actionWrapper = new ActionWrapper();
        actionWrapper.Register(action, postActions);

        eventTypes.Add(new ActionDescription<TState>(typeof(T), actionWrapper, targetState));
    }
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T">EventType</typeparam>
    /// <param name="initialState"></param>
    /// <param name="action"></param>
    public void RegisterState<T>(TState initialState, Func<T, bool> action)
    {
        this.RegisterState<T>(initialState, action, initialState);
    }

    public void ProcessEvent<T>(T @event)
    {
        IList<ActionDescription<TState>> eventTypes;
        if (!this._stateToRegisterEventTypes.TryGetValue(this._currentState, out eventTypes))
        {
            return;
        }
        var actionDescriptions = eventTypes.Where(z => z.EventType.Equals(typeof(T))).ToList();
        if (0 == actionDescriptions.Count)
        {
            return;
        }
        foreach (var actionDescription in actionDescriptions)
        {
            var actionWrapper = actionDescription.ActionWrapper;
            var action = actionWrapper.GetAction<T>();
            if (action(@event))
            {
                this._currentState = actionDescription.TargetState;
                actionWrapper.PostActions.Foreach(z => z());
                break;
            }
        }
    }
}
