using System;
using System.Collections.Generic;

using Framework.ExpressionParsers;
using Framework.Workflow.Domain.Definition;

namespace Framework.Workflow.BLL
{
    public class WorkflowSourcePathLambdaProcessor<TBLLContext, TSourceDomainObject, TElementWorkflow> : LambdaProcessorBase<WorkflowSource, Func<TBLLContext, TSourceDomainObject, TElementWorkflow, IEnumerable<string>>>
    {
        public WorkflowSourcePathLambdaProcessor(INativeExpressionParser parser)
            : base(parser, workflowSource => workflowSource.Path)
        {

        }
    }
}