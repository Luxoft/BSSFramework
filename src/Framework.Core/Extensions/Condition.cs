namespace Framework.Core;

public struct Condition<T>
{
    private readonly T _source;
    private bool _conditionResult;

    public Condition(T source, bool conditionResult)
            : this()
    {
        this._source = source;
        this._conditionResult = conditionResult;
    }

    public bool ConditionResult
    {
        get { return this._conditionResult; }
    }

    public T Source
    {
        get { return this._source; }
    }
}
