using System;
using System.Linq;
using System.Linq.Expressions;

using Framework.Core;
using Framework.SecuritySystem;
using Framework.Workflow.Domain.Definition;
using Framework.Workflow.Domain.Runtime;

using JetBrains.Annotations;

namespace Framework.Workflow.BLL
{
    public partial class TargetSystemService<TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode>
    {
        public SecurityProvider<TaskInstance> GetTaskInstanceSecurityProvider([NotNull] IGrouping<Role, Task> taskRoleGroup)
        {
            if (taskRoleGroup == null) throw new ArgumentNullException(nameof(taskRoleGroup));

            return new Func<IGrouping<Role, Task>, SecurityProvider<TaskInstance>>(this.GetTaskInstanceSecurityProvider<TPersistentDomainObjectBase>)
                  .CreateGenericMethod(this.TypeResolver.Resolve(taskRoleGroup.Key.Workflow.DomainType))
                  .Invoke<SecurityProvider<TaskInstance>>(this, new object[] { taskRoleGroup });
        }


        private SecurityProvider<TaskInstance> GetTaskInstanceSecurityProvider<TDomainObject>(IGrouping<Role, Task> taskRoleGroup)
            where TDomainObject : class, TPersistentDomainObjectBase
        {
            return new TaskInstanceSecurityProvider<TDomainObject>(this.Context, this, taskRoleGroup);
        }

        private ISecurityProvider<WorkflowInstance> GetWorkflowInstanceSecurityProvider(Domain.Definition.Workflow workflow)
        {
            return new Func<Domain.Definition.Workflow, ISecurityProvider<WorkflowInstance>>(this.GetWorkflowInstanceSecurityProvider<TPersistentDomainObjectBase>)
                  .CreateGenericMethod(this.TypeResolver.Resolve(workflow.DomainType, true))
                  .Invoke<ISecurityProvider<WorkflowInstance>>(this, workflow);
        }

        private ISecurityProvider<WorkflowInstance> GetWorkflowInstanceSecurityProvider<TDomainObject>(Domain.Definition.Workflow workflow)
            where TDomainObject : class, TPersistentDomainObjectBase
        {
            return new WorkflowInstanceSecurityProvider<TDomainObject>(this.Context, this, workflow);
        }


        private class TaskInstanceSecurityProvider<TDomainObject> : SecurityProvider<TaskInstance>
            where TDomainObject : class, TPersistentDomainObjectBase
        {
            private readonly ITargetSystemService<TBLLContext, TPersistentDomainObjectBase> targetSystemService;
            private readonly IGrouping<Role, Task> taskRoleGroup;
            private readonly Lazy<Expression<Func<TaskInstance, bool>>> lazySecurityFilter;


            public TaskInstanceSecurityProvider(IWorkflowBLLContext context, [NotNull] ITargetSystemService<TBLLContext, TPersistentDomainObjectBase> targetSystemService, [NotNull] IGrouping<Role, Task> taskRoleGroup)
                : base(context.AccessDeniedExceptionService)
            {
                this.targetSystemService = targetSystemService ?? throw new ArgumentNullException(nameof(targetSystemService));
                this.taskRoleGroup = taskRoleGroup ?? throw new ArgumentNullException(nameof(taskRoleGroup));

                var roleSecurityProvider = targetSystemService.GetSecurityProvider<TDomainObject>(taskRoleGroup.Key);


                this.lazySecurityFilter = LazyHelper.Create(() =>
                {
                    var queryable = targetSystemService.TargetSystemContext.Logics.Default.Create<TDomainObject>()
                                                       .GetUnsecureQueryable()
                                                       .Pipe(q => roleSecurityProvider.InjectFilter(q));

                    return taskRoleGroup.Distinct()
                                        .BuildOr<Task, TaskInstance>(task => ti => ti.Definition == task)
                                        .BuildAnd(ti => queryable.Any(domainObject => domainObject.Id == ti.Workflow.DomainObjectId));
                });
            }

            public override Expression<Func<TaskInstance, bool>> SecurityFilter
            {
                get { return this.lazySecurityFilter.Value; }
            }

            public override UnboundedList<string> GetAccessors([NotNull] TaskInstance taskInstance)
            {
                if (taskInstance == null) throw new ArgumentNullException(nameof(taskInstance));


                if (this.taskRoleGroup.Contains(taskInstance.Definition))
                {
                    var container = (IDomainObjectContainer<TDomainObject>)this.targetSystemService.GetAnonymousObject(taskInstance.Workflow);

                    return this.targetSystemService.GetSecurityProvider<TDomainObject>(taskInstance.Definition)
                                                    .GetAccessors(container.DomainObject);
                }
                else
                {
                    return UnboundedList<string>.Empty;
                }
            }
        }

        private class WorkflowInstanceSecurityProvider<TDomainObject> : SecurityProvider<WorkflowInstance>
            where TDomainObject : class, TPersistentDomainObjectBase
        {
            private readonly ITargetSystemService<TBLLContext, TPersistentDomainObjectBase> targetSystemService;
            private readonly Domain.Definition.Workflow workflow;
            private readonly Lazy<Expression<Func<WorkflowInstance, bool>>> lazySecurityFilter;

            public WorkflowInstanceSecurityProvider(IWorkflowBLLContext context, [NotNull] ITargetSystemService<TBLLContext, TPersistentDomainObjectBase> targetSystemService, [NotNull] Domain.Definition.Workflow workflow)
                : base(context.AccessDeniedExceptionService)
            {
                if (targetSystemService == null) throw new ArgumentNullException(nameof(targetSystemService));
                if (workflow == null) throw new ArgumentNullException(nameof(workflow));

                this.targetSystemService = targetSystemService;
                this.workflow = workflow;

                var byRolesSecurityProvider = targetSystemService.GetSecurityProvider<TDomainObject>(workflow);


                this.lazySecurityFilter = LazyHelper.Create(() =>
                {
                    var queryable = targetSystemService.TargetSystemContext.Logics.Default.Create<TDomainObject>()
                                                       .GetUnsecureQueryable()
                                                       .Pipe(q => byRolesSecurityProvider.InjectFilter(q));

                    return ExpressionHelper.Create((WorkflowInstance workflowInstance) => workflowInstance.Definition == workflow)
                                           .BuildAnd(wi => queryable.Any(domainObject => domainObject.Id == wi.DomainObjectId));
                });
            }

            public override Expression<Func<WorkflowInstance, bool>> SecurityFilter
            {
                get { return this.lazySecurityFilter.Value; }
            }

            public override UnboundedList<string> GetAccessors([NotNull] WorkflowInstance workflowInstance)
            {
                if (workflowInstance == null) throw new ArgumentNullException("WorkflowInstance");


                if (this.workflow == workflowInstance.Definition)
                {
                    var container = (IDomainObjectContainer<TDomainObject>)this.targetSystemService.GetAnonymousObject(workflowInstance);

                    return this.targetSystemService.GetSecurityProvider<TDomainObject>(this.workflow).GetAccessors(container.DomainObject);
                }
                else
                {
                    return UnboundedList<string>.Empty;
                }
            }
        }
    }
}
