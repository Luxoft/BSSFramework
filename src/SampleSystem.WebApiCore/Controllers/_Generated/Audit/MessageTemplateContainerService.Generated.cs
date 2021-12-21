namespace SampleSystem.WebApiCore.Controllers.Audit
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/v{version:apiVersion}/[controller]")]
    public partial class MessageTemplateContainerController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<SampleSystem.BLL.ISampleSystemBLLContext>, SampleSystem.BLL.ISampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>>
    {
        
        public MessageTemplateContainerController(Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<SampleSystem.BLL.ISampleSystemBLLContext> serviceEnvironment, Framework.Exceptions.IExceptionProcessor exceptionProcessor) : 
                base(serviceEnvironment, exceptionProcessor)
        {
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, SampleSystem.BLL.ISampleSystemBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>(session, context, new SampleSystemServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get MessageTemplateContainer (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullMessageTemplateContainerWithRevision")]
        public virtual SampleSystem.Generated.DTO.MessageTemplateContainerFullDTO GetFullMessageTemplateContainerWithRevision(GetFullMessageTemplateContainerWithRevisionAutoRequest getFullMessageTemplateContainerWithRevisionAutoRequest)
        {
            long revision = getFullMessageTemplateContainerWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.MessageTemplateContainerIdentityDTO messageTemplateContainerIdentity = getFullMessageTemplateContainerWithRevisionAutoRequest.messageTemplateContainerIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullMessageTemplateContainerWithRevisionInternal(messageTemplateContainerIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.MessageTemplateContainerFullDTO GetFullMessageTemplateContainerWithRevisionInternal(SampleSystem.Generated.DTO.MessageTemplateContainerIdentityDTO messageTemplateContainerIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IMessageTemplateContainerBLL bll = evaluateData.Context.Logics.MessageTemplateContainerFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.MessageTemplateContainer domainObject = bll.GetObjectByRevision(messageTemplateContainerIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get MessageTemplateContainer Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetMessageTemplateContainerPropertyRevisionByDateRange")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetMessageTemplateContainerPropertyRevisionByDateRange(GetMessageTemplateContainerPropertyRevisionByDateRangeAutoRequest getMessageTemplateContainerPropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getMessageTemplateContainerPropertyRevisionByDateRangeAutoRequest.period;
            string propertyName = getMessageTemplateContainerPropertyRevisionByDateRangeAutoRequest.propertyName;
            SampleSystem.Generated.DTO.MessageTemplateContainerIdentityDTO messageTemplateContainerIdentity = getMessageTemplateContainerPropertyRevisionByDateRangeAutoRequest.messageTemplateContainerIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetMessageTemplateContainerPropertyRevisionByDateRangeInternal(messageTemplateContainerIdentity, propertyName, period, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetMessageTemplateContainerPropertyRevisionByDateRangeInternal(SampleSystem.Generated.DTO.MessageTemplateContainerIdentityDTO messageTemplateContainerIdentity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IMessageTemplateContainerBLL bll = evaluateData.Context.Logics.MessageTemplateContainerFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, SampleSystem.BLL.ISampleSystemSecurityService, SampleSystem.SampleSystemSecurityOperationCode, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.MessageTemplateContainer>(messageTemplateContainerIdentity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get MessageTemplateContainer Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetMessageTemplateContainerPropertyRevisions")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetMessageTemplateContainerPropertyRevisions(GetMessageTemplateContainerPropertyRevisionsAutoRequest getMessageTemplateContainerPropertyRevisionsAutoRequest)
        {
            string propertyName = getMessageTemplateContainerPropertyRevisionsAutoRequest.propertyName;
            SampleSystem.Generated.DTO.MessageTemplateContainerIdentityDTO messageTemplateContainerIdentity = getMessageTemplateContainerPropertyRevisionsAutoRequest.messageTemplateContainerIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetMessageTemplateContainerPropertyRevisionsInternal(messageTemplateContainerIdentity, propertyName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetMessageTemplateContainerPropertyRevisionsInternal(SampleSystem.Generated.DTO.MessageTemplateContainerIdentityDTO messageTemplateContainerIdentity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IMessageTemplateContainerBLL bll = evaluateData.Context.Logics.MessageTemplateContainerFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, SampleSystem.BLL.ISampleSystemSecurityService, SampleSystem.SampleSystemSecurityOperationCode, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.MessageTemplateContainer>(messageTemplateContainerIdentity.Id, propertyName);
        }
        
        /// <summary>
        /// Get MessageTemplateContainer revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetMessageTemplateContainerRevisions")]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetMessageTemplateContainerRevisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.MessageTemplateContainerIdentityDTO messageTemplateContainerIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetMessageTemplateContainerRevisionsInternal(messageTemplateContainerIdentity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetMessageTemplateContainerRevisionsInternal(SampleSystem.Generated.DTO.MessageTemplateContainerIdentityDTO messageTemplateContainerIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IMessageTemplateContainerBLL bll = evaluateData.Context.Logics.MessageTemplateContainerFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(messageTemplateContainerIdentity.Id));
        }
        
        /// <summary>
        /// Get MessageTemplateContainer (RichDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichMessageTemplateContainerWithRevision")]
        public virtual SampleSystem.Generated.DTO.MessageTemplateContainerRichDTO GetRichMessageTemplateContainerWithRevision(GetRichMessageTemplateContainerWithRevisionAutoRequest getRichMessageTemplateContainerWithRevisionAutoRequest)
        {
            long revision = getRichMessageTemplateContainerWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.MessageTemplateContainerIdentityDTO messageTemplateContainerIdentity = getRichMessageTemplateContainerWithRevisionAutoRequest.messageTemplateContainerIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichMessageTemplateContainerWithRevisionInternal(messageTemplateContainerIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.MessageTemplateContainerRichDTO GetRichMessageTemplateContainerWithRevisionInternal(SampleSystem.Generated.DTO.MessageTemplateContainerIdentityDTO messageTemplateContainerIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IMessageTemplateContainerBLL bll = evaluateData.Context.Logics.MessageTemplateContainerFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.MessageTemplateContainer domainObject = bll.GetObjectByRevision(messageTemplateContainerIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get MessageTemplateContainer (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleMessageTemplateContainerWithRevision")]
        public virtual SampleSystem.Generated.DTO.MessageTemplateContainerSimpleDTO GetSimpleMessageTemplateContainerWithRevision(GetSimpleMessageTemplateContainerWithRevisionAutoRequest getSimpleMessageTemplateContainerWithRevisionAutoRequest)
        {
            long revision = getSimpleMessageTemplateContainerWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.MessageTemplateContainerIdentityDTO messageTemplateContainerIdentity = getSimpleMessageTemplateContainerWithRevisionAutoRequest.messageTemplateContainerIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleMessageTemplateContainerWithRevisionInternal(messageTemplateContainerIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.MessageTemplateContainerSimpleDTO GetSimpleMessageTemplateContainerWithRevisionInternal(SampleSystem.Generated.DTO.MessageTemplateContainerIdentityDTO messageTemplateContainerIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IMessageTemplateContainerBLL bll = evaluateData.Context.Logics.MessageTemplateContainerFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.MessageTemplateContainer domainObject = bll.GetObjectByRevision(messageTemplateContainerIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetFullMessageTemplateContainerWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.MessageTemplateContainerIdentityDTO messageTemplateContainerIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetMessageTemplateContainerPropertyRevisionByDateRangeAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.MessageTemplateContainerIdentityDTO messageTemplateContainerIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=2)]
        public Framework.Core.Period? period;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetMessageTemplateContainerPropertyRevisionsAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.MessageTemplateContainerIdentityDTO messageTemplateContainerIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetRichMessageTemplateContainerWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.MessageTemplateContainerIdentityDTO messageTemplateContainerIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetSimpleMessageTemplateContainerWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.MessageTemplateContainerIdentityDTO messageTemplateContainerIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
}
