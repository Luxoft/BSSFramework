using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;
using Framework.Validation;

namespace Framework.Configuration.Domain
{
    public class SubscriptionLambdaTypeValidatorAttribute : PropertyValidatorAttribute
    {
        public SubscriptionLambdaTypeValidatorAttribute(SubscriptionLambdaType type)
        {
            this.Type = type;
        }

        public SubscriptionLambdaType Type { get; }


        public override IPropertyValidator CreateValidator()
        {
            return new SubscriptionLambdaTypeValidator(this.Type);
        }
    }


    public class SubscriptionLambdaTypeValidator : IPropertyValidator<AuditPersistentDomainObjectBase, SubscriptionLambda>
    {
        private readonly SubscriptionLambdaType _type;

        public SubscriptionLambdaTypeValidator(SubscriptionLambdaType type)
        {
            this._type = type;
        }

        public ValidationResult GetValidationResult(IPropertyValidationContext<AuditPersistentDomainObjectBase, SubscriptionLambda> validationContext)
        {
            if (validationContext == null) throw new ArgumentNullException(nameof(validationContext));

            var errorRequest = from lambda in validationContext.Value.ToMaybe()

                               where this._type != lambda.Type

                               select $"Invalid Lambda ({lambda.Name}) Type: \"{lambda.Type}\". Property \"{validationContext.GetPropertyName()}\" of object \"{validationContext.GetSourceTypeName()}\" expected \"{this._type}\" format.";


            return ValidationResult.FromMaybe(errorRequest);
        }
    }
}