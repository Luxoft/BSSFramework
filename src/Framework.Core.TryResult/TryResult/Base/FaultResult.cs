using System;

namespace Framework.Core;

public interface IFaultResult
{
    Exception Error { get; }
}

public interface IFaultResult<out T> : ITryResult<T>, IFaultResult
{
}

public interface IFaultResult<TArgs, TResult> : ITryResult<TArgs, TResult>, IFaultResult
{
    TArgs Args { get; }
}
