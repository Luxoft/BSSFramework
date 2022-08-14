using System;
using System.Threading.Tasks;

using Framework.Core;
using JetBrains.Annotations;

namespace Framework.DomainDriven.BLL
{
    public static class ContextEvaluatorExtensions
    {
        public static Task<TResult> EvaluateAsync<TBLLContext, TResult>(this IContextEvaluator<TBLLContext> contextEvaluator, DBSessionMode sessionMode, [NotNull] Func<TBLLContext, IDBSession, Task<TResult>> getResult)
        {
            return contextEvaluator.EvaluateAsync(sessionMode, null, getResult);
        }

        public static Task<TResult> EvaluateAsync<TBLLContext, TResult>(this IContextEvaluator<TBLLContext> contextEvaluator, DBSessionMode sessionMode, [NotNull] Func<TBLLContext, Task<TResult>> getResult)
        {
            return contextEvaluator.EvaluateAsync(sessionMode, null, (ctx, _) => getResult(ctx));
        }

        public static Task EvaluateAsync<TBLLContext>(this IContextEvaluator<TBLLContext> contextEvaluator, DBSessionMode sessionMode, string customPrincipalName, [NotNull] Func<TBLLContext, IDBSession, Task> action)
        {
            return contextEvaluator.EvaluateAsync(sessionMode, customPrincipalName, (ctx, session) => action(ctx, session).ContinueWith(_ => default(object)));
        }

        public static Task EvaluateAsync<TBLLContext>(this IContextEvaluator<TBLLContext> contextEvaluator, DBSessionMode sessionMode, [NotNull] Func<TBLLContext, IDBSession, Task> action)
        {
            return contextEvaluator.EvaluateAsync(sessionMode, null, action);
        }

        public static Task EvaluateAsync<TBLLContext>(this IContextEvaluator<TBLLContext> contextEvaluator, DBSessionMode sessionMode, [NotNull] Func<TBLLContext, Task> action)
        {
            return contextEvaluator.EvaluateAsync(sessionMode, null, (ctx, _) => action(ctx));
        }

        public static void Evaluate<TBLLContext>(
            this IContextEvaluator<TBLLContext> contextEvaluator,
            DBSessionMode sessionMode,
            [NotNull] Action<TBLLContext, IDBSession> action)
        {
            contextEvaluator.Evaluate(sessionMode, action.ToDefaultFunc());
        }

        public static void Evaluate<TBLLContext>(this IContextEvaluator<TBLLContext> contextEvaluator, DBSessionMode sessionMode, string customPrincipalName, [NotNull] Action<TBLLContext, IDBSession> action)
        {
            contextEvaluator.Evaluate(sessionMode, customPrincipalName, action.ToDefaultFunc());
        }

        public static TResult Evaluate<TBLLContext, TResult>(this IContextEvaluator<TBLLContext> contextEvaluator, DBSessionMode sessionMode, [NotNull] Func<TBLLContext, IDBSession, TResult> getResult)
        {
            return contextEvaluator.Evaluate(sessionMode, null, getResult);
        }

        public static void Evaluate<TBLLContext>(this IContextEvaluator<TBLLContext> contextEvaluator, DBSessionMode sessionMode, [NotNull] Action<TBLLContext> action)
        {
            contextEvaluator.Evaluate(sessionMode, null, action);
        }

        public static void Evaluate<TBLLContext>(this IContextEvaluator<TBLLContext> contextEvaluator, DBSessionMode sessionMode, string customPrincipalName, [NotNull] Action<TBLLContext> action)
        {
            contextEvaluator.Evaluate(sessionMode, customPrincipalName, action.ToDefaultFunc());
        }

        public static TResult Evaluate<TBLLContext, TResult>(this IContextEvaluator<TBLLContext> contextEvaluator, DBSessionMode sessionMode, [NotNull] Func<TBLLContext, TResult> getResult)
        {
            return contextEvaluator.Evaluate(sessionMode, null, getResult);
        }

        public static TResult Evaluate<TBLLContext, TResult>(this IContextEvaluator<TBLLContext> contextEvaluator, DBSessionMode sessionMode, string customPrincipalName, [NotNull] Func<TBLLContext, TResult> getResult)
        {
            return contextEvaluator.Evaluate(sessionMode, customPrincipalName, (c, _) => getResult(c));
        }

        public static TResult Evaluate<TBLLContext, TResult>(this IContextEvaluator<TBLLContext> contextEvaluator, DBSessionMode sessionMode, string customPrincipalName, [NotNull] Func<TBLLContext, IDBSession, TResult> getResult)
        {
            return contextEvaluator.EvaluateAsync(sessionMode, customPrincipalName, (c, s) => Task.FromResult(getResult(c, s))).GetAwaiter().GetResult();
        }
    }
}
