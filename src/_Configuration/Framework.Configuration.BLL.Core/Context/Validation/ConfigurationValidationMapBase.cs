using System;
using System.Linq.Expressions;

using Framework.Configuration.Domain;
using Framework.Core;
using Framework.Validation;

namespace Framework.Configuration.BLL
{
    public partial class ConfigurationValidationMapBase
    {
        private IClassValidator<SubscriptionLambda> GetReferencedSubscriptionLambdaValidator<TDomainObject>(Expression<Func<TDomainObject, SubscriptionLambda>> propExpr)
            where TDomainObject : PersistentDomainObjectBase
        {
            return new SubscriptionLambdaValidator<TDomainObject>(propExpr);
        }

        private class SubscriptionLambdaValidator<TDomainObject> : IClassValidator<SubscriptionLambda>
            where TDomainObject : PersistentDomainObjectBase
        {
            private readonly Func<SubscriptionLambda, Expression<Func<TDomainObject, bool>>> getFilter;

            public SubscriptionLambdaValidator(Expression<Func<TDomainObject, SubscriptionLambda>> propExpr)
            {
                if (propExpr == null) throw new ArgumentNullException(nameof(propExpr));

                this.getFilter = lambda => from domainObjectLambda in propExpr

                                           select domainObjectLambda == lambda;
            }

            public ValidationResult GetValidationResult(IClassValidationContext<SubscriptionLambda> validationContext)
            {
                if (validationContext == null) throw new ArgumentNullException(nameof(validationContext));

                var context = validationContext.ExtendedValidationData.GetValue<IConfigurationBLLContext>(true);

                var objects = context.Logics.Default.Create<TDomainObject>().GetObjectsBy(this.getFilter(validationContext.Source));

                return objects.Sum(obj => validationContext.Validator.GetValidationResult(obj, validationContext.OperationContext));
            }
        }
    }
}