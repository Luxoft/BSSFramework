namespace SampleSystem.WebApiCore.Controllers.Main
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("api/v{version:apiVersion}/[controller]")]
    public partial class MessageTemplateContainerController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<SampleSystem.BLL.ISampleSystemBLLContext>, SampleSystem.BLL.ISampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>>
    {
        
        public MessageTemplateContainerController(Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<SampleSystem.BLL.ISampleSystemBLLContext> serviceEnvironment, Framework.Exceptions.IExceptionProcessor exceptionProcessor) : 
                base(serviceEnvironment, exceptionProcessor)
        {
        }
        
        /// <summary>
        /// Check MessageTemplateContainer access
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CheckMessageTemplateContainerAccess")]
        public virtual void CheckMessageTemplateContainerAccess(CheckMessageTemplateContainerAccessAutoRequest checkMessageTemplateContainerAccessAutoRequest)
        {
            SampleSystem.SampleSystemSecurityOperationCode securityOperationCode = checkMessageTemplateContainerAccessAutoRequest.securityOperationCode;
            SampleSystem.Generated.DTO.MessageTemplateContainerIdentityDTO messageTemplateContainerIdent = checkMessageTemplateContainerAccessAutoRequest.messageTemplateContainerIdent;
            this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.CheckMessageTemplateContainerAccessInternal(messageTemplateContainerIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual void CheckMessageTemplateContainerAccessInternal(SampleSystem.Generated.DTO.MessageTemplateContainerIdentityDTO messageTemplateContainerIdent, SampleSystem.SampleSystemSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IMessageTemplateContainerBLL bll = evaluateData.Context.Logics.MessageTemplateContainer;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            SampleSystem.Domain.MessageTemplateContainer domainObject = bll.GetById(messageTemplateContainerIdent.Id, true);
            Framework.SecuritySystem.SecurityProviderExtensions.CheckAccess(evaluateData.Context.SecurityService.GetSecurityProvider<SampleSystem.Domain.MessageTemplateContainer>(securityOperationCode), domainObject);
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, SampleSystem.BLL.ISampleSystemBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>(session, context, new SampleSystemServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get MessageTemplateContainer (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullMessageTemplateContainer")]
        public virtual SampleSystem.Generated.DTO.MessageTemplateContainerFullDTO GetFullMessageTemplateContainer([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.MessageTemplateContainerIdentityDTO messageTemplateContainerIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullMessageTemplateContainerInternal(messageTemplateContainerIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.MessageTemplateContainerFullDTO GetFullMessageTemplateContainerInternal(SampleSystem.Generated.DTO.MessageTemplateContainerIdentityDTO messageTemplateContainerIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IMessageTemplateContainerBLL bll = evaluateData.Context.Logics.MessageTemplateContainerFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.MessageTemplateContainer domainObject = bll.GetById(messageTemplateContainerIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.MessageTemplateContainer>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of MessageTemplateContainers (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullMessageTemplateContainers")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.MessageTemplateContainerFullDTO> GetFullMessageTemplateContainers()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullMessageTemplateContainersInternal(evaluateData));
        }
        
        /// <summary>
        /// Get MessageTemplateContainers (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullMessageTemplateContainersByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.MessageTemplateContainerFullDTO> GetFullMessageTemplateContainersByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.MessageTemplateContainerIdentityDTO[] messageTemplateContainerIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullMessageTemplateContainersByIdentsInternal(messageTemplateContainerIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.MessageTemplateContainerFullDTO> GetFullMessageTemplateContainersByIdentsInternal(SampleSystem.Generated.DTO.MessageTemplateContainerIdentityDTO[] messageTemplateContainerIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IMessageTemplateContainerBLL bll = evaluateData.Context.Logics.MessageTemplateContainerFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(messageTemplateContainerIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.MessageTemplateContainer>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.MessageTemplateContainerFullDTO> GetFullMessageTemplateContainersInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IMessageTemplateContainerBLL bll = evaluateData.Context.Logics.MessageTemplateContainerFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.MessageTemplateContainer>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get MessageTemplateContainer (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichMessageTemplateContainer")]
        public virtual SampleSystem.Generated.DTO.MessageTemplateContainerRichDTO GetRichMessageTemplateContainer([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.MessageTemplateContainerIdentityDTO messageTemplateContainerIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichMessageTemplateContainerInternal(messageTemplateContainerIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.MessageTemplateContainerRichDTO GetRichMessageTemplateContainerInternal(SampleSystem.Generated.DTO.MessageTemplateContainerIdentityDTO messageTemplateContainerIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IMessageTemplateContainerBLL bll = evaluateData.Context.Logics.MessageTemplateContainerFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.MessageTemplateContainer domainObject = bll.GetById(messageTemplateContainerIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.MessageTemplateContainer>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get MessageTemplateContainer (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleMessageTemplateContainer")]
        public virtual SampleSystem.Generated.DTO.MessageTemplateContainerSimpleDTO GetSimpleMessageTemplateContainer([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.MessageTemplateContainerIdentityDTO messageTemplateContainerIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleMessageTemplateContainerInternal(messageTemplateContainerIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.MessageTemplateContainerSimpleDTO GetSimpleMessageTemplateContainerInternal(SampleSystem.Generated.DTO.MessageTemplateContainerIdentityDTO messageTemplateContainerIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IMessageTemplateContainerBLL bll = evaluateData.Context.Logics.MessageTemplateContainerFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.MessageTemplateContainer domainObject = bll.GetById(messageTemplateContainerIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.MessageTemplateContainer>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of MessageTemplateContainers (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleMessageTemplateContainers")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.MessageTemplateContainerSimpleDTO> GetSimpleMessageTemplateContainers()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleMessageTemplateContainersInternal(evaluateData));
        }
        
        /// <summary>
        /// Get MessageTemplateContainers (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleMessageTemplateContainersByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.MessageTemplateContainerSimpleDTO> GetSimpleMessageTemplateContainersByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.MessageTemplateContainerIdentityDTO[] messageTemplateContainerIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleMessageTemplateContainersByIdentsInternal(messageTemplateContainerIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.MessageTemplateContainerSimpleDTO> GetSimpleMessageTemplateContainersByIdentsInternal(SampleSystem.Generated.DTO.MessageTemplateContainerIdentityDTO[] messageTemplateContainerIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IMessageTemplateContainerBLL bll = evaluateData.Context.Logics.MessageTemplateContainerFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(messageTemplateContainerIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.MessageTemplateContainer>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.MessageTemplateContainerSimpleDTO> GetSimpleMessageTemplateContainersInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IMessageTemplateContainerBLL bll = evaluateData.Context.Logics.MessageTemplateContainerFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.MessageTemplateContainer>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Check access for MessageTemplateContainer
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("HasMessageTemplateContainerAccess")]
        public virtual bool HasMessageTemplateContainerAccess(HasMessageTemplateContainerAccessAutoRequest hasMessageTemplateContainerAccessAutoRequest)
        {
            SampleSystem.SampleSystemSecurityOperationCode securityOperationCode = hasMessageTemplateContainerAccessAutoRequest.securityOperationCode;
            SampleSystem.Generated.DTO.MessageTemplateContainerIdentityDTO messageTemplateContainerIdent = hasMessageTemplateContainerAccessAutoRequest.messageTemplateContainerIdent;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.HasMessageTemplateContainerAccessInternal(messageTemplateContainerIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual bool HasMessageTemplateContainerAccessInternal(SampleSystem.Generated.DTO.MessageTemplateContainerIdentityDTO messageTemplateContainerIdent, SampleSystem.SampleSystemSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IMessageTemplateContainerBLL bll = evaluateData.Context.Logics.MessageTemplateContainer;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            SampleSystem.Domain.MessageTemplateContainer domainObject = bll.GetById(messageTemplateContainerIdent.Id, true);
            return evaluateData.Context.SecurityService.GetSecurityProvider<SampleSystem.Domain.MessageTemplateContainer>(securityOperationCode).HasAccess(domainObject);
        }
        
        /// <summary>
        /// Save MessageTemplateContainers
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("SaveMessageTemplateContainer")]
        public virtual SampleSystem.Generated.DTO.MessageTemplateContainerIdentityDTO SaveMessageTemplateContainer([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.MessageTemplateContainerStrictDTO messageTemplateContainerStrict)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Write, evaluateData => this.SaveMessageTemplateContainerInternal(messageTemplateContainerStrict, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.MessageTemplateContainerIdentityDTO SaveMessageTemplateContainerInternal(SampleSystem.Generated.DTO.MessageTemplateContainerStrictDTO messageTemplateContainerStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IMessageTemplateContainerBLL bll = evaluateData.Context.Logics.MessageTemplateContainerFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            return this.SaveMessageTemplateContainerInternal(messageTemplateContainerStrict, evaluateData, bll);
        }
        
        protected virtual SampleSystem.Generated.DTO.MessageTemplateContainerIdentityDTO SaveMessageTemplateContainerInternal(SampleSystem.Generated.DTO.MessageTemplateContainerStrictDTO messageTemplateContainerStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData, SampleSystem.BLL.IMessageTemplateContainerBLL bll)
        {
            SampleSystem.Domain.MessageTemplateContainer domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByIdOrCreate(bll, messageTemplateContainerStrict.Id);
            messageTemplateContainerStrict.MapToDomainObject(evaluateData.MappingService, domainObject);
            bll.Save(domainObject);
            return SampleSystem.Generated.DTO.LambdaHelper.ToIdentityDTO(domainObject);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class CheckMessageTemplateContainerAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.MessageTemplateContainerIdentityDTO messageTemplateContainerIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public SampleSystem.SampleSystemSecurityOperationCode securityOperationCode;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class HasMessageTemplateContainerAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.MessageTemplateContainerIdentityDTO messageTemplateContainerIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public SampleSystem.SampleSystemSecurityOperationCode securityOperationCode;
    }
}
