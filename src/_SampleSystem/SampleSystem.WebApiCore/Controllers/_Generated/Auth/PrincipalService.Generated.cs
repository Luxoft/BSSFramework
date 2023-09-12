namespace Authorization.WebApi.Controllers
{
    using Framework.Authorization.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("authApi/v{version:apiVersion}/[controller]")]
    public partial class PrincipalController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService>>
    {
        
        /// <summary>
        /// Check Principal access
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CheckPrincipalAccess")]
        public virtual void CheckPrincipalAccess(CheckPrincipalAccessAutoRequest checkPrincipalAccessAutoRequest)
        {
            Framework.Authorization.AuthorizationSecurityOperationCode securityOperationCode = checkPrincipalAccessAutoRequest.securityOperationCode;
            Framework.Authorization.Generated.DTO.PrincipalIdentityDTO principalIdent = checkPrincipalAccessAutoRequest.principalIdent;
            this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.CheckPrincipalAccessInternal(principalIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual void CheckPrincipalAccessInternal(Framework.Authorization.Generated.DTO.PrincipalIdentityDTO principalIdent, Framework.Authorization.AuthorizationSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.Principal;
            Framework.Security.SecurityOperationParser.Check(securityOperationCode);
            Framework.Authorization.Domain.Principal domainObject = bll.GetById(principalIdent.Id, true);
            Framework.SecuritySystem.SecurityProviderExtensions.CheckAccess(evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Authorization.Domain.Principal>(securityOperationCode), domainObject, evaluateData.Context.AccessDeniedExceptionService);
        }
        
        /// <summary>
        /// Create Principal by model (PrincipalCreateModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CreatePrincipal")]
        public virtual Framework.Authorization.Generated.DTO.PrincipalRichDTO CreatePrincipal([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.PrincipalCreateModelStrictDTO principalCreateModel)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.CreatePrincipalInternal(principalCreateModel, evaluateData));
        }
        
        protected virtual Framework.Authorization.Generated.DTO.PrincipalRichDTO CreatePrincipalInternal(Framework.Authorization.Generated.DTO.PrincipalCreateModelStrictDTO principalCreateModel, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            Framework.Authorization.Domain.PrincipalCreateModel createModel = principalCreateModel.ToDomainObject(evaluateData.MappingService);
            Framework.Authorization.Domain.Principal domainObject = bll.Create(createModel);
            bll.CheckAccess(domainObject);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Principal (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullPrincipal")]
        public virtual Framework.Authorization.Generated.DTO.PrincipalFullDTO GetFullPrincipal([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.PrincipalIdentityDTO principalIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullPrincipalInternal(principalIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get Principal (FullDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullPrincipalByName")]
        public virtual Framework.Authorization.Generated.DTO.PrincipalFullDTO GetFullPrincipalByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string principalName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullPrincipalByNameInternal(principalName, evaluateData));
        }
        
        protected virtual Framework.Authorization.Generated.DTO.PrincipalFullDTO GetFullPrincipalByNameInternal(string principalName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Authorization.Domain.Principal domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, principalName, true, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Principal>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Authorization.Generated.DTO.PrincipalFullDTO GetFullPrincipalInternal(Framework.Authorization.Generated.DTO.PrincipalIdentityDTO principalIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Authorization.Domain.Principal domainObject = bll.GetById(principalIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Principal>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of Principals (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullPrincipals")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.PrincipalFullDTO> GetFullPrincipals()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullPrincipalsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get Principals (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullPrincipalsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.PrincipalFullDTO> GetFullPrincipalsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.PrincipalIdentityDTO[] principalIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullPrincipalsByIdentsInternal(principalIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.PrincipalFullDTO> GetFullPrincipalsByIdentsInternal(Framework.Authorization.Generated.DTO.PrincipalIdentityDTO[] principalIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(principalIdents, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Principal>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Principals (FullDTO) by filter (Framework.Authorization.Domain.PrincipalRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullPrincipalsByRootFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.PrincipalFullDTO> GetFullPrincipalsByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.PrincipalRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullPrincipalsByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.PrincipalFullDTO> GetFullPrincipalsByRootFilterInternal(Framework.Authorization.Generated.DTO.PrincipalRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Authorization.Domain.PrincipalRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Principal>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.PrincipalFullDTO> GetFullPrincipalsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Principal>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Principal (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichPrincipal")]
        public virtual Framework.Authorization.Generated.DTO.PrincipalRichDTO GetRichPrincipal([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.PrincipalIdentityDTO principalIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichPrincipalInternal(principalIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get Principal (RichDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichPrincipalByName")]
        public virtual Framework.Authorization.Generated.DTO.PrincipalRichDTO GetRichPrincipalByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string principalName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichPrincipalByNameInternal(principalName, evaluateData));
        }
        
        protected virtual Framework.Authorization.Generated.DTO.PrincipalRichDTO GetRichPrincipalByNameInternal(string principalName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Authorization.Domain.Principal domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, principalName, true, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Principal>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Authorization.Generated.DTO.PrincipalRichDTO GetRichPrincipalInternal(Framework.Authorization.Generated.DTO.PrincipalIdentityDTO principalIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Authorization.Domain.Principal domainObject = bll.GetById(principalIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Principal>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Principal (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimplePrincipal")]
        public virtual Framework.Authorization.Generated.DTO.PrincipalSimpleDTO GetSimplePrincipal([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.PrincipalIdentityDTO principalIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimplePrincipalInternal(principalIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get Principal (SimpleDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimplePrincipalByName")]
        public virtual Framework.Authorization.Generated.DTO.PrincipalSimpleDTO GetSimplePrincipalByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string principalName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimplePrincipalByNameInternal(principalName, evaluateData));
        }
        
        protected virtual Framework.Authorization.Generated.DTO.PrincipalSimpleDTO GetSimplePrincipalByNameInternal(string principalName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Authorization.Domain.Principal domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, principalName, true, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Principal>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Authorization.Generated.DTO.PrincipalSimpleDTO GetSimplePrincipalInternal(Framework.Authorization.Generated.DTO.PrincipalIdentityDTO principalIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Authorization.Domain.Principal domainObject = bll.GetById(principalIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Principal>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of Principals (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimplePrincipals")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.PrincipalSimpleDTO> GetSimplePrincipals()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimplePrincipalsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get Principals (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimplePrincipalsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.PrincipalSimpleDTO> GetSimplePrincipalsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.PrincipalIdentityDTO[] principalIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimplePrincipalsByIdentsInternal(principalIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.PrincipalSimpleDTO> GetSimplePrincipalsByIdentsInternal(Framework.Authorization.Generated.DTO.PrincipalIdentityDTO[] principalIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(principalIdents, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Principal>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Principals (SimpleDTO) by filter (Framework.Authorization.Domain.PrincipalRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimplePrincipalsByRootFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.PrincipalSimpleDTO> GetSimplePrincipalsByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.PrincipalRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimplePrincipalsByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.PrincipalSimpleDTO> GetSimplePrincipalsByRootFilterInternal(Framework.Authorization.Generated.DTO.PrincipalRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Authorization.Domain.PrincipalRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Principal>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.PrincipalSimpleDTO> GetSimplePrincipalsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Principal>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Principal (VisualDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualPrincipal")]
        public virtual Framework.Authorization.Generated.DTO.PrincipalVisualDTO GetVisualPrincipal([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.PrincipalIdentityDTO principalIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualPrincipalInternal(principalIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get Principal (VisualDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualPrincipalByName")]
        public virtual Framework.Authorization.Generated.DTO.PrincipalVisualDTO GetVisualPrincipalByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string principalName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualPrincipalByNameInternal(principalName, evaluateData));
        }
        
        protected virtual Framework.Authorization.Generated.DTO.PrincipalVisualDTO GetVisualPrincipalByNameInternal(string principalName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Authorization.Domain.Principal domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, principalName, true, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Principal>(Framework.Transfering.ViewDTOType.VisualDTO));
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Authorization.Generated.DTO.PrincipalVisualDTO GetVisualPrincipalInternal(Framework.Authorization.Generated.DTO.PrincipalIdentityDTO principalIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Authorization.Domain.Principal domainObject = bll.GetById(principalIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Principal>(Framework.Transfering.ViewDTOType.VisualDTO));
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of Principals (VisualDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualPrincipals")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.PrincipalVisualDTO> GetVisualPrincipals()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualPrincipalsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get Principals (VisualDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualPrincipalsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.PrincipalVisualDTO> GetVisualPrincipalsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.PrincipalIdentityDTO[] principalIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualPrincipalsByIdentsInternal(principalIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.PrincipalVisualDTO> GetVisualPrincipalsByIdentsInternal(Framework.Authorization.Generated.DTO.PrincipalIdentityDTO[] principalIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListByIdents(principalIdents, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Principal>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Principals (VisualDTO) by filter (Framework.Authorization.Domain.PrincipalRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualPrincipalsByRootFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.PrincipalVisualDTO> GetVisualPrincipalsByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.PrincipalRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualPrincipalsByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.PrincipalVisualDTO> GetVisualPrincipalsByRootFilterInternal(Framework.Authorization.Generated.DTO.PrincipalRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Authorization.Domain.PrincipalRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Principal>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.PrincipalVisualDTO> GetVisualPrincipalsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.Principal>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Check access for Principal
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("HasPrincipalAccess")]
        public virtual bool HasPrincipalAccess(HasPrincipalAccessAutoRequest hasPrincipalAccessAutoRequest)
        {
            Framework.Authorization.AuthorizationSecurityOperationCode securityOperationCode = hasPrincipalAccessAutoRequest.securityOperationCode;
            Framework.Authorization.Generated.DTO.PrincipalIdentityDTO principalIdent = hasPrincipalAccessAutoRequest.principalIdent;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.HasPrincipalAccessInternal(principalIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual bool HasPrincipalAccessInternal(Framework.Authorization.Generated.DTO.PrincipalIdentityDTO principalIdent, Framework.Authorization.AuthorizationSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.Principal;
            Framework.Security.SecurityOperationParser.Check(securityOperationCode);
            Framework.Authorization.Domain.Principal domainObject = bll.GetById(principalIdent.Id, true);
            return evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Authorization.Domain.Principal>(securityOperationCode).HasAccess(domainObject);
        }
        
        /// <summary>
        /// Remove Principal
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("RemovePrincipal")]
        public virtual void RemovePrincipal([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.PrincipalIdentityDTO principalIdent)
        {
            this.Evaluate(Framework.DomainDriven.DBSessionMode.Write, evaluateData => this.RemovePrincipalInternal(principalIdent, evaluateData));
        }
        
        protected virtual void RemovePrincipalInternal(Framework.Authorization.Generated.DTO.PrincipalIdentityDTO principalIdent, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            this.RemovePrincipalInternal(principalIdent, evaluateData, bll);
        }
        
        protected virtual void RemovePrincipalInternal(Framework.Authorization.Generated.DTO.PrincipalIdentityDTO principalIdent, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData, Framework.Authorization.BLL.IPrincipalBLL bll)
        {
            Framework.Authorization.Domain.Principal domainObject = bll.GetById(principalIdent.Id, true);
            bll.Remove(domainObject);
        }
        
        /// <summary>
        /// Save Principals
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("SavePrincipal")]
        public virtual Framework.Authorization.Generated.DTO.PrincipalIdentityDTO SavePrincipal([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.PrincipalStrictDTO principalStrict)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Write, evaluateData => this.SavePrincipalInternal(principalStrict, evaluateData));
        }
        
        protected virtual Framework.Authorization.Generated.DTO.PrincipalIdentityDTO SavePrincipalInternal(Framework.Authorization.Generated.DTO.PrincipalStrictDTO principalStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            return this.SavePrincipalInternal(principalStrict, evaluateData, bll);
        }
        
        protected virtual Framework.Authorization.Generated.DTO.PrincipalIdentityDTO SavePrincipalInternal(Framework.Authorization.Generated.DTO.PrincipalStrictDTO principalStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData, Framework.Authorization.BLL.IPrincipalBLL bll)
        {
            Framework.Authorization.Domain.Principal domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByIdOrCreate(bll, principalStrict.Id);
            principalStrict.MapToDomainObject(evaluateData.MappingService, domainObject);
            bll.Save(domainObject);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToIdentityDTO(domainObject);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class CheckPrincipalAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Authorization.Generated.DTO.PrincipalIdentityDTO principalIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public Framework.Authorization.AuthorizationSecurityOperationCode securityOperationCode;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class HasPrincipalAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Authorization.Generated.DTO.PrincipalIdentityDTO principalIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public Framework.Authorization.AuthorizationSecurityOperationCode securityOperationCode;
    }
}
