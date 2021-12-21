using System;

using Framework.Validation;

namespace Framework.Configuration.Domain
{
    public class AuthDomainTypeRequiredValidatorAttribute : PropertyValidatorAttribute
    {
        public override IPropertyValidator CreateValidator()
        {
            return AuthDomainTypeRequiredValidator.Value;
        }
    }

    public class AuthDomainTypeRequiredValidator : IPropertyValidator<SubscriptionLambda, Guid>
    {
        private AuthDomainTypeRequiredValidator()
        {
        }


        public ValidationResult GetValidationResult(IPropertyValidationContext<SubscriptionLambda, Guid> validationContext)
        {
            if (validationContext == null) throw new ArgumentNullException(nameof(validationContext));

            return validationContext.Source.Type == SubscriptionLambdaType.AuthSource
                 ? RequiredValidator<SubscriptionLambda, Guid>.Default.GetValidationResult(validationContext)
                 : ValidationResult.Success;
        }


        public static AuthDomainTypeRequiredValidator Value { get; } = new AuthDomainTypeRequiredValidator();
    }
}