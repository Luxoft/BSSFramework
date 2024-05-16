﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Framework.Authorization.BLL
{
    
    
    public partial class AuthorizationValidatorBase : Framework.DomainDriven.BLL.BLLContextHandlerValidator<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Domain.AuthorizationOperationContext>
    {
        
        public AuthorizationValidatorBase(Framework.Authorization.BLL.IAuthorizationBLLContext context, Framework.Validation.ValidatorCompileCache cache) : 
                base(context, cache)
        {
            base.RegisterHandler<Framework.Authorization.Domain.BusinessRole>(this.GetBusinessRoleValidationResult);
            base.RegisterHandler<Framework.Authorization.Domain.ChangePermissionDelegatesModel>(this.GetChangePermissionDelegatesModelValidationResult);
            base.RegisterHandler<Framework.Authorization.Domain.DelegateToItemModel>(this.GetDelegateToItemModelValidationResult);
            base.RegisterHandler<Framework.Authorization.Domain.Permission>(this.GetPermissionValidationResult);
            base.RegisterHandler<Framework.Authorization.Domain.PermissionDirectFilterModel>(this.GetPermissionDirectFilterModelValidationResult);
            base.RegisterHandler<Framework.Authorization.Domain.PermissionRestriction>(this.GetPermissionRestrictionValidationResult);
            base.RegisterHandler<Framework.Authorization.Domain.Principal>(this.GetPrincipalValidationResult);
            base.RegisterHandler<Framework.Authorization.Domain.SecurityContextType>(this.GetSecurityContextTypeValidationResult);
            base.RegisterHandler<Framework.Authorization.Domain.SecurityEntity>(this.GetSecurityEntityValidationResult);
            base.RegisterHandler<Framework.Authorization.Domain.UpdatePermissionDelegatesModel>(this.GetUpdatePermissionDelegatesModelValidationResult);
        }
        
        protected virtual Framework.Validation.ValidationResult GetBusinessRoleValidationResult(Framework.Authorization.Domain.BusinessRole source, Framework.Authorization.Domain.AuthorizationOperationContext operationContext, Framework.Validation.IValidationState ownerState)
        {
            return base.GetValidationResult(source, operationContext, ownerState, false);
        }
        
        protected virtual Framework.Validation.ValidationResult GetChangePermissionDelegatesModelValidationResult(Framework.Authorization.Domain.ChangePermissionDelegatesModel source, Framework.Authorization.Domain.AuthorizationOperationContext operationContext, Framework.Validation.IValidationState ownerState)
        {
            return base.GetValidationResult(source, operationContext, ownerState, false);
        }
        
        protected virtual Framework.Validation.ValidationResult GetDelegateToItemModelValidationResult(Framework.Authorization.Domain.DelegateToItemModel source, Framework.Authorization.Domain.AuthorizationOperationContext operationContext, Framework.Validation.IValidationState ownerState)
        {
            return base.GetValidationResult(source, operationContext, ownerState, false);
        }
        
        protected virtual Framework.Validation.ValidationResult GetPermissionDirectFilterModelValidationResult(Framework.Authorization.Domain.PermissionDirectFilterModel source, Framework.Authorization.Domain.AuthorizationOperationContext operationContext, Framework.Validation.IValidationState ownerState)
        {
            return base.GetValidationResult(source, operationContext, ownerState, false);
        }
        
        protected virtual Framework.Validation.ValidationResult GetPermissionRestrictionValidationResult(Framework.Authorization.Domain.PermissionRestriction source, Framework.Authorization.Domain.AuthorizationOperationContext operationContext, Framework.Validation.IValidationState ownerState)
        {
            return base.GetValidationResult(source, operationContext, ownerState, false);
        }
        
        protected virtual Framework.Validation.ValidationResult GetPermissionValidationResult(Framework.Authorization.Domain.Permission source, Framework.Authorization.Domain.AuthorizationOperationContext operationContext, Framework.Validation.IValidationState ownerState)
        {
            return base.GetValidationResult(source, operationContext, ownerState, false);
        }
        
        protected virtual Framework.Validation.ValidationResult GetPrincipalValidationResult(Framework.Authorization.Domain.Principal source, Framework.Authorization.Domain.AuthorizationOperationContext operationContext, Framework.Validation.IValidationState ownerState)
        {
            return base.GetValidationResult(source, operationContext, ownerState, false);
        }
        
        protected virtual Framework.Validation.ValidationResult GetSecurityContextTypeValidationResult(Framework.Authorization.Domain.SecurityContextType source, Framework.Authorization.Domain.AuthorizationOperationContext operationContext, Framework.Validation.IValidationState ownerState)
        {
            return base.GetValidationResult(source, operationContext, ownerState, false);
        }
        
        protected virtual Framework.Validation.ValidationResult GetSecurityEntityValidationResult(Framework.Authorization.Domain.SecurityEntity source, Framework.Authorization.Domain.AuthorizationOperationContext operationContext, Framework.Validation.IValidationState ownerState)
        {
            return base.GetValidationResult(source, operationContext, ownerState, false);
        }
        
        protected virtual Framework.Validation.ValidationResult GetUpdatePermissionDelegatesModelValidationResult(Framework.Authorization.Domain.UpdatePermissionDelegatesModel source, Framework.Authorization.Domain.AuthorizationOperationContext operationContext, Framework.Validation.IValidationState ownerState)
        {
            return base.GetValidationResult(source, operationContext, ownerState, false);
        }
    }
    
    public partial class AuthorizationValidator : Framework.Authorization.BLL.AuthorizationValidatorBase
    {
        
        public AuthorizationValidator(Framework.Authorization.BLL.IAuthorizationBLLContext context, Framework.Validation.ValidatorCompileCache cache) : 
                base(context, cache)
        {
        }
    }
}
