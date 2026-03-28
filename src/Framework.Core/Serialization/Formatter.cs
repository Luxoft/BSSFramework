namespace Framework.Core.Serialization;

public class Formatter<TValue, TResult> : IFormatter<TValue, TResult>
{
    private readonly Func<TValue, TResult> formatFunc;


    public Formatter(Func<TValue, TResult> formatFunc)
    {
        if (formatFunc == null) throw new ArgumentNullException(nameof(formatFunc));

        this.formatFunc = formatFunc;
    }


    public virtual TResult Format(TValue value) => this.formatFunc(value);
}
