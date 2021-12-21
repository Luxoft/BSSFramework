using System;
using System.Linq;
using System.Collections.Generic;

using Framework.Workflow.Generated.DTO;

using Framework.DomainDriven.BLL;
using Framework.SecuritySystem;
using Framework.Workflow.Domain.Projections;

using Microsoft.AspNetCore.Mvc;

namespace Framework.Workflow.WebApi
{
    public partial class WorkflowSLJsonController
    {
        [Microsoft.AspNetCore.Mvc.HttpPost(nameof(GetMiniAvailableTaskInstancesByMainFilter))]
        public List<MiniAvailableTaskInstanceWorkflowGroupProjectionDTO> GetMiniAvailableTaskInstancesByMainFilter([FromForm] AvailableTaskInstanceMainFilterModelStrictDTO filter)
        {
            return this.Evaluate(DBSessionMode.Read, evaluateData =>
            {
                if (filter == null) throw new ArgumentNullException(nameof(filter));

                var taskInstanceBLL = evaluateData.Context.Logics.TaskInstanceFactory.Create(BLLSecurityMode.View);

                return taskInstanceBLL.GetAvailableGroups(filter.ToDomainObject(evaluateData.MappingService))
                                      .Select(v => new MiniAvailableTaskInstanceWorkflowGroup(v))
                                      .ToProjectionDTOList(evaluateData.MappingService);
            });
        }
    }
}
