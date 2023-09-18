namespace Configuration.WebApi.Controllers
{
    using Framework.Configuration.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("configApi/v{version:apiVersion}/[controller]")]
    public partial class SystemConstantController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService>>
    {
        
        /// <summary>
        /// Check SystemConstant access
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CheckSystemConstantAccess")]
        public virtual void CheckSystemConstantAccess(CheckSystemConstantAccessAutoRequest checkSystemConstantAccessAutoRequest)
        {
            string securityOperationName = checkSystemConstantAccessAutoRequest.securityOperationName;
            Framework.Configuration.Generated.DTO.SystemConstantIdentityDTO systemConstantIdent = checkSystemConstantAccessAutoRequest.systemConstantIdent;
            this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.CheckSystemConstantAccessInternal(systemConstantIdent, securityOperationName, evaluateData));
        }
        
        protected virtual void CheckSystemConstantAccessInternal(Framework.Configuration.Generated.DTO.SystemConstantIdentityDTO systemConstantIdent, string securityOperationName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISystemConstantBLL bll = evaluateData.Context.Logics.SystemConstant;
            Framework.SecuritySystem.SecurityOperation operation = Framework.Security.SecurityOperationHelper.Parse(typeof(Framework.Configuration.ConfigurationSecurityOperation), securityOperationName);
            Framework.Configuration.Domain.SystemConstant domainObject = bll.GetById(systemConstantIdent.Id, true);
            Framework.SecuritySystem.SecurityProviderBaseExtensions.CheckAccess(evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Configuration.Domain.SystemConstant>(operation), domainObject, evaluateData.Context.AccessDeniedExceptionService);
        }
        
        /// <summary>
        /// Get SystemConstant (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullSystemConstant")]
        public virtual Framework.Configuration.Generated.DTO.SystemConstantFullDTO GetFullSystemConstant([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SystemConstantIdentityDTO systemConstantIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullSystemConstantInternal(systemConstantIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get SystemConstant (FullDTO) by code
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullSystemConstantByCode")]
        public virtual Framework.Configuration.Generated.DTO.SystemConstantFullDTO GetFullSystemConstantByCode([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string systemConstantCode)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullSystemConstantByCodeInternal(systemConstantCode, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.SystemConstantFullDTO GetFullSystemConstantByCodeInternal(string systemConstantCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISystemConstantBLL bll = evaluateData.Context.Logics.SystemConstantFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.SystemConstant domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByCode(bll, systemConstantCode, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.SystemConstant>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Configuration.Generated.DTO.SystemConstantFullDTO GetFullSystemConstantInternal(Framework.Configuration.Generated.DTO.SystemConstantIdentityDTO systemConstantIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISystemConstantBLL bll = evaluateData.Context.Logics.SystemConstantFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.SystemConstant domainObject = bll.GetById(systemConstantIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.SystemConstant>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of SystemConstants (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullSystemConstants")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SystemConstantFullDTO> GetFullSystemConstants()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullSystemConstantsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get SystemConstants (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullSystemConstantsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SystemConstantFullDTO> GetFullSystemConstantsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SystemConstantIdentityDTO[] systemConstantIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullSystemConstantsByIdentsInternal(systemConstantIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SystemConstantFullDTO> GetFullSystemConstantsByIdentsInternal(Framework.Configuration.Generated.DTO.SystemConstantIdentityDTO[] systemConstantIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISystemConstantBLL bll = evaluateData.Context.Logics.SystemConstantFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(systemConstantIdents, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.SystemConstant>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get SystemConstants (FullDTO) by filter (Framework.Configuration.Domain.SystemConstantRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullSystemConstantsByRootFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SystemConstantFullDTO> GetFullSystemConstantsByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SystemConstantRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullSystemConstantsByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SystemConstantFullDTO> GetFullSystemConstantsByRootFilterInternal(Framework.Configuration.Generated.DTO.SystemConstantRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISystemConstantBLL bll = evaluateData.Context.Logics.SystemConstantFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.SystemConstantRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.SystemConstant>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SystemConstantFullDTO> GetFullSystemConstantsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISystemConstantBLL bll = evaluateData.Context.Logics.SystemConstantFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.SystemConstant>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get SystemConstant (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichSystemConstant")]
        public virtual Framework.Configuration.Generated.DTO.SystemConstantRichDTO GetRichSystemConstant([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SystemConstantIdentityDTO systemConstantIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichSystemConstantInternal(systemConstantIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get SystemConstant (RichDTO) by code
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichSystemConstantByCode")]
        public virtual Framework.Configuration.Generated.DTO.SystemConstantRichDTO GetRichSystemConstantByCode([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string systemConstantCode)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichSystemConstantByCodeInternal(systemConstantCode, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.SystemConstantRichDTO GetRichSystemConstantByCodeInternal(string systemConstantCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISystemConstantBLL bll = evaluateData.Context.Logics.SystemConstantFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.SystemConstant domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByCode(bll, systemConstantCode, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.SystemConstant>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Configuration.Generated.DTO.SystemConstantRichDTO GetRichSystemConstantInternal(Framework.Configuration.Generated.DTO.SystemConstantIdentityDTO systemConstantIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISystemConstantBLL bll = evaluateData.Context.Logics.SystemConstantFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.SystemConstant domainObject = bll.GetById(systemConstantIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.SystemConstant>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get SystemConstant (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleSystemConstant")]
        public virtual Framework.Configuration.Generated.DTO.SystemConstantSimpleDTO GetSimpleSystemConstant([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SystemConstantIdentityDTO systemConstantIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleSystemConstantInternal(systemConstantIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get SystemConstant (SimpleDTO) by code
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleSystemConstantByCode")]
        public virtual Framework.Configuration.Generated.DTO.SystemConstantSimpleDTO GetSimpleSystemConstantByCode([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string systemConstantCode)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleSystemConstantByCodeInternal(systemConstantCode, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.SystemConstantSimpleDTO GetSimpleSystemConstantByCodeInternal(string systemConstantCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISystemConstantBLL bll = evaluateData.Context.Logics.SystemConstantFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.SystemConstant domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByCode(bll, systemConstantCode, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.SystemConstant>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Configuration.Generated.DTO.SystemConstantSimpleDTO GetSimpleSystemConstantInternal(Framework.Configuration.Generated.DTO.SystemConstantIdentityDTO systemConstantIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISystemConstantBLL bll = evaluateData.Context.Logics.SystemConstantFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.SystemConstant domainObject = bll.GetById(systemConstantIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.SystemConstant>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of SystemConstants (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleSystemConstants")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SystemConstantSimpleDTO> GetSimpleSystemConstants()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleSystemConstantsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get SystemConstants (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleSystemConstantsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SystemConstantSimpleDTO> GetSimpleSystemConstantsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SystemConstantIdentityDTO[] systemConstantIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleSystemConstantsByIdentsInternal(systemConstantIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SystemConstantSimpleDTO> GetSimpleSystemConstantsByIdentsInternal(Framework.Configuration.Generated.DTO.SystemConstantIdentityDTO[] systemConstantIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISystemConstantBLL bll = evaluateData.Context.Logics.SystemConstantFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(systemConstantIdents, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.SystemConstant>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get SystemConstants (SimpleDTO) by filter (Framework.Configuration.Domain.SystemConstantRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleSystemConstantsByRootFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SystemConstantSimpleDTO> GetSimpleSystemConstantsByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SystemConstantRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleSystemConstantsByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SystemConstantSimpleDTO> GetSimpleSystemConstantsByRootFilterInternal(Framework.Configuration.Generated.DTO.SystemConstantRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISystemConstantBLL bll = evaluateData.Context.Logics.SystemConstantFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.SystemConstantRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.SystemConstant>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SystemConstantSimpleDTO> GetSimpleSystemConstantsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISystemConstantBLL bll = evaluateData.Context.Logics.SystemConstantFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.SystemConstant>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get SystemConstant (VisualDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualSystemConstant")]
        public virtual Framework.Configuration.Generated.DTO.SystemConstantVisualDTO GetVisualSystemConstant([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SystemConstantIdentityDTO systemConstantIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualSystemConstantInternal(systemConstantIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get SystemConstant (VisualDTO) by code
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualSystemConstantByCode")]
        public virtual Framework.Configuration.Generated.DTO.SystemConstantVisualDTO GetVisualSystemConstantByCode([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string systemConstantCode)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualSystemConstantByCodeInternal(systemConstantCode, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.SystemConstantVisualDTO GetVisualSystemConstantByCodeInternal(string systemConstantCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISystemConstantBLL bll = evaluateData.Context.Logics.SystemConstantFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.SystemConstant domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByCode(bll, systemConstantCode, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.SystemConstant>(Framework.Transfering.ViewDTOType.VisualDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Configuration.Generated.DTO.SystemConstantVisualDTO GetVisualSystemConstantInternal(Framework.Configuration.Generated.DTO.SystemConstantIdentityDTO systemConstantIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISystemConstantBLL bll = evaluateData.Context.Logics.SystemConstantFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.SystemConstant domainObject = bll.GetById(systemConstantIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.SystemConstant>(Framework.Transfering.ViewDTOType.VisualDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of SystemConstants (VisualDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualSystemConstants")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SystemConstantVisualDTO> GetVisualSystemConstants()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualSystemConstantsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get SystemConstants (VisualDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualSystemConstantsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SystemConstantVisualDTO> GetVisualSystemConstantsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SystemConstantIdentityDTO[] systemConstantIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualSystemConstantsByIdentsInternal(systemConstantIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SystemConstantVisualDTO> GetVisualSystemConstantsByIdentsInternal(Framework.Configuration.Generated.DTO.SystemConstantIdentityDTO[] systemConstantIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISystemConstantBLL bll = evaluateData.Context.Logics.SystemConstantFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListByIdents(systemConstantIdents, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.SystemConstant>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get SystemConstants (VisualDTO) by filter (Framework.Configuration.Domain.SystemConstantRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualSystemConstantsByRootFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SystemConstantVisualDTO> GetVisualSystemConstantsByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SystemConstantRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualSystemConstantsByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SystemConstantVisualDTO> GetVisualSystemConstantsByRootFilterInternal(Framework.Configuration.Generated.DTO.SystemConstantRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISystemConstantBLL bll = evaluateData.Context.Logics.SystemConstantFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.SystemConstantRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.SystemConstant>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SystemConstantVisualDTO> GetVisualSystemConstantsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISystemConstantBLL bll = evaluateData.Context.Logics.SystemConstantFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.SystemConstant>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Check access for SystemConstant
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("HasSystemConstantAccess")]
        public virtual bool HasSystemConstantAccess(HasSystemConstantAccessAutoRequest hasSystemConstantAccessAutoRequest)
        {
            string securityOperationName = hasSystemConstantAccessAutoRequest.securityOperationName;
            Framework.Configuration.Generated.DTO.SystemConstantIdentityDTO systemConstantIdent = hasSystemConstantAccessAutoRequest.systemConstantIdent;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.HasSystemConstantAccessInternal(systemConstantIdent, securityOperationName, evaluateData));
        }
        
        protected virtual bool HasSystemConstantAccessInternal(Framework.Configuration.Generated.DTO.SystemConstantIdentityDTO systemConstantIdent, string securityOperationName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISystemConstantBLL bll = evaluateData.Context.Logics.SystemConstant;
            Framework.SecuritySystem.SecurityOperation operation = Framework.Security.SecurityOperationHelper.Parse(typeof(Framework.Configuration.ConfigurationSecurityOperation), securityOperationName);
            Framework.Configuration.Domain.SystemConstant domainObject = bll.GetById(systemConstantIdent.Id, true);
            return evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Configuration.Domain.SystemConstant>(operation).HasAccess(domainObject);
        }
        
        /// <summary>
        /// Save SystemConstants
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("SaveSystemConstant")]
        public virtual Framework.Configuration.Generated.DTO.SystemConstantIdentityDTO SaveSystemConstant([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SystemConstantStrictDTO systemConstantStrict)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Write, evaluateData => this.SaveSystemConstantInternal(systemConstantStrict, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.SystemConstantIdentityDTO SaveSystemConstantInternal(Framework.Configuration.Generated.DTO.SystemConstantStrictDTO systemConstantStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISystemConstantBLL bll = evaluateData.Context.Logics.SystemConstantFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            return this.SaveSystemConstantInternal(systemConstantStrict, evaluateData, bll);
        }
        
        protected virtual Framework.Configuration.Generated.DTO.SystemConstantIdentityDTO SaveSystemConstantInternal(Framework.Configuration.Generated.DTO.SystemConstantStrictDTO systemConstantStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData, Framework.Configuration.BLL.ISystemConstantBLL bll)
        {
            Framework.Configuration.Domain.SystemConstant domainObject = bll.GetById(systemConstantStrict.Id, true);
            systemConstantStrict.MapToDomainObject(evaluateData.MappingService, domainObject);
            bll.Save(domainObject);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToIdentityDTO(domainObject);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class CheckSystemConstantAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Configuration.Generated.DTO.SystemConstantIdentityDTO systemConstantIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string securityOperationName;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class HasSystemConstantAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Configuration.Generated.DTO.SystemConstantIdentityDTO systemConstantIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string securityOperationName;
    }
}
