using System;

namespace Framework.Core;

public static class ExceptionHelper
{
    public static void RepeatAction<TException>(Action action, Action waitAction, int count)
            where TException : Exception
    {
        if (action == null) throw new ArgumentNullException(nameof(action));
        if (waitAction == null) throw new ArgumentNullException(nameof(waitAction));
        if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count));

        RepeatAction<TException, object>(() => { action(); return default(object); }, waitAction, count);
    }

    public static TResult RepeatAction<TException, TResult>(Func<TResult> getResultFunc, Action waitAction, int count)
            where TException : Exception
    {
        if (getResultFunc == null) throw new ArgumentNullException(nameof(getResultFunc));
        if (waitAction == null) throw new ArgumentNullException(nameof(waitAction));
        if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count));

        while (true)
        {
            try
            {
                return getResultFunc();
            }
            catch (TException)
            {
                count--;

                if (count == 0)
                {
                    throw;
                }
                else
                {
                    waitAction();
                }
            }
        }
    }
}
