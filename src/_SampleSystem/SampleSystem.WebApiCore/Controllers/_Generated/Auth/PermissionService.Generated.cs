namespace Authorization.WebApi.Controllers
{
    using Framework.Authorization.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("authApi/v{version:apiVersion}/[controller]")]
    public partial class PermissionController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService>>
    {
        
        /// <summary>
        /// Check Permission access
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CheckPermissionAccess")]
        public virtual void CheckPermissionAccess(CheckPermissionAccessAutoRequest checkPermissionAccessAutoRequest)
        {
            string securityOperationName = checkPermissionAccessAutoRequest.securityOperationName;
            Framework.Authorization.Generated.DTO.PermissionIdentityDTO permissionIdent = checkPermissionAccessAutoRequest.permissionIdent;
            this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.CheckPermissionAccessInternal(permissionIdent, securityOperationName, evaluateData));
        }
        
        protected virtual void CheckPermissionAccessInternal(Framework.Authorization.Generated.DTO.PermissionIdentityDTO permissionIdent, string securityOperationName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IPermissionBLL bll = evaluateData.Context.Logics.Permission;
            Framework.SecuritySystem.SecurityOperation operation = Framework.Security.SecurityOperationHelper.Parse(typeof(Framework.Authorization.AuthorizationSecurityOperation), securityOperationName);
            Framework.Authorization.Domain.Permission domainObject = bll.GetById(permissionIdent.Id, true);
            Framework.SecuritySystem.SecurityProviderExtensions.CheckAccess(evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Authorization.Domain.Permission>(operation), domainObject, evaluateData.Context.AccessDeniedExceptionService);
        }
        
        /// <summary>
        /// Get Permission (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullPermission")]
        public virtual Framework.Authorization.Generated.DTO.PermissionFullDTO GetFullPermission([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.PermissionIdentityDTO permissionIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullPermissionInternal(permissionIdentity, evaluateData));
        }
        
        protected virtual Framework.Authorization.Generated.DTO.PermissionFullDTO GetFullPermissionInternal(Framework.Authorization.Generated.DTO.PermissionIdentityDTO permissionIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IPermissionBLL bll = evaluateData.Context.Logics.PermissionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Authorization.Domain.Permission domainObject = bll.GetById(permissionIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Permission>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of Permissions (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullPermissions")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.PermissionFullDTO> GetFullPermissions()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullPermissionsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get Permissions (FullDTO) by filter (Framework.Authorization.Domain.PermissionDirectFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullPermissionsByDirectFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.PermissionFullDTO> GetFullPermissionsByDirectFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.PermissionDirectFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullPermissionsByDirectFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.PermissionFullDTO> GetFullPermissionsByDirectFilterInternal(Framework.Authorization.Generated.DTO.PermissionDirectFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IPermissionBLL bll = evaluateData.Context.Logics.PermissionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Authorization.Domain.PermissionDirectFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Permission>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Permissions (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullPermissionsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.PermissionFullDTO> GetFullPermissionsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.PermissionIdentityDTO[] permissionIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullPermissionsByIdentsInternal(permissionIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.PermissionFullDTO> GetFullPermissionsByIdentsInternal(Framework.Authorization.Generated.DTO.PermissionIdentityDTO[] permissionIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IPermissionBLL bll = evaluateData.Context.Logics.PermissionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(permissionIdents, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Permission>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Permissions (FullDTO) by filter (Framework.Authorization.Domain.PermissionRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullPermissionsByRootFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.PermissionFullDTO> GetFullPermissionsByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.PermissionRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullPermissionsByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.PermissionFullDTO> GetFullPermissionsByRootFilterInternal(Framework.Authorization.Generated.DTO.PermissionRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IPermissionBLL bll = evaluateData.Context.Logics.PermissionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Authorization.Domain.PermissionRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Permission>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.PermissionFullDTO> GetFullPermissionsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IPermissionBLL bll = evaluateData.Context.Logics.PermissionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Permission>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Permission (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichPermission")]
        public virtual Framework.Authorization.Generated.DTO.PermissionRichDTO GetRichPermission([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.PermissionIdentityDTO permissionIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichPermissionInternal(permissionIdentity, evaluateData));
        }
        
        protected virtual Framework.Authorization.Generated.DTO.PermissionRichDTO GetRichPermissionInternal(Framework.Authorization.Generated.DTO.PermissionIdentityDTO permissionIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IPermissionBLL bll = evaluateData.Context.Logics.PermissionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Authorization.Domain.Permission domainObject = bll.GetById(permissionIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Permission>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of Permissions (RichDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichPermissions")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.PermissionRichDTO> GetRichPermissions()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichPermissionsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get Permissions (RichDTO) by filter (Framework.Authorization.Domain.PermissionDirectFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichPermissionsByDirectFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.PermissionRichDTO> GetRichPermissionsByDirectFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.PermissionDirectFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichPermissionsByDirectFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.PermissionRichDTO> GetRichPermissionsByDirectFilterInternal(Framework.Authorization.Generated.DTO.PermissionDirectFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IPermissionBLL bll = evaluateData.Context.Logics.PermissionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Authorization.Domain.PermissionDirectFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToRichDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Permission>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Permissions (RichDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichPermissionsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.PermissionRichDTO> GetRichPermissionsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.PermissionIdentityDTO[] permissionIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichPermissionsByIdentsInternal(permissionIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.PermissionRichDTO> GetRichPermissionsByIdentsInternal(Framework.Authorization.Generated.DTO.PermissionIdentityDTO[] permissionIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IPermissionBLL bll = evaluateData.Context.Logics.PermissionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToRichDTOList(bll.GetListByIdents(permissionIdents, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Permission>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Permissions (RichDTO) by filter (Framework.Authorization.Domain.PermissionRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichPermissionsByRootFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.PermissionRichDTO> GetRichPermissionsByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.PermissionRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichPermissionsByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.PermissionRichDTO> GetRichPermissionsByRootFilterInternal(Framework.Authorization.Generated.DTO.PermissionRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IPermissionBLL bll = evaluateData.Context.Logics.PermissionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Authorization.Domain.PermissionRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToRichDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Permission>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.PermissionRichDTO> GetRichPermissionsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IPermissionBLL bll = evaluateData.Context.Logics.PermissionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToRichDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Permission>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Permission (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimplePermission")]
        public virtual Framework.Authorization.Generated.DTO.PermissionSimpleDTO GetSimplePermission([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.PermissionIdentityDTO permissionIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimplePermissionInternal(permissionIdentity, evaluateData));
        }
        
        protected virtual Framework.Authorization.Generated.DTO.PermissionSimpleDTO GetSimplePermissionInternal(Framework.Authorization.Generated.DTO.PermissionIdentityDTO permissionIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IPermissionBLL bll = evaluateData.Context.Logics.PermissionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Authorization.Domain.Permission domainObject = bll.GetById(permissionIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Permission>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of Permissions (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimplePermissions")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.PermissionSimpleDTO> GetSimplePermissions()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimplePermissionsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get Permissions (SimpleDTO) by filter (Framework.Authorization.Domain.PermissionDirectFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimplePermissionsByDirectFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.PermissionSimpleDTO> GetSimplePermissionsByDirectFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.PermissionDirectFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimplePermissionsByDirectFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.PermissionSimpleDTO> GetSimplePermissionsByDirectFilterInternal(Framework.Authorization.Generated.DTO.PermissionDirectFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IPermissionBLL bll = evaluateData.Context.Logics.PermissionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Authorization.Domain.PermissionDirectFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Permission>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Permissions (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimplePermissionsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.PermissionSimpleDTO> GetSimplePermissionsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.PermissionIdentityDTO[] permissionIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimplePermissionsByIdentsInternal(permissionIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.PermissionSimpleDTO> GetSimplePermissionsByIdentsInternal(Framework.Authorization.Generated.DTO.PermissionIdentityDTO[] permissionIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IPermissionBLL bll = evaluateData.Context.Logics.PermissionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(permissionIdents, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Permission>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Permissions (SimpleDTO) by filter (Framework.Authorization.Domain.PermissionRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimplePermissionsByRootFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.PermissionSimpleDTO> GetSimplePermissionsByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.PermissionRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimplePermissionsByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.PermissionSimpleDTO> GetSimplePermissionsByRootFilterInternal(Framework.Authorization.Generated.DTO.PermissionRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IPermissionBLL bll = evaluateData.Context.Logics.PermissionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Authorization.Domain.PermissionRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Permission>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.PermissionSimpleDTO> GetSimplePermissionsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IPermissionBLL bll = evaluateData.Context.Logics.PermissionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Permission>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Check access for Permission
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("HasPermissionAccess")]
        public virtual bool HasPermissionAccess(HasPermissionAccessAutoRequest hasPermissionAccessAutoRequest)
        {
            string securityOperationName = hasPermissionAccessAutoRequest.securityOperationName;
            Framework.Authorization.Generated.DTO.PermissionIdentityDTO permissionIdent = hasPermissionAccessAutoRequest.permissionIdent;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.HasPermissionAccessInternal(permissionIdent, securityOperationName, evaluateData));
        }
        
        protected virtual bool HasPermissionAccessInternal(Framework.Authorization.Generated.DTO.PermissionIdentityDTO permissionIdent, string securityOperationName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IPermissionBLL bll = evaluateData.Context.Logics.Permission;
            Framework.SecuritySystem.SecurityOperation operation = Framework.Security.SecurityOperationHelper.Parse(typeof(Framework.Authorization.AuthorizationSecurityOperation), securityOperationName);
            Framework.Authorization.Domain.Permission domainObject = bll.GetById(permissionIdent.Id, true);
            return evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Authorization.Domain.Permission>(operation).HasAccess(domainObject);
        }
        
        /// <summary>
        /// Remove Permission
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("RemovePermission")]
        public virtual void RemovePermission([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.PermissionIdentityDTO permissionIdent)
        {
            this.Evaluate(Framework.DomainDriven.DBSessionMode.Write, evaluateData => this.RemovePermissionInternal(permissionIdent, evaluateData));
        }
        
        protected virtual void RemovePermissionInternal(Framework.Authorization.Generated.DTO.PermissionIdentityDTO permissionIdent, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IPermissionBLL bll = evaluateData.Context.Logics.PermissionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            this.RemovePermissionInternal(permissionIdent, evaluateData, bll);
        }
        
        protected virtual void RemovePermissionInternal(Framework.Authorization.Generated.DTO.PermissionIdentityDTO permissionIdent, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData, Framework.Authorization.BLL.IPermissionBLL bll)
        {
            Framework.Authorization.Domain.Permission domainObject = bll.GetById(permissionIdent.Id, true);
            bll.Remove(domainObject);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class CheckPermissionAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Authorization.Generated.DTO.PermissionIdentityDTO permissionIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string securityOperationName;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class HasPermissionAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Authorization.Generated.DTO.PermissionIdentityDTO permissionIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string securityOperationName;
    }
}
