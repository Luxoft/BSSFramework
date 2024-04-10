namespace SampleSystem.WebApiCore.Controllers.Audit
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/v{version:apiVersion}/[controller]")]
    public partial class BusinessUnitManagerCommissionLinkController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get BusinessUnitManagerCommissionLink Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetBusinessUnitManagerCommissionLinkPropertyRevisionByDateRange")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetBusinessUnitManagerCommissionLinkPropertyRevisionByDateRange(GetBusinessUnitManagerCommissionLinkPropertyRevisionByDateRangeAutoRequest getBusinessUnitManagerCommissionLinkPropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getBusinessUnitManagerCommissionLinkPropertyRevisionByDateRangeAutoRequest.period;
            string propertyName = getBusinessUnitManagerCommissionLinkPropertyRevisionByDateRangeAutoRequest.propertyName;
            SampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkIdentityDTO businessUnitManagerCommissionLinkIdentity = getBusinessUnitManagerCommissionLinkPropertyRevisionByDateRangeAutoRequest.businessUnitManagerCommissionLinkIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetBusinessUnitManagerCommissionLinkPropertyRevisionByDateRangeInternal(businessUnitManagerCommissionLinkIdentity, propertyName, period, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetBusinessUnitManagerCommissionLinkPropertyRevisionByDateRangeInternal(SampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkIdentityDTO businessUnitManagerCommissionLinkIdentity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitManagerCommissionLinkBLL bll = evaluateData.Context.Logics.BusinessUnitManagerCommissionLinkFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.BusinessUnitManagerCommissionLink>(businessUnitManagerCommissionLinkIdentity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get BusinessUnitManagerCommissionLink Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetBusinessUnitManagerCommissionLinkPropertyRevisions")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetBusinessUnitManagerCommissionLinkPropertyRevisions(GetBusinessUnitManagerCommissionLinkPropertyRevisionsAutoRequest getBusinessUnitManagerCommissionLinkPropertyRevisionsAutoRequest)
        {
            string propertyName = getBusinessUnitManagerCommissionLinkPropertyRevisionsAutoRequest.propertyName;
            SampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkIdentityDTO businessUnitManagerCommissionLinkIdentity = getBusinessUnitManagerCommissionLinkPropertyRevisionsAutoRequest.businessUnitManagerCommissionLinkIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetBusinessUnitManagerCommissionLinkPropertyRevisionsInternal(businessUnitManagerCommissionLinkIdentity, propertyName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetBusinessUnitManagerCommissionLinkPropertyRevisionsInternal(SampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkIdentityDTO businessUnitManagerCommissionLinkIdentity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitManagerCommissionLinkBLL bll = evaluateData.Context.Logics.BusinessUnitManagerCommissionLinkFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.BusinessUnitManagerCommissionLink>(businessUnitManagerCommissionLinkIdentity.Id, propertyName);
        }
        
        /// <summary>
        /// Get BusinessUnitManagerCommissionLink revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetBusinessUnitManagerCommissionLinkRevisions")]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetBusinessUnitManagerCommissionLinkRevisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkIdentityDTO businessUnitManagerCommissionLinkIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetBusinessUnitManagerCommissionLinkRevisionsInternal(businessUnitManagerCommissionLinkIdentity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetBusinessUnitManagerCommissionLinkRevisionsInternal(SampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkIdentityDTO businessUnitManagerCommissionLinkIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitManagerCommissionLinkBLL bll = evaluateData.Context.Logics.BusinessUnitManagerCommissionLinkFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(businessUnitManagerCommissionLinkIdentity.Id));
        }
        
        /// <summary>
        /// Get BusinessUnitManagerCommissionLink (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullBusinessUnitManagerCommissionLinkWithRevision")]
        public virtual SampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkFullDTO GetFullBusinessUnitManagerCommissionLinkWithRevision(GetFullBusinessUnitManagerCommissionLinkWithRevisionAutoRequest getFullBusinessUnitManagerCommissionLinkWithRevisionAutoRequest)
        {
            long revision = getFullBusinessUnitManagerCommissionLinkWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkIdentityDTO businessUnitManagerCommissionLinkIdentity = getFullBusinessUnitManagerCommissionLinkWithRevisionAutoRequest.businessUnitManagerCommissionLinkIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullBusinessUnitManagerCommissionLinkWithRevisionInternal(businessUnitManagerCommissionLinkIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkFullDTO GetFullBusinessUnitManagerCommissionLinkWithRevisionInternal(SampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkIdentityDTO businessUnitManagerCommissionLinkIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitManagerCommissionLinkBLL bll = evaluateData.Context.Logics.BusinessUnitManagerCommissionLinkFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.BusinessUnitManagerCommissionLink domainObject = bll.GetObjectByRevision(businessUnitManagerCommissionLinkIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get BusinessUnitManagerCommissionLink (RichDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichBusinessUnitManagerCommissionLinkWithRevision")]
        public virtual SampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkRichDTO GetRichBusinessUnitManagerCommissionLinkWithRevision(GetRichBusinessUnitManagerCommissionLinkWithRevisionAutoRequest getRichBusinessUnitManagerCommissionLinkWithRevisionAutoRequest)
        {
            long revision = getRichBusinessUnitManagerCommissionLinkWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkIdentityDTO businessUnitManagerCommissionLinkIdentity = getRichBusinessUnitManagerCommissionLinkWithRevisionAutoRequest.businessUnitManagerCommissionLinkIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichBusinessUnitManagerCommissionLinkWithRevisionInternal(businessUnitManagerCommissionLinkIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkRichDTO GetRichBusinessUnitManagerCommissionLinkWithRevisionInternal(SampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkIdentityDTO businessUnitManagerCommissionLinkIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitManagerCommissionLinkBLL bll = evaluateData.Context.Logics.BusinessUnitManagerCommissionLinkFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.BusinessUnitManagerCommissionLink domainObject = bll.GetObjectByRevision(businessUnitManagerCommissionLinkIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get BusinessUnitManagerCommissionLink (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleBusinessUnitManagerCommissionLinkWithRevision")]
        public virtual SampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkSimpleDTO GetSimpleBusinessUnitManagerCommissionLinkWithRevision(GetSimpleBusinessUnitManagerCommissionLinkWithRevisionAutoRequest getSimpleBusinessUnitManagerCommissionLinkWithRevisionAutoRequest)
        {
            long revision = getSimpleBusinessUnitManagerCommissionLinkWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkIdentityDTO businessUnitManagerCommissionLinkIdentity = getSimpleBusinessUnitManagerCommissionLinkWithRevisionAutoRequest.businessUnitManagerCommissionLinkIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleBusinessUnitManagerCommissionLinkWithRevisionInternal(businessUnitManagerCommissionLinkIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkSimpleDTO GetSimpleBusinessUnitManagerCommissionLinkWithRevisionInternal(SampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkIdentityDTO businessUnitManagerCommissionLinkIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitManagerCommissionLinkBLL bll = evaluateData.Context.Logics.BusinessUnitManagerCommissionLinkFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.BusinessUnitManagerCommissionLink domainObject = bll.GetObjectByRevision(businessUnitManagerCommissionLinkIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetBusinessUnitManagerCommissionLinkPropertyRevisionByDateRangeAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkIdentityDTO businessUnitManagerCommissionLinkIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=2)]
        public Framework.Core.Period? period;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetBusinessUnitManagerCommissionLinkPropertyRevisionsAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkIdentityDTO businessUnitManagerCommissionLinkIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetFullBusinessUnitManagerCommissionLinkWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkIdentityDTO businessUnitManagerCommissionLinkIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetRichBusinessUnitManagerCommissionLinkWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkIdentityDTO businessUnitManagerCommissionLinkIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetSimpleBusinessUnitManagerCommissionLinkWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkIdentityDTO businessUnitManagerCommissionLinkIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
}
