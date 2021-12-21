//using System;
//using Framework.Core;
//using Framework.Validation;

//namespace Framework.Configuration.Domain
//{
//    public class PrincipalRoleRequiredValidatorAttribute : PropertyValidatorAttribute
//    {
//        public override IPropertyValidator CreateValidator()
//        {
//            return new PrincipalRoleRequiredValidator();
//        }

//        private class PrincipalRoleRequiredValidator : IPropertyValidator<Subscription, Guid>
//        {
//            public ValidationResult GetValidationResult(IPropertyValidationContext<Subscription, Guid> validationContext)
//            {
//                if (validationContext == null) throw new ArgumentNullException("validationContext");

//                return validationContext.Source.PrincipalSource != null
//                    ? RequiredValidator.Value.GetValidationResult(validationContext.Cast(v => v, g => (object)g))
//                    : ValidationResult.Success;
//            }
//        }
//    }
//}