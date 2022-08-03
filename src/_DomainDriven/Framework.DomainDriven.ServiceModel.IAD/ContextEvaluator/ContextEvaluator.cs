using System;
using System.Threading.Tasks;

using Framework.Core.Services;
using Framework.DomainDriven.BLL;

using JetBrains.Annotations;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.ServiceModel.IAD
{
    public class ContextEvaluator<TBLLContext> : IContextEvaluator<TBLLContext>
    {
        private readonly IServiceProvider rootServiceProvider;

        public ContextEvaluator([NotNull] IServiceProvider rootServiceProvider)
        {
            this.rootServiceProvider = rootServiceProvider ?? throw new ArgumentNullException(nameof(rootServiceProvider));
        }

        public async Task<TResult> EvaluateAsync<TResult>(DBSessionMode sessionMode, string customPrincipalName, Func<TBLLContext, IDBSession, Task<TResult>> getResult)
        {
            await using var scope = this.rootServiceProvider.CreateAsyncScope();

            var scopeServiceProvider = scope.ServiceProvider;
            using var session = scopeServiceProvider.GetRequiredService<IDBSession>();

            if (sessionMode == DBSessionMode.Read)
            {
                session.AsReadOnly();
            }

            var defaultPrincipalName = scopeServiceProvider.GetRequiredService<IUserAuthenticationService>().GetUserName();

            var impersonateService = !string.IsNullOrWhiteSpace(customPrincipalName) && customPrincipalName != defaultPrincipalName
                                             ? scopeServiceProvider.GetRequiredService<IImpersonateService>()
                                             : null;

            var context = scopeServiceProvider.GetRequiredService<TBLLContext>();

            TResult result;

            try
            {
                if (impersonateService == null)
                {
                    result = await getResult(context, session);
                }
                else
                {
                    result = await impersonateService.WithImpersonateAsync(customPrincipalName, () => getResult(context, session));
                }
            }
            catch
            {
                session.AsFault();

                throw;
            }

            return result;
        }
    }
}
