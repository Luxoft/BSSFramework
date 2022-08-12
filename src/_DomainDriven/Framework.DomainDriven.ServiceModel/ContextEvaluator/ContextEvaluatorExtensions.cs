using System;
using System.Threading.Tasks;

using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.ServiceModel.Service;

using JetBrains.Annotations;

namespace Framework.DomainDriven.ServiceModel
{
    public static class ContextEvaluatorExtensions
    {
        public static Task<TResult> EvaluateAsync<TBLLContext, TDTOMappingService, TResult>(this IContextEvaluator<TBLLContext, TDTOMappingService> contextEvaluator, DBSessionMode sessionMode, [NotNull] Func<EvaluatedData<TBLLContext, TDTOMappingService>, Task<TResult>> getResult)
            where TBLLContext : class
            where TDTOMappingService : class
        {
            return contextEvaluator.EvaluateAsync(sessionMode, null, getResult);
        }

        public static Task EvaluateAsync<TBLLContext, TDTOMappingService>(this IContextEvaluator<TBLLContext, TDTOMappingService> contextEvaluator, DBSessionMode sessionMode, string customPrincipalName, [NotNull] Func<EvaluatedData<TBLLContext, TDTOMappingService>, Task> action)
            where TBLLContext : class
            where TDTOMappingService : class
        {
            return contextEvaluator.EvaluateAsync(sessionMode, customPrincipalName, evaluatedData => action(evaluatedData).ContinueWith(_ => default(object)));
        }

        public static Task EvaluateAsync<TBLLContext, TDTOMappingService>(this IContextEvaluator<TBLLContext, TDTOMappingService> contextEvaluator, DBSessionMode sessionMode, [NotNull] Func<EvaluatedData<TBLLContext, TDTOMappingService>, Task> action)
            where TBLLContext : class
            where TDTOMappingService : class
        {
            return contextEvaluator.EvaluateAsync(sessionMode, null, action);
        }

        public static void Evaluate<TBLLContext, TDTOMappingService>(this IContextEvaluator<TBLLContext, TDTOMappingService> contextEvaluator, DBSessionMode sessionMode, [NotNull] Action<EvaluatedData<TBLLContext, TDTOMappingService>> action)
            where TBLLContext : class
            where TDTOMappingService : class
        {
            contextEvaluator.Evaluate(sessionMode, null, action);
        }

        public static void Evaluate<TBLLContext, TDTOMappingService>(this IContextEvaluator<TBLLContext, TDTOMappingService> contextEvaluator, DBSessionMode sessionMode, string customPrincipalName, [NotNull] Action<EvaluatedData<TBLLContext, TDTOMappingService>> action)
            where TBLLContext : class
            where TDTOMappingService : class
        {
            contextEvaluator.Evaluate(sessionMode, customPrincipalName, action.ToDefaultFunc());
        }

        public static TResult Evaluate<TBLLContext, TDTOMappingService, TResult>(this IContextEvaluator<TBLLContext, TDTOMappingService> contextEvaluator, DBSessionMode sessionMode, [NotNull] Func<EvaluatedData<TBLLContext, TDTOMappingService>, TResult> getResult)
            where TBLLContext : class
            where TDTOMappingService : class
        {
            return contextEvaluator.Evaluate(sessionMode, null, getResult);
        }
    }
}
