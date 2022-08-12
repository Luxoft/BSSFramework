namespace SampleSystem.WebApiCore.Controllers.Audit
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/v{version:apiVersion}/[controller]")]
    public partial class BusinessUnitTypeController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>>
    {
        
        /// <summary>
        /// Get BusinessUnitType Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetBusinessUnitTypePropertyRevisionByDateRange")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetBusinessUnitTypePropertyRevisionByDateRange(GetBusinessUnitTypePropertyRevisionByDateRangeAutoRequest getBusinessUnitTypePropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getBusinessUnitTypePropertyRevisionByDateRangeAutoRequest.period;
            string propertyName = getBusinessUnitTypePropertyRevisionByDateRangeAutoRequest.propertyName;
            SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdentity = getBusinessUnitTypePropertyRevisionByDateRangeAutoRequest.businessUnitTypeIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetBusinessUnitTypePropertyRevisionByDateRangeInternal(businessUnitTypeIdentity, propertyName, period, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetBusinessUnitTypePropertyRevisionByDateRangeInternal(SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdentity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitTypeBLL bll = evaluateData.Context.Logics.BusinessUnitTypeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, SampleSystem.BLL.ISampleSystemSecurityService, SampleSystem.SampleSystemSecurityOperationCode, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.BusinessUnitType>(businessUnitTypeIdentity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get BusinessUnitType Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetBusinessUnitTypePropertyRevisions")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetBusinessUnitTypePropertyRevisions(GetBusinessUnitTypePropertyRevisionsAutoRequest getBusinessUnitTypePropertyRevisionsAutoRequest)
        {
            string propertyName = getBusinessUnitTypePropertyRevisionsAutoRequest.propertyName;
            SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdentity = getBusinessUnitTypePropertyRevisionsAutoRequest.businessUnitTypeIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetBusinessUnitTypePropertyRevisionsInternal(businessUnitTypeIdentity, propertyName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetBusinessUnitTypePropertyRevisionsInternal(SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdentity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitTypeBLL bll = evaluateData.Context.Logics.BusinessUnitTypeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, SampleSystem.BLL.ISampleSystemSecurityService, SampleSystem.SampleSystemSecurityOperationCode, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.BusinessUnitType>(businessUnitTypeIdentity.Id, propertyName);
        }
        
        /// <summary>
        /// Get BusinessUnitType revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetBusinessUnitTypeRevisions")]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetBusinessUnitTypeRevisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetBusinessUnitTypeRevisionsInternal(businessUnitTypeIdentity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetBusinessUnitTypeRevisionsInternal(SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitTypeBLL bll = evaluateData.Context.Logics.BusinessUnitTypeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(businessUnitTypeIdentity.Id));
        }
        
        /// <summary>
        /// Get BusinessUnitType (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullBusinessUnitTypeWithRevision")]
        public virtual SampleSystem.Generated.DTO.BusinessUnitTypeFullDTO GetFullBusinessUnitTypeWithRevision(GetFullBusinessUnitTypeWithRevisionAutoRequest getFullBusinessUnitTypeWithRevisionAutoRequest)
        {
            long revision = getFullBusinessUnitTypeWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdentity = getFullBusinessUnitTypeWithRevisionAutoRequest.businessUnitTypeIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullBusinessUnitTypeWithRevisionInternal(businessUnitTypeIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.BusinessUnitTypeFullDTO GetFullBusinessUnitTypeWithRevisionInternal(SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitTypeBLL bll = evaluateData.Context.Logics.BusinessUnitTypeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.BusinessUnitType domainObject = bll.GetObjectByRevision(businessUnitTypeIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get BusinessUnitType (RichDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichBusinessUnitTypeWithRevision")]
        public virtual SampleSystem.Generated.DTO.BusinessUnitTypeRichDTO GetRichBusinessUnitTypeWithRevision(GetRichBusinessUnitTypeWithRevisionAutoRequest getRichBusinessUnitTypeWithRevisionAutoRequest)
        {
            long revision = getRichBusinessUnitTypeWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdentity = getRichBusinessUnitTypeWithRevisionAutoRequest.businessUnitTypeIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichBusinessUnitTypeWithRevisionInternal(businessUnitTypeIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.BusinessUnitTypeRichDTO GetRichBusinessUnitTypeWithRevisionInternal(SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitTypeBLL bll = evaluateData.Context.Logics.BusinessUnitTypeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.BusinessUnitType domainObject = bll.GetObjectByRevision(businessUnitTypeIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get BusinessUnitType (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleBusinessUnitTypeWithRevision")]
        public virtual SampleSystem.Generated.DTO.BusinessUnitTypeSimpleDTO GetSimpleBusinessUnitTypeWithRevision(GetSimpleBusinessUnitTypeWithRevisionAutoRequest getSimpleBusinessUnitTypeWithRevisionAutoRequest)
        {
            long revision = getSimpleBusinessUnitTypeWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdentity = getSimpleBusinessUnitTypeWithRevisionAutoRequest.businessUnitTypeIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleBusinessUnitTypeWithRevisionInternal(businessUnitTypeIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.BusinessUnitTypeSimpleDTO GetSimpleBusinessUnitTypeWithRevisionInternal(SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitTypeBLL bll = evaluateData.Context.Logics.BusinessUnitTypeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.BusinessUnitType domainObject = bll.GetObjectByRevision(businessUnitTypeIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get BusinessUnitType (VisualDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualBusinessUnitTypeWithRevision")]
        public virtual SampleSystem.Generated.DTO.BusinessUnitTypeVisualDTO GetVisualBusinessUnitTypeWithRevision(GetVisualBusinessUnitTypeWithRevisionAutoRequest getVisualBusinessUnitTypeWithRevisionAutoRequest)
        {
            long revision = getVisualBusinessUnitTypeWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdentity = getVisualBusinessUnitTypeWithRevisionAutoRequest.businessUnitTypeIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualBusinessUnitTypeWithRevisionInternal(businessUnitTypeIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.BusinessUnitTypeVisualDTO GetVisualBusinessUnitTypeWithRevisionInternal(SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitTypeBLL bll = evaluateData.Context.Logics.BusinessUnitTypeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.BusinessUnitType domainObject = bll.GetObjectByRevision(businessUnitTypeIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetBusinessUnitTypePropertyRevisionByDateRangeAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=2)]
        public Framework.Core.Period? period;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetBusinessUnitTypePropertyRevisionsAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetFullBusinessUnitTypeWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetRichBusinessUnitTypeWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetSimpleBusinessUnitTypeWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetVisualBusinessUnitTypeWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
}
