using System;
using System.Reflection;

namespace Framework.Core
{
    public static class MethodBaseExtensions
    {
        [Obsolete]
        public static object InvokeWithExceptionProcessed(this MethodBase methodBase, object obj, params object[] parameters)
        {
            if (methodBase == null) throw new ArgumentNullException(nameof(methodBase));

            return methodBase.Invoke(obj, parameters);
        }
    }
}
