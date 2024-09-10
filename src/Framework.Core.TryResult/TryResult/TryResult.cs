namespace Framework.Core;

public static class TryResult
{
    public static ITryResult<TResult> Return<TResult>(TResult result)
    {
        return new SuccessResult<TResult>(result);
    }

    public static ITryResult<TArgs, TResult> Return<TArgs, TResult>(TArgs args, TResult result)
    {
        return new SuccessResult<TArgs, TResult>(args, result);
    }


    public static ITryResult<TResult> CreateFault<TResult>(Exception error)
    {
        if (error == null) throw new ArgumentNullException(nameof(error));

        return new FaultResult<TResult>(error);
    }

    public static ITryResult<TArgs, TResult> CreateFault<TArgs, TResult>(TArgs args, Exception error)
    {
        if (error == null) throw new ArgumentNullException(nameof(error));

        return new FaultResult<TArgs, TResult>(args, error);
    }


    public static ITryResult<TResult> CreateBreak<TResult>()
    {
        return new FaultResult<TResult>(new BreakException());
    }


    public static ITryResult<Ignore> Return()
    {
        return Return(Ignore.Value);
    }

    public static ITryResult<Ignore> CreateFault(Exception error)
    {
        if (error == null) throw new ArgumentNullException(nameof(error));

        return CreateFault<Ignore>(error);
    }

    public static ITryResult<Ignore> CreateBreak()
    {
        return CreateBreak<Ignore>();
    }


    public static ITryResult<Ignore> Catch(Action tryGetResult)
    {
        return Catch<Ignore>(() => { tryGetResult(); return Ignore.Value; });
    }

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

    public static ITryResult<TArgs, TResult> Catch<TArgs, TResult>(TArgs args, Func<TArgs, TResult> tryGetResult)
    {
        return Catch(args, tryGetResult, z => z);
    }

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


    private class SuccessResult<T> : ISuccessResult<T>
    {
        public SuccessResult(T result)
        {
            this.Result = result;
        }

        public T Result { get; private set; }
    }

    private class FaultResult<T> : IFaultResult<T>
    {
        public FaultResult(Exception error)
        {
            if (error == null) throw new ArgumentNullException(nameof(error));

            this.Error = error;
        }

        public Exception Error { get; private set; }
    }

    private class SuccessResult<TArgs, TResult> : ISuccessResult<TArgs, TResult>
    {
        public SuccessResult(TArgs args, TResult result)
        {
            this.Result = result;
            this.Args = args;
        }

        public TResult Result { get; private set; }

        public TArgs Args { get; private set; }
    }

    private class FaultResult<TArgs, TResult> : IFaultResult<TArgs, TResult>
    {
        public FaultResult(TArgs args, Exception error)
        {
            if (error == null) throw new ArgumentNullException(nameof(error));

            this.Error = error;
            this.Args = args;
        }

        public Exception Error { get; private set; }

        public TArgs Args { get; private set; }
    }
}
