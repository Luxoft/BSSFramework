using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using Framework.ExpressionParsers;
using Framework.Workflow.Domain.Definition;

namespace Framework.Workflow.BLL
{
    public class WorkflowSourceElementsLambdaProcessor<TBLLContext, TSourceDomainObject, TElementDomainObject> : LambdaProcessorBase<WorkflowSource, Func<TBLLContext, Expression<Func<TSourceDomainObject, IEnumerable<TElementDomainObject>>>>>
    {
        public WorkflowSourceElementsLambdaProcessor(INativeExpressionParser parser)
            : base(parser, workflowSource => workflowSource.Elements)
        {

        }
    }
}