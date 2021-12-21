using System;

using Framework.DomainDriven.BLL;
using Framework.DomainDriven.ServiceModel.Service;

using JetBrains.Annotations;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.ServiceModel.IAD
{
    public class RootContextEvaluator<TBLLContext> : IContextEvaluator<TBLLContext>
    {
        private readonly IServiceEnvironment<TBLLContext> serviceEnvironment;

        private readonly IServiceProvider rootServiceProvider;

        public RootContextEvaluator(
            [NotNull] IServiceEnvironment<TBLLContext> serviceEnvironment,
            [NotNull] IServiceProvider rootServiceProvider)
        {
            this.serviceEnvironment = serviceEnvironment ?? throw new ArgumentNullException(nameof(serviceEnvironment));
            this.rootServiceProvider =
                rootServiceProvider ?? throw new ArgumentNullException(nameof(rootServiceProvider));
        }

        public TResult Evaluate<TResult>(DBSessionMode sessionMode, string principalName, Func<TBLLContext, IDBSession, TResult> getResult)
        {
            using var scope = this.rootServiceProvider.CreateScope();

            return new ScopeContextEvaluator<TBLLContext>(this.serviceEnvironment, scope.ServiceProvider).Evaluate(sessionMode, principalName, getResult);
        }
    }
}
