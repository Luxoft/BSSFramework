using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Framework.Workflow.Domain;
using Framework.Core;
using Framework.Workflow.Domain.Runtime;

namespace Framework.Workflow.BLL
{
    internal class TaskInstanceInternalRootFilterModel : DomainObjectMultiFilterModel<TaskInstance>
    {
        private readonly IWorkflowBLLContext _context;

        private readonly TaskInstanceRootFilterModel _baseFilterModel;


        public TaskInstanceInternalRootFilterModel(IWorkflowBLLContext context, TaskInstanceRootFilterModel baseFilterModel)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (baseFilterModel == null) throw new ArgumentNullException(nameof(baseFilterModel));

            this._context = context;
            this._baseFilterModel = baseFilterModel;
        }


        protected override IEnumerable<Expression<Func<TaskInstance, bool>>> ToFilterExpressionItems()
        {
            if (this._baseFilterModel.IsAvailable.HasValue)
            {
                var baseAvailableFilter = ExpressionHelper.Create((TaskInstance taskInstance) => !taskInstance.State.Workflow.IsFinished && taskInstance.State.Workflow.Active && taskInstance.State.Workflow.CurrentState == taskInstance.State);

                if (this._baseFilterModel.IsAvailable.Value)
                {
                    yield return baseAvailableFilter;
                }
                else
                {
                    yield return baseAvailableFilter.Not();
                }
            }

            //if (this._baseFilterModel.AssignedToMe.HasValue)
            //{
            //    var baseAssignedToMeFilter = ExpressionHelper.Create((TaskInstance taskInstance) => taskInstance.Assignees.Any(assignee => assignee.Login == this._context.Authorization.RunAsManager.PrincipalName));

            //    if (this._baseFilterModel.AssignedToMe.Value)
            //    {
            //        yield return baseAssignedToMeFilter;
            //    }
            //    else
            //    {
            //        yield return baseAssignedToMeFilter.Not();
            //    }
            //}

            if (this._baseFilterModel.WorkflowDefinition != null)
            {
                yield return taskInstance => taskInstance.Definition.Workflow == this._baseFilterModel.WorkflowDefinition;
            }

            if (this._baseFilterModel.WorkflowInstance != null)
            {
                yield return taskInstance => taskInstance.State.Workflow == this._baseFilterModel.WorkflowInstance;
            }

            if (!this._baseFilterModel.DomainObjectId.IsDefault())
            {
                yield return taskInstance => taskInstance.State.Workflow.DomainObjectId == this._baseFilterModel.DomainObjectId;
            }
        }
    }
}
