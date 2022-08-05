namespace SampleSystem.WebApiCore.Controllers.Audit
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/v{version:apiVersion}/[controller]")]
    public partial class BusinessUnitHrDepartmentController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>>
    {
        
        /// <summary>
        /// Get BusinessUnitHrDepartment Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetBusinessUnitHrDepartmentPropertyRevisionByDateRange")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetBusinessUnitHrDepartmentPropertyRevisionByDateRange(GetBusinessUnitHrDepartmentPropertyRevisionByDateRangeAutoRequest getBusinessUnitHrDepartmentPropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getBusinessUnitHrDepartmentPropertyRevisionByDateRangeAutoRequest.period;
            string propertyName = getBusinessUnitHrDepartmentPropertyRevisionByDateRangeAutoRequest.propertyName;
            SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO businessUnitHrDepartmentIdentity = getBusinessUnitHrDepartmentPropertyRevisionByDateRangeAutoRequest.businessUnitHrDepartmentIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetBusinessUnitHrDepartmentPropertyRevisionByDateRangeInternal(businessUnitHrDepartmentIdentity, propertyName, period, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetBusinessUnitHrDepartmentPropertyRevisionByDateRangeInternal(SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO businessUnitHrDepartmentIdentity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitHrDepartmentBLL bll = evaluateData.Context.Logics.BusinessUnitHrDepartmentFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, SampleSystem.BLL.ISampleSystemSecurityService, SampleSystem.SampleSystemSecurityOperationCode, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.BusinessUnitHrDepartment>(businessUnitHrDepartmentIdentity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get BusinessUnitHrDepartment Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetBusinessUnitHrDepartmentPropertyRevisions")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetBusinessUnitHrDepartmentPropertyRevisions(GetBusinessUnitHrDepartmentPropertyRevisionsAutoRequest getBusinessUnitHrDepartmentPropertyRevisionsAutoRequest)
        {
            string propertyName = getBusinessUnitHrDepartmentPropertyRevisionsAutoRequest.propertyName;
            SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO businessUnitHrDepartmentIdentity = getBusinessUnitHrDepartmentPropertyRevisionsAutoRequest.businessUnitHrDepartmentIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetBusinessUnitHrDepartmentPropertyRevisionsInternal(businessUnitHrDepartmentIdentity, propertyName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetBusinessUnitHrDepartmentPropertyRevisionsInternal(SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO businessUnitHrDepartmentIdentity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitHrDepartmentBLL bll = evaluateData.Context.Logics.BusinessUnitHrDepartmentFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, SampleSystem.BLL.ISampleSystemSecurityService, SampleSystem.SampleSystemSecurityOperationCode, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.BusinessUnitHrDepartment>(businessUnitHrDepartmentIdentity.Id, propertyName);
        }
        
        /// <summary>
        /// Get BusinessUnitHrDepartment revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetBusinessUnitHrDepartmentRevisions")]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetBusinessUnitHrDepartmentRevisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO businessUnitHrDepartmentIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetBusinessUnitHrDepartmentRevisionsInternal(businessUnitHrDepartmentIdentity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetBusinessUnitHrDepartmentRevisionsInternal(SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO businessUnitHrDepartmentIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitHrDepartmentBLL bll = evaluateData.Context.Logics.BusinessUnitHrDepartmentFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(businessUnitHrDepartmentIdentity.Id));
        }
        
        /// <summary>
        /// Get BusinessUnitHrDepartment (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullBusinessUnitHrDepartmentWithRevision")]
        public virtual SampleSystem.Generated.DTO.BusinessUnitHrDepartmentFullDTO GetFullBusinessUnitHrDepartmentWithRevision(GetFullBusinessUnitHrDepartmentWithRevisionAutoRequest getFullBusinessUnitHrDepartmentWithRevisionAutoRequest)
        {
            long revision = getFullBusinessUnitHrDepartmentWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO businessUnitHrDepartmentIdentity = getFullBusinessUnitHrDepartmentWithRevisionAutoRequest.businessUnitHrDepartmentIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullBusinessUnitHrDepartmentWithRevisionInternal(businessUnitHrDepartmentIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.BusinessUnitHrDepartmentFullDTO GetFullBusinessUnitHrDepartmentWithRevisionInternal(SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO businessUnitHrDepartmentIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitHrDepartmentBLL bll = evaluateData.Context.Logics.BusinessUnitHrDepartmentFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.BusinessUnitHrDepartment domainObject = bll.GetObjectByRevision(businessUnitHrDepartmentIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get BusinessUnitHrDepartment (RichDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichBusinessUnitHrDepartmentWithRevision")]
        public virtual SampleSystem.Generated.DTO.BusinessUnitHrDepartmentRichDTO GetRichBusinessUnitHrDepartmentWithRevision(GetRichBusinessUnitHrDepartmentWithRevisionAutoRequest getRichBusinessUnitHrDepartmentWithRevisionAutoRequest)
        {
            long revision = getRichBusinessUnitHrDepartmentWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO businessUnitHrDepartmentIdentity = getRichBusinessUnitHrDepartmentWithRevisionAutoRequest.businessUnitHrDepartmentIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichBusinessUnitHrDepartmentWithRevisionInternal(businessUnitHrDepartmentIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.BusinessUnitHrDepartmentRichDTO GetRichBusinessUnitHrDepartmentWithRevisionInternal(SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO businessUnitHrDepartmentIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitHrDepartmentBLL bll = evaluateData.Context.Logics.BusinessUnitHrDepartmentFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.BusinessUnitHrDepartment domainObject = bll.GetObjectByRevision(businessUnitHrDepartmentIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get BusinessUnitHrDepartment (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleBusinessUnitHrDepartmentWithRevision")]
        public virtual SampleSystem.Generated.DTO.BusinessUnitHrDepartmentSimpleDTO GetSimpleBusinessUnitHrDepartmentWithRevision(GetSimpleBusinessUnitHrDepartmentWithRevisionAutoRequest getSimpleBusinessUnitHrDepartmentWithRevisionAutoRequest)
        {
            long revision = getSimpleBusinessUnitHrDepartmentWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO businessUnitHrDepartmentIdentity = getSimpleBusinessUnitHrDepartmentWithRevisionAutoRequest.businessUnitHrDepartmentIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleBusinessUnitHrDepartmentWithRevisionInternal(businessUnitHrDepartmentIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.BusinessUnitHrDepartmentSimpleDTO GetSimpleBusinessUnitHrDepartmentWithRevisionInternal(SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO businessUnitHrDepartmentIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitHrDepartmentBLL bll = evaluateData.Context.Logics.BusinessUnitHrDepartmentFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.BusinessUnitHrDepartment domainObject = bll.GetObjectByRevision(businessUnitHrDepartmentIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetBusinessUnitHrDepartmentPropertyRevisionByDateRangeAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO businessUnitHrDepartmentIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=2)]
        public Framework.Core.Period? period;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetBusinessUnitHrDepartmentPropertyRevisionsAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO businessUnitHrDepartmentIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetFullBusinessUnitHrDepartmentWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO businessUnitHrDepartmentIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetRichBusinessUnitHrDepartmentWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO businessUnitHrDepartmentIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetSimpleBusinessUnitHrDepartmentWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO businessUnitHrDepartmentIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
}
