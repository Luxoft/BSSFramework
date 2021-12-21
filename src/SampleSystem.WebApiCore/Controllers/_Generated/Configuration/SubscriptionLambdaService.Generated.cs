namespace Configuration.WebApi.Controllers
{
    using Framework.Configuration.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("configApi/v{version:apiVersion}/[controller]")]
    public partial class SubscriptionLambdaController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<Framework.Configuration.BLL.IConfigurationBLLContext>, Framework.Configuration.BLL.IConfigurationBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService>>
    {
        
        public SubscriptionLambdaController(Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<Framework.Configuration.BLL.IConfigurationBLLContext> serviceEnvironment, Framework.Exceptions.IExceptionProcessor exceptionProcessor) : 
                base(serviceEnvironment, exceptionProcessor)
        {
        }
        
        /// <summary>
        /// Check SubscriptionLambda access
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CheckSubscriptionLambdaAccess")]
        public virtual void CheckSubscriptionLambdaAccess(CheckSubscriptionLambdaAccessAutoRequest checkSubscriptionLambdaAccessAutoRequest)
        {
            Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode = checkSubscriptionLambdaAccessAutoRequest.securityOperationCode;
            Framework.Configuration.Generated.DTO.SubscriptionLambdaIdentityDTO subscriptionLambdaIdent = checkSubscriptionLambdaAccessAutoRequest.subscriptionLambdaIdent;
            this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.CheckSubscriptionLambdaAccessInternal(subscriptionLambdaIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual void CheckSubscriptionLambdaAccessInternal(Framework.Configuration.Generated.DTO.SubscriptionLambdaIdentityDTO subscriptionLambdaIdent, Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISubscriptionLambdaBLL bll = evaluateData.Context.Logics.SubscriptionLambda;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            Framework.Configuration.Domain.SubscriptionLambda domainObject = bll.GetById(subscriptionLambdaIdent.Id, true);
            Framework.SecuritySystem.SecurityProviderExtensions.CheckAccess(evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Configuration.Domain.SubscriptionLambda>(securityOperationCode), domainObject);
        }
        
        /// <summary>
        /// Create SubscriptionLambda by model (SubscriptionLambdaCreateModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CreateSubscriptionLambda")]
        public virtual Framework.Configuration.Generated.DTO.SubscriptionLambdaRichDTO CreateSubscriptionLambda([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SubscriptionLambdaCreateModelStrictDTO subscriptionLambdaCreateModel)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.CreateSubscriptionLambdaInternal(subscriptionLambdaCreateModel, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.SubscriptionLambdaRichDTO CreateSubscriptionLambdaInternal(Framework.Configuration.Generated.DTO.SubscriptionLambdaCreateModelStrictDTO subscriptionLambdaCreateModel, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISubscriptionLambdaBLL bll = evaluateData.Context.Logics.SubscriptionLambdaFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            Framework.Configuration.Domain.SubscriptionLambdaCreateModel createModel = subscriptionLambdaCreateModel.ToDomainObject(evaluateData.MappingService);
            Framework.Configuration.Domain.SubscriptionLambda domainObject = bll.Create(createModel);
            bll.CheckAccess(domainObject);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, Framework.Configuration.BLL.IConfigurationBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService>(session, context, new ConfigurationServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get SubscriptionLambda (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullSubscriptionLambda")]
        public virtual Framework.Configuration.Generated.DTO.SubscriptionLambdaFullDTO GetFullSubscriptionLambda([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SubscriptionLambdaIdentityDTO subscriptionLambdaIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullSubscriptionLambdaInternal(subscriptionLambdaIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get SubscriptionLambda (FullDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullSubscriptionLambdaByName")]
        public virtual Framework.Configuration.Generated.DTO.SubscriptionLambdaFullDTO GetFullSubscriptionLambdaByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string subscriptionLambdaName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullSubscriptionLambdaByNameInternal(subscriptionLambdaName, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.SubscriptionLambdaFullDTO GetFullSubscriptionLambdaByNameInternal(string subscriptionLambdaName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISubscriptionLambdaBLL bll = evaluateData.Context.Logics.SubscriptionLambdaFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.SubscriptionLambda domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, subscriptionLambdaName, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.SubscriptionLambda>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Configuration.Generated.DTO.SubscriptionLambdaFullDTO GetFullSubscriptionLambdaInternal(Framework.Configuration.Generated.DTO.SubscriptionLambdaIdentityDTO subscriptionLambdaIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISubscriptionLambdaBLL bll = evaluateData.Context.Logics.SubscriptionLambdaFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.SubscriptionLambda domainObject = bll.GetById(subscriptionLambdaIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.SubscriptionLambda>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of SubscriptionLambdas (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullSubscriptionLambdas")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SubscriptionLambdaFullDTO> GetFullSubscriptionLambdas()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullSubscriptionLambdasInternal(evaluateData));
        }
        
        /// <summary>
        /// Get SubscriptionLambdas (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullSubscriptionLambdasByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SubscriptionLambdaFullDTO> GetFullSubscriptionLambdasByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SubscriptionLambdaIdentityDTO[] subscriptionLambdaIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullSubscriptionLambdasByIdentsInternal(subscriptionLambdaIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SubscriptionLambdaFullDTO> GetFullSubscriptionLambdasByIdentsInternal(Framework.Configuration.Generated.DTO.SubscriptionLambdaIdentityDTO[] subscriptionLambdaIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISubscriptionLambdaBLL bll = evaluateData.Context.Logics.SubscriptionLambdaFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(subscriptionLambdaIdents, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.SubscriptionLambda>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get SubscriptionLambdas (FullDTO) by filter (Framework.Configuration.Domain.SubscriptionLambdaRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullSubscriptionLambdasByRootFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SubscriptionLambdaFullDTO> GetFullSubscriptionLambdasByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SubscriptionLambdaRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullSubscriptionLambdasByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SubscriptionLambdaFullDTO> GetFullSubscriptionLambdasByRootFilterInternal(Framework.Configuration.Generated.DTO.SubscriptionLambdaRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISubscriptionLambdaBLL bll = evaluateData.Context.Logics.SubscriptionLambdaFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.SubscriptionLambdaRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.SubscriptionLambda>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SubscriptionLambdaFullDTO> GetFullSubscriptionLambdasInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISubscriptionLambdaBLL bll = evaluateData.Context.Logics.SubscriptionLambdaFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.SubscriptionLambda>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get SubscriptionLambda (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichSubscriptionLambda")]
        public virtual Framework.Configuration.Generated.DTO.SubscriptionLambdaRichDTO GetRichSubscriptionLambda([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SubscriptionLambdaIdentityDTO subscriptionLambdaIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichSubscriptionLambdaInternal(subscriptionLambdaIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get SubscriptionLambda (RichDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichSubscriptionLambdaByName")]
        public virtual Framework.Configuration.Generated.DTO.SubscriptionLambdaRichDTO GetRichSubscriptionLambdaByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string subscriptionLambdaName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichSubscriptionLambdaByNameInternal(subscriptionLambdaName, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.SubscriptionLambdaRichDTO GetRichSubscriptionLambdaByNameInternal(string subscriptionLambdaName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISubscriptionLambdaBLL bll = evaluateData.Context.Logics.SubscriptionLambdaFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.SubscriptionLambda domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, subscriptionLambdaName, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.SubscriptionLambda>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Configuration.Generated.DTO.SubscriptionLambdaRichDTO GetRichSubscriptionLambdaInternal(Framework.Configuration.Generated.DTO.SubscriptionLambdaIdentityDTO subscriptionLambdaIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISubscriptionLambdaBLL bll = evaluateData.Context.Logics.SubscriptionLambdaFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.SubscriptionLambda domainObject = bll.GetById(subscriptionLambdaIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.SubscriptionLambda>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get SubscriptionLambda (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleSubscriptionLambda")]
        public virtual Framework.Configuration.Generated.DTO.SubscriptionLambdaSimpleDTO GetSimpleSubscriptionLambda([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SubscriptionLambdaIdentityDTO subscriptionLambdaIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleSubscriptionLambdaInternal(subscriptionLambdaIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get SubscriptionLambda (SimpleDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleSubscriptionLambdaByName")]
        public virtual Framework.Configuration.Generated.DTO.SubscriptionLambdaSimpleDTO GetSimpleSubscriptionLambdaByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string subscriptionLambdaName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleSubscriptionLambdaByNameInternal(subscriptionLambdaName, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.SubscriptionLambdaSimpleDTO GetSimpleSubscriptionLambdaByNameInternal(string subscriptionLambdaName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISubscriptionLambdaBLL bll = evaluateData.Context.Logics.SubscriptionLambdaFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.SubscriptionLambda domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, subscriptionLambdaName, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.SubscriptionLambda>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Configuration.Generated.DTO.SubscriptionLambdaSimpleDTO GetSimpleSubscriptionLambdaInternal(Framework.Configuration.Generated.DTO.SubscriptionLambdaIdentityDTO subscriptionLambdaIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISubscriptionLambdaBLL bll = evaluateData.Context.Logics.SubscriptionLambdaFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.SubscriptionLambda domainObject = bll.GetById(subscriptionLambdaIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.SubscriptionLambda>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of SubscriptionLambdas (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleSubscriptionLambdas")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SubscriptionLambdaSimpleDTO> GetSimpleSubscriptionLambdas()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleSubscriptionLambdasInternal(evaluateData));
        }
        
        /// <summary>
        /// Get SubscriptionLambdas (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleSubscriptionLambdasByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SubscriptionLambdaSimpleDTO> GetSimpleSubscriptionLambdasByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SubscriptionLambdaIdentityDTO[] subscriptionLambdaIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleSubscriptionLambdasByIdentsInternal(subscriptionLambdaIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SubscriptionLambdaSimpleDTO> GetSimpleSubscriptionLambdasByIdentsInternal(Framework.Configuration.Generated.DTO.SubscriptionLambdaIdentityDTO[] subscriptionLambdaIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISubscriptionLambdaBLL bll = evaluateData.Context.Logics.SubscriptionLambdaFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(subscriptionLambdaIdents, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.SubscriptionLambda>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get SubscriptionLambdas (SimpleDTO) by filter (Framework.Configuration.Domain.SubscriptionLambdaRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleSubscriptionLambdasByRootFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SubscriptionLambdaSimpleDTO> GetSimpleSubscriptionLambdasByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SubscriptionLambdaRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleSubscriptionLambdasByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SubscriptionLambdaSimpleDTO> GetSimpleSubscriptionLambdasByRootFilterInternal(Framework.Configuration.Generated.DTO.SubscriptionLambdaRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISubscriptionLambdaBLL bll = evaluateData.Context.Logics.SubscriptionLambdaFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.SubscriptionLambdaRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.SubscriptionLambda>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SubscriptionLambdaSimpleDTO> GetSimpleSubscriptionLambdasInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISubscriptionLambdaBLL bll = evaluateData.Context.Logics.SubscriptionLambdaFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.SubscriptionLambda>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get SubscriptionLambda (VisualDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualSubscriptionLambda")]
        public virtual Framework.Configuration.Generated.DTO.SubscriptionLambdaVisualDTO GetVisualSubscriptionLambda([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SubscriptionLambdaIdentityDTO subscriptionLambdaIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualSubscriptionLambdaInternal(subscriptionLambdaIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get SubscriptionLambda (VisualDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualSubscriptionLambdaByName")]
        public virtual Framework.Configuration.Generated.DTO.SubscriptionLambdaVisualDTO GetVisualSubscriptionLambdaByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string subscriptionLambdaName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualSubscriptionLambdaByNameInternal(subscriptionLambdaName, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.SubscriptionLambdaVisualDTO GetVisualSubscriptionLambdaByNameInternal(string subscriptionLambdaName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISubscriptionLambdaBLL bll = evaluateData.Context.Logics.SubscriptionLambdaFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.SubscriptionLambda domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, subscriptionLambdaName, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.SubscriptionLambda>(Framework.Transfering.ViewDTOType.VisualDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Configuration.Generated.DTO.SubscriptionLambdaVisualDTO GetVisualSubscriptionLambdaInternal(Framework.Configuration.Generated.DTO.SubscriptionLambdaIdentityDTO subscriptionLambdaIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISubscriptionLambdaBLL bll = evaluateData.Context.Logics.SubscriptionLambdaFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.SubscriptionLambda domainObject = bll.GetById(subscriptionLambdaIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.SubscriptionLambda>(Framework.Transfering.ViewDTOType.VisualDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of SubscriptionLambdas (VisualDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualSubscriptionLambdas")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SubscriptionLambdaVisualDTO> GetVisualSubscriptionLambdas()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualSubscriptionLambdasInternal(evaluateData));
        }
        
        /// <summary>
        /// Get SubscriptionLambdas (VisualDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualSubscriptionLambdasByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SubscriptionLambdaVisualDTO> GetVisualSubscriptionLambdasByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SubscriptionLambdaIdentityDTO[] subscriptionLambdaIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualSubscriptionLambdasByIdentsInternal(subscriptionLambdaIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SubscriptionLambdaVisualDTO> GetVisualSubscriptionLambdasByIdentsInternal(Framework.Configuration.Generated.DTO.SubscriptionLambdaIdentityDTO[] subscriptionLambdaIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISubscriptionLambdaBLL bll = evaluateData.Context.Logics.SubscriptionLambdaFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListByIdents(subscriptionLambdaIdents, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.SubscriptionLambda>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get SubscriptionLambdas (VisualDTO) by filter (Framework.Configuration.Domain.SubscriptionLambdaRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualSubscriptionLambdasByRootFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SubscriptionLambdaVisualDTO> GetVisualSubscriptionLambdasByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SubscriptionLambdaRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualSubscriptionLambdasByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SubscriptionLambdaVisualDTO> GetVisualSubscriptionLambdasByRootFilterInternal(Framework.Configuration.Generated.DTO.SubscriptionLambdaRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISubscriptionLambdaBLL bll = evaluateData.Context.Logics.SubscriptionLambdaFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.SubscriptionLambdaRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.SubscriptionLambda>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SubscriptionLambdaVisualDTO> GetVisualSubscriptionLambdasInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISubscriptionLambdaBLL bll = evaluateData.Context.Logics.SubscriptionLambdaFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.SubscriptionLambda>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Check access for SubscriptionLambda
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("HasSubscriptionLambdaAccess")]
        public virtual bool HasSubscriptionLambdaAccess(HasSubscriptionLambdaAccessAutoRequest hasSubscriptionLambdaAccessAutoRequest)
        {
            Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode = hasSubscriptionLambdaAccessAutoRequest.securityOperationCode;
            Framework.Configuration.Generated.DTO.SubscriptionLambdaIdentityDTO subscriptionLambdaIdent = hasSubscriptionLambdaAccessAutoRequest.subscriptionLambdaIdent;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.HasSubscriptionLambdaAccessInternal(subscriptionLambdaIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual bool HasSubscriptionLambdaAccessInternal(Framework.Configuration.Generated.DTO.SubscriptionLambdaIdentityDTO subscriptionLambdaIdent, Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISubscriptionLambdaBLL bll = evaluateData.Context.Logics.SubscriptionLambda;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            Framework.Configuration.Domain.SubscriptionLambda domainObject = bll.GetById(subscriptionLambdaIdent.Id, true);
            return evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Configuration.Domain.SubscriptionLambda>(securityOperationCode).HasAccess(domainObject);
        }
        
        /// <summary>
        /// Remove SubscriptionLambda
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("RemoveSubscriptionLambda")]
        public virtual void RemoveSubscriptionLambda([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SubscriptionLambdaIdentityDTO subscriptionLambdaIdent)
        {
            this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Write, evaluateData => this.RemoveSubscriptionLambdaInternal(subscriptionLambdaIdent, evaluateData));
        }
        
        protected virtual void RemoveSubscriptionLambdaInternal(Framework.Configuration.Generated.DTO.SubscriptionLambdaIdentityDTO subscriptionLambdaIdent, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISubscriptionLambdaBLL bll = evaluateData.Context.Logics.SubscriptionLambdaFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            this.RemoveSubscriptionLambdaInternal(subscriptionLambdaIdent, evaluateData, bll);
        }
        
        protected virtual void RemoveSubscriptionLambdaInternal(Framework.Configuration.Generated.DTO.SubscriptionLambdaIdentityDTO subscriptionLambdaIdent, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData, Framework.Configuration.BLL.ISubscriptionLambdaBLL bll)
        {
            Framework.Configuration.Domain.SubscriptionLambda domainObject = bll.GetById(subscriptionLambdaIdent.Id, true);
            bll.Remove(domainObject);
        }
        
        /// <summary>
        /// Save SubscriptionLambdas
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("SaveSubscriptionLambda")]
        public virtual Framework.Configuration.Generated.DTO.SubscriptionLambdaIdentityDTO SaveSubscriptionLambda([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SubscriptionLambdaStrictDTO subscriptionLambdaStrict)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Write, evaluateData => this.SaveSubscriptionLambdaInternal(subscriptionLambdaStrict, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.SubscriptionLambdaIdentityDTO SaveSubscriptionLambdaInternal(Framework.Configuration.Generated.DTO.SubscriptionLambdaStrictDTO subscriptionLambdaStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISubscriptionLambdaBLL bll = evaluateData.Context.Logics.SubscriptionLambdaFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            return this.SaveSubscriptionLambdaInternal(subscriptionLambdaStrict, evaluateData, bll);
        }
        
        protected virtual Framework.Configuration.Generated.DTO.SubscriptionLambdaIdentityDTO SaveSubscriptionLambdaInternal(Framework.Configuration.Generated.DTO.SubscriptionLambdaStrictDTO subscriptionLambdaStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData, Framework.Configuration.BLL.ISubscriptionLambdaBLL bll)
        {
            Framework.Configuration.Domain.SubscriptionLambda domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByIdOrCreate(bll, subscriptionLambdaStrict.Id);
            subscriptionLambdaStrict.MapToDomainObject(evaluateData.MappingService, domainObject);
            bll.Save(domainObject);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToIdentityDTO(domainObject);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class CheckSubscriptionLambdaAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Configuration.Generated.DTO.SubscriptionLambdaIdentityDTO subscriptionLambdaIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class HasSubscriptionLambdaAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Configuration.Generated.DTO.SubscriptionLambdaIdentityDTO subscriptionLambdaIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode;
    }
}
