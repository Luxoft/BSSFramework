namespace Configuration.WebApi.Controllers
{
    using Framework.Configuration.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("configApi/v{version:apiVersion}/[controller]")]
    public partial class SubscriptionController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<Framework.Configuration.BLL.IConfigurationBLLContext>, Framework.Configuration.BLL.IConfigurationBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService>>
    {
        
        public SubscriptionController(Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<Framework.Configuration.BLL.IConfigurationBLLContext> serviceEnvironment, Framework.Exceptions.IExceptionProcessor exceptionProcessor) : 
                base(serviceEnvironment, exceptionProcessor)
        {
        }
        
        /// <summary>
        /// Check Subscription access
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CheckSubscriptionAccess")]
        public virtual void CheckSubscriptionAccess(CheckSubscriptionAccessAutoRequest checkSubscriptionAccessAutoRequest)
        {
            Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode = checkSubscriptionAccessAutoRequest.securityOperationCode;
            Framework.Configuration.Generated.DTO.SubscriptionIdentityDTO subscriptionIdent = checkSubscriptionAccessAutoRequest.subscriptionIdent;
            this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.CheckSubscriptionAccessInternal(subscriptionIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual void CheckSubscriptionAccessInternal(Framework.Configuration.Generated.DTO.SubscriptionIdentityDTO subscriptionIdent, Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISubscriptionBLL bll = evaluateData.Context.Logics.Subscription;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            Framework.Configuration.Domain.Subscription domainObject = bll.GetById(subscriptionIdent.Id, true);
            Framework.SecuritySystem.SecurityProviderExtensions.CheckAccess(evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Configuration.Domain.Subscription>(securityOperationCode), domainObject);
        }
        
        /// <summary>
        /// Create Subscription by model (SubscriptionCreateModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CreateSubscription")]
        public virtual Framework.Configuration.Generated.DTO.SubscriptionRichDTO CreateSubscription([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SubscriptionCreateModelStrictDTO subscriptionCreateModel)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.CreateSubscriptionInternal(subscriptionCreateModel, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.SubscriptionRichDTO CreateSubscriptionInternal(Framework.Configuration.Generated.DTO.SubscriptionCreateModelStrictDTO subscriptionCreateModel, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISubscriptionBLL bll = evaluateData.Context.Logics.SubscriptionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            Framework.Configuration.Domain.SubscriptionCreateModel createModel = subscriptionCreateModel.ToDomainObject(evaluateData.MappingService);
            Framework.Configuration.Domain.Subscription domainObject = bll.Create(createModel);
            bll.CheckAccess(domainObject);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, Framework.Configuration.BLL.IConfigurationBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService>(session, context, new ConfigurationServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get Subscription (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullSubscription")]
        public virtual Framework.Configuration.Generated.DTO.SubscriptionFullDTO GetFullSubscription([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SubscriptionIdentityDTO subscriptionIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullSubscriptionInternal(subscriptionIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get Subscription (FullDTO) by code
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullSubscriptionByCode")]
        public virtual Framework.Configuration.Generated.DTO.SubscriptionFullDTO GetFullSubscriptionByCode([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string subscriptionCode)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullSubscriptionByCodeInternal(subscriptionCode, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.SubscriptionFullDTO GetFullSubscriptionByCodeInternal(string subscriptionCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISubscriptionBLL bll = evaluateData.Context.Logics.SubscriptionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.Subscription domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByCode(bll, subscriptionCode, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Subscription>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Configuration.Generated.DTO.SubscriptionFullDTO GetFullSubscriptionInternal(Framework.Configuration.Generated.DTO.SubscriptionIdentityDTO subscriptionIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISubscriptionBLL bll = evaluateData.Context.Logics.SubscriptionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.Subscription domainObject = bll.GetById(subscriptionIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Subscription>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of Subscriptions (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullSubscriptions")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SubscriptionFullDTO> GetFullSubscriptions()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullSubscriptionsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get Subscriptions (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullSubscriptionsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SubscriptionFullDTO> GetFullSubscriptionsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SubscriptionIdentityDTO[] subscriptionIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullSubscriptionsByIdentsInternal(subscriptionIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SubscriptionFullDTO> GetFullSubscriptionsByIdentsInternal(Framework.Configuration.Generated.DTO.SubscriptionIdentityDTO[] subscriptionIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISubscriptionBLL bll = evaluateData.Context.Logics.SubscriptionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(subscriptionIdents, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Subscription>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Subscriptions (FullDTO) by filter (Framework.Configuration.Domain.SubscriptionRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullSubscriptionsByRootFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SubscriptionFullDTO> GetFullSubscriptionsByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SubscriptionRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullSubscriptionsByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SubscriptionFullDTO> GetFullSubscriptionsByRootFilterInternal(Framework.Configuration.Generated.DTO.SubscriptionRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISubscriptionBLL bll = evaluateData.Context.Logics.SubscriptionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.SubscriptionRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Subscription>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SubscriptionFullDTO> GetFullSubscriptionsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISubscriptionBLL bll = evaluateData.Context.Logics.SubscriptionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Subscription>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Subscription (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichSubscription")]
        public virtual Framework.Configuration.Generated.DTO.SubscriptionRichDTO GetRichSubscription([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SubscriptionIdentityDTO subscriptionIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichSubscriptionInternal(subscriptionIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get Subscription (RichDTO) by code
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichSubscriptionByCode")]
        public virtual Framework.Configuration.Generated.DTO.SubscriptionRichDTO GetRichSubscriptionByCode([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string subscriptionCode)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichSubscriptionByCodeInternal(subscriptionCode, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.SubscriptionRichDTO GetRichSubscriptionByCodeInternal(string subscriptionCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISubscriptionBLL bll = evaluateData.Context.Logics.SubscriptionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.Subscription domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByCode(bll, subscriptionCode, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Subscription>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Configuration.Generated.DTO.SubscriptionRichDTO GetRichSubscriptionInternal(Framework.Configuration.Generated.DTO.SubscriptionIdentityDTO subscriptionIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISubscriptionBLL bll = evaluateData.Context.Logics.SubscriptionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.Subscription domainObject = bll.GetById(subscriptionIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Subscription>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Subscription (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleSubscription")]
        public virtual Framework.Configuration.Generated.DTO.SubscriptionSimpleDTO GetSimpleSubscription([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SubscriptionIdentityDTO subscriptionIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleSubscriptionInternal(subscriptionIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get Subscription (SimpleDTO) by code
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleSubscriptionByCode")]
        public virtual Framework.Configuration.Generated.DTO.SubscriptionSimpleDTO GetSimpleSubscriptionByCode([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string subscriptionCode)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleSubscriptionByCodeInternal(subscriptionCode, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.SubscriptionSimpleDTO GetSimpleSubscriptionByCodeInternal(string subscriptionCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISubscriptionBLL bll = evaluateData.Context.Logics.SubscriptionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.Subscription domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByCode(bll, subscriptionCode, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Subscription>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Configuration.Generated.DTO.SubscriptionSimpleDTO GetSimpleSubscriptionInternal(Framework.Configuration.Generated.DTO.SubscriptionIdentityDTO subscriptionIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISubscriptionBLL bll = evaluateData.Context.Logics.SubscriptionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.Subscription domainObject = bll.GetById(subscriptionIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Subscription>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of Subscriptions (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleSubscriptions")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SubscriptionSimpleDTO> GetSimpleSubscriptions()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleSubscriptionsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get Subscriptions (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleSubscriptionsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SubscriptionSimpleDTO> GetSimpleSubscriptionsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SubscriptionIdentityDTO[] subscriptionIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleSubscriptionsByIdentsInternal(subscriptionIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SubscriptionSimpleDTO> GetSimpleSubscriptionsByIdentsInternal(Framework.Configuration.Generated.DTO.SubscriptionIdentityDTO[] subscriptionIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISubscriptionBLL bll = evaluateData.Context.Logics.SubscriptionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(subscriptionIdents, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Subscription>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Subscriptions (SimpleDTO) by filter (Framework.Configuration.Domain.SubscriptionRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleSubscriptionsByRootFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SubscriptionSimpleDTO> GetSimpleSubscriptionsByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SubscriptionRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleSubscriptionsByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SubscriptionSimpleDTO> GetSimpleSubscriptionsByRootFilterInternal(Framework.Configuration.Generated.DTO.SubscriptionRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISubscriptionBLL bll = evaluateData.Context.Logics.SubscriptionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.SubscriptionRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Subscription>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SubscriptionSimpleDTO> GetSimpleSubscriptionsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISubscriptionBLL bll = evaluateData.Context.Logics.SubscriptionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Subscription>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Subscription (VisualDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualSubscription")]
        public virtual Framework.Configuration.Generated.DTO.SubscriptionVisualDTO GetVisualSubscription([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SubscriptionIdentityDTO subscriptionIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualSubscriptionInternal(subscriptionIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get Subscription (VisualDTO) by code
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualSubscriptionByCode")]
        public virtual Framework.Configuration.Generated.DTO.SubscriptionVisualDTO GetVisualSubscriptionByCode([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string subscriptionCode)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualSubscriptionByCodeInternal(subscriptionCode, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.SubscriptionVisualDTO GetVisualSubscriptionByCodeInternal(string subscriptionCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISubscriptionBLL bll = evaluateData.Context.Logics.SubscriptionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.Subscription domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByCode(bll, subscriptionCode, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Subscription>(Framework.Transfering.ViewDTOType.VisualDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Configuration.Generated.DTO.SubscriptionVisualDTO GetVisualSubscriptionInternal(Framework.Configuration.Generated.DTO.SubscriptionIdentityDTO subscriptionIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISubscriptionBLL bll = evaluateData.Context.Logics.SubscriptionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.Subscription domainObject = bll.GetById(subscriptionIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Subscription>(Framework.Transfering.ViewDTOType.VisualDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of Subscriptions (VisualDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualSubscriptions")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SubscriptionVisualDTO> GetVisualSubscriptions()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualSubscriptionsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get Subscriptions (VisualDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualSubscriptionsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SubscriptionVisualDTO> GetVisualSubscriptionsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SubscriptionIdentityDTO[] subscriptionIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualSubscriptionsByIdentsInternal(subscriptionIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SubscriptionVisualDTO> GetVisualSubscriptionsByIdentsInternal(Framework.Configuration.Generated.DTO.SubscriptionIdentityDTO[] subscriptionIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISubscriptionBLL bll = evaluateData.Context.Logics.SubscriptionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListByIdents(subscriptionIdents, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Subscription>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Subscriptions (VisualDTO) by filter (Framework.Configuration.Domain.SubscriptionRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualSubscriptionsByRootFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SubscriptionVisualDTO> GetVisualSubscriptionsByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SubscriptionRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualSubscriptionsByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SubscriptionVisualDTO> GetVisualSubscriptionsByRootFilterInternal(Framework.Configuration.Generated.DTO.SubscriptionRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISubscriptionBLL bll = evaluateData.Context.Logics.SubscriptionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.SubscriptionRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Subscription>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.SubscriptionVisualDTO> GetVisualSubscriptionsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISubscriptionBLL bll = evaluateData.Context.Logics.SubscriptionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.Subscription>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Check access for Subscription
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("HasSubscriptionAccess")]
        public virtual bool HasSubscriptionAccess(HasSubscriptionAccessAutoRequest hasSubscriptionAccessAutoRequest)
        {
            Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode = hasSubscriptionAccessAutoRequest.securityOperationCode;
            Framework.Configuration.Generated.DTO.SubscriptionIdentityDTO subscriptionIdent = hasSubscriptionAccessAutoRequest.subscriptionIdent;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.HasSubscriptionAccessInternal(subscriptionIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual bool HasSubscriptionAccessInternal(Framework.Configuration.Generated.DTO.SubscriptionIdentityDTO subscriptionIdent, Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISubscriptionBLL bll = evaluateData.Context.Logics.Subscription;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            Framework.Configuration.Domain.Subscription domainObject = bll.GetById(subscriptionIdent.Id, true);
            return evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Configuration.Domain.Subscription>(securityOperationCode).HasAccess(domainObject);
        }
        
        /// <summary>
        /// Remove Subscription
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("RemoveSubscription")]
        public virtual void RemoveSubscription([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SubscriptionIdentityDTO subscriptionIdent)
        {
            this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Write, evaluateData => this.RemoveSubscriptionInternal(subscriptionIdent, evaluateData));
        }
        
        protected virtual void RemoveSubscriptionInternal(Framework.Configuration.Generated.DTO.SubscriptionIdentityDTO subscriptionIdent, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISubscriptionBLL bll = evaluateData.Context.Logics.SubscriptionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            this.RemoveSubscriptionInternal(subscriptionIdent, evaluateData, bll);
        }
        
        protected virtual void RemoveSubscriptionInternal(Framework.Configuration.Generated.DTO.SubscriptionIdentityDTO subscriptionIdent, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData, Framework.Configuration.BLL.ISubscriptionBLL bll)
        {
            Framework.Configuration.Domain.Subscription domainObject = bll.GetById(subscriptionIdent.Id, true);
            bll.Remove(domainObject);
        }
        
        /// <summary>
        /// Save Subscriptions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("SaveSubscription")]
        public virtual Framework.Configuration.Generated.DTO.SubscriptionIdentityDTO SaveSubscription([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.SubscriptionStrictDTO subscriptionStrict)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Write, evaluateData => this.SaveSubscriptionInternal(subscriptionStrict, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.SubscriptionIdentityDTO SaveSubscriptionInternal(Framework.Configuration.Generated.DTO.SubscriptionStrictDTO subscriptionStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ISubscriptionBLL bll = evaluateData.Context.Logics.SubscriptionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            return this.SaveSubscriptionInternal(subscriptionStrict, evaluateData, bll);
        }
        
        protected virtual Framework.Configuration.Generated.DTO.SubscriptionIdentityDTO SaveSubscriptionInternal(Framework.Configuration.Generated.DTO.SubscriptionStrictDTO subscriptionStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData, Framework.Configuration.BLL.ISubscriptionBLL bll)
        {
            Framework.Configuration.Domain.Subscription domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByIdOrCreate(bll, subscriptionStrict.Id);
            subscriptionStrict.MapToDomainObject(evaluateData.MappingService, domainObject);
            bll.Save(domainObject);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToIdentityDTO(domainObject);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class CheckSubscriptionAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Configuration.Generated.DTO.SubscriptionIdentityDTO subscriptionIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class HasSubscriptionAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Configuration.Generated.DTO.SubscriptionIdentityDTO subscriptionIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode;
    }
}
