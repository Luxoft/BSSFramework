using System.Collections.Generic;

using Framework.Workflow.Domain;

using JetBrains.Annotations;

namespace Framework.Workflow.BLL
{
    public partial interface ITaskInstanceBLL : ITaskInstanceContract
    {
        List<AvailableTaskInstanceWorkflowGroup> GetAvailableGroups([NotNull]AvailableTaskInstanceMainFilterModel filter);

        List<AvailableTaskInstanceWorkflowGroup> GetAvailableGroups([NotNull]AvailableTaskInstanceUntypedMainFilterModel filter);
    }
}
