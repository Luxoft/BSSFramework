using Framework.Core;

using SecuritySystem.Credential;

namespace Framework.DomainDriven;

public static class ServiceEvaluatorExtensions
{
    extension<TService>(IServiceEvaluator<TService> contextEvaluator)
    {
        public async Task<TResult> EvaluateAsync<TResult>(DBSessionMode sessionMode, Func<TService, Task<TResult>> getResult) => await contextEvaluator.EvaluateAsync(sessionMode, null, getResult);

        public async Task EvaluateAsync(DBSessionMode sessionMode, UserCredential? customUserCredential, Func<TService, Task> action) =>
            await contextEvaluator.EvaluateAsync(sessionMode, customUserCredential, async service =>
            {
                await action(service);
                return default(object?);
            });

        public async Task EvaluateAsync(DBSessionMode sessionMode, Func<TService, Task> action) => await contextEvaluator.EvaluateAsync(sessionMode, null, action);

        public void Evaluate(
            DBSessionMode sessionMode,
            Action<TService> action) =>
            contextEvaluator.Evaluate(sessionMode, action.ToDefaultFunc());

        public void Evaluate(DBSessionMode sessionMode, UserCredential? customUserCredential, Action<TService> action) => contextEvaluator.Evaluate(sessionMode, customUserCredential, action.ToDefaultFunc());

        public TResult Evaluate<TResult>(DBSessionMode sessionMode, Func<TService, TResult> getResult) => contextEvaluator.Evaluate(sessionMode, null, getResult);

        public TResult Evaluate<TResult>(DBSessionMode sessionMode, UserCredential? customUserCredential, Func<TService, TResult> getResult)
        {
            TaskResultHelper<TResult>.TypeIsNotTaskValidate();

            return contextEvaluator.EvaluateAsync(sessionMode, customUserCredential, async service => getResult(service)).GetAwaiter().GetResult();
        }
    }
}
