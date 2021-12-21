using System;
using System.Linq.Expressions;
using System.Reflection;

using Framework.Core;
using Framework.Persistent;
using Framework.Workflow.Domain;
using Framework.Workflow.Domain.Definition;
using Framework.Workflow.Domain.Runtime;

namespace Framework.Workflow.BLL
{
    public static class WorkflowBLLContextExtensions
    {
        public static ITargetSystemService GetTargetSystemService(this IWorkflowBLLContext context, IDefinitionDomainObject<ITargetSystemElement<TargetSystem>> definitionContainer)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (definitionContainer == null) throw new ArgumentNullException(nameof(definitionContainer));

            return context.GetTargetSystemService(definitionContainer.Definition.TargetSystem);
        }

        public static ITargetSystemService GetTargetSystemService(this IWorkflowBLLContext context, IWorkflowInstanceElement workflowInstanceElement)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (workflowInstanceElement == null) throw new ArgumentNullException(nameof(workflowInstanceElement));

            return context.GetTargetSystemService(workflowInstanceElement.WorkflowInstance);
        }

        public static ITargetSystemService GetTargetSystemService(this IWorkflowBLLContext context, IWorkflowElement workflowElement)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (workflowElement == null) throw new ArgumentNullException(nameof(workflowElement));

            return context.GetTargetSystemService(workflowElement.Workflow);
        }

        public static ITargetSystemService GetTargetSystemService(this IWorkflowBLLContext context, Domain.Definition.Workflow workflow)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            return context.GetTargetSystemService(workflow.TargetSystem);
        }



        public static void ValidateLambda<TDomainObject>(this IWorkflowBLLContext context, TDomainObject domainObject, PropertyInfo property)

            where TDomainObject : PersistentDomainObjectBase
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

            context.GetLambdaProcessorBase(domainObject, property).Validate(domainObject);
        }

        public static IExpressionParser<TDomainObject, Delegate, LambdaExpression> GetLambdaProcessorBase<TDomainObject>(this IWorkflowBLLContext context, TDomainObject domainObject, PropertyInfo property)

            where TDomainObject : PersistentDomainObjectBase
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));
            if (property == null) throw new ArgumentNullException(nameof(property));

            return (IExpressionParser<TDomainObject, Delegate, LambdaExpression>)context.GetLambdaProcessorBaseMethod(domainObject, property).Invoke(context.ExpressionParsers, new object[] { });
        }


        private static MethodInfo GetLambdaProcessorBaseMethod<TDomainObject>(this IWorkflowBLLContext context, TDomainObject domainObject, PropertyInfo property)
            where TDomainObject : PersistentDomainObjectBase
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));


            var lambdaProcessor = context.ExpressionParsers;

            var workflowLambda = property.GetValue<TDomainObject, WorkflowLambda>(domainObject);

            var targetSystemService = context.GetTargetSystemService(workflowLambda.Workflow);

            var workflowType = targetSystemService.WorkflowTypeBuilder.GetAnonymousType(workflowLambda.Workflow);

            var targetSystemContextType = targetSystemService.TargetSystemContextType;


            if (domainObject is StartWorkflowDomainObjectCondition)
            {
                if (property == StartWorkflowDomainObjectConditionProperty)
                {
                    var startWorkflowDomainObjectCondition = domainObject as StartWorkflowDomainObjectCondition;

                    var domainType = targetSystemService.TypeResolver.Resolve(startWorkflowDomainObjectCondition.Workflow.DomainType, true);

                    return new Func<StartWorkflowDomainObjectConditionLambdaProcessor<object>>(lambdaProcessor.GetByStartWorkflowDomainObjectCondition<object>).Method
                          .GetGenericMethodDefinition()
                          .MakeGenericMethod(domainType);
                }
                else if (property == StartWorkflowDomainObjectConditionFactoryProperty)
                {
                    var startWorkflowDomainObjectCondition = domainObject as StartWorkflowDomainObjectCondition;

                    var domainType = targetSystemService.TypeResolver.Resolve(startWorkflowDomainObjectCondition.Workflow.DomainType, true);

                    return new Func<StartWorkflowDomainObjectConditionFactoryLambdaProcessor<object, object, object>>(lambdaProcessor.GetByStartWorkflowDomainObjectConditionFactory<object, object, object>).Method
                          .GetGenericMethodDefinition()
                          .MakeGenericMethod(targetSystemContextType, domainType, workflowType);
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(property));
                }
            }
            else if (domainObject is ConditionState)
            {
                return new Func<ConditionStateConditionLambdaProcessor<object, object>>(lambdaProcessor.GetByConditionState<object, object>).Method
                      .GetGenericMethodDefinition()
                      .MakeGenericMethod(targetSystemContextType, workflowType);
            }
            else if (domainObject is StateTimeoutEvent)
            {
                return new Func<StateTimeoutConditionLambdaProcessor<object, object>>(lambdaProcessor.GetByStateTimeoutCondition<object, object>).Method
                      .GetGenericMethodDefinition()
                      .MakeGenericMethod(targetSystemContextType, workflowType);
            }
            else if (domainObject is StateDomainObjectEvent)
            {
                return new Func<StateDomainObjectConditionLambdaProcessor<object, object>>(lambdaProcessor.GetByStateDomainObjectCondition<object, object>).Method
                      .GetGenericMethodDefinition()
                      .MakeGenericMethod(targetSystemContextType, workflowType);
            }
            else if (domainObject is TransitionAction)
            {
                return new Func<TransitionActionLambdaProcessor<object, object>>(lambdaProcessor.GetByTransitionAction<object, object>).Method
                      .GetGenericMethodDefinition()
                      .MakeGenericMethod(targetSystemContextType, workflowType);
            }
            else if (domainObject is ParallelStateFinalEvent)
            {
                return new Func<ParallelStateFinalEventConditionLambdaProcessor<object, object>>(lambdaProcessor.GetByParallelStateFinalEventCondition<object, object>).Method
                      .GetGenericMethodDefinition()
                      .MakeGenericMethod(targetSystemContextType, workflowType);
            }
            else if (domainObject is Role)
            {
                var role = domainObject as Role;

                var domainType = targetSystemService.TypeResolver.Resolve(role.Workflow.DomainType, true);

                return new Func<RoleCustomSecurityProviderLambdaProcessor<object, object>>(lambdaProcessor.GetByRoleCustomSecurityProvider<object, object>).Method
                     .GetGenericMethodDefinition()
                     .MakeGenericMethod(targetSystemContextType, domainType);
            }
            else if (domainObject is ParallelStateStartItem)
            {
                var parallelStateItem = domainObject as ParallelStateStartItem;

                var subWorkflowType = targetSystemService.WorkflowTypeBuilder.GetAnonymousType(parallelStateItem.SubWorkflow);

                return new Func<ParallelStateStartItemFactoryLambdaProcessor<object, object, object>>(lambdaProcessor.GetByParallelStateStartItemFactory<object, object, object>).Method
                      .GetGenericMethodDefinition()
                      .MakeGenericMethod(targetSystemContextType, workflowType, subWorkflowType);
            }
            else if (domainObject is Command)
            {
                var command = domainObject as Command;

                var commandType = targetSystemService.CommandTypeBuilder.GetAnonymousType(command);

                return new Func<CommandExecuteActionLambdaProcessor<object, object, object>>(lambdaProcessor.GetByCommandExecuteAction<object, object, object>).Method
                     .GetGenericMethodDefinition()
                     .MakeGenericMethod(targetSystemContextType, workflowType, commandType);
            }
            else if (domainObject is Framework.Workflow.Domain.Definition.Workflow)
            {
                if (property == WorkflowActiveConditionProperty)
                {
                    return new Func<WorkflowActiveConditionLambdaProcessor<object, object>>(lambdaProcessor.GetByWorkflowActiveCondition<object, object>).Method
                          .GetGenericMethodDefinition()
                          .MakeGenericMethod(targetSystemContextType, workflowType);
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(property));
                }
            }
            else if (domainObject is Framework.Workflow.Domain.Definition.WorkflowSource)
            {
                var workflowSource = domainObject as Framework.Workflow.Domain.Definition.WorkflowSource;

                var sourceType = targetSystemService.TypeResolver.Resolve(workflowSource.Type, true);

                var elementType = targetSystemService.TypeResolver.Resolve(workflowSource.Workflow.DomainType, true);

                if (property == WorkflowSourceElementsProperty)
                {
                    return new Func<WorkflowSourceElementsLambdaProcessor<object, object, object>>(lambdaProcessor.GetByWorkflowSourceElements<object, object, object>).Method
                                        .GetGenericMethodDefinition()
                                        .MakeGenericMethod(targetSystemContextType, sourceType, elementType);
                }
                else if (property == WorkflowSourcePathProperty)
                {
                    return new Func<WorkflowSourcePathLambdaProcessor<object, object, object>>(lambdaProcessor.GetByWorkflowSourcePath<object, object, object>).Method
                                        .GetGenericMethodDefinition()
                                        .MakeGenericMethod(targetSystemContextType, sourceType, workflowType);
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(property));
                }
            }
            else
            {
                throw new NotImplementedException("TDomainObject");
            }
        }


        private static PropertyInfo GetProperty<TDomainObject>(Expression<Func<TDomainObject, WorkflowLambda>> path)
        {
            return path.GetProperty();
        }


        private static readonly PropertyInfo StartWorkflowDomainObjectConditionProperty = GetProperty((StartWorkflowDomainObjectCondition obj) => obj.Condition);

        private static readonly PropertyInfo StartWorkflowDomainObjectConditionFactoryProperty = GetProperty((StartWorkflowDomainObjectCondition obj) => obj.Factory);


        private static readonly PropertyInfo WorkflowSourceElementsProperty = GetProperty((WorkflowSource workflowSource) => workflowSource.Elements);

        private static readonly PropertyInfo WorkflowSourcePathProperty = GetProperty((WorkflowSource workflowSource) => workflowSource.Path);


        private static readonly PropertyInfo WorkflowActiveConditionProperty = GetProperty((Domain.Definition.Workflow workflow) => workflow.ActiveCondition);
    }
}
