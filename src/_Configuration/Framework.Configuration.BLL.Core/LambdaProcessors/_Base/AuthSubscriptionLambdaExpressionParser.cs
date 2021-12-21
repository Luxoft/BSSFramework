using System;
using System.Collections.Generic;

using Framework.ExpressionParsers;
using Framework.DomainDriven.BLL;
using Framework.Configuration.Domain;

namespace Framework.Configuration.BLL
{
    public class AuthSubscriptionLambdaExpressionParser<TDomainObject, TSecurityObject> : LambdaObjectExpressionParser<SubscriptionLambda, Func<TDomainObject, TDomainObject, IEnumerable<TSecurityObject>>>
    {
        public AuthSubscriptionLambdaExpressionParser(INativeExpressionParser parser)
            : base(parser)
        {

        }
    }

    public class AuthSubscriptionLambdaExpressionParser<TBLLContext, TDomainObject, TSecurityObject> : LambdaObjectExpressionParser<SubscriptionLambda, Func<TBLLContext, TDomainObject, TDomainObject, IEnumerable<TSecurityObject>>>
    {
        public AuthSubscriptionLambdaExpressionParser(INativeExpressionParser parser) : base(parser)
        {

        }
    }
}