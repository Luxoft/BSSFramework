﻿namespace Configuration.WebApi.Controllers
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("configApi/[controller]/[action]")]
    public partial class TargetSystemController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService>
    {
        
        /// <summary>
        /// Get TargetSystem (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.Configuration.Generated.DTO.TargetSystemFullDTO GetFullTargetSystem([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.TargetSystemIdentityDTO targetSystemIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullTargetSystemInternal(targetSystemIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get TargetSystem (FullDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.Configuration.Generated.DTO.TargetSystemFullDTO GetFullTargetSystemByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string targetSystemName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullTargetSystemByNameInternal(targetSystemName, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.TargetSystemFullDTO GetFullTargetSystemByNameInternal(string targetSystemName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ITargetSystemBLL bll = evaluateData.Context.Logics.TargetSystemFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.Configuration.Domain.TargetSystem domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, targetSystemName, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.TargetSystem>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Configuration.Generated.DTO.TargetSystemFullDTO GetFullTargetSystemInternal(Framework.Configuration.Generated.DTO.TargetSystemIdentityDTO targetSystemIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ITargetSystemBLL bll = evaluateData.Context.Logics.TargetSystemFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.Configuration.Domain.TargetSystem domainObject = bll.GetById(targetSystemIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.TargetSystem>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of TargetSystems (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.TargetSystemFullDTO> GetFullTargetSystems()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullTargetSystemsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get TargetSystems (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.TargetSystemFullDTO> GetFullTargetSystemsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.TargetSystemIdentityDTO[] targetSystemIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullTargetSystemsByIdentsInternal(targetSystemIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.TargetSystemFullDTO> GetFullTargetSystemsByIdentsInternal(Framework.Configuration.Generated.DTO.TargetSystemIdentityDTO[] targetSystemIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ITargetSystemBLL bll = evaluateData.Context.Logics.TargetSystemFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(targetSystemIdents, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.TargetSystem>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TargetSystems (FullDTO) by filter (Framework.Configuration.Domain.TargetSystemRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.TargetSystemFullDTO> GetFullTargetSystemsByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.TargetSystemRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullTargetSystemsByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.TargetSystemFullDTO> GetFullTargetSystemsByRootFilterInternal(Framework.Configuration.Generated.DTO.TargetSystemRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ITargetSystemBLL bll = evaluateData.Context.Logics.TargetSystemFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.Configuration.Domain.TargetSystemRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.TargetSystem>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.TargetSystemFullDTO> GetFullTargetSystemsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ITargetSystemBLL bll = evaluateData.Context.Logics.TargetSystemFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.TargetSystem>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TargetSystem (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.Configuration.Generated.DTO.TargetSystemRichDTO GetRichTargetSystem([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.TargetSystemIdentityDTO targetSystemIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichTargetSystemInternal(targetSystemIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get TargetSystem (RichDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.Configuration.Generated.DTO.TargetSystemRichDTO GetRichTargetSystemByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string targetSystemName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichTargetSystemByNameInternal(targetSystemName, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.TargetSystemRichDTO GetRichTargetSystemByNameInternal(string targetSystemName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ITargetSystemBLL bll = evaluateData.Context.Logics.TargetSystemFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.Configuration.Domain.TargetSystem domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, targetSystemName, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.TargetSystem>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Configuration.Generated.DTO.TargetSystemRichDTO GetRichTargetSystemInternal(Framework.Configuration.Generated.DTO.TargetSystemIdentityDTO targetSystemIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ITargetSystemBLL bll = evaluateData.Context.Logics.TargetSystemFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.Configuration.Domain.TargetSystem domainObject = bll.GetById(targetSystemIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.TargetSystem>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TargetSystem (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.Configuration.Generated.DTO.TargetSystemSimpleDTO GetSimpleTargetSystem([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.TargetSystemIdentityDTO targetSystemIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleTargetSystemInternal(targetSystemIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get TargetSystem (SimpleDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.Configuration.Generated.DTO.TargetSystemSimpleDTO GetSimpleTargetSystemByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string targetSystemName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleTargetSystemByNameInternal(targetSystemName, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.TargetSystemSimpleDTO GetSimpleTargetSystemByNameInternal(string targetSystemName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ITargetSystemBLL bll = evaluateData.Context.Logics.TargetSystemFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.Configuration.Domain.TargetSystem domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, targetSystemName, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.TargetSystem>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Configuration.Generated.DTO.TargetSystemSimpleDTO GetSimpleTargetSystemInternal(Framework.Configuration.Generated.DTO.TargetSystemIdentityDTO targetSystemIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ITargetSystemBLL bll = evaluateData.Context.Logics.TargetSystemFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.Configuration.Domain.TargetSystem domainObject = bll.GetById(targetSystemIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.TargetSystem>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of TargetSystems (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.TargetSystemSimpleDTO> GetSimpleTargetSystems()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleTargetSystemsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get TargetSystems (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.TargetSystemSimpleDTO> GetSimpleTargetSystemsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.TargetSystemIdentityDTO[] targetSystemIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleTargetSystemsByIdentsInternal(targetSystemIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.TargetSystemSimpleDTO> GetSimpleTargetSystemsByIdentsInternal(Framework.Configuration.Generated.DTO.TargetSystemIdentityDTO[] targetSystemIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ITargetSystemBLL bll = evaluateData.Context.Logics.TargetSystemFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(targetSystemIdents, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.TargetSystem>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TargetSystems (SimpleDTO) by filter (Framework.Configuration.Domain.TargetSystemRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.TargetSystemSimpleDTO> GetSimpleTargetSystemsByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.TargetSystemRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleTargetSystemsByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.TargetSystemSimpleDTO> GetSimpleTargetSystemsByRootFilterInternal(Framework.Configuration.Generated.DTO.TargetSystemRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ITargetSystemBLL bll = evaluateData.Context.Logics.TargetSystemFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.Configuration.Domain.TargetSystemRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.TargetSystem>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.TargetSystemSimpleDTO> GetSimpleTargetSystemsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ITargetSystemBLL bll = evaluateData.Context.Logics.TargetSystemFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.TargetSystem>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TargetSystem (VisualDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.Configuration.Generated.DTO.TargetSystemVisualDTO GetVisualTargetSystem([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.TargetSystemIdentityDTO targetSystemIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualTargetSystemInternal(targetSystemIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get TargetSystem (VisualDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.Configuration.Generated.DTO.TargetSystemVisualDTO GetVisualTargetSystemByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string targetSystemName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualTargetSystemByNameInternal(targetSystemName, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.TargetSystemVisualDTO GetVisualTargetSystemByNameInternal(string targetSystemName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ITargetSystemBLL bll = evaluateData.Context.Logics.TargetSystemFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.Configuration.Domain.TargetSystem domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, targetSystemName, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.TargetSystem>(Framework.Transfering.ViewDTOType.VisualDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Configuration.Generated.DTO.TargetSystemVisualDTO GetVisualTargetSystemInternal(Framework.Configuration.Generated.DTO.TargetSystemIdentityDTO targetSystemIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ITargetSystemBLL bll = evaluateData.Context.Logics.TargetSystemFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.Configuration.Domain.TargetSystem domainObject = bll.GetById(targetSystemIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.TargetSystem>(Framework.Transfering.ViewDTOType.VisualDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of TargetSystems (VisualDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.TargetSystemVisualDTO> GetVisualTargetSystems()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualTargetSystemsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get TargetSystems (VisualDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.TargetSystemVisualDTO> GetVisualTargetSystemsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.TargetSystemIdentityDTO[] targetSystemIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualTargetSystemsByIdentsInternal(targetSystemIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.TargetSystemVisualDTO> GetVisualTargetSystemsByIdentsInternal(Framework.Configuration.Generated.DTO.TargetSystemIdentityDTO[] targetSystemIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ITargetSystemBLL bll = evaluateData.Context.Logics.TargetSystemFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListByIdents(targetSystemIdents, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.TargetSystem>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TargetSystems (VisualDTO) by filter (Framework.Configuration.Domain.TargetSystemRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.TargetSystemVisualDTO> GetVisualTargetSystemsByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.TargetSystemRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualTargetSystemsByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.TargetSystemVisualDTO> GetVisualTargetSystemsByRootFilterInternal(Framework.Configuration.Generated.DTO.TargetSystemRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ITargetSystemBLL bll = evaluateData.Context.Logics.TargetSystemFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.Configuration.Domain.TargetSystemRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.TargetSystem>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.TargetSystemVisualDTO> GetVisualTargetSystemsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ITargetSystemBLL bll = evaluateData.Context.Logics.TargetSystemFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.TargetSystem>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Save TargetSystems
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.Configuration.Generated.DTO.TargetSystemIdentityDTO SaveTargetSystem([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.TargetSystemStrictDTO targetSystemStrict)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Write, evaluateData => this.SaveTargetSystemInternal(targetSystemStrict, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.TargetSystemIdentityDTO SaveTargetSystemInternal(Framework.Configuration.Generated.DTO.TargetSystemStrictDTO targetSystemStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ITargetSystemBLL bll = evaluateData.Context.Logics.TargetSystemFactory.Create(Framework.SecuritySystem.SecurityRule.Edit);
            return this.SaveTargetSystemInternal(targetSystemStrict, evaluateData, bll);
        }
        
        protected virtual Framework.Configuration.Generated.DTO.TargetSystemIdentityDTO SaveTargetSystemInternal(Framework.Configuration.Generated.DTO.TargetSystemStrictDTO targetSystemStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData, Framework.Configuration.BLL.ITargetSystemBLL bll)
        {
            Framework.Configuration.Domain.TargetSystem domainObject = bll.GetById(targetSystemStrict.Id, true);
            targetSystemStrict.MapToDomainObject(evaluateData.MappingService, domainObject);
            bll.Save(domainObject);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToIdentityDTO(domainObject);
        }
    }
}
