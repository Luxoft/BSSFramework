﻿namespace Configuration.WebApi.Controllers
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("configApi/[controller]/[action]")]
    public partial class DomainTypeController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService>
    {
        
        /// <summary>
        /// Get DomainType (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.Configuration.Generated.DTO.DomainTypeFullDTO GetFullDomainType([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.DomainTypeIdentityDTO domainTypeIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullDomainTypeInternal(domainTypeIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get DomainType (FullDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.Configuration.Generated.DTO.DomainTypeFullDTO GetFullDomainTypeByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string domainTypeName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullDomainTypeByNameInternal(domainTypeName, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.DomainTypeFullDTO GetFullDomainTypeByNameInternal(string domainTypeName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IDomainTypeBLL bll = evaluateData.Context.Logics.DomainTypeFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.Configuration.Domain.DomainType domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, domainTypeName, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.DomainType>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Configuration.Generated.DTO.DomainTypeFullDTO GetFullDomainTypeInternal(Framework.Configuration.Generated.DTO.DomainTypeIdentityDTO domainTypeIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IDomainTypeBLL bll = evaluateData.Context.Logics.DomainTypeFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.Configuration.Domain.DomainType domainObject = bll.GetById(domainTypeIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.DomainType>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of DomainTypes (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.DomainTypeFullDTO> GetFullDomainTypes()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullDomainTypesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get DomainTypes (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.DomainTypeFullDTO> GetFullDomainTypesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.DomainTypeIdentityDTO[] domainTypeIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullDomainTypesByIdentsInternal(domainTypeIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.DomainTypeFullDTO> GetFullDomainTypesByIdentsInternal(Framework.Configuration.Generated.DTO.DomainTypeIdentityDTO[] domainTypeIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IDomainTypeBLL bll = evaluateData.Context.Logics.DomainTypeFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(domainTypeIdents, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.DomainType>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get DomainTypes (FullDTO) by filter (Framework.Configuration.Domain.DomainTypeRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.DomainTypeFullDTO> GetFullDomainTypesByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.DomainTypeRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullDomainTypesByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.DomainTypeFullDTO> GetFullDomainTypesByRootFilterInternal(Framework.Configuration.Generated.DTO.DomainTypeRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IDomainTypeBLL bll = evaluateData.Context.Logics.DomainTypeFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.Configuration.Domain.DomainTypeRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.DomainType>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.DomainTypeFullDTO> GetFullDomainTypesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IDomainTypeBLL bll = evaluateData.Context.Logics.DomainTypeFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.DomainType>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get DomainType (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.Configuration.Generated.DTO.DomainTypeRichDTO GetRichDomainType([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.DomainTypeIdentityDTO domainTypeIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichDomainTypeInternal(domainTypeIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get DomainType (RichDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.Configuration.Generated.DTO.DomainTypeRichDTO GetRichDomainTypeByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string domainTypeName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichDomainTypeByNameInternal(domainTypeName, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.DomainTypeRichDTO GetRichDomainTypeByNameInternal(string domainTypeName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IDomainTypeBLL bll = evaluateData.Context.Logics.DomainTypeFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.Configuration.Domain.DomainType domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, domainTypeName, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.DomainType>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Configuration.Generated.DTO.DomainTypeRichDTO GetRichDomainTypeInternal(Framework.Configuration.Generated.DTO.DomainTypeIdentityDTO domainTypeIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IDomainTypeBLL bll = evaluateData.Context.Logics.DomainTypeFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.Configuration.Domain.DomainType domainObject = bll.GetById(domainTypeIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.DomainType>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get DomainType (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.Configuration.Generated.DTO.DomainTypeSimpleDTO GetSimpleDomainType([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.DomainTypeIdentityDTO domainTypeIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleDomainTypeInternal(domainTypeIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get DomainType (SimpleDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.Configuration.Generated.DTO.DomainTypeSimpleDTO GetSimpleDomainTypeByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string domainTypeName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleDomainTypeByNameInternal(domainTypeName, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.DomainTypeSimpleDTO GetSimpleDomainTypeByNameInternal(string domainTypeName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IDomainTypeBLL bll = evaluateData.Context.Logics.DomainTypeFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.Configuration.Domain.DomainType domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, domainTypeName, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.DomainType>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Configuration.Generated.DTO.DomainTypeSimpleDTO GetSimpleDomainTypeInternal(Framework.Configuration.Generated.DTO.DomainTypeIdentityDTO domainTypeIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IDomainTypeBLL bll = evaluateData.Context.Logics.DomainTypeFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.Configuration.Domain.DomainType domainObject = bll.GetById(domainTypeIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.DomainType>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of DomainTypes (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.DomainTypeSimpleDTO> GetSimpleDomainTypes()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleDomainTypesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get DomainTypes (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.DomainTypeSimpleDTO> GetSimpleDomainTypesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.DomainTypeIdentityDTO[] domainTypeIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleDomainTypesByIdentsInternal(domainTypeIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.DomainTypeSimpleDTO> GetSimpleDomainTypesByIdentsInternal(Framework.Configuration.Generated.DTO.DomainTypeIdentityDTO[] domainTypeIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IDomainTypeBLL bll = evaluateData.Context.Logics.DomainTypeFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(domainTypeIdents, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.DomainType>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get DomainTypes (SimpleDTO) by filter (Framework.Configuration.Domain.DomainTypeRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.DomainTypeSimpleDTO> GetSimpleDomainTypesByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.DomainTypeRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleDomainTypesByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.DomainTypeSimpleDTO> GetSimpleDomainTypesByRootFilterInternal(Framework.Configuration.Generated.DTO.DomainTypeRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IDomainTypeBLL bll = evaluateData.Context.Logics.DomainTypeFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.Configuration.Domain.DomainTypeRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.DomainType>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.DomainTypeSimpleDTO> GetSimpleDomainTypesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IDomainTypeBLL bll = evaluateData.Context.Logics.DomainTypeFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.DomainType>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get DomainType (VisualDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.Configuration.Generated.DTO.DomainTypeVisualDTO GetVisualDomainType([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.DomainTypeIdentityDTO domainTypeIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualDomainTypeInternal(domainTypeIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get DomainType (VisualDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.Configuration.Generated.DTO.DomainTypeVisualDTO GetVisualDomainTypeByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string domainTypeName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualDomainTypeByNameInternal(domainTypeName, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.DomainTypeVisualDTO GetVisualDomainTypeByNameInternal(string domainTypeName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IDomainTypeBLL bll = evaluateData.Context.Logics.DomainTypeFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.Configuration.Domain.DomainType domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, domainTypeName, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.DomainType>(Framework.Transfering.ViewDTOType.VisualDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Configuration.Generated.DTO.DomainTypeVisualDTO GetVisualDomainTypeInternal(Framework.Configuration.Generated.DTO.DomainTypeIdentityDTO domainTypeIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IDomainTypeBLL bll = evaluateData.Context.Logics.DomainTypeFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.Configuration.Domain.DomainType domainObject = bll.GetById(domainTypeIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.DomainType>(Framework.Transfering.ViewDTOType.VisualDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of DomainTypes (VisualDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.DomainTypeVisualDTO> GetVisualDomainTypes()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualDomainTypesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get DomainTypes (VisualDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.DomainTypeVisualDTO> GetVisualDomainTypesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.DomainTypeIdentityDTO[] domainTypeIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualDomainTypesByIdentsInternal(domainTypeIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.DomainTypeVisualDTO> GetVisualDomainTypesByIdentsInternal(Framework.Configuration.Generated.DTO.DomainTypeIdentityDTO[] domainTypeIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IDomainTypeBLL bll = evaluateData.Context.Logics.DomainTypeFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListByIdents(domainTypeIdents, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.DomainType>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get DomainTypes (VisualDTO) by filter (Framework.Configuration.Domain.DomainTypeRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.DomainTypeVisualDTO> GetVisualDomainTypesByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.DomainTypeRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualDomainTypesByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.DomainTypeVisualDTO> GetVisualDomainTypesByRootFilterInternal(Framework.Configuration.Generated.DTO.DomainTypeRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IDomainTypeBLL bll = evaluateData.Context.Logics.DomainTypeFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.Configuration.Domain.DomainTypeRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.DomainType>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.DomainTypeVisualDTO> GetVisualDomainTypesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IDomainTypeBLL bll = evaluateData.Context.Logics.DomainTypeFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.DomainType>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
    }
}
