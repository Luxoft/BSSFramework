using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using Framework.Core;
using Framework.DomainDriven.BLL.Security;
using Framework.Notification;
using Framework.Configuration.Domain;

namespace Framework.Configuration.BLL
{
    public static class ConfigurationContextExtensions
    {
        public static IExpressionParser<SubscriptionLambda, Delegate, LambdaExpression> GetSubscriptionExpressionParser(this IConfigurationBLLContext context, ISubscriptionLambdaHeader lambda)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (lambda == null) throw new ArgumentNullException(nameof(lambda));

            var targetSystem = context.GetTargetSystemService(lambda.TargetSystem);

            var contextType = targetSystem.TargetSystemContextType;

            var domainType = targetSystem.TypeResolver.Resolve(lambda.DomainType, true);


            return new Func<ISubscriptionLambdaHeader, IExpressionParser<SubscriptionLambda, Delegate, LambdaExpression>>(context.GetSubscriptionExpressionParser<object, object>)
                  .CreateGenericMethod(contextType, domainType)
                  .Invoke<IExpressionParser<SubscriptionLambda, Delegate, LambdaExpression>>(null, new object[] { context, lambda} );
        }

        private static IExpressionParser<SubscriptionLambda, Delegate, LambdaExpression> GetSubscriptionExpressionParser<TBLLContext, TDomainObject>(this IConfigurationBLLContext context, ISubscriptionLambdaHeader lambda)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (lambda == null) throw new ArgumentNullException(nameof(lambda));

            switch (lambda.Type)
            {
                case SubscriptionLambdaType.Condition:
                {
                    if (lambda.WithContext)
                    {
                        return context.ExpressionParsers.GetSubscriptionLambdaExpressionParser<Func<TBLLContext, TDomainObject, TDomainObject, bool>>();
                    }
                    else
                    {
                        return context.ExpressionParsers.GetSubscriptionLambdaExpressionParser<Func<TDomainObject, TDomainObject, bool>>();
                    }
                }

                case SubscriptionLambdaType.DynamicSource:
                {
                    if (lambda.WithContext)
                    {
                        return context.ExpressionParsers.GetSubscriptionLambdaExpressionParser<Func<TBLLContext, TDomainObject, TDomainObject, IEnumerable<FilterItemIdentity>>>();
                    }
                    else
                    {
                        return context.ExpressionParsers.GetSubscriptionLambdaExpressionParser<Func<TDomainObject, TDomainObject, IEnumerable<FilterItemIdentity>>>();
                    }
                }

                case SubscriptionLambdaType.Generation:
                {
                    if (lambda.WithContext)
                    {
                        return context.ExpressionParsers.GetSubscriptionLambdaExpressionParser<Func<TBLLContext, TDomainObject, TDomainObject, IEnumerable<NotificationMessageGenerationInfo>>>();
                    }
                    else
                    {
                        return context.ExpressionParsers.GetSubscriptionLambdaExpressionParser<Func<TDomainObject, TDomainObject, IEnumerable<NotificationMessageGenerationInfo>>>();
                    }
                }

                case SubscriptionLambdaType.AuthSource:
                {
                    var securityType = context.Authorization.SecurityTypeResolver.Resolve(context.Authorization.GetEntityType(lambda.AuthDomainTypeId));

                    return new Func<ISubscriptionLambdaHeader, IExpressionParser<SubscriptionLambda, Delegate, LambdaExpression>>(context.GetSubscriptionExpressionParser<object, object, object>)

                        .CreateGenericMethod(typeof(TBLLContext), typeof(TDomainObject), securityType)
                        .Invoke<IExpressionParser<SubscriptionLambda, Delegate, LambdaExpression>>(null, new object[] { context, lambda });
                }

                default:
                {
                    throw new ArgumentException("invalid Type", nameof(lambda));
                }
            }
        }

        private static IExpressionParser<SubscriptionLambda, Delegate, LambdaExpression> GetSubscriptionExpressionParser<TBLLContext, TDomainObject, TSecurityType>(this IConfigurationBLLContext context, ISubscriptionLambdaHeader lambda)
        {
            if (lambda.WithContext)
            {
                return context.ExpressionParsers.GetSubscriptionLambdaExpressionParser<Func<TBLLContext, TDomainObject, TDomainObject, IEnumerable<TSecurityType>>>();
            }
            else
            {
                return context.ExpressionParsers.GetSubscriptionLambdaExpressionParser<Func<TDomainObject, TDomainObject, IEnumerable<TSecurityType>>>();
            }
        }
    }
}