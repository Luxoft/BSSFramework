namespace Authorization.WebApi.Controllers
{
    using Framework.Authorization.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("authApi/v{version:apiVersion}/[controller]")]
    public partial class BusinessRoleController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<Framework.Authorization.BLL.IAuthorizationBLLContext>, Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService>>
    {
        
        public BusinessRoleController(Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<Framework.Authorization.BLL.IAuthorizationBLLContext> serviceEnvironment, Framework.Exceptions.IExceptionProcessor exceptionProcessor) : 
                base(serviceEnvironment, exceptionProcessor)
        {
        }
        
        /// <summary>
        /// Check BusinessRole access
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CheckBusinessRoleAccess")]
        public virtual void CheckBusinessRoleAccess(CheckBusinessRoleAccessAutoRequest checkBusinessRoleAccessAutoRequest)
        {
            Framework.Authorization.AuthorizationSecurityOperationCode securityOperationCode = checkBusinessRoleAccessAutoRequest.securityOperationCode;
            Framework.Authorization.Generated.DTO.BusinessRoleIdentityDTO businessRoleIdent = checkBusinessRoleAccessAutoRequest.businessRoleIdent;
            this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.CheckBusinessRoleAccessInternal(businessRoleIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual void CheckBusinessRoleAccessInternal(Framework.Authorization.Generated.DTO.BusinessRoleIdentityDTO businessRoleIdent, Framework.Authorization.AuthorizationSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IBusinessRoleBLL bll = evaluateData.Context.Logics.BusinessRole;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            Framework.Authorization.Domain.BusinessRole domainObject = bll.GetById(businessRoleIdent.Id, true);
            Framework.SecuritySystem.SecurityProviderExtensions.CheckAccess(evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Authorization.Domain.BusinessRole>(securityOperationCode), domainObject);
        }
        
        /// <summary>
        /// Create BusinessRole by model (BusinessRoleCreateModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CreateBusinessRole")]
        public virtual Framework.Authorization.Generated.DTO.BusinessRoleRichDTO CreateBusinessRole([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.BusinessRoleCreateModelStrictDTO businessRoleCreateModel)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.CreateBusinessRoleInternal(businessRoleCreateModel, evaluateData));
        }
        
        protected virtual Framework.Authorization.Generated.DTO.BusinessRoleRichDTO CreateBusinessRoleInternal(Framework.Authorization.Generated.DTO.BusinessRoleCreateModelStrictDTO businessRoleCreateModel, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IBusinessRoleBLL bll = evaluateData.Context.Logics.BusinessRoleFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            Framework.Authorization.Domain.BusinessRoleCreateModel createModel = businessRoleCreateModel.ToDomainObject(evaluateData.MappingService);
            Framework.Authorization.Domain.BusinessRole domainObject = bll.Create(createModel);
            bll.CheckAccess(domainObject);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, Framework.Authorization.BLL.IAuthorizationBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService>(session, context, new AuthorizationServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get BusinessRole (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullBusinessRole")]
        public virtual Framework.Authorization.Generated.DTO.BusinessRoleFullDTO GetFullBusinessRole([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.BusinessRoleIdentityDTO businessRoleIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullBusinessRoleInternal(businessRoleIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get BusinessRole (FullDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullBusinessRoleByName")]
        public virtual Framework.Authorization.Generated.DTO.BusinessRoleFullDTO GetFullBusinessRoleByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string businessRoleName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullBusinessRoleByNameInternal(businessRoleName, evaluateData));
        }
        
        protected virtual Framework.Authorization.Generated.DTO.BusinessRoleFullDTO GetFullBusinessRoleByNameInternal(string businessRoleName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IBusinessRoleBLL bll = evaluateData.Context.Logics.BusinessRoleFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Authorization.Domain.BusinessRole domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, businessRoleName, true, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.BusinessRole>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Authorization.Generated.DTO.BusinessRoleFullDTO GetFullBusinessRoleInternal(Framework.Authorization.Generated.DTO.BusinessRoleIdentityDTO businessRoleIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IBusinessRoleBLL bll = evaluateData.Context.Logics.BusinessRoleFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Authorization.Domain.BusinessRole domainObject = bll.GetById(businessRoleIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.BusinessRole>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of BusinessRoles (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullBusinessRoles")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.BusinessRoleFullDTO> GetFullBusinessRoles()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullBusinessRolesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get BusinessRoles (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullBusinessRolesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.BusinessRoleFullDTO> GetFullBusinessRolesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.BusinessRoleIdentityDTO[] businessRoleIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullBusinessRolesByIdentsInternal(businessRoleIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.BusinessRoleFullDTO> GetFullBusinessRolesByIdentsInternal(Framework.Authorization.Generated.DTO.BusinessRoleIdentityDTO[] businessRoleIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IBusinessRoleBLL bll = evaluateData.Context.Logics.BusinessRoleFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(businessRoleIdents, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.BusinessRole>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get BusinessRoles (FullDTO) by filter (Framework.Authorization.Domain.BusinessRoleRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullBusinessRolesByRootFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.BusinessRoleFullDTO> GetFullBusinessRolesByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.BusinessRoleRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullBusinessRolesByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.BusinessRoleFullDTO> GetFullBusinessRolesByRootFilterInternal(Framework.Authorization.Generated.DTO.BusinessRoleRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IBusinessRoleBLL bll = evaluateData.Context.Logics.BusinessRoleFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Authorization.Domain.BusinessRoleRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.BusinessRole>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.BusinessRoleFullDTO> GetFullBusinessRolesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IBusinessRoleBLL bll = evaluateData.Context.Logics.BusinessRoleFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.BusinessRole>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get BusinessRole (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichBusinessRole")]
        public virtual Framework.Authorization.Generated.DTO.BusinessRoleRichDTO GetRichBusinessRole([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.BusinessRoleIdentityDTO businessRoleIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichBusinessRoleInternal(businessRoleIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get BusinessRole (RichDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichBusinessRoleByName")]
        public virtual Framework.Authorization.Generated.DTO.BusinessRoleRichDTO GetRichBusinessRoleByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string businessRoleName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichBusinessRoleByNameInternal(businessRoleName, evaluateData));
        }
        
        protected virtual Framework.Authorization.Generated.DTO.BusinessRoleRichDTO GetRichBusinessRoleByNameInternal(string businessRoleName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IBusinessRoleBLL bll = evaluateData.Context.Logics.BusinessRoleFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Authorization.Domain.BusinessRole domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, businessRoleName, true, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.BusinessRole>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Authorization.Generated.DTO.BusinessRoleRichDTO GetRichBusinessRoleInternal(Framework.Authorization.Generated.DTO.BusinessRoleIdentityDTO businessRoleIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IBusinessRoleBLL bll = evaluateData.Context.Logics.BusinessRoleFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Authorization.Domain.BusinessRole domainObject = bll.GetById(businessRoleIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.BusinessRole>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get BusinessRole (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleBusinessRole")]
        public virtual Framework.Authorization.Generated.DTO.BusinessRoleSimpleDTO GetSimpleBusinessRole([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.BusinessRoleIdentityDTO businessRoleIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleBusinessRoleInternal(businessRoleIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get BusinessRole (SimpleDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleBusinessRoleByName")]
        public virtual Framework.Authorization.Generated.DTO.BusinessRoleSimpleDTO GetSimpleBusinessRoleByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string businessRoleName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleBusinessRoleByNameInternal(businessRoleName, evaluateData));
        }
        
        protected virtual Framework.Authorization.Generated.DTO.BusinessRoleSimpleDTO GetSimpleBusinessRoleByNameInternal(string businessRoleName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IBusinessRoleBLL bll = evaluateData.Context.Logics.BusinessRoleFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Authorization.Domain.BusinessRole domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, businessRoleName, true, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.BusinessRole>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Authorization.Generated.DTO.BusinessRoleSimpleDTO GetSimpleBusinessRoleInternal(Framework.Authorization.Generated.DTO.BusinessRoleIdentityDTO businessRoleIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IBusinessRoleBLL bll = evaluateData.Context.Logics.BusinessRoleFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Authorization.Domain.BusinessRole domainObject = bll.GetById(businessRoleIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.BusinessRole>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of BusinessRoles (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleBusinessRoles")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.BusinessRoleSimpleDTO> GetSimpleBusinessRoles()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleBusinessRolesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get BusinessRoles (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleBusinessRolesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.BusinessRoleSimpleDTO> GetSimpleBusinessRolesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.BusinessRoleIdentityDTO[] businessRoleIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleBusinessRolesByIdentsInternal(businessRoleIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.BusinessRoleSimpleDTO> GetSimpleBusinessRolesByIdentsInternal(Framework.Authorization.Generated.DTO.BusinessRoleIdentityDTO[] businessRoleIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IBusinessRoleBLL bll = evaluateData.Context.Logics.BusinessRoleFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(businessRoleIdents, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.BusinessRole>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get BusinessRoles (SimpleDTO) by filter (Framework.Authorization.Domain.BusinessRoleRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleBusinessRolesByRootFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.BusinessRoleSimpleDTO> GetSimpleBusinessRolesByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.BusinessRoleRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleBusinessRolesByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.BusinessRoleSimpleDTO> GetSimpleBusinessRolesByRootFilterInternal(Framework.Authorization.Generated.DTO.BusinessRoleRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IBusinessRoleBLL bll = evaluateData.Context.Logics.BusinessRoleFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Authorization.Domain.BusinessRoleRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.BusinessRole>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.BusinessRoleSimpleDTO> GetSimpleBusinessRolesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IBusinessRoleBLL bll = evaluateData.Context.Logics.BusinessRoleFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.BusinessRole>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get BusinessRole (VisualDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualBusinessRole")]
        public virtual Framework.Authorization.Generated.DTO.BusinessRoleVisualDTO GetVisualBusinessRole([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.BusinessRoleIdentityDTO businessRoleIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualBusinessRoleInternal(businessRoleIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get BusinessRole (VisualDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualBusinessRoleByName")]
        public virtual Framework.Authorization.Generated.DTO.BusinessRoleVisualDTO GetVisualBusinessRoleByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string businessRoleName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualBusinessRoleByNameInternal(businessRoleName, evaluateData));
        }
        
        protected virtual Framework.Authorization.Generated.DTO.BusinessRoleVisualDTO GetVisualBusinessRoleByNameInternal(string businessRoleName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IBusinessRoleBLL bll = evaluateData.Context.Logics.BusinessRoleFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Authorization.Domain.BusinessRole domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, businessRoleName, true, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.BusinessRole>(Framework.Transfering.ViewDTOType.VisualDTO));
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Authorization.Generated.DTO.BusinessRoleVisualDTO GetVisualBusinessRoleInternal(Framework.Authorization.Generated.DTO.BusinessRoleIdentityDTO businessRoleIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IBusinessRoleBLL bll = evaluateData.Context.Logics.BusinessRoleFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Authorization.Domain.BusinessRole domainObject = bll.GetById(businessRoleIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.BusinessRole>(Framework.Transfering.ViewDTOType.VisualDTO));
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of BusinessRoles (VisualDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualBusinessRoles")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.BusinessRoleVisualDTO> GetVisualBusinessRoles()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualBusinessRolesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get BusinessRoles (VisualDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualBusinessRolesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.BusinessRoleVisualDTO> GetVisualBusinessRolesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.BusinessRoleIdentityDTO[] businessRoleIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualBusinessRolesByIdentsInternal(businessRoleIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.BusinessRoleVisualDTO> GetVisualBusinessRolesByIdentsInternal(Framework.Authorization.Generated.DTO.BusinessRoleIdentityDTO[] businessRoleIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IBusinessRoleBLL bll = evaluateData.Context.Logics.BusinessRoleFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListByIdents(businessRoleIdents, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.BusinessRole>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get BusinessRoles (VisualDTO) by filter (Framework.Authorization.Domain.BusinessRoleRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualBusinessRolesByRootFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.BusinessRoleVisualDTO> GetVisualBusinessRolesByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.BusinessRoleRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualBusinessRolesByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.BusinessRoleVisualDTO> GetVisualBusinessRolesByRootFilterInternal(Framework.Authorization.Generated.DTO.BusinessRoleRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IBusinessRoleBLL bll = evaluateData.Context.Logics.BusinessRoleFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Authorization.Domain.BusinessRoleRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.BusinessRole>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.BusinessRoleVisualDTO> GetVisualBusinessRolesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IBusinessRoleBLL bll = evaluateData.Context.Logics.BusinessRoleFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.BusinessRole>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Check access for BusinessRole
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("HasBusinessRoleAccess")]
        public virtual bool HasBusinessRoleAccess(HasBusinessRoleAccessAutoRequest hasBusinessRoleAccessAutoRequest)
        {
            Framework.Authorization.AuthorizationSecurityOperationCode securityOperationCode = hasBusinessRoleAccessAutoRequest.securityOperationCode;
            Framework.Authorization.Generated.DTO.BusinessRoleIdentityDTO businessRoleIdent = hasBusinessRoleAccessAutoRequest.businessRoleIdent;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.HasBusinessRoleAccessInternal(businessRoleIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual bool HasBusinessRoleAccessInternal(Framework.Authorization.Generated.DTO.BusinessRoleIdentityDTO businessRoleIdent, Framework.Authorization.AuthorizationSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IBusinessRoleBLL bll = evaluateData.Context.Logics.BusinessRole;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            Framework.Authorization.Domain.BusinessRole domainObject = bll.GetById(businessRoleIdent.Id, true);
            return evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Authorization.Domain.BusinessRole>(securityOperationCode).HasAccess(domainObject);
        }
        
        /// <summary>
        /// Remove BusinessRole
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("RemoveBusinessRole")]
        public virtual void RemoveBusinessRole([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.BusinessRoleIdentityDTO businessRoleIdent)
        {
            this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Write, evaluateData => this.RemoveBusinessRoleInternal(businessRoleIdent, evaluateData));
        }
        
        protected virtual void RemoveBusinessRoleInternal(Framework.Authorization.Generated.DTO.BusinessRoleIdentityDTO businessRoleIdent, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IBusinessRoleBLL bll = evaluateData.Context.Logics.BusinessRoleFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            this.RemoveBusinessRoleInternal(businessRoleIdent, evaluateData, bll);
        }
        
        protected virtual void RemoveBusinessRoleInternal(Framework.Authorization.Generated.DTO.BusinessRoleIdentityDTO businessRoleIdent, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData, Framework.Authorization.BLL.IBusinessRoleBLL bll)
        {
            Framework.Authorization.Domain.BusinessRole domainObject = bll.GetById(businessRoleIdent.Id, true);
            bll.Remove(domainObject);
        }
        
        /// <summary>
        /// Save BusinessRoles
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("SaveBusinessRole")]
        public virtual Framework.Authorization.Generated.DTO.BusinessRoleIdentityDTO SaveBusinessRole([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.BusinessRoleStrictDTO businessRoleStrict)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Write, evaluateData => this.SaveBusinessRoleInternal(businessRoleStrict, evaluateData));
        }
        
        protected virtual Framework.Authorization.Generated.DTO.BusinessRoleIdentityDTO SaveBusinessRoleInternal(Framework.Authorization.Generated.DTO.BusinessRoleStrictDTO businessRoleStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IBusinessRoleBLL bll = evaluateData.Context.Logics.BusinessRoleFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            return this.SaveBusinessRoleInternal(businessRoleStrict, evaluateData, bll);
        }
        
        protected virtual Framework.Authorization.Generated.DTO.BusinessRoleIdentityDTO SaveBusinessRoleInternal(Framework.Authorization.Generated.DTO.BusinessRoleStrictDTO businessRoleStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData, Framework.Authorization.BLL.IBusinessRoleBLL bll)
        {
            Framework.Authorization.Domain.BusinessRole domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByIdOrCreate(bll, businessRoleStrict.Id);
            businessRoleStrict.MapToDomainObject(evaluateData.MappingService, domainObject);
            bll.Save(domainObject);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToIdentityDTO(domainObject);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class CheckBusinessRoleAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Authorization.Generated.DTO.BusinessRoleIdentityDTO businessRoleIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public Framework.Authorization.AuthorizationSecurityOperationCode securityOperationCode;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class HasBusinessRoleAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Authorization.Generated.DTO.BusinessRoleIdentityDTO businessRoleIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public Framework.Authorization.AuthorizationSecurityOperationCode securityOperationCode;
    }
}
