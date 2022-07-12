using System;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace Framework.DomainDriven.BLL
{
    public static class ContextEvaluatorExtensions
    {
        public static void Evaluate<TBLLContext>(
            this IContextEvaluator<TBLLContext> context,
            DBSessionMode sessionMode,
            [NotNull] Action<TBLLContext, IDBSession> action)
        {
            context.Evaluate(sessionMode, action.ToDefaultFunc());
        }

        public static void Evaluate<TBLLContext>(this IContextEvaluator<TBLLContext> context, DBSessionMode sessionMode, string principalName, [NotNull] Action<TBLLContext, IDBSession> action)
        {
            context.Evaluate(sessionMode, principalName, action.ToDefaultFunc());
        }

        public static TResult Evaluate<TBLLContext, TResult>(this IContextEvaluator<TBLLContext> context, DBSessionMode sessionMode, [NotNull] Func<TBLLContext, IDBSession, TResult> getResult)
        {
            return context.Evaluate(sessionMode, null, getResult);
        }

        public static void Evaluate<TBLLContext>(this IContextEvaluator<TBLLContext> context, DBSessionMode sessionMode, [NotNull] Action<TBLLContext> action)
        {
            context.Evaluate(sessionMode, null, action);
        }

        public static void Evaluate<TBLLContext>(this IContextEvaluator<TBLLContext> context, DBSessionMode sessionMode, string principalName, [NotNull] Action<TBLLContext> action)
        {
            context.Evaluate(sessionMode, principalName, action.ToDefaultFunc());
        }

        public static TResult Evaluate<TBLLContext, TResult>(this IContextEvaluator<TBLLContext> context, DBSessionMode sessionMode, [NotNull] Func<TBLLContext, TResult> getResult)
        {
            return context.Evaluate(sessionMode, null, getResult);
        }

        public static TResult Evaluate<TBLLContext, TResult>(this IContextEvaluator<TBLLContext> context, DBSessionMode sessionMode, string principalName, [NotNull] Func<TBLLContext, TResult> getResult)
        {
            return context.Evaluate(sessionMode, principalName, (c, _) => getResult(c));
        }

        public static TResult Evaluate<TBLLContext, TResult>(this IContextEvaluator<TBLLContext> context, DBSessionMode sessionMode, string principalName, [NotNull] Func<TBLLContext, IDBSession, TResult> getResult)
        {
            return context.EvaluateAsync(sessionMode, principalName, (c, s) => Task.FromResult(getResult (c, s))).Result;
        }

        private static Func<T, object> ToDefaultFunc<T>(this Action<T> action)
        {
            return a => { action(a); return null; };
        }


        private static Func<T1, T2, object> ToDefaultFunc<T1, T2>(this Action<T1, T2> action)
        {
            return (a1, a2) => { action(a1, a2); return null; };
        }
    }
}
