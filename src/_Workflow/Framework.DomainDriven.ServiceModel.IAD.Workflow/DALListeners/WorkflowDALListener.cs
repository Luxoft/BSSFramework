using System;

using Framework.DomainDriven.BLL;
using Framework.Workflow.BLL;

using JetBrains.Annotations;

namespace Framework.DomainDriven.ServiceModel.IAD
{
    public class WorkflowDALListener : IDALListener
    {
        private readonly ITargetSystemService _targetSystemService;


        public WorkflowDALListener([NotNull] ITargetSystemService targetSystemService)
        {
            if (targetSystemService == null) throw new ArgumentNullException(nameof(targetSystemService));

            this._targetSystemService = targetSystemService;
        }

        public void Process(DALChangesEventArgs eventArgs)
        {
            if (eventArgs == null) throw new ArgumentNullException(nameof(eventArgs));

            var result = this._targetSystemService.ProcessDALChanges(eventArgs.Changes);
        }
    }
}
