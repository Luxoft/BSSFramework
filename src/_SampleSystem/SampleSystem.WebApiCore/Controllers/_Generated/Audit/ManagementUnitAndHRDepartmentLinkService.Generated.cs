namespace SampleSystem.WebApiCore.Controllers.Audit
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/v{version:apiVersion}/[controller]")]
    public partial class ManagementUnitAndHRDepartmentLinkController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get ManagementUnitAndHRDepartmentLink (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullManagementUnitAndHRDepartmentLinkWithRevision")]
        public virtual SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkFullDTO GetFullManagementUnitAndHRDepartmentLinkWithRevision(GetFullManagementUnitAndHRDepartmentLinkWithRevisionAutoRequest getFullManagementUnitAndHRDepartmentLinkWithRevisionAutoRequest)
        {
            long revision = getFullManagementUnitAndHRDepartmentLinkWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO managementUnitAndHRDepartmentLinkIdentity = getFullManagementUnitAndHRDepartmentLinkWithRevisionAutoRequest.managementUnitAndHRDepartmentLinkIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullManagementUnitAndHRDepartmentLinkWithRevisionInternal(managementUnitAndHRDepartmentLinkIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkFullDTO GetFullManagementUnitAndHRDepartmentLinkWithRevisionInternal(SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO managementUnitAndHRDepartmentLinkIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitAndHRDepartmentLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndHRDepartmentLinkFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.ManagementUnitAndHRDepartmentLink domainObject = bll.GetObjectByRevision(managementUnitAndHRDepartmentLinkIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get ManagementUnitAndHRDepartmentLink Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetManagementUnitAndHRDepartmentLinkPropertyRevisionByDateRange")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetManagementUnitAndHRDepartmentLinkPropertyRevisionByDateRange(GetManagementUnitAndHRDepartmentLinkPropertyRevisionByDateRangeAutoRequest getManagementUnitAndHRDepartmentLinkPropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getManagementUnitAndHRDepartmentLinkPropertyRevisionByDateRangeAutoRequest.period;
            string propertyName = getManagementUnitAndHRDepartmentLinkPropertyRevisionByDateRangeAutoRequest.propertyName;
            SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO managementUnitAndHRDepartmentLinkIdentity = getManagementUnitAndHRDepartmentLinkPropertyRevisionByDateRangeAutoRequest.managementUnitAndHRDepartmentLinkIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetManagementUnitAndHRDepartmentLinkPropertyRevisionByDateRangeInternal(managementUnitAndHRDepartmentLinkIdentity, propertyName, period, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetManagementUnitAndHRDepartmentLinkPropertyRevisionByDateRangeInternal(SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO managementUnitAndHRDepartmentLinkIdentity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitAndHRDepartmentLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndHRDepartmentLinkFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.ManagementUnitAndHRDepartmentLink>(managementUnitAndHRDepartmentLinkIdentity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get ManagementUnitAndHRDepartmentLink Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetManagementUnitAndHRDepartmentLinkPropertyRevisions")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetManagementUnitAndHRDepartmentLinkPropertyRevisions(GetManagementUnitAndHRDepartmentLinkPropertyRevisionsAutoRequest getManagementUnitAndHRDepartmentLinkPropertyRevisionsAutoRequest)
        {
            string propertyName = getManagementUnitAndHRDepartmentLinkPropertyRevisionsAutoRequest.propertyName;
            SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO managementUnitAndHRDepartmentLinkIdentity = getManagementUnitAndHRDepartmentLinkPropertyRevisionsAutoRequest.managementUnitAndHRDepartmentLinkIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetManagementUnitAndHRDepartmentLinkPropertyRevisionsInternal(managementUnitAndHRDepartmentLinkIdentity, propertyName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetManagementUnitAndHRDepartmentLinkPropertyRevisionsInternal(SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO managementUnitAndHRDepartmentLinkIdentity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitAndHRDepartmentLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndHRDepartmentLinkFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.ManagementUnitAndHRDepartmentLink>(managementUnitAndHRDepartmentLinkIdentity.Id, propertyName);
        }
        
        /// <summary>
        /// Get ManagementUnitAndHRDepartmentLink revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetManagementUnitAndHRDepartmentLinkRevisions")]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetManagementUnitAndHRDepartmentLinkRevisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO managementUnitAndHRDepartmentLinkIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetManagementUnitAndHRDepartmentLinkRevisionsInternal(managementUnitAndHRDepartmentLinkIdentity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetManagementUnitAndHRDepartmentLinkRevisionsInternal(SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO managementUnitAndHRDepartmentLinkIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitAndHRDepartmentLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndHRDepartmentLinkFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(managementUnitAndHRDepartmentLinkIdentity.Id));
        }
        
        /// <summary>
        /// Get ManagementUnitAndHRDepartmentLink (RichDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichManagementUnitAndHRDepartmentLinkWithRevision")]
        public virtual SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkRichDTO GetRichManagementUnitAndHRDepartmentLinkWithRevision(GetRichManagementUnitAndHRDepartmentLinkWithRevisionAutoRequest getRichManagementUnitAndHRDepartmentLinkWithRevisionAutoRequest)
        {
            long revision = getRichManagementUnitAndHRDepartmentLinkWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO managementUnitAndHRDepartmentLinkIdentity = getRichManagementUnitAndHRDepartmentLinkWithRevisionAutoRequest.managementUnitAndHRDepartmentLinkIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichManagementUnitAndHRDepartmentLinkWithRevisionInternal(managementUnitAndHRDepartmentLinkIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkRichDTO GetRichManagementUnitAndHRDepartmentLinkWithRevisionInternal(SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO managementUnitAndHRDepartmentLinkIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitAndHRDepartmentLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndHRDepartmentLinkFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.ManagementUnitAndHRDepartmentLink domainObject = bll.GetObjectByRevision(managementUnitAndHRDepartmentLinkIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get ManagementUnitAndHRDepartmentLink (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleManagementUnitAndHRDepartmentLinkWithRevision")]
        public virtual SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkSimpleDTO GetSimpleManagementUnitAndHRDepartmentLinkWithRevision(GetSimpleManagementUnitAndHRDepartmentLinkWithRevisionAutoRequest getSimpleManagementUnitAndHRDepartmentLinkWithRevisionAutoRequest)
        {
            long revision = getSimpleManagementUnitAndHRDepartmentLinkWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO managementUnitAndHRDepartmentLinkIdentity = getSimpleManagementUnitAndHRDepartmentLinkWithRevisionAutoRequest.managementUnitAndHRDepartmentLinkIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleManagementUnitAndHRDepartmentLinkWithRevisionInternal(managementUnitAndHRDepartmentLinkIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkSimpleDTO GetSimpleManagementUnitAndHRDepartmentLinkWithRevisionInternal(SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO managementUnitAndHRDepartmentLinkIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitAndHRDepartmentLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndHRDepartmentLinkFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.ManagementUnitAndHRDepartmentLink domainObject = bll.GetObjectByRevision(managementUnitAndHRDepartmentLinkIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetFullManagementUnitAndHRDepartmentLinkWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO managementUnitAndHRDepartmentLinkIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetManagementUnitAndHRDepartmentLinkPropertyRevisionByDateRangeAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO managementUnitAndHRDepartmentLinkIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=2)]
        public Framework.Core.Period? period;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetManagementUnitAndHRDepartmentLinkPropertyRevisionsAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO managementUnitAndHRDepartmentLinkIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetRichManagementUnitAndHRDepartmentLinkWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO managementUnitAndHRDepartmentLinkIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetSimpleManagementUnitAndHRDepartmentLinkWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO managementUnitAndHRDepartmentLinkIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
}
