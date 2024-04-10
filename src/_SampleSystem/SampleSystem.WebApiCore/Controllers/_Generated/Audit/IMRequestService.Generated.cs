namespace SampleSystem.WebApiCore.Controllers.Audit
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/v{version:apiVersion}/[controller]")]
    public partial class IMRequestController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get IMRequest (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullIMRequestWithRevision")]
        public virtual SampleSystem.Generated.DTO.IMRequestFullDTO GetFullIMRequestWithRevision(GetFullIMRequestWithRevisionAutoRequest getFullIMRequestWithRevisionAutoRequest)
        {
            long revision = getFullIMRequestWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.IMRequestIdentityDTO iMRequestIdentity = getFullIMRequestWithRevisionAutoRequest.iMRequestIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullIMRequestWithRevisionInternal(iMRequestIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.IMRequestFullDTO GetFullIMRequestWithRevisionInternal(SampleSystem.Generated.DTO.IMRequestIdentityDTO iMRequestIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IIMRequestBLL bll = evaluateData.Context.Logics.IMRequestFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.IMRequest domainObject = bll.GetObjectByRevision(iMRequestIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get IMRequest Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetIMRequestPropertyRevisionByDateRange")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetIMRequestPropertyRevisionByDateRange(GetIMRequestPropertyRevisionByDateRangeAutoRequest getIMRequestPropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getIMRequestPropertyRevisionByDateRangeAutoRequest.period;
            string propertyName = getIMRequestPropertyRevisionByDateRangeAutoRequest.propertyName;
            SampleSystem.Generated.DTO.IMRequestIdentityDTO iMRequestIdentity = getIMRequestPropertyRevisionByDateRangeAutoRequest.iMRequestIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetIMRequestPropertyRevisionByDateRangeInternal(iMRequestIdentity, propertyName, period, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetIMRequestPropertyRevisionByDateRangeInternal(SampleSystem.Generated.DTO.IMRequestIdentityDTO iMRequestIdentity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IIMRequestBLL bll = evaluateData.Context.Logics.IMRequestFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.IMRequest>(iMRequestIdentity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get IMRequest Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetIMRequestPropertyRevisions")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetIMRequestPropertyRevisions(GetIMRequestPropertyRevisionsAutoRequest getIMRequestPropertyRevisionsAutoRequest)
        {
            string propertyName = getIMRequestPropertyRevisionsAutoRequest.propertyName;
            SampleSystem.Generated.DTO.IMRequestIdentityDTO iMRequestIdentity = getIMRequestPropertyRevisionsAutoRequest.iMRequestIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetIMRequestPropertyRevisionsInternal(iMRequestIdentity, propertyName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetIMRequestPropertyRevisionsInternal(SampleSystem.Generated.DTO.IMRequestIdentityDTO iMRequestIdentity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IIMRequestBLL bll = evaluateData.Context.Logics.IMRequestFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.IMRequest>(iMRequestIdentity.Id, propertyName);
        }
        
        /// <summary>
        /// Get IMRequest revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetIMRequestRevisions")]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetIMRequestRevisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.IMRequestIdentityDTO iMRequestIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetIMRequestRevisionsInternal(iMRequestIdentity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetIMRequestRevisionsInternal(SampleSystem.Generated.DTO.IMRequestIdentityDTO iMRequestIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IIMRequestBLL bll = evaluateData.Context.Logics.IMRequestFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(iMRequestIdentity.Id));
        }
        
        /// <summary>
        /// Get IMRequest (RichDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichIMRequestWithRevision")]
        public virtual SampleSystem.Generated.DTO.IMRequestRichDTO GetRichIMRequestWithRevision(GetRichIMRequestWithRevisionAutoRequest getRichIMRequestWithRevisionAutoRequest)
        {
            long revision = getRichIMRequestWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.IMRequestIdentityDTO iMRequestIdentity = getRichIMRequestWithRevisionAutoRequest.iMRequestIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichIMRequestWithRevisionInternal(iMRequestIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.IMRequestRichDTO GetRichIMRequestWithRevisionInternal(SampleSystem.Generated.DTO.IMRequestIdentityDTO iMRequestIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IIMRequestBLL bll = evaluateData.Context.Logics.IMRequestFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.IMRequest domainObject = bll.GetObjectByRevision(iMRequestIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get IMRequest (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleIMRequestWithRevision")]
        public virtual SampleSystem.Generated.DTO.IMRequestSimpleDTO GetSimpleIMRequestWithRevision(GetSimpleIMRequestWithRevisionAutoRequest getSimpleIMRequestWithRevisionAutoRequest)
        {
            long revision = getSimpleIMRequestWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.IMRequestIdentityDTO iMRequestIdentity = getSimpleIMRequestWithRevisionAutoRequest.iMRequestIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleIMRequestWithRevisionInternal(iMRequestIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.IMRequestSimpleDTO GetSimpleIMRequestWithRevisionInternal(SampleSystem.Generated.DTO.IMRequestIdentityDTO iMRequestIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IIMRequestBLL bll = evaluateData.Context.Logics.IMRequestFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.IMRequest domainObject = bll.GetObjectByRevision(iMRequestIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get IMRequest (VisualDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualIMRequestWithRevision")]
        public virtual SampleSystem.Generated.DTO.IMRequestVisualDTO GetVisualIMRequestWithRevision(GetVisualIMRequestWithRevisionAutoRequest getVisualIMRequestWithRevisionAutoRequest)
        {
            long revision = getVisualIMRequestWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.IMRequestIdentityDTO iMRequestIdentity = getVisualIMRequestWithRevisionAutoRequest.iMRequestIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualIMRequestWithRevisionInternal(iMRequestIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.IMRequestVisualDTO GetVisualIMRequestWithRevisionInternal(SampleSystem.Generated.DTO.IMRequestIdentityDTO iMRequestIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IIMRequestBLL bll = evaluateData.Context.Logics.IMRequestFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.IMRequest domainObject = bll.GetObjectByRevision(iMRequestIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetFullIMRequestWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.IMRequestIdentityDTO iMRequestIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetIMRequestPropertyRevisionByDateRangeAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.IMRequestIdentityDTO iMRequestIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=2)]
        public Framework.Core.Period? period;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetIMRequestPropertyRevisionsAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.IMRequestIdentityDTO iMRequestIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetRichIMRequestWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.IMRequestIdentityDTO iMRequestIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetSimpleIMRequestWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.IMRequestIdentityDTO iMRequestIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetVisualIMRequestWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.IMRequestIdentityDTO iMRequestIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
}
