using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Framework.Core;
using Framework.ExpressionParsers;
using Framework.DomainDriven.BLL;
using Framework.Configuration.Domain;

namespace Framework.Configuration.BLL
{
    public class AuthSubscriptionLambdaCompositeExpressionParser<TBLLContext, TDomainObject> : LambdaObjectCompositeExpressionParser<SubscriptionLambda>
    {
        private readonly Type[] _authTypes;


        public AuthSubscriptionLambdaCompositeExpressionParser(INativeExpressionParser parser, Type[] authTypes)
            : base(parser)
        {
            this._authTypes = authTypes;
        }


        protected override IEnumerable<IExpressionParser<string, Delegate, LambdaExpression>> GetParsers()
        {
            var method = new Func<IExpressionParser<string, Delegate, LambdaExpression>>(this.GetAuthParser<TDomainObject>).Method.GetGenericMethodDefinition();

            return this._authTypes.Select(authType => (IExpressionParser<string, Delegate, LambdaExpression>)method.MakeGenericMethod(authType).Invoke(this, new object[0]));
        }

        private AuthSubscriptionLambdaExpressionParser<TBLLContext, TDomainObject, TSecurityObject> GetAuthParser<TSecurityObject>()
        {
            return new AuthSubscriptionLambdaExpressionParser<TBLLContext, TDomainObject, TSecurityObject>(this.Parser);
        }
    }

    public class AuthSubscriptionLambdaCompositeExpressionParser<TDomainObject> : LambdaObjectCompositeExpressionParser<SubscriptionLambda>
    {
        private readonly Type[] _authTypes;


        public AuthSubscriptionLambdaCompositeExpressionParser(INativeExpressionParser parser, Type[] authTypes)
            : base(parser)
        {
            this._authTypes = authTypes;
        }


        protected override IEnumerable<IExpressionParser<string, Delegate, LambdaExpression>> GetParsers()
        {
            var method = new Func<IExpressionParser<string, Delegate, LambdaExpression>>(this.GetAuthParser<TDomainObject>).Method.GetGenericMethodDefinition();

            return this._authTypes.Select(authType => (IExpressionParser<string, Delegate, LambdaExpression>)method.MakeGenericMethod(authType).Invoke(this, new object[0]));
        }

        private AuthSubscriptionLambdaExpressionParser<TDomainObject, TSecurityObject> GetAuthParser<TSecurityObject>()
        {
            return new AuthSubscriptionLambdaExpressionParser<TDomainObject, TSecurityObject>(this.Parser);
        }
    }
}