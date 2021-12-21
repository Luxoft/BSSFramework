using System;
using System.Collections.Generic;
using System.Linq;
using Framework.Core;
using Framework.DomainDriven;
using Framework.Workflow.Domain;
using Framework.Workflow.Domain.Definition;

namespace Framework.Workflow.BLL
{
    public partial class EventBLL
    {
        public List<Event> GetListBy(EventRootFilterModel filterModel, IFetchContainer<Event> fetchContainer)
        {
            if (filterModel == null) throw new ArgumentNullException(nameof(filterModel));

            var fullList = this.GetFullList(fetchContainer);

            var workflow = filterModel.Workflow;

            if (workflow == null)
            {
                return fullList;
            }
            else
            {
                return fullList.Select(this.Context.GetNestedEvent)
                               .Where(e => e.SourceState.Workflow == workflow)
                               .ToList();
            }
        }
    }
}
