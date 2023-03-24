namespace Framework.Parsing;

public interface IResult<out TInput, out TValue>
{
    TValue Value { get; }

    TInput Rest { get; }
}

public class Result<TInput, TValue> : IResult<TInput, TValue>
{
    public Result(TValue value, TInput rest)
    {
        this.Value = value;
        this.Rest = rest;
    }


    public TValue Value { get; private set; }

    public TInput Rest { get; private set; }
}
