using Anch.Core;
using Anch.SecuritySystem;

using Framework.Database;

namespace Framework.BLL;

public static class SyncServiceEvaluatorExtensions
{
    extension<TService>(ISyncServiceEvaluator<TService> serviceEvaluator)
    {
        public void Evaluate(
            DBSessionMode sessionMode,
            Action<TService> action) =>
            serviceEvaluator.Evaluate(sessionMode, action.ToDefaultFunc());

        public void Evaluate(DBSessionMode sessionMode, UserCredential? customUserCredential, Action<TService> action) =>
            serviceEvaluator.Evaluate(sessionMode, customUserCredential, action.ToDefaultFunc());

        public TResult Evaluate<TResult>(DBSessionMode sessionMode, Func<TService, TResult> getResult) =>
            serviceEvaluator.Evaluate(sessionMode, null, getResult);
    }
}
