using System;
using System.Collections.Generic;
using System.Linq;

using Framework.DomainDriven;
using Framework.Validation;
using Framework.Workflow.Domain;
using Framework.Workflow.Domain.Definition;

namespace Framework.Workflow.BLL
{
    public partial class CommandBLL
    {
        public List<Command> GetListBy(AvailableCommandFilterModel filter, IFetchContainer<Command> fetchs)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));

            this.Context.Validator.Validate(filter);

            var taskInstance = filter.TaskInstance;
            var wfInstance = taskInstance.Workflow;

            var targetSystemService = this.Context.GetTargetSystemService(wfInstance);

            var idents = taskInstance.Definition.Commands
                                     .Where(command => targetSystemService.CommandAccessService.HasAccess(command, wfInstance))
                                     .Select(command => command.Id);

            return this.GetListByIdents(idents, fetchs);
        }
    }
}
