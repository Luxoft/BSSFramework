namespace Framework.Core.Serialization;

public class Formatter<TValue, TResult>(Func<TValue, TResult> formatFunc) : IFormatter<TValue, TResult>
{
    private readonly Func<TValue, TResult> formatFunc = formatFunc ?? throw new ArgumentNullException(nameof(formatFunc));

    public virtual TResult Format(TValue value) => this.formatFunc(value);
}
