namespace Framework.Core;

public interface ISuccessResult<out T> : ITryResult<T>
{
    T Result { get; }
}

public interface ISuccessResult<TArgs, TResult> : ITryResult<TArgs, TResult>
{
    TResult Result { get; }

    TArgs Args { get; }
}
