using System.Collections.Generic;

using Framework.Restriction;
using Framework.Workflow.Domain.Definition;
using Framework.Workflow.Domain.Runtime;

namespace Framework.Workflow.Domain
{
    public abstract class ExecuteCommandRequestBase : DomainObjectBase, IParameterizedRequest<Command, ExecuteCommandRequestParameter>
    {
        protected ExecuteCommandRequestBase()
        {
            this.Parameters = new List<ExecuteCommandRequestParameter>();
        }


        [Required]
        public Command Command { get; set; }

        [Required]
        [UniqueGroup]
        public IList<ExecuteCommandRequestParameter> Parameters { get; set; }


        Command IDefinitionDomainObject<Command>.Definition
        {
            get { return this.Command; }
        }

        IEnumerable<ExecuteCommandRequestParameter> IParametersContainer<ExecuteCommandRequestParameter>.Parameters
        {
            get { return this.Parameters; }
        }
    }
}