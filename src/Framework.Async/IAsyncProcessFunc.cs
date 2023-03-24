using System;

namespace Framework.Async;

public interface IAsyncProcessFunc<in TArg, out TResult>
{
}

public interface IAsyncProcessFunc<in TArg1, in TArg2, out TResult>
{
}

public interface IAsyncProcessFunc<in TArg1, in TArg2, in TArg3, out TResult>
{
}

public interface IAsyncProcessFunc<in TArg1, in TArg2, in TArg3, in TArg4, out TResult>
{
}

public interface IAsyncProcessFunc<in TArg1, in TArg2, in TArg3, in TArg4, in TArg5, out TResult>
{
}

public interface IAsyncProcessFunc<in TArg1, in TArg2, in TArg3, in TArg4, in TArg5, in TArg6, out TResult>
{
}
