using System;

using Framework.Core;
using Framework.Validation;

namespace Framework.Configuration.Domain
{
    public class SyncAuthDomainTypeValidatorAttribute : PropertyValidatorAttribute
    {
        public override IPropertyValidator CreateValidator()
        {
            return SyncAuthDomainTypeValidator.Value;
        }
    }

    public class SyncAuthDomainTypeValidator : IPropertyValidator<SubscriptionSecurityItem, Guid>
    {
        private SyncAuthDomainTypeValidator()
        {
        }


        public ValidationResult GetValidationResult(IPropertyValidationContext<SubscriptionSecurityItem, Guid> validationContext)
        {
            var errorRequest = from authLambda in validationContext.Source.Source.ToMaybe()

                               where authLambda.AuthDomainTypeId != validationContext.Value

                               select $"Auth domain Type \"{validationContext.Value}\" of object \"{validationContext.GetSourceTypeName()}\" not syncronized with lambda ({authLambda.Name}) auth domain type \"{authLambda.AuthDomainTypeId}\"";

            return ValidationResult.FromMaybe(errorRequest);
        }


        public static SyncAuthDomainTypeValidator Value { get; } = new SyncAuthDomainTypeValidator();
    }
}