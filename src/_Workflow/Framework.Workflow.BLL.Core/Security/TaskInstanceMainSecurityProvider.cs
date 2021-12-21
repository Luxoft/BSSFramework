using System;
using System.Linq;
using System.Linq.Expressions;

using Framework.Core;
using Framework.DomainDriven.BLL.Security;
using Framework.SecuritySystem;
using Framework.Workflow.Domain.Runtime;

using JetBrains.Annotations;

namespace Framework.Workflow.BLL
{
    public class TaskInstanceMainSecurityProvider : SecurityProvider<TaskInstance>
    {
        private readonly Lazy<Expression<Func<TaskInstance, bool>>> lazySecurityFilter;


        public TaskInstanceMainSecurityProvider(IWorkflowBLLContext context)
            : base(context.AccessDeniedExceptionService)
        {
            this.Context = context;

            this.lazySecurityFilter = LazyHelper.Create(() =>
            {
                var tasks = context.Logics.Task.GetObjectsBy(task => task.Workflow.Active && task.Workflow.IsValid, rule => rule.SelectNested(task => task.Workflow)
                                                                                                                                .Select(wf => wf.DomainType)
                                                                                                                                .SelectMany(wf => wf.Roles),

                                                             rule => rule.SelectMany(task => task.Commands)
                                                                         .SelectMany(command => command.RoleAccesses));

                var groupedByType = from task in tasks

                                    let domainType = task.Workflow.DomainType

                                    group task by domainType into groupedTasksByDomainType

                                    let domainType = groupedTasksByDomainType.Key

                                    let targetSystemService = context.GetTargetSystemService(domainType.TargetSystem)

                                    let roleSecurityProviders = from task in groupedTasksByDomainType

                                                                from command in task.Commands

                                                                from roleAccess in command.RoleAccesses

                                                                group task by roleAccess.Role into groupedTasksByRole

                                                                select targetSystemService.GetTaskInstanceSecurityProvider(groupedTasksByRole)

                                    select new
                                           {
                                               DomainType = domainType,
                                               RolesSecurityFilter = roleSecurityProviders.BuildOr(secProvider => secProvider.SecurityFilter)
                                           };

                return groupedByType.BuildOr(pair => ExpressionHelper.Create((TaskInstance ti) => ti.Workflow.Definition.DomainType == pair.DomainType)
                                                                     .BuildAnd(pair.RolesSecurityFilter));
            });
        }
        public IWorkflowBLLContext Context { get; }


        public override Expression<Func<TaskInstance, bool>> SecurityFilter
        {
            get { return this.lazySecurityFilter.Value; }
        }


        public override UnboundedList<string> GetAccessors([NotNull] TaskInstance taskInstance)
        {
            if (taskInstance == null) throw new ArgumentNullException(nameof(taskInstance));

            return this.Context.GetWorkflowMachine(taskInstance.Workflow).GetAccessors(taskInstance.Definition);
        }
    }
}
