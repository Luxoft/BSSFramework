namespace SampleSystem.WebApiCore.Controllers.Audit
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/v{version:apiVersion}/[controller]")]
    public partial class SampleSystemMessageTemplateController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<SampleSystem.BLL.ISampleSystemBLLContext>, SampleSystem.BLL.ISampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>>
    {
        
        public SampleSystemMessageTemplateController(Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<SampleSystem.BLL.ISampleSystemBLLContext> serviceEnvironment, Framework.Exceptions.IExceptionProcessor exceptionProcessor) : 
                base(serviceEnvironment, exceptionProcessor)
        {
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, SampleSystem.BLL.ISampleSystemBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>(session, context, new SampleSystemServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get SampleSystemMessageTemplate (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullSampleSystemMessageTemplateWithRevision")]
        public virtual SampleSystem.Generated.DTO.SampleSystemMessageTemplateFullDTO GetFullSampleSystemMessageTemplateWithRevision(GetFullSampleSystemMessageTemplateWithRevisionAutoRequest getFullSampleSystemMessageTemplateWithRevisionAutoRequest)
        {
            long revision = getFullSampleSystemMessageTemplateWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.SampleSystemMessageTemplateIdentityDTO sampleSystemMessageTemplateIdentity = getFullSampleSystemMessageTemplateWithRevisionAutoRequest.sampleSystemMessageTemplateIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullSampleSystemMessageTemplateWithRevisionInternal(sampleSystemMessageTemplateIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemMessageTemplateFullDTO GetFullSampleSystemMessageTemplateWithRevisionInternal(SampleSystem.Generated.DTO.SampleSystemMessageTemplateIdentityDTO sampleSystemMessageTemplateIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISampleSystemMessageTemplateBLL bll = evaluateData.Context.Logics.SampleSystemMessageTemplateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.SampleSystemMessageTemplate domainObject = bll.GetObjectByRevision(sampleSystemMessageTemplateIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get SampleSystemMessageTemplate (RichDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichSampleSystemMessageTemplateWithRevision")]
        public virtual SampleSystem.Generated.DTO.SampleSystemMessageTemplateRichDTO GetRichSampleSystemMessageTemplateWithRevision(GetRichSampleSystemMessageTemplateWithRevisionAutoRequest getRichSampleSystemMessageTemplateWithRevisionAutoRequest)
        {
            long revision = getRichSampleSystemMessageTemplateWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.SampleSystemMessageTemplateIdentityDTO sampleSystemMessageTemplateIdentity = getRichSampleSystemMessageTemplateWithRevisionAutoRequest.sampleSystemMessageTemplateIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichSampleSystemMessageTemplateWithRevisionInternal(sampleSystemMessageTemplateIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemMessageTemplateRichDTO GetRichSampleSystemMessageTemplateWithRevisionInternal(SampleSystem.Generated.DTO.SampleSystemMessageTemplateIdentityDTO sampleSystemMessageTemplateIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISampleSystemMessageTemplateBLL bll = evaluateData.Context.Logics.SampleSystemMessageTemplateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.SampleSystemMessageTemplate domainObject = bll.GetObjectByRevision(sampleSystemMessageTemplateIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get SampleSystemMessageTemplate Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSampleSystemMessageTemplatePropertyRevisionByDateRange")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetSampleSystemMessageTemplatePropertyRevisionByDateRange(GetSampleSystemMessageTemplatePropertyRevisionByDateRangeAutoRequest getSampleSystemMessageTemplatePropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getSampleSystemMessageTemplatePropertyRevisionByDateRangeAutoRequest.period;
            string propertyName = getSampleSystemMessageTemplatePropertyRevisionByDateRangeAutoRequest.propertyName;
            SampleSystem.Generated.DTO.SampleSystemMessageTemplateIdentityDTO sampleSystemMessageTemplateIdentity = getSampleSystemMessageTemplatePropertyRevisionByDateRangeAutoRequest.sampleSystemMessageTemplateIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSampleSystemMessageTemplatePropertyRevisionByDateRangeInternal(sampleSystemMessageTemplateIdentity, propertyName, period, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetSampleSystemMessageTemplatePropertyRevisionByDateRangeInternal(SampleSystem.Generated.DTO.SampleSystemMessageTemplateIdentityDTO sampleSystemMessageTemplateIdentity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISampleSystemMessageTemplateBLL bll = evaluateData.Context.Logics.SampleSystemMessageTemplateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, SampleSystem.BLL.ISampleSystemSecurityService, SampleSystem.SampleSystemSecurityOperationCode, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.SampleSystemMessageTemplate>(sampleSystemMessageTemplateIdentity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get SampleSystemMessageTemplate Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSampleSystemMessageTemplatePropertyRevisions")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetSampleSystemMessageTemplatePropertyRevisions(GetSampleSystemMessageTemplatePropertyRevisionsAutoRequest getSampleSystemMessageTemplatePropertyRevisionsAutoRequest)
        {
            string propertyName = getSampleSystemMessageTemplatePropertyRevisionsAutoRequest.propertyName;
            SampleSystem.Generated.DTO.SampleSystemMessageTemplateIdentityDTO sampleSystemMessageTemplateIdentity = getSampleSystemMessageTemplatePropertyRevisionsAutoRequest.sampleSystemMessageTemplateIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSampleSystemMessageTemplatePropertyRevisionsInternal(sampleSystemMessageTemplateIdentity, propertyName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetSampleSystemMessageTemplatePropertyRevisionsInternal(SampleSystem.Generated.DTO.SampleSystemMessageTemplateIdentityDTO sampleSystemMessageTemplateIdentity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISampleSystemMessageTemplateBLL bll = evaluateData.Context.Logics.SampleSystemMessageTemplateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, SampleSystem.BLL.ISampleSystemSecurityService, SampleSystem.SampleSystemSecurityOperationCode, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.SampleSystemMessageTemplate>(sampleSystemMessageTemplateIdentity.Id, propertyName);
        }
        
        /// <summary>
        /// Get SampleSystemMessageTemplate revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSampleSystemMessageTemplateRevisions")]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetSampleSystemMessageTemplateRevisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.SampleSystemMessageTemplateIdentityDTO sampleSystemMessageTemplateIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSampleSystemMessageTemplateRevisionsInternal(sampleSystemMessageTemplateIdentity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetSampleSystemMessageTemplateRevisionsInternal(SampleSystem.Generated.DTO.SampleSystemMessageTemplateIdentityDTO sampleSystemMessageTemplateIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISampleSystemMessageTemplateBLL bll = evaluateData.Context.Logics.SampleSystemMessageTemplateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(sampleSystemMessageTemplateIdentity.Id));
        }
        
        /// <summary>
        /// Get SampleSystemMessageTemplate (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleSampleSystemMessageTemplateWithRevision")]
        public virtual SampleSystem.Generated.DTO.SampleSystemMessageTemplateSimpleDTO GetSimpleSampleSystemMessageTemplateWithRevision(GetSimpleSampleSystemMessageTemplateWithRevisionAutoRequest getSimpleSampleSystemMessageTemplateWithRevisionAutoRequest)
        {
            long revision = getSimpleSampleSystemMessageTemplateWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.SampleSystemMessageTemplateIdentityDTO sampleSystemMessageTemplateIdentity = getSimpleSampleSystemMessageTemplateWithRevisionAutoRequest.sampleSystemMessageTemplateIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleSampleSystemMessageTemplateWithRevisionInternal(sampleSystemMessageTemplateIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemMessageTemplateSimpleDTO GetSimpleSampleSystemMessageTemplateWithRevisionInternal(SampleSystem.Generated.DTO.SampleSystemMessageTemplateIdentityDTO sampleSystemMessageTemplateIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISampleSystemMessageTemplateBLL bll = evaluateData.Context.Logics.SampleSystemMessageTemplateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.SampleSystemMessageTemplate domainObject = bll.GetObjectByRevision(sampleSystemMessageTemplateIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetFullSampleSystemMessageTemplateWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.SampleSystemMessageTemplateIdentityDTO sampleSystemMessageTemplateIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetRichSampleSystemMessageTemplateWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.SampleSystemMessageTemplateIdentityDTO sampleSystemMessageTemplateIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetSampleSystemMessageTemplatePropertyRevisionByDateRangeAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.SampleSystemMessageTemplateIdentityDTO sampleSystemMessageTemplateIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=2)]
        public Framework.Core.Period? period;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetSampleSystemMessageTemplatePropertyRevisionsAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.SampleSystemMessageTemplateIdentityDTO sampleSystemMessageTemplateIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetSimpleSampleSystemMessageTemplateWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.SampleSystemMessageTemplateIdentityDTO sampleSystemMessageTemplateIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
}
