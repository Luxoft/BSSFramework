using System;

using Framework.DomainDriven.BLL;
using Framework.DomainDriven.ServiceModel.Service;

using JetBrains.Annotations;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.ServiceModel.IAD
{
    public class ScopeContextEvaluator<TBLLContext> : IScopedContextEvaluator<TBLLContext>
    {
        private readonly IServiceEnvironment<TBLLContext> serviceEnvironment;

        private readonly IServiceProvider scopedServiceProvider;

        public ScopeContextEvaluator(
            [NotNull] IServiceEnvironment<TBLLContext> serviceEnvironment,
            [NotNull] IServiceProvider scopedServiceProvider)
        {
            this.serviceEnvironment = serviceEnvironment ?? throw new ArgumentNullException(nameof(serviceEnvironment));
            this.scopedServiceProvider =
                scopedServiceProvider ?? throw new ArgumentNullException(nameof(scopedServiceProvider));
        }

        public TResult Evaluate<TResult>(
            DBSessionMode sessionMode,
            string principalName,
            Func<TBLLContext, IDBSession, TResult> getResult)
        {
            var evaluateScopeManager = this.scopedServiceProvider.GetRequiredService<IEvaluateScopeManager>();

            IBLLContextContainer<TBLLContext> contextContainer = null;

            try
            {
                return this.serviceEnvironment.SessionFactory.Evaluate(
                    sessionMode,
                    session =>
                    {
                        contextContainer = this.serviceEnvironment.GetBLLContextContainer(this.scopedServiceProvider, session, principalName);

                        evaluateScopeManager.BeginScope(contextContainer);

                        return getResult(contextContainer.Context, session);
                    });
            }
            finally
            {
                if (contextContainer != null)
                {
                    evaluateScopeManager?.EndScope(contextContainer);
                }
            }
        }
    }
}
