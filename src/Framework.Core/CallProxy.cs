namespace Framework.Core;

public class CallProxy<T>(Func<T> baseInstance)
{
    public T Value => baseInstance();
}
