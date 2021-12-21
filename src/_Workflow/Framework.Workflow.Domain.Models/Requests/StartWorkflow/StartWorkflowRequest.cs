using System.Collections.Generic;

using Framework.Restriction;
using Framework.Workflow.Domain.Definition;
using Framework.Workflow.Domain.Runtime;

namespace Framework.Workflow.Domain
{
    public class StartWorkflowRequest : DomainObjectBase, IParameterizedRequest<Workflow.Domain.Definition.Workflow, StartWorkflowRequestParameter>
    {
        [Required]
        public Workflow.Domain.Definition.Workflow Workflow { get; set; }

        [Required]
        [UniqueGroup]
        public IList<StartWorkflowRequestParameter> Parameters { get; set; } = new List<StartWorkflowRequestParameter>();


        public class StartSubWorkflowRequest : StartWorkflowRequest
        {
            [Required]
            public StateInstance OwnerWorkflowState { get; set; }
        }

        public class CustomStateStartWorkflowRequest : StartWorkflowRequest
        {
            [Required]
            public StateBase StartState { get; set; }
        }



        Definition.Workflow IDefinitionDomainObject<Definition.Workflow>.Definition => this.Workflow;

        IEnumerable<StartWorkflowRequestParameter> IParametersContainer<StartWorkflowRequestParameter>.Parameters => this.Parameters;
    }
}