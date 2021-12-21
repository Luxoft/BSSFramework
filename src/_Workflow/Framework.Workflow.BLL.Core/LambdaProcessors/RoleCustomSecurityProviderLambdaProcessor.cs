using System;

using Framework.ExpressionParsers;
using Framework.SecuritySystem;
using Framework.Workflow.Domain.Definition;

namespace Framework.Workflow.BLL
{
    public class RoleCustomSecurityProviderLambdaProcessor<TBLLContext, TDomainObject> : LambdaProcessorBase<Role, Func<TBLLContext, ISecurityProvider<TDomainObject>>>
    {
        public RoleCustomSecurityProviderLambdaProcessor(INativeExpressionParser parser)
            : base(parser, role => role.CustomSecurityProvider)
        {

        }
    }
}
