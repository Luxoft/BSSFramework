using System.Collections.Generic;

using Framework.DomainDriven.DBGenerator;
using Framework.DomainDriven.NHibernate;
using Framework.Workflow.Domain.Runtime;

namespace Framework.Workflow.TestGenerate
{
    public class WorkflowDBGenerator : DBGenerator
    {
        public WorkflowDBGenerator(IMappingSettings settings)
            : base(settings)
        {

        }


        protected override IEnumerable<IgnoreLink> GetIgnoreLinks()
        {
            yield return IgnoreLink.Create((WorkflowInstance wi) => wi.OwnerState);
            yield return IgnoreLink.Create((WorkflowInstance wi) => wi.CurrentState);
        }
    }
}