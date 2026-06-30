using Anch.Core;

// ReSharper disable once CheckNamespace
namespace Framework.Core;

public static class TryResult
{
    public static ITryResult<TResult> Return<TResult>(TResult result) => new SuccessResult<TResult>(result);

    public static ITryResult<TArgs, TResult> Return<TArgs, TResult>(TArgs args, TResult result) => new SuccessResult<TArgs, TResult>(args, result);

    public static ITryResult<TResult> CreateFault<TResult>(Exception error)
    {
        if (error is null) throw new ArgumentNullException(nameof(error));

        return new FaultResult<TResult>(error);
    }

    public static ITryResult<TArgs, TResult> CreateFault<TArgs, TResult>(TArgs args, Exception error)
    {
        if (error is null) throw new ArgumentNullException(nameof(error));

        return new FaultResult<TArgs, TResult>(args, error);
    }


    public static ITryResult<TResult> CreateBreak<TResult>() => new FaultResult<TResult>(new BreakException());

    public static ITryResult<Ignore> Return() => Return(Ignore.Value);

    public static ITryResult<Ignore> CreateFault(Exception error)
    {
        if (error is null) throw new ArgumentNullException(nameof(error));

        return CreateFault<Ignore>(error);
    }

    public static ITryResult<Ignore> CreateBreak() => CreateBreak<Ignore>();

    public static ITryResult<Ignore> Catch(Action tryGetResult) => Catch<Ignore>(() => { tryGetResult(); return Ignore.Value; });

    public static ITryResult<TResult> Catch<TResult>(Func<TResult> tryGetResult)
    {
        try
        {
            return Return(tryGetResult());
        }
        catch (Exception ex)
        {
            return CreateFault<TResult>(ex);
        }
    }

    public static ITryResult<TArgs, TResult> Catch<TArgs, TResult>(TArgs args, Func<TArgs, TResult> tryGetResult) => Catch(args, tryGetResult, z => z);

    public static ITryResult<TArgs, TResult> Catch<TArgs, TResult>(TArgs args, Func<TArgs, TResult> tryGetResult, Func<Exception, Exception> exceptionSelector)
    {
        try
        {
            return Return(args, tryGetResult(args));
        }
        catch (Exception ex)
        {
            return CreateFault<TArgs, TResult>(args, exceptionSelector(ex));
        }
    }



    public static ITryResult<TArgs, Ignore> Catch<TArgs>(TArgs args, Action<TArgs> tryGetResult)
    {
        try
        {
            tryGetResult(args);
            return Return(args, Ignore.Value);
        }
        catch (Exception ex)
        {
            return CreateFault<TArgs, Ignore>(args, ex);
        }
    }


    private class SuccessResult<T>(T result) : ISuccessResult<T>
    {
        public T Result { get; } = result;
    }

    private class FaultResult<T>(Exception error) : IFaultResult<T>
    {
        public Exception Error { get; } = error ?? throw new ArgumentNullException(nameof(error));
    }

    private class SuccessResult<TArgs, TResult>(TArgs args, TResult result) : ISuccessResult<TArgs, TResult>
    {
        public TResult Result { get; } = result;

        public TArgs Args { get; } = args;
    }

    private class FaultResult<TArgs, TResult>(TArgs args, Exception error) : IFaultResult<TArgs, TResult>
    {
        public Exception Error { get; } = error ?? throw new ArgumentNullException(nameof(error));

        public TArgs Args { get; } = args;
    }
}

