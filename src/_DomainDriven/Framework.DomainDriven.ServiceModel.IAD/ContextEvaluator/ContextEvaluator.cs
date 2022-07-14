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

        public async Task<TResult> EvaluateAsync<TResult>(DBSessionMode sessionMode, string principalName, Func<TBLLContext, IDBSession, Task<TResult>> getResult)
        {
            await using var scope = this.rootServiceProvider.CreateAsyncScope();

            var scopeServiceProvider = scope.ServiceProvider;
            var session = scopeServiceProvider.GetRequiredService<IDBSession>();

            if (sessionMode == DBSessionMode.Read)
            {
                session.AsReadOnly();
            }

            var defaultPrincipalName = scopeServiceProvider.GetRequiredService<IUserAuthenticationService>().GetUserName();

            var impersonateService = !string.IsNullOrWhiteSpace(principalName) && principalName != defaultPrincipalName
                                             ? scopeServiceProvider.GetRequiredService<IImpersonateService>()
                                             : null;

            impersonateService?.RunAs(principalName);

            try
            {
                var context = scopeServiceProvider.GetRequiredService<TBLLContext>();

                return await getResult(context, session);
            }
            finally
            {
                impersonateService?.FinishRunAs();
            }
        }
    }

    public interface IImpersonateService
    {
        void RunAs(string principalName);

        void FinishRunAs();
    }
}
