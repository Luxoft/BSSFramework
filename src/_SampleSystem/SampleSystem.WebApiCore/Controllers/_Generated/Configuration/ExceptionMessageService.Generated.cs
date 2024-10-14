namespace Configuration.WebApi.Controllers
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("configApi/[controller]/[action]")]
    public partial class ExceptionMessageController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService>
    {
        
        /// <summary>
        /// Get ExceptionMessage (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.Configuration.Generated.DTO.ExceptionMessageFullDTO GetFullExceptionMessage([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.ExceptionMessageIdentityDTO exceptionMessageIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullExceptionMessageInternal(exceptionMessageIdentity, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.ExceptionMessageFullDTO GetFullExceptionMessageInternal(Framework.Configuration.Generated.DTO.ExceptionMessageIdentityDTO exceptionMessageIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IExceptionMessageBLL bll = evaluateData.Context.Logics.ExceptionMessageFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.Configuration.Domain.ExceptionMessage domainObject = bll.GetById(exceptionMessageIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.ExceptionMessage>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of ExceptionMessages (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.ExceptionMessageFullDTO> GetFullExceptionMessages()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullExceptionMessagesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get ExceptionMessages (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.ExceptionMessageFullDTO> GetFullExceptionMessagesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.ExceptionMessageIdentityDTO[] exceptionMessageIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullExceptionMessagesByIdentsInternal(exceptionMessageIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.ExceptionMessageFullDTO> GetFullExceptionMessagesByIdentsInternal(Framework.Configuration.Generated.DTO.ExceptionMessageIdentityDTO[] exceptionMessageIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IExceptionMessageBLL bll = evaluateData.Context.Logics.ExceptionMessageFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(exceptionMessageIdents, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.ExceptionMessage>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get ExceptionMessages (FullDTO) by filter (Framework.Configuration.Domain.ExceptionMessageRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.ExceptionMessageFullDTO> GetFullExceptionMessagesByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.ExceptionMessageRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullExceptionMessagesByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.ExceptionMessageFullDTO> GetFullExceptionMessagesByRootFilterInternal(Framework.Configuration.Generated.DTO.ExceptionMessageRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IExceptionMessageBLL bll = evaluateData.Context.Logics.ExceptionMessageFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.Configuration.Domain.ExceptionMessageRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.ExceptionMessage>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.ExceptionMessageFullDTO> GetFullExceptionMessagesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IExceptionMessageBLL bll = evaluateData.Context.Logics.ExceptionMessageFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.ExceptionMessage>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get ExceptionMessage (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.Configuration.Generated.DTO.ExceptionMessageRichDTO GetRichExceptionMessage([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.ExceptionMessageIdentityDTO exceptionMessageIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichExceptionMessageInternal(exceptionMessageIdentity, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.ExceptionMessageRichDTO GetRichExceptionMessageInternal(Framework.Configuration.Generated.DTO.ExceptionMessageIdentityDTO exceptionMessageIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IExceptionMessageBLL bll = evaluateData.Context.Logics.ExceptionMessageFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.Configuration.Domain.ExceptionMessage domainObject = bll.GetById(exceptionMessageIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.ExceptionMessage>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get ExceptionMessage (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.Configuration.Generated.DTO.ExceptionMessageSimpleDTO GetSimpleExceptionMessage([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.ExceptionMessageIdentityDTO exceptionMessageIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleExceptionMessageInternal(exceptionMessageIdentity, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.ExceptionMessageSimpleDTO GetSimpleExceptionMessageInternal(Framework.Configuration.Generated.DTO.ExceptionMessageIdentityDTO exceptionMessageIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IExceptionMessageBLL bll = evaluateData.Context.Logics.ExceptionMessageFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.Configuration.Domain.ExceptionMessage domainObject = bll.GetById(exceptionMessageIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.ExceptionMessage>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of ExceptionMessages (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.ExceptionMessageSimpleDTO> GetSimpleExceptionMessages()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleExceptionMessagesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get ExceptionMessages (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.ExceptionMessageSimpleDTO> GetSimpleExceptionMessagesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.ExceptionMessageIdentityDTO[] exceptionMessageIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleExceptionMessagesByIdentsInternal(exceptionMessageIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.ExceptionMessageSimpleDTO> GetSimpleExceptionMessagesByIdentsInternal(Framework.Configuration.Generated.DTO.ExceptionMessageIdentityDTO[] exceptionMessageIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IExceptionMessageBLL bll = evaluateData.Context.Logics.ExceptionMessageFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(exceptionMessageIdents, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.ExceptionMessage>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get ExceptionMessages (SimpleDTO) by filter (Framework.Configuration.Domain.ExceptionMessageRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.ExceptionMessageSimpleDTO> GetSimpleExceptionMessagesByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.ExceptionMessageRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleExceptionMessagesByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.ExceptionMessageSimpleDTO> GetSimpleExceptionMessagesByRootFilterInternal(Framework.Configuration.Generated.DTO.ExceptionMessageRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IExceptionMessageBLL bll = evaluateData.Context.Logics.ExceptionMessageFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            Framework.Configuration.Domain.ExceptionMessageRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.ExceptionMessage>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.ExceptionMessageSimpleDTO> GetSimpleExceptionMessagesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IExceptionMessageBLL bll = evaluateData.Context.Logics.ExceptionMessageFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.ExceptionMessage>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual Framework.Configuration.Generated.DTO.ExceptionMessageIdentityDTO SaveExceptionMessageInternal(Framework.Configuration.Generated.DTO.ExceptionMessageStrictDTO exceptionMessageStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData, Framework.Configuration.BLL.IExceptionMessageBLL bll)
        {
            Framework.Configuration.Domain.ExceptionMessage domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByIdOrCreate(bll, exceptionMessageStrict.Id);
            exceptionMessageStrict.MapToDomainObject(evaluateData.MappingService, domainObject);
            bll.Save(domainObject);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToIdentityDTO(domainObject);
        }
    }
}
