using System;
using System.Collections.Generic;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.Persistent;
using Framework.Workflow.Domain;
using Framework.Workflow.Domain.Runtime;

namespace Framework.Workflow.BLL
{
    public class TargetSystemServiceCompileCache<TBLLContext, TPersistentDomainObjectBase>
        where TBLLContext : IDefaultBLLContext<TPersistentDomainObjectBase, Guid>
        where TPersistentDomainObjectBase : class, IIdentityObject<Guid>
    {
        //private readonly IDictionaryCache<Type, Delegate> _massWorkflowCache;

        private readonly IDictionaryCache<Type, Delegate> _safeMassWorkflowCache;

        private readonly IDictionaryCache<Type, Delegate> _workflowCache;

        private readonly IDictionaryCache<Type, Delegate> _commandCache;

        //private readonly IDictionaryCache<Type, Delegate> _taskDynamicFilerCache;


        public TargetSystemServiceCompileCache()
        {
            //this._massWorkflowCache = new ConcurrentDictionaryCache<Type, Delegate>(anonType =>
            //{
            //    var expressionBuilder = new MassDefaultWorkflowInstanceParserExpressionBuilder<TBLLContext, TPersistentDomainObjectBase>(anonType);

            //    return expressionBuilder.GetLambdaParseExpression().Compile();
            //});


            this._safeMassWorkflowCache = new DictionaryCache<Type, Delegate>(anonType =>
            {
                var expressionBuilder = new SafeMassDefaultWorkflowInstanceParserExpressionBuilder<TBLLContext, TPersistentDomainObjectBase>(anonType);

                return expressionBuilder.GetLambdaParseExpression().Compile();
            }).WithLock();


            this._workflowCache = new DictionaryCache<Type, Delegate>(anonType =>
            {
                var expressionBuilder = new DefaultWorkflowInstanceParserExpressionBuilder<TBLLContext, TPersistentDomainObjectBase>(anonType);

                return expressionBuilder.GetLambdaParseExpression().Compile();
            }).WithLock();


            this._commandCache = new DictionaryCache<Type, Delegate>(anonType =>
            {
                var expressionBuilder = new DefaultParameterizedObjectParserExpressionBuilder<TBLLContext, TPersistentDomainObjectBase, ExecutedCommand>(anonType);

                return expressionBuilder.GetLambdaParseExpression().Compile();
            }).WithLock();

            //this._taskDynamicFilerCache = new ConcurrentDictionaryCache<Type, Delegate>(anonType =>
            //{
            //    var expressionBuilder = new DefaultParameterizedObjectParserExpressionBuilder<TBLLContext, TPersistentDomainObjectBase, TaskDynamicFilterRequest>(anonType);

            //    return expressionBuilder.GetLambdaParseExpression().Compile();
            //});
        }




        internal Func<TBLLContext, IEnumerable<WorkflowInstance>, IEnumerable<ITryResult<object>>> GetSafeMassWorkflowMapFunc(Type workflowType)
        {
            if (workflowType == null) throw new ArgumentNullException(nameof(workflowType));

            return (Func<TBLLContext, IEnumerable<WorkflowInstance>, IEnumerable<ITryResult<object>>>)this._safeMassWorkflowCache[workflowType];
        }

        //internal Func<TBLLContext, IEnumerable<WorkflowInstance>, IEnumerable<object>> GetMassWorflowMapFunc(Type workflowType)
        //{
        //    if (workflowType == null) throw new ArgumentNullException("workflowType");

        //    return (Func<TBLLContext, IEnumerable<WorkflowInstance>, IEnumerable<object>>)this._massWorkflowCache[workflowType];
        //}

        internal Func<TBLLContext, WorkflowInstance, object> GetWorkflowMapFunc(Type workflowType)
        {
            if (workflowType == null) throw new ArgumentNullException(nameof(workflowType));

            return (Func<TBLLContext, WorkflowInstance, object>)this._workflowCache[workflowType];
        }

        internal Func<TBLLContext, ExecutedCommand, object> GetCommandMapFunc(Type commandType)
        {
            if (commandType == null) throw new ArgumentNullException(nameof(commandType));

            return (Func<TBLLContext, ExecutedCommand, object>)this._commandCache[commandType];
        }

        //internal Func<TBLLContext, TaskDynamicFilterRequest, object> GetTaskDynamicFilterRequestMapFunc(Type taskDynamicFilterRequestType)
        //{
        //    if (taskDynamicFilterRequestType == null) throw new ArgumentNullException("taskDynamicFilterRequestType");

        //    return (Func<TBLLContext, TaskDynamicFilterRequest, object>)this._taskDynamicFilerCache[taskDynamicFilterRequestType];
        //}
    }
}