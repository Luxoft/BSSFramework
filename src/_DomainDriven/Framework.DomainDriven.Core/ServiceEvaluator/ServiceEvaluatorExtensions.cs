using Framework.Core;

using SecuritySystem.Credential;

namespace Framework.DomainDriven;

public static class ServiceEvaluatorExtensions
{
    extension<TService>(IServiceEvaluator<TService> serviceEvaluator)
    {
        public async Task<TResult> EvaluateAsync<TResult>(DBSessionMode sessionMode, Func<TService, Task<TResult>> getResult) => await serviceEvaluator.EvaluateAsync(sessionMode, null, getResult);

        public async Task EvaluateAsync(DBSessionMode sessionMode, UserCredential? customUserCredential, Func<TService, Task> action) =>
            await serviceEvaluator.EvaluateAsync(sessionMode, customUserCredential, action.ToDefaultTask());

        public async Task EvaluateAsync(DBSessionMode sessionMode, Func<TService, Task> action) => await serviceEvaluator.EvaluateAsync(sessionMode, null, action);

        public void Evaluate(
            DBSessionMode sessionMode,
            Action<TService> action) =>
            serviceEvaluator.Evaluate(sessionMode, action.ToDefaultFunc());

        public void Evaluate(DBSessionMode sessionMode, UserCredential? customUserCredential, Action<TService> action) => serviceEvaluator.Evaluate(sessionMode, customUserCredential, action.ToDefaultFunc());

        public TResult Evaluate<TResult>(DBSessionMode sessionMode, Func<TService, TResult> getResult) => serviceEvaluator.Evaluate(sessionMode, null, getResult);

        public TResult Evaluate<TResult>(DBSessionMode sessionMode, UserCredential? customUserCredential, Func<TService, TResult> getResult)
        {
            TaskResultHelper<TResult>.TypeIsNotTaskValidate();

            return serviceEvaluator.EvaluateAsync(sessionMode, customUserCredential, async service => getResult(service)).GetAwaiter().GetResult();
        }
    }
}
