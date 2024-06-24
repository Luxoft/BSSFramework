namespace Configuration.WebApi.Controllers
{
    using Framework.Configuration.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("configApi/[controller]")]
    public partial class CodeFirstSubscriptionController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService>
    {
        
        /// <summary>
        /// Get CodeFirstSubscription (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullCodeFirstSubscription")]
        public virtual Framework.Configuration.Generated.DTO.CodeFirstSubscriptionFullDTO GetFullCodeFirstSubscription([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.CodeFirstSubscriptionIdentityDTO codeFirstSubscriptionIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullCodeFirstSubscriptionInternal(codeFirstSubscriptionIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get CodeFirstSubscription (FullDTO) by code
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullCodeFirstSubscriptionByCode")]
        public virtual Framework.Configuration.Generated.DTO.CodeFirstSubscriptionFullDTO GetFullCodeFirstSubscriptionByCode([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string codeFirstSubscriptionCode)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullCodeFirstSubscriptionByCodeInternal(codeFirstSubscriptionCode, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.CodeFirstSubscriptionFullDTO GetFullCodeFirstSubscriptionByCodeInternal(string codeFirstSubscriptionCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ICodeFirstSubscriptionBLL bll = evaluateData.Context.Logics.CodeFirstSubscriptionFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.Configuration.Domain.CodeFirstSubscription domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByCode(bll, codeFirstSubscriptionCode, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.CodeFirstSubscription>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Configuration.Generated.DTO.CodeFirstSubscriptionFullDTO GetFullCodeFirstSubscriptionInternal(Framework.Configuration.Generated.DTO.CodeFirstSubscriptionIdentityDTO codeFirstSubscriptionIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ICodeFirstSubscriptionBLL bll = evaluateData.Context.Logics.CodeFirstSubscriptionFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.Configuration.Domain.CodeFirstSubscription domainObject = bll.GetById(codeFirstSubscriptionIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.CodeFirstSubscription>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of CodeFirstSubscriptions (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullCodeFirstSubscriptions")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.CodeFirstSubscriptionFullDTO> GetFullCodeFirstSubscriptions()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullCodeFirstSubscriptionsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get CodeFirstSubscriptions (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullCodeFirstSubscriptionsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.CodeFirstSubscriptionFullDTO> GetFullCodeFirstSubscriptionsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.CodeFirstSubscriptionIdentityDTO[] codeFirstSubscriptionIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullCodeFirstSubscriptionsByIdentsInternal(codeFirstSubscriptionIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.CodeFirstSubscriptionFullDTO> GetFullCodeFirstSubscriptionsByIdentsInternal(Framework.Configuration.Generated.DTO.CodeFirstSubscriptionIdentityDTO[] codeFirstSubscriptionIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ICodeFirstSubscriptionBLL bll = evaluateData.Context.Logics.CodeFirstSubscriptionFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(codeFirstSubscriptionIdents, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.CodeFirstSubscription>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get CodeFirstSubscriptions (FullDTO) by filter (Framework.Configuration.Domain.CodeFirstSubscriptionRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullCodeFirstSubscriptionsByRootFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.CodeFirstSubscriptionFullDTO> GetFullCodeFirstSubscriptionsByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.CodeFirstSubscriptionRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullCodeFirstSubscriptionsByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.CodeFirstSubscriptionFullDTO> GetFullCodeFirstSubscriptionsByRootFilterInternal(Framework.Configuration.Generated.DTO.CodeFirstSubscriptionRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ICodeFirstSubscriptionBLL bll = evaluateData.Context.Logics.CodeFirstSubscriptionFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.Configuration.Domain.CodeFirstSubscriptionRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.CodeFirstSubscription>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.CodeFirstSubscriptionFullDTO> GetFullCodeFirstSubscriptionsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ICodeFirstSubscriptionBLL bll = evaluateData.Context.Logics.CodeFirstSubscriptionFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.CodeFirstSubscription>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get CodeFirstSubscription (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichCodeFirstSubscription")]
        public virtual Framework.Configuration.Generated.DTO.CodeFirstSubscriptionRichDTO GetRichCodeFirstSubscription([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.CodeFirstSubscriptionIdentityDTO codeFirstSubscriptionIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichCodeFirstSubscriptionInternal(codeFirstSubscriptionIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get CodeFirstSubscription (RichDTO) by code
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichCodeFirstSubscriptionByCode")]
        public virtual Framework.Configuration.Generated.DTO.CodeFirstSubscriptionRichDTO GetRichCodeFirstSubscriptionByCode([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string codeFirstSubscriptionCode)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichCodeFirstSubscriptionByCodeInternal(codeFirstSubscriptionCode, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.CodeFirstSubscriptionRichDTO GetRichCodeFirstSubscriptionByCodeInternal(string codeFirstSubscriptionCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ICodeFirstSubscriptionBLL bll = evaluateData.Context.Logics.CodeFirstSubscriptionFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.Configuration.Domain.CodeFirstSubscription domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByCode(bll, codeFirstSubscriptionCode, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.CodeFirstSubscription>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Configuration.Generated.DTO.CodeFirstSubscriptionRichDTO GetRichCodeFirstSubscriptionInternal(Framework.Configuration.Generated.DTO.CodeFirstSubscriptionIdentityDTO codeFirstSubscriptionIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ICodeFirstSubscriptionBLL bll = evaluateData.Context.Logics.CodeFirstSubscriptionFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.Configuration.Domain.CodeFirstSubscription domainObject = bll.GetById(codeFirstSubscriptionIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.CodeFirstSubscription>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get CodeFirstSubscription (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleCodeFirstSubscription")]
        public virtual Framework.Configuration.Generated.DTO.CodeFirstSubscriptionSimpleDTO GetSimpleCodeFirstSubscription([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.CodeFirstSubscriptionIdentityDTO codeFirstSubscriptionIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleCodeFirstSubscriptionInternal(codeFirstSubscriptionIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get CodeFirstSubscription (SimpleDTO) by code
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleCodeFirstSubscriptionByCode")]
        public virtual Framework.Configuration.Generated.DTO.CodeFirstSubscriptionSimpleDTO GetSimpleCodeFirstSubscriptionByCode([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string codeFirstSubscriptionCode)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleCodeFirstSubscriptionByCodeInternal(codeFirstSubscriptionCode, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.CodeFirstSubscriptionSimpleDTO GetSimpleCodeFirstSubscriptionByCodeInternal(string codeFirstSubscriptionCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ICodeFirstSubscriptionBLL bll = evaluateData.Context.Logics.CodeFirstSubscriptionFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.Configuration.Domain.CodeFirstSubscription domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByCode(bll, codeFirstSubscriptionCode, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.CodeFirstSubscription>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Configuration.Generated.DTO.CodeFirstSubscriptionSimpleDTO GetSimpleCodeFirstSubscriptionInternal(Framework.Configuration.Generated.DTO.CodeFirstSubscriptionIdentityDTO codeFirstSubscriptionIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ICodeFirstSubscriptionBLL bll = evaluateData.Context.Logics.CodeFirstSubscriptionFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.Configuration.Domain.CodeFirstSubscription domainObject = bll.GetById(codeFirstSubscriptionIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.CodeFirstSubscription>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of CodeFirstSubscriptions (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleCodeFirstSubscriptions")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.CodeFirstSubscriptionSimpleDTO> GetSimpleCodeFirstSubscriptions()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleCodeFirstSubscriptionsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get CodeFirstSubscriptions (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleCodeFirstSubscriptionsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.CodeFirstSubscriptionSimpleDTO> GetSimpleCodeFirstSubscriptionsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.CodeFirstSubscriptionIdentityDTO[] codeFirstSubscriptionIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleCodeFirstSubscriptionsByIdentsInternal(codeFirstSubscriptionIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.CodeFirstSubscriptionSimpleDTO> GetSimpleCodeFirstSubscriptionsByIdentsInternal(Framework.Configuration.Generated.DTO.CodeFirstSubscriptionIdentityDTO[] codeFirstSubscriptionIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ICodeFirstSubscriptionBLL bll = evaluateData.Context.Logics.CodeFirstSubscriptionFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(codeFirstSubscriptionIdents, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.CodeFirstSubscription>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get CodeFirstSubscriptions (SimpleDTO) by filter (Framework.Configuration.Domain.CodeFirstSubscriptionRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleCodeFirstSubscriptionsByRootFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.CodeFirstSubscriptionSimpleDTO> GetSimpleCodeFirstSubscriptionsByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.CodeFirstSubscriptionRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleCodeFirstSubscriptionsByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.CodeFirstSubscriptionSimpleDTO> GetSimpleCodeFirstSubscriptionsByRootFilterInternal(Framework.Configuration.Generated.DTO.CodeFirstSubscriptionRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ICodeFirstSubscriptionBLL bll = evaluateData.Context.Logics.CodeFirstSubscriptionFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.Configuration.Domain.CodeFirstSubscriptionRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.CodeFirstSubscription>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.CodeFirstSubscriptionSimpleDTO> GetSimpleCodeFirstSubscriptionsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ICodeFirstSubscriptionBLL bll = evaluateData.Context.Logics.CodeFirstSubscriptionFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.CodeFirstSubscription>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Save CodeFirstSubscriptions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("SaveCodeFirstSubscription")]
        public virtual Framework.Configuration.Generated.DTO.CodeFirstSubscriptionIdentityDTO SaveCodeFirstSubscription([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.CodeFirstSubscriptionStrictDTO codeFirstSubscriptionStrict)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Write, evaluateData => this.SaveCodeFirstSubscriptionInternal(codeFirstSubscriptionStrict, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.CodeFirstSubscriptionIdentityDTO SaveCodeFirstSubscriptionInternal(Framework.Configuration.Generated.DTO.CodeFirstSubscriptionStrictDTO codeFirstSubscriptionStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.ICodeFirstSubscriptionBLL bll = evaluateData.Context.Logics.CodeFirstSubscriptionFactory.Create(Framework.SecuritySystem.SecurityRule.Edit);
            return this.SaveCodeFirstSubscriptionInternal(codeFirstSubscriptionStrict, evaluateData, bll);
        }
        
        protected virtual Framework.Configuration.Generated.DTO.CodeFirstSubscriptionIdentityDTO SaveCodeFirstSubscriptionInternal(Framework.Configuration.Generated.DTO.CodeFirstSubscriptionStrictDTO codeFirstSubscriptionStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData, Framework.Configuration.BLL.ICodeFirstSubscriptionBLL bll)
        {
            Framework.Configuration.Domain.CodeFirstSubscription domainObject = bll.GetById(codeFirstSubscriptionStrict.Id, true);
            codeFirstSubscriptionStrict.MapToDomainObject(evaluateData.MappingService, domainObject);
            bll.Save(domainObject);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToIdentityDTO(domainObject);
        }
    }
}
