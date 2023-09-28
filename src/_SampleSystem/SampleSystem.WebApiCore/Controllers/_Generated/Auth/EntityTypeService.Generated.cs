namespace Authorization.WebApi.Controllers
{
    using Framework.Authorization.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("authApi/v{version:apiVersion}/[controller]")]
    public partial class EntityTypeController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService>>
    {
        
        /// <summary>
        /// Check EntityType access
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CheckEntityTypeAccess")]
        public virtual void CheckEntityTypeAccess(CheckEntityTypeAccessAutoRequest checkEntityTypeAccessAutoRequest)
        {
            string securityOperationName = checkEntityTypeAccessAutoRequest.securityOperationName;
            Framework.Authorization.Generated.DTO.EntityTypeIdentityDTO entityTypeIdent = checkEntityTypeAccessAutoRequest.entityTypeIdent;
            this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.CheckEntityTypeAccessInternal(entityTypeIdent, securityOperationName, evaluateData));
        }
        
        protected virtual void CheckEntityTypeAccessInternal(Framework.Authorization.Generated.DTO.EntityTypeIdentityDTO entityTypeIdent, string securityOperationName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IEntityTypeBLL bll = evaluateData.Context.Logics.EntityType;
            Framework.SecuritySystem.SecurityOperation operation = evaluateData.Context.Authorization.SecurityOperationParser.Parse(securityOperationName.ToString());
            Framework.Authorization.Domain.EntityType domainObject = bll.GetById(entityTypeIdent.Id, true);
            Framework.SecuritySystem.SecurityProviderBaseExtensions.CheckAccess(evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Authorization.Domain.EntityType>(operation), domainObject, evaluateData.Context.AccessDeniedExceptionService);
        }
        
        /// <summary>
        /// Get EntityType (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullEntityType")]
        public virtual Framework.Authorization.Generated.DTO.EntityTypeFullDTO GetFullEntityType([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.EntityTypeIdentityDTO entityTypeIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullEntityTypeInternal(entityTypeIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get EntityType (FullDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullEntityTypeByName")]
        public virtual Framework.Authorization.Generated.DTO.EntityTypeFullDTO GetFullEntityTypeByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string entityTypeName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullEntityTypeByNameInternal(entityTypeName, evaluateData));
        }
        
        protected virtual Framework.Authorization.Generated.DTO.EntityTypeFullDTO GetFullEntityTypeByNameInternal(string entityTypeName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IEntityTypeBLL bll = evaluateData.Context.Logics.EntityTypeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Authorization.Domain.EntityType domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, entityTypeName, true, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.EntityType>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Authorization.Generated.DTO.EntityTypeFullDTO GetFullEntityTypeInternal(Framework.Authorization.Generated.DTO.EntityTypeIdentityDTO entityTypeIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IEntityTypeBLL bll = evaluateData.Context.Logics.EntityTypeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Authorization.Domain.EntityType domainObject = bll.GetById(entityTypeIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.EntityType>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of EntityTypes (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullEntityTypes")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.EntityTypeFullDTO> GetFullEntityTypes()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullEntityTypesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get EntityTypes (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullEntityTypesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.EntityTypeFullDTO> GetFullEntityTypesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.EntityTypeIdentityDTO[] entityTypeIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullEntityTypesByIdentsInternal(entityTypeIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.EntityTypeFullDTO> GetFullEntityTypesByIdentsInternal(Framework.Authorization.Generated.DTO.EntityTypeIdentityDTO[] entityTypeIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IEntityTypeBLL bll = evaluateData.Context.Logics.EntityTypeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(entityTypeIdents, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.EntityType>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get EntityTypes (FullDTO) by filter (Framework.Authorization.Domain.EntityTypeRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullEntityTypesByRootFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.EntityTypeFullDTO> GetFullEntityTypesByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.EntityTypeRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullEntityTypesByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.EntityTypeFullDTO> GetFullEntityTypesByRootFilterInternal(Framework.Authorization.Generated.DTO.EntityTypeRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IEntityTypeBLL bll = evaluateData.Context.Logics.EntityTypeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Authorization.Domain.EntityTypeRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.EntityType>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.EntityTypeFullDTO> GetFullEntityTypesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IEntityTypeBLL bll = evaluateData.Context.Logics.EntityTypeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.EntityType>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get EntityType (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichEntityType")]
        public virtual Framework.Authorization.Generated.DTO.EntityTypeRichDTO GetRichEntityType([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.EntityTypeIdentityDTO entityTypeIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichEntityTypeInternal(entityTypeIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get EntityType (RichDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichEntityTypeByName")]
        public virtual Framework.Authorization.Generated.DTO.EntityTypeRichDTO GetRichEntityTypeByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string entityTypeName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichEntityTypeByNameInternal(entityTypeName, evaluateData));
        }
        
        protected virtual Framework.Authorization.Generated.DTO.EntityTypeRichDTO GetRichEntityTypeByNameInternal(string entityTypeName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IEntityTypeBLL bll = evaluateData.Context.Logics.EntityTypeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Authorization.Domain.EntityType domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, entityTypeName, true, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.EntityType>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Authorization.Generated.DTO.EntityTypeRichDTO GetRichEntityTypeInternal(Framework.Authorization.Generated.DTO.EntityTypeIdentityDTO entityTypeIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IEntityTypeBLL bll = evaluateData.Context.Logics.EntityTypeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Authorization.Domain.EntityType domainObject = bll.GetById(entityTypeIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.EntityType>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get EntityType (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleEntityType")]
        public virtual Framework.Authorization.Generated.DTO.EntityTypeSimpleDTO GetSimpleEntityType([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.EntityTypeIdentityDTO entityTypeIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleEntityTypeInternal(entityTypeIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get EntityType (SimpleDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleEntityTypeByName")]
        public virtual Framework.Authorization.Generated.DTO.EntityTypeSimpleDTO GetSimpleEntityTypeByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string entityTypeName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleEntityTypeByNameInternal(entityTypeName, evaluateData));
        }
        
        protected virtual Framework.Authorization.Generated.DTO.EntityTypeSimpleDTO GetSimpleEntityTypeByNameInternal(string entityTypeName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IEntityTypeBLL bll = evaluateData.Context.Logics.EntityTypeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Authorization.Domain.EntityType domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, entityTypeName, true, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.EntityType>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Authorization.Generated.DTO.EntityTypeSimpleDTO GetSimpleEntityTypeInternal(Framework.Authorization.Generated.DTO.EntityTypeIdentityDTO entityTypeIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IEntityTypeBLL bll = evaluateData.Context.Logics.EntityTypeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Authorization.Domain.EntityType domainObject = bll.GetById(entityTypeIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.EntityType>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of EntityTypes (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleEntityTypes")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.EntityTypeSimpleDTO> GetSimpleEntityTypes()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleEntityTypesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get EntityTypes (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleEntityTypesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.EntityTypeSimpleDTO> GetSimpleEntityTypesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.EntityTypeIdentityDTO[] entityTypeIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleEntityTypesByIdentsInternal(entityTypeIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.EntityTypeSimpleDTO> GetSimpleEntityTypesByIdentsInternal(Framework.Authorization.Generated.DTO.EntityTypeIdentityDTO[] entityTypeIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IEntityTypeBLL bll = evaluateData.Context.Logics.EntityTypeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(entityTypeIdents, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.EntityType>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get EntityTypes (SimpleDTO) by filter (Framework.Authorization.Domain.EntityTypeRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleEntityTypesByRootFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.EntityTypeSimpleDTO> GetSimpleEntityTypesByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.EntityTypeRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleEntityTypesByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.EntityTypeSimpleDTO> GetSimpleEntityTypesByRootFilterInternal(Framework.Authorization.Generated.DTO.EntityTypeRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IEntityTypeBLL bll = evaluateData.Context.Logics.EntityTypeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Authorization.Domain.EntityTypeRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.EntityType>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.EntityTypeSimpleDTO> GetSimpleEntityTypesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IEntityTypeBLL bll = evaluateData.Context.Logics.EntityTypeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.EntityType>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get EntityType (VisualDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualEntityType")]
        public virtual Framework.Authorization.Generated.DTO.EntityTypeVisualDTO GetVisualEntityType([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.EntityTypeIdentityDTO entityTypeIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualEntityTypeInternal(entityTypeIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get EntityType (VisualDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualEntityTypeByName")]
        public virtual Framework.Authorization.Generated.DTO.EntityTypeVisualDTO GetVisualEntityTypeByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string entityTypeName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualEntityTypeByNameInternal(entityTypeName, evaluateData));
        }
        
        protected virtual Framework.Authorization.Generated.DTO.EntityTypeVisualDTO GetVisualEntityTypeByNameInternal(string entityTypeName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IEntityTypeBLL bll = evaluateData.Context.Logics.EntityTypeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Authorization.Domain.EntityType domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, entityTypeName, true, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.EntityType>(Framework.Transfering.ViewDTOType.VisualDTO));
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Authorization.Generated.DTO.EntityTypeVisualDTO GetVisualEntityTypeInternal(Framework.Authorization.Generated.DTO.EntityTypeIdentityDTO entityTypeIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IEntityTypeBLL bll = evaluateData.Context.Logics.EntityTypeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Authorization.Domain.EntityType domainObject = bll.GetById(entityTypeIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.EntityType>(Framework.Transfering.ViewDTOType.VisualDTO));
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of EntityTypes (VisualDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualEntityTypes")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.EntityTypeVisualDTO> GetVisualEntityTypes()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualEntityTypesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get EntityTypes (VisualDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualEntityTypesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.EntityTypeVisualDTO> GetVisualEntityTypesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.EntityTypeIdentityDTO[] entityTypeIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualEntityTypesByIdentsInternal(entityTypeIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.EntityTypeVisualDTO> GetVisualEntityTypesByIdentsInternal(Framework.Authorization.Generated.DTO.EntityTypeIdentityDTO[] entityTypeIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IEntityTypeBLL bll = evaluateData.Context.Logics.EntityTypeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListByIdents(entityTypeIdents, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.EntityType>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get EntityTypes (VisualDTO) by filter (Framework.Authorization.Domain.EntityTypeRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualEntityTypesByRootFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.EntityTypeVisualDTO> GetVisualEntityTypesByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Authorization.Generated.DTO.EntityTypeRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualEntityTypesByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.EntityTypeVisualDTO> GetVisualEntityTypesByRootFilterInternal(Framework.Authorization.Generated.DTO.EntityTypeRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IEntityTypeBLL bll = evaluateData.Context.Logics.EntityTypeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Authorization.Domain.EntityTypeRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.EntityType>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Authorization.Generated.DTO.EntityTypeVisualDTO> GetVisualEntityTypesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IEntityTypeBLL bll = evaluateData.Context.Logics.EntityTypeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Authorization.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Authorization.Domain.EntityType>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Check access for EntityType
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("HasEntityTypeAccess")]
        public virtual bool HasEntityTypeAccess(HasEntityTypeAccessAutoRequest hasEntityTypeAccessAutoRequest)
        {
            string securityOperationName = hasEntityTypeAccessAutoRequest.securityOperationName;
            Framework.Authorization.Generated.DTO.EntityTypeIdentityDTO entityTypeIdent = hasEntityTypeAccessAutoRequest.entityTypeIdent;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.HasEntityTypeAccessInternal(entityTypeIdent, securityOperationName, evaluateData));
        }
        
        protected virtual bool HasEntityTypeAccessInternal(Framework.Authorization.Generated.DTO.EntityTypeIdentityDTO entityTypeIdent, string securityOperationName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Generated.DTO.IAuthorizationDTOMappingService> evaluateData)
        {
            Framework.Authorization.BLL.IEntityTypeBLL bll = evaluateData.Context.Logics.EntityType;
            Framework.SecuritySystem.SecurityOperation operation = evaluateData.Context.Authorization.SecurityOperationParser.Parse(securityOperationName.ToString());
            Framework.Authorization.Domain.EntityType domainObject = bll.GetById(entityTypeIdent.Id, true);
            return evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Authorization.Domain.EntityType>(operation).HasAccess(domainObject);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class CheckEntityTypeAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Authorization.Generated.DTO.EntityTypeIdentityDTO entityTypeIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string securityOperationName;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class HasEntityTypeAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Authorization.Generated.DTO.EntityTypeIdentityDTO entityTypeIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string securityOperationName;
    }
}
