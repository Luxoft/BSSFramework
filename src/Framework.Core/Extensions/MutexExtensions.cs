namespace Framework.Core;

public static class MutexHelper
{
    public static TResult GlobalLock<TResult> (string name, Func<TResult> func)
    {
        if (name == null) throw new ArgumentNullException(nameof(name));
        if (func == null) throw new ArgumentNullException(nameof(func));

        using (var mutex = new Mutex(true, name))
        {
            mutex.WaitOne();

            try
            {
                return func();
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }
    }

    public static void GlobalLock(string name, Action action)
    {
        if (name == null) throw new ArgumentNullException(nameof(name));
        if (action == null) throw new ArgumentNullException(nameof(action));

        GlobalLock(name, () => { action(); return Ignore.Value; });
    }
}
