namespace Configuration.WebApi.Controllers
{
    using Framework.Configuration.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("configApi/v{version:apiVersion}/[controller]")]
    public partial class TargetSystemController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService>>
    {
        
        /// <summary>
        /// Check TargetSystem access
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CheckTargetSystemAccess")]
        public virtual void CheckTargetSystemAccess(CheckTargetSystemAccessAutoRequest checkTargetSystemAccessAutoRequest)
        {
            Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode = checkTargetSystemAccessAutoRequest.securityOperationCode;
            Framework.Configuration.Generated.DTO.TargetSystemIdentityDTO targetSystemIdent = checkTargetSystemAccessAutoRequest.targetSystemIdent;
            this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.CheckTargetSystemAccessInternal(targetSystemIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual void CheckTargetSystemAccessInternal(Framework.Configuration.Generated.DTO.TargetSystemIdentityDTO targetSystemIdent, Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ITargetSystemBLL bll = evaluateData.Context.Logics.TargetSystem;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            Framework.Configuration.Domain.TargetSystem domainObject = bll.GetById(targetSystemIdent.Id, true);
            Framework.SecuritySystem.SecurityProviderExtensions.CheckAccess(evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Configuration.Domain.TargetSystem>(securityOperationCode), domainObject);
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, Framework.Configuration.BLL.IConfigurationBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService>(session, context, new ConfigurationServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get TargetSystem (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullTargetSystem")]
        public virtual Framework.Configuration.Generated.DTO.TargetSystemFullDTO GetFullTargetSystem([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.TargetSystemIdentityDTO targetSystemIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullTargetSystemInternal(targetSystemIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get TargetSystem (FullDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullTargetSystemByName")]
        public virtual Framework.Configuration.Generated.DTO.TargetSystemFullDTO GetFullTargetSystemByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string targetSystemName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullTargetSystemByNameInternal(targetSystemName, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.TargetSystemFullDTO GetFullTargetSystemByNameInternal(string targetSystemName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ITargetSystemBLL bll = evaluateData.Context.Logics.TargetSystemFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.TargetSystem domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, targetSystemName, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.TargetSystem>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Configuration.Generated.DTO.TargetSystemFullDTO GetFullTargetSystemInternal(Framework.Configuration.Generated.DTO.TargetSystemIdentityDTO targetSystemIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ITargetSystemBLL bll = evaluateData.Context.Logics.TargetSystemFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.TargetSystem domainObject = bll.GetById(targetSystemIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.TargetSystem>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of TargetSystems (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullTargetSystems")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.TargetSystemFullDTO> GetFullTargetSystems()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullTargetSystemsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get TargetSystems (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullTargetSystemsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.TargetSystemFullDTO> GetFullTargetSystemsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.TargetSystemIdentityDTO[] targetSystemIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullTargetSystemsByIdentsInternal(targetSystemIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.TargetSystemFullDTO> GetFullTargetSystemsByIdentsInternal(Framework.Configuration.Generated.DTO.TargetSystemIdentityDTO[] targetSystemIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ITargetSystemBLL bll = evaluateData.Context.Logics.TargetSystemFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(targetSystemIdents, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.TargetSystem>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TargetSystems (FullDTO) by filter (Framework.Configuration.Domain.TargetSystemRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullTargetSystemsByRootFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.TargetSystemFullDTO> GetFullTargetSystemsByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.TargetSystemRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullTargetSystemsByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.TargetSystemFullDTO> GetFullTargetSystemsByRootFilterInternal(Framework.Configuration.Generated.DTO.TargetSystemRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ITargetSystemBLL bll = evaluateData.Context.Logics.TargetSystemFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.TargetSystemRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.TargetSystem>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.TargetSystemFullDTO> GetFullTargetSystemsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ITargetSystemBLL bll = evaluateData.Context.Logics.TargetSystemFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.TargetSystem>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TargetSystem (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichTargetSystem")]
        public virtual Framework.Configuration.Generated.DTO.TargetSystemRichDTO GetRichTargetSystem([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.TargetSystemIdentityDTO targetSystemIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichTargetSystemInternal(targetSystemIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get TargetSystem (RichDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichTargetSystemByName")]
        public virtual Framework.Configuration.Generated.DTO.TargetSystemRichDTO GetRichTargetSystemByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string targetSystemName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichTargetSystemByNameInternal(targetSystemName, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.TargetSystemRichDTO GetRichTargetSystemByNameInternal(string targetSystemName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ITargetSystemBLL bll = evaluateData.Context.Logics.TargetSystemFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.TargetSystem domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, targetSystemName, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.TargetSystem>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Configuration.Generated.DTO.TargetSystemRichDTO GetRichTargetSystemInternal(Framework.Configuration.Generated.DTO.TargetSystemIdentityDTO targetSystemIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ITargetSystemBLL bll = evaluateData.Context.Logics.TargetSystemFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.TargetSystem domainObject = bll.GetById(targetSystemIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.TargetSystem>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TargetSystem (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleTargetSystem")]
        public virtual Framework.Configuration.Generated.DTO.TargetSystemSimpleDTO GetSimpleTargetSystem([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.TargetSystemIdentityDTO targetSystemIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleTargetSystemInternal(targetSystemIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get TargetSystem (SimpleDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleTargetSystemByName")]
        public virtual Framework.Configuration.Generated.DTO.TargetSystemSimpleDTO GetSimpleTargetSystemByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string targetSystemName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleTargetSystemByNameInternal(targetSystemName, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.TargetSystemSimpleDTO GetSimpleTargetSystemByNameInternal(string targetSystemName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ITargetSystemBLL bll = evaluateData.Context.Logics.TargetSystemFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.TargetSystem domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, targetSystemName, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.TargetSystem>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Configuration.Generated.DTO.TargetSystemSimpleDTO GetSimpleTargetSystemInternal(Framework.Configuration.Generated.DTO.TargetSystemIdentityDTO targetSystemIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ITargetSystemBLL bll = evaluateData.Context.Logics.TargetSystemFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.TargetSystem domainObject = bll.GetById(targetSystemIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.TargetSystem>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of TargetSystems (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleTargetSystems")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.TargetSystemSimpleDTO> GetSimpleTargetSystems()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleTargetSystemsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get TargetSystems (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleTargetSystemsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.TargetSystemSimpleDTO> GetSimpleTargetSystemsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.TargetSystemIdentityDTO[] targetSystemIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleTargetSystemsByIdentsInternal(targetSystemIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.TargetSystemSimpleDTO> GetSimpleTargetSystemsByIdentsInternal(Framework.Configuration.Generated.DTO.TargetSystemIdentityDTO[] targetSystemIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ITargetSystemBLL bll = evaluateData.Context.Logics.TargetSystemFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(targetSystemIdents, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.TargetSystem>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TargetSystems (SimpleDTO) by filter (Framework.Configuration.Domain.TargetSystemRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleTargetSystemsByRootFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.TargetSystemSimpleDTO> GetSimpleTargetSystemsByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.TargetSystemRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleTargetSystemsByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.TargetSystemSimpleDTO> GetSimpleTargetSystemsByRootFilterInternal(Framework.Configuration.Generated.DTO.TargetSystemRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ITargetSystemBLL bll = evaluateData.Context.Logics.TargetSystemFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.TargetSystemRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.TargetSystem>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.TargetSystemSimpleDTO> GetSimpleTargetSystemsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ITargetSystemBLL bll = evaluateData.Context.Logics.TargetSystemFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.TargetSystem>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TargetSystem (VisualDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualTargetSystem")]
        public virtual Framework.Configuration.Generated.DTO.TargetSystemVisualDTO GetVisualTargetSystem([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.TargetSystemIdentityDTO targetSystemIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualTargetSystemInternal(targetSystemIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get TargetSystem (VisualDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualTargetSystemByName")]
        public virtual Framework.Configuration.Generated.DTO.TargetSystemVisualDTO GetVisualTargetSystemByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string targetSystemName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualTargetSystemByNameInternal(targetSystemName, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.TargetSystemVisualDTO GetVisualTargetSystemByNameInternal(string targetSystemName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ITargetSystemBLL bll = evaluateData.Context.Logics.TargetSystemFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.TargetSystem domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, targetSystemName, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.TargetSystem>(Framework.Transfering.ViewDTOType.VisualDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Configuration.Generated.DTO.TargetSystemVisualDTO GetVisualTargetSystemInternal(Framework.Configuration.Generated.DTO.TargetSystemIdentityDTO targetSystemIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ITargetSystemBLL bll = evaluateData.Context.Logics.TargetSystemFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.TargetSystem domainObject = bll.GetById(targetSystemIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.TargetSystem>(Framework.Transfering.ViewDTOType.VisualDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of TargetSystems (VisualDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualTargetSystems")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.TargetSystemVisualDTO> GetVisualTargetSystems()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualTargetSystemsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get TargetSystems (VisualDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualTargetSystemsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.TargetSystemVisualDTO> GetVisualTargetSystemsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.TargetSystemIdentityDTO[] targetSystemIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualTargetSystemsByIdentsInternal(targetSystemIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.TargetSystemVisualDTO> GetVisualTargetSystemsByIdentsInternal(Framework.Configuration.Generated.DTO.TargetSystemIdentityDTO[] targetSystemIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ITargetSystemBLL bll = evaluateData.Context.Logics.TargetSystemFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListByIdents(targetSystemIdents, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.TargetSystem>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TargetSystems (VisualDTO) by filter (Framework.Configuration.Domain.TargetSystemRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualTargetSystemsByRootFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.TargetSystemVisualDTO> GetVisualTargetSystemsByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.TargetSystemRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualTargetSystemsByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.TargetSystemVisualDTO> GetVisualTargetSystemsByRootFilterInternal(Framework.Configuration.Generated.DTO.TargetSystemRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ITargetSystemBLL bll = evaluateData.Context.Logics.TargetSystemFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.TargetSystemRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.TargetSystem>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.TargetSystemVisualDTO> GetVisualTargetSystemsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ITargetSystemBLL bll = evaluateData.Context.Logics.TargetSystemFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.TargetSystem>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Check access for TargetSystem
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("HasTargetSystemAccess")]
        public virtual bool HasTargetSystemAccess(HasTargetSystemAccessAutoRequest hasTargetSystemAccessAutoRequest)
        {
            Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode = hasTargetSystemAccessAutoRequest.securityOperationCode;
            Framework.Configuration.Generated.DTO.TargetSystemIdentityDTO targetSystemIdent = hasTargetSystemAccessAutoRequest.targetSystemIdent;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.HasTargetSystemAccessInternal(targetSystemIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual bool HasTargetSystemAccessInternal(Framework.Configuration.Generated.DTO.TargetSystemIdentityDTO targetSystemIdent, Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ITargetSystemBLL bll = evaluateData.Context.Logics.TargetSystem;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            Framework.Configuration.Domain.TargetSystem domainObject = bll.GetById(targetSystemIdent.Id, true);
            return evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Configuration.Domain.TargetSystem>(securityOperationCode).HasAccess(domainObject);
        }
        
        /// <summary>
        /// Save TargetSystems
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("SaveTargetSystem")]
        public virtual Framework.Configuration.Generated.DTO.TargetSystemIdentityDTO SaveTargetSystem([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.TargetSystemStrictDTO targetSystemStrict)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Write, evaluateData => this.SaveTargetSystemInternal(targetSystemStrict, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.TargetSystemIdentityDTO SaveTargetSystemInternal(Framework.Configuration.Generated.DTO.TargetSystemStrictDTO targetSystemStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ITargetSystemBLL bll = evaluateData.Context.Logics.TargetSystemFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
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
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class CheckTargetSystemAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Configuration.Generated.DTO.TargetSystemIdentityDTO targetSystemIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class HasTargetSystemAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Configuration.Generated.DTO.TargetSystemIdentityDTO targetSystemIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode;
    }
}
