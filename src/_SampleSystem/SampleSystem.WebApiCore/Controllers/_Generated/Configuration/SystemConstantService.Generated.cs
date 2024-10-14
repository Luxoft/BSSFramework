namespace Configuration.WebApi.Controllers
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("configApi/[controller]/[action]")]
    public partial class SystemConstantController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService>
    {
        
        /// <summary>
        /// Get SystemConstant (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.Configuration.Generated.DTO.SystemConstantFullDTO GetFullSystemConstant([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SystemConstantIdentityDTO systemConstantIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullSystemConstantInternal(systemConstantIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get SystemConstant (FullDTO) by code
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.Configuration.Generated.DTO.SystemConstantFullDTO GetFullSystemConstantByCode([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string systemConstantCode)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullSystemConstantByCodeInternal(systemConstantCode, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.SystemConstantFullDTO GetFullSystemConstantByCodeInternal(string systemConstantCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISystemConstantBLL bll = evaluateData.Context.Logics.SystemConstantFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.Configuration.Domain.SystemConstant domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByCode(bll, systemConstantCode, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.SystemConstant>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Configuration.Generated.DTO.SystemConstantFullDTO GetFullSystemConstantInternal(Framework.Configuration.Generated.DTO.SystemConstantIdentityDTO systemConstantIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISystemConstantBLL bll = evaluateData.Context.Logics.SystemConstantFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.Configuration.Domain.SystemConstant domainObject = bll.GetById(systemConstantIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.SystemConstant>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of SystemConstants (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SystemConstantFullDTO> GetFullSystemConstants()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullSystemConstantsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get SystemConstants (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SystemConstantFullDTO> GetFullSystemConstantsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SystemConstantIdentityDTO[] systemConstantIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullSystemConstantsByIdentsInternal(systemConstantIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SystemConstantFullDTO> GetFullSystemConstantsByIdentsInternal(Framework.Configuration.Generated.DTO.SystemConstantIdentityDTO[] systemConstantIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISystemConstantBLL bll = evaluateData.Context.Logics.SystemConstantFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(systemConstantIdents, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.SystemConstant>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get SystemConstants (FullDTO) by operation
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SystemConstantFullDTO> GetFullSystemConstantsByOperation([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.SecuritySystem.DomainSecurityRule.ClientSecurityRule securityRule)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullSystemConstantsByOperationInternal(securityRule, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SystemConstantFullDTO> GetFullSystemConstantsByOperationInternal(Framework.SecuritySystem.DomainSecurityRule.ClientSecurityRule securityRule, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISystemConstantBLL bll = evaluateData.Context.Logics.SystemConstantFactory.Create(securityRule);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.SystemConstant>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get SystemConstants (FullDTO) by filter (Framework.Configuration.Domain.SystemConstantRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SystemConstantFullDTO> GetFullSystemConstantsByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SystemConstantRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullSystemConstantsByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SystemConstantFullDTO> GetFullSystemConstantsByRootFilterInternal(Framework.Configuration.Generated.DTO.SystemConstantRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISystemConstantBLL bll = evaluateData.Context.Logics.SystemConstantFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.Configuration.Domain.SystemConstantRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.SystemConstant>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SystemConstantFullDTO> GetFullSystemConstantsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISystemConstantBLL bll = evaluateData.Context.Logics.SystemConstantFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.SystemConstant>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get SystemConstant (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.Configuration.Generated.DTO.SystemConstantRichDTO GetRichSystemConstant([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SystemConstantIdentityDTO systemConstantIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichSystemConstantInternal(systemConstantIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get SystemConstant (RichDTO) by code
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.Configuration.Generated.DTO.SystemConstantRichDTO GetRichSystemConstantByCode([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string systemConstantCode)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichSystemConstantByCodeInternal(systemConstantCode, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.SystemConstantRichDTO GetRichSystemConstantByCodeInternal(string systemConstantCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISystemConstantBLL bll = evaluateData.Context.Logics.SystemConstantFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.Configuration.Domain.SystemConstant domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByCode(bll, systemConstantCode, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.SystemConstant>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Configuration.Generated.DTO.SystemConstantRichDTO GetRichSystemConstantInternal(Framework.Configuration.Generated.DTO.SystemConstantIdentityDTO systemConstantIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISystemConstantBLL bll = evaluateData.Context.Logics.SystemConstantFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.Configuration.Domain.SystemConstant domainObject = bll.GetById(systemConstantIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.SystemConstant>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get SystemConstant (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.Configuration.Generated.DTO.SystemConstantSimpleDTO GetSimpleSystemConstant([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SystemConstantIdentityDTO systemConstantIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleSystemConstantInternal(systemConstantIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get SystemConstant (SimpleDTO) by code
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.Configuration.Generated.DTO.SystemConstantSimpleDTO GetSimpleSystemConstantByCode([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string systemConstantCode)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleSystemConstantByCodeInternal(systemConstantCode, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.SystemConstantSimpleDTO GetSimpleSystemConstantByCodeInternal(string systemConstantCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISystemConstantBLL bll = evaluateData.Context.Logics.SystemConstantFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.Configuration.Domain.SystemConstant domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByCode(bll, systemConstantCode, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.SystemConstant>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Configuration.Generated.DTO.SystemConstantSimpleDTO GetSimpleSystemConstantInternal(Framework.Configuration.Generated.DTO.SystemConstantIdentityDTO systemConstantIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISystemConstantBLL bll = evaluateData.Context.Logics.SystemConstantFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.Configuration.Domain.SystemConstant domainObject = bll.GetById(systemConstantIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.SystemConstant>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of SystemConstants (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SystemConstantSimpleDTO> GetSimpleSystemConstants()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleSystemConstantsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get SystemConstants (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SystemConstantSimpleDTO> GetSimpleSystemConstantsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SystemConstantIdentityDTO[] systemConstantIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleSystemConstantsByIdentsInternal(systemConstantIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SystemConstantSimpleDTO> GetSimpleSystemConstantsByIdentsInternal(Framework.Configuration.Generated.DTO.SystemConstantIdentityDTO[] systemConstantIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISystemConstantBLL bll = evaluateData.Context.Logics.SystemConstantFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(systemConstantIdents, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.SystemConstant>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get SystemConstants (SimpleDTO) by operation
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SystemConstantSimpleDTO> GetSimpleSystemConstantsByOperation([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.SecuritySystem.DomainSecurityRule.ClientSecurityRule securityRule)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleSystemConstantsByOperationInternal(securityRule, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SystemConstantSimpleDTO> GetSimpleSystemConstantsByOperationInternal(Framework.SecuritySystem.DomainSecurityRule.ClientSecurityRule securityRule, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISystemConstantBLL bll = evaluateData.Context.Logics.SystemConstantFactory.Create(securityRule);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.SystemConstant>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get SystemConstants (SimpleDTO) by filter (Framework.Configuration.Domain.SystemConstantRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SystemConstantSimpleDTO> GetSimpleSystemConstantsByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SystemConstantRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleSystemConstantsByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SystemConstantSimpleDTO> GetSimpleSystemConstantsByRootFilterInternal(Framework.Configuration.Generated.DTO.SystemConstantRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISystemConstantBLL bll = evaluateData.Context.Logics.SystemConstantFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.Configuration.Domain.SystemConstantRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.SystemConstant>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SystemConstantSimpleDTO> GetSimpleSystemConstantsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISystemConstantBLL bll = evaluateData.Context.Logics.SystemConstantFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.SystemConstant>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get SystemConstant (VisualDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.Configuration.Generated.DTO.SystemConstantVisualDTO GetVisualSystemConstant([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SystemConstantIdentityDTO systemConstantIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualSystemConstantInternal(systemConstantIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get SystemConstant (VisualDTO) by code
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.Configuration.Generated.DTO.SystemConstantVisualDTO GetVisualSystemConstantByCode([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string systemConstantCode)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualSystemConstantByCodeInternal(systemConstantCode, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.SystemConstantVisualDTO GetVisualSystemConstantByCodeInternal(string systemConstantCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISystemConstantBLL bll = evaluateData.Context.Logics.SystemConstantFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.Configuration.Domain.SystemConstant domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByCode(bll, systemConstantCode, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.SystemConstant>(Framework.Transfering.ViewDTOType.VisualDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Configuration.Generated.DTO.SystemConstantVisualDTO GetVisualSystemConstantInternal(Framework.Configuration.Generated.DTO.SystemConstantIdentityDTO systemConstantIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISystemConstantBLL bll = evaluateData.Context.Logics.SystemConstantFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.Configuration.Domain.SystemConstant domainObject = bll.GetById(systemConstantIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.SystemConstant>(Framework.Transfering.ViewDTOType.VisualDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of SystemConstants (VisualDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SystemConstantVisualDTO> GetVisualSystemConstants()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualSystemConstantsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get SystemConstants (VisualDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SystemConstantVisualDTO> GetVisualSystemConstantsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SystemConstantIdentityDTO[] systemConstantIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualSystemConstantsByIdentsInternal(systemConstantIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SystemConstantVisualDTO> GetVisualSystemConstantsByIdentsInternal(Framework.Configuration.Generated.DTO.SystemConstantIdentityDTO[] systemConstantIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISystemConstantBLL bll = evaluateData.Context.Logics.SystemConstantFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListByIdents(systemConstantIdents, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.SystemConstant>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get SystemConstants (VisualDTO) by operation
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SystemConstantVisualDTO> GetVisualSystemConstantsByOperation([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.SecuritySystem.DomainSecurityRule.ClientSecurityRule securityRule)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualSystemConstantsByOperationInternal(securityRule, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SystemConstantVisualDTO> GetVisualSystemConstantsByOperationInternal(Framework.SecuritySystem.DomainSecurityRule.ClientSecurityRule securityRule, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISystemConstantBLL bll = evaluateData.Context.Logics.SystemConstantFactory.Create(securityRule);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.SystemConstant>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get SystemConstants (VisualDTO) by filter (Framework.Configuration.Domain.SystemConstantRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SystemConstantVisualDTO> GetVisualSystemConstantsByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SystemConstantRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualSystemConstantsByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SystemConstantVisualDTO> GetVisualSystemConstantsByRootFilterInternal(Framework.Configuration.Generated.DTO.SystemConstantRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISystemConstantBLL bll = evaluateData.Context.Logics.SystemConstantFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.Configuration.Domain.SystemConstantRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.SystemConstant>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SystemConstantVisualDTO> GetVisualSystemConstantsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISystemConstantBLL bll = evaluateData.Context.Logics.SystemConstantFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.SystemConstant>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Save SystemConstants
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.Configuration.Generated.DTO.SystemConstantIdentityDTO SaveSystemConstant([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SystemConstantStrictDTO systemConstantStrict)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Write, evaluateData => this.SaveSystemConstantInternal(systemConstantStrict, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.SystemConstantIdentityDTO SaveSystemConstantInternal(Framework.Configuration.Generated.DTO.SystemConstantStrictDTO systemConstantStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISystemConstantBLL bll = evaluateData.Context.Logics.SystemConstantFactory.Create(Framework.SecuritySystem.SecurityRule.Edit);
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
}
