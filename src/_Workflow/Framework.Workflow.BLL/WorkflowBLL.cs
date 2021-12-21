using System;
using System.Linq;

using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.Exceptions;
using Framework.HierarchicalExpand;
using Framework.Persistent;
using Framework.Workflow.Domain;
using Framework.Workflow.Domain.Definition;

using JetBrains.Annotations;

namespace Framework.Workflow.BLL
{
    public partial class WorkflowBLL
    {
        public Domain.Definition.Workflow GetByPath(string path)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));

            var blocks = path.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            if (blocks.Any())
            {
                var rootWF = this.Context.Logics.Workflow.GetObjectBy(wf => wf.Owner == null && wf.Name == blocks[0], true);

                return blocks.Skip(1).Aggregate(rootWF, (wf, subWfName) => wf.SubWorkflows.GetByName(subWfName, StringComparison.CurrentCultureIgnoreCase));
            }
            else
            {
                throw new System.ArgumentException("invalid block count", nameof(path));
            }
        }

        public override void Save(Domain.Definition.Workflow workflow)
        {
            if (workflow == null) throw new ArgumentNullException(nameof(workflow));

            this.Recalculate(workflow);

            this.Validate(workflow, WorkflowOperationContext.Save);

            this.InternalSave(workflow);
        }

        private void InternalSave(Domain.Definition.Workflow workflow)
        {
            if (workflow == null) throw new ArgumentNullException(nameof(workflow));

            this.SaveWithoutCascade(workflow);

            workflow.SubWorkflows.Foreach(this.InternalSave);

            this.Context.Logics.WorkflowLambda.Save(workflow.Lambdas);
            this.Context.Logics.Role.Save(workflow.Roles);

            base.Save(workflow);
        }

        public IQueryable<Domain.Definition.Workflow> GetForActiveLambdaAvailable(Type domainObjectType)
        {
            if (domainObjectType == null) throw new ArgumentNullException(nameof(domainObjectType));

            var domainDomainType = this.Context.GetDomainType(domainObjectType);

            return this.GetForActiveLambdaAvailable(domainDomainType);
        }

        public IQueryable<Domain.Definition.Workflow> GetForActiveLambdaAvailable(DomainType domainType)
        {
            if (domainType == null) throw new ArgumentNullException(nameof(domainType));

            return this.GetUnsecureQueryable().Where(w => w.Active && w.ActiveCondition != null && w.DomainType == domainType);
        }

        public IQueryable<Domain.Definition.Workflow> GetForAutoRemovingAvailable(Type domainObjectType)
        {
            if (domainObjectType == null) throw new ArgumentNullException(nameof(domainObjectType));

            var domainDomainType = this.Context.GetDomainType(domainObjectType);

            return this.GetForAutoRemovingAvailable(domainDomainType);
        }

        public IQueryable<Domain.Definition.Workflow> GetForAutoRemovingAvailable(DomainType domainType)
        {
            if (domainType == null) throw new ArgumentNullException(nameof(domainType));

            return this.GetUnsecureQueryable().Where(w => w.Active && w.AutoRemoveWithDomainObject && w.DomainType == domainType);
        }

        public Domain.Definition.Workflow Create(WorkflowCreateModel createModel)
        {
            if (createModel == null) throw new ArgumentNullException(nameof(createModel));

            return new Domain.Definition.Workflow();
        }


        protected override void Validate(Domain.Definition.Workflow domainObject, WorkflowOperationContext context)
        {
            base.Validate(domainObject, context);

            if (domainObject.DomainObjectParameter.Maybe(domainObjectParameter => domainObjectParameter.Type.Role != DomainTypeRole.Domain))
            {
                throw new BusinessLogicException("Invalid DomainObject parameter type");
            }
        }

        private void Recalculate([NotNull] Domain.Definition.Workflow workflow)
        {
            if (workflow == null) throw new ArgumentNullException(nameof(workflow));

            workflow.GetAllChildren().Foreach(wf =>
            {
                this.RecalculateDomainType(wf);
                this.RecalculateDefaultSource(wf);
                //this.RecalculateCommands(wf);
                this.RecalculateTransitions(wf);
            });

            this.RecalculateValidFlag(workflow);
            this.RecalculateActiveFlag(workflow);
        }

        private void RecalculateDomainType(Domain.Definition.Workflow workflow)
        {
            if (workflow == null) throw new ArgumentNullException(nameof(workflow));

            workflow.DomainType = workflow.DomainObjectParameter.Maybe(v => v.Type);
        }

        private void RecalculateDefaultSource([NotNull] Domain.Definition.Workflow workflow)
        {
            if (workflow == null) throw new ArgumentNullException(nameof(workflow));

            if (workflow.DomainType != null)
            {
                var defaultWorkflowSourceLambda = workflow.Lambdas.GetByName("DefaultSource", false) ?? new WorkflowLambda(workflow) { Name = "DefaultSource", Value = "context => obj => new [] { obj }" };

                var defaultWorkflowSource = workflow.Sources.GetByName(WorkflowSource.DefaultName, false) ?? new WorkflowSource(workflow) { Name = WorkflowSource.DefaultName };

                defaultWorkflowSource.Type = workflow.DomainType;
                defaultWorkflowSource.Elements = defaultWorkflowSourceLambda;
            }
        }

        //private void RecalculateCommands([NotNull] Domain.Definition.Workflow workflow)
        //{
        //    if (workflow == null) throw new ArgumentNullException("workflow");

        //    workflow.States.SelectMany(state => state.Tasks)
        //                   .SelectMany(task => task.Commands)
        //                   .Foreach(this.Context.Logics.Command.Recalculate);
        //}

        private void RecalculateTransitions([NotNull] Domain.Definition.Workflow workflow)
        {
            if (workflow == null) throw new ArgumentNullException(nameof(workflow));

            workflow.Transitions.Foreach(this.Context.Logics.Transition.Recalculate);
        }


        private void RecalculateValidFlag(Domain.Definition.Workflow workflow, Domain.Definition.Workflow except = null)
        {
            if (workflow == null) throw new ArgumentNullException(nameof(workflow));

            workflow.SubWorkflows.Where(swf => swf != except).Foreach(swf => this.RecalculateValidFlag(swf, workflow));

            workflow.ValidationError = this.Context.Validator
                                                   .GetValidationResult(workflow, WorkflowOperationContextC.Start)
                                                   .Errors
                                                   .Join(Environment.NewLine);

            workflow.IsValid = workflow.SubWorkflows.All(w => w.IsValid) && workflow.StateBases.Count(s => s.IsInitial) == 1 && string.IsNullOrWhiteSpace(workflow.ValidationError);

            if (workflow.IsValid)
            {
                if (workflow.Owner.Maybe(o => !o.IsValid && o != except))
                {
                    this.RecalculateValidFlag(workflow.Owner, workflow);
                }
            }
            else
            {
                workflow.GetAllParents().Foreach(w => w.IsValid = false);
            }
        }

        private void RecalculateActiveFlag(Domain.Definition.Workflow workflow)
        {
            if (workflow == null) throw new ArgumentNullException(nameof(workflow));

            if (workflow.IsRoot)
            {
                workflow.GetAllChildren(true).Foreach(subWf => subWf.Active = workflow.Active);
            }
            else
            {
                workflow.Active = workflow.Owner.Active;
            }
        }
    }
}
