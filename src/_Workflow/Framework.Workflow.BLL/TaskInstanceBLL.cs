using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;
using Framework.DomainDriven;
using Framework.Validation;

using Framework.Workflow.Domain;
using Framework.Workflow.Domain.Runtime;

namespace Framework.Workflow.BLL
{
    public partial class TaskInstanceBLL
    {
        public List<AvailableTaskInstanceWorkflowGroup> GetAvailableGroups(AvailableTaskInstanceUntypedMainFilterModel filter)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));

            var domainType = filter.SourceTypePath.IsNullOrWhiteSpace() ? null : this.Context.Logics.DomainType.GetByPath(filter.SourceTypePath);

            return this.GetAvailableGroups(new AvailableTaskInstanceMainFilterModel { SourceType = domainType, DomainObjectId = filter.DomainObjectId });
        }

        public List<AvailableTaskInstanceWorkflowGroup> GetAvailableGroups(AvailableTaskInstanceMainFilterModel filter)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));

            this.Context.Validator.Validate(filter);

            if (filter.SourceType == null)
            {
                var request = from domainType in this.Context.Logics.WorkflowSource.GetUnsecureQueryable()//.Where(ws => ws.Workflow.Active && ws.Workflow.IsValid)
                                                                                                  .Select(ws => ws.Type).Distinct().ToList()

                              group domainType by domainType.TargetSystem into targetSystemGroup

                              let targetSystemService = this.Context.GetTargetSystemService(targetSystemGroup.Key)

                              from domainType in targetSystemGroup

                              where filter.DomainObjectId.IsDefault() || targetSystemService.ExistsObject(domainType, filter.DomainObjectId)

                              from result in targetSystemService.GetAvailableTaskInstanceWorkflowGroups(domainType, filter.DomainObjectId)

                              select result;

                return request.ToList();
            }
            else
            {
                var targetSystemService = this.Context.GetTargetSystemService(filter.SourceType.TargetSystem);

                return targetSystemService.GetAvailableTaskInstanceWorkflowGroups(filter.SourceType, filter.DomainObjectId);
            }
        }

        public List<TaskInstance> GetListBy(TaskInstanceRootFilterModel filter, IFetchContainer<TaskInstance> fetchs)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));

            this.Context.Validator.Validate(filter);

            return this.GetListBy(new TaskInstanceInternalRootFilterModel(this.Context, filter), fetchs);
        }
    }
}
