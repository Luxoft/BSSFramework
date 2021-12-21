using System;

using Framework.DomainDriven.ServiceModel.IAD;

using JetBrains.Annotations;

namespace Framework.DomainDriven.ServiceModel
{
    public class EvaluateScopeManager<TMainBLLContext> : IEvaluateScopeManager<TMainBLLContext>
    {
        private IServiceEnvironmentBLLContextContainer<TMainBLLContext> currentBLLContextContainer;


        public IServiceEnvironmentBLLContextContainer<TMainBLLContext> CurrentBLLContextContainer => this.currentBLLContextContainer ?? throw new InvalidOperationException("Invalid call time. Evaluate not started.");


        public void BeginScope([NotNull] object bllContextContainer)
        {
            if (bllContextContainer == null) throw new ArgumentNullException(nameof(bllContextContainer));

            if (this.currentBLLContextContainer != null)
            {
                throw new InvalidOperationException("Invalid call time. Evaluate not finished.");
            }

            this.currentBLLContextContainer = (IServiceEnvironmentBLLContextContainer<TMainBLLContext>)bllContextContainer;
        }

        public void EndScope([NotNull] object bllContextContainer)
        {
            if (bllContextContainer == null) throw new ArgumentNullException(nameof(bllContextContainer));

            if (this.currentBLLContextContainer != bllContextContainer)
            {
                throw new InvalidOperationException("Invalid call time.");
            }

            this.currentBLLContextContainer = null;
        }

        IServiceEnvironmentBLLContextContainer IEvaluateScopeManager.CurrentBLLContextContainer => this.CurrentBLLContextContainer;
    }
}
