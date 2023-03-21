namespace Framework.StateMachine;

/// <summary>
///
/// </summary>
/// <typeparam name="T">јргументы событи¤</typeparam>
public sealed class Event<T>
{
    private readonly T arg;

    public Event(T arg)
    {
        this.arg = arg;
    }

    public T Arg
    {
        get { return this.arg; }
    }
}
