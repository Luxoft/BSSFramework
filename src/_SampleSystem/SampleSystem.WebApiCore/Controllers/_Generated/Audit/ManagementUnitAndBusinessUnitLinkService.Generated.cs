namespace SampleSystem.WebApiCore.Controllers.Audit
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/v{version:apiVersion}/[controller]")]
    public partial class ManagementUnitAndBusinessUnitLinkController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>>
    {
        
        /// <summary>
        /// Get ManagementUnitAndBusinessUnitLink (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullManagementUnitAndBusinessUnitLinkWithRevision")]
        public virtual SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkFullDTO GetFullManagementUnitAndBusinessUnitLinkWithRevision(GetFullManagementUnitAndBusinessUnitLinkWithRevisionAutoRequest getFullManagementUnitAndBusinessUnitLinkWithRevisionAutoRequest)
        {
            long revision = getFullManagementUnitAndBusinessUnitLinkWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkIdentityDTO managementUnitAndBusinessUnitLinkIdentity = getFullManagementUnitAndBusinessUnitLinkWithRevisionAutoRequest.managementUnitAndBusinessUnitLinkIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullManagementUnitAndBusinessUnitLinkWithRevisionInternal(managementUnitAndBusinessUnitLinkIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkFullDTO GetFullManagementUnitAndBusinessUnitLinkWithRevisionInternal(SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkIdentityDTO managementUnitAndBusinessUnitLinkIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitAndBusinessUnitLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndBusinessUnitLinkFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.ManagementUnitAndBusinessUnitLink domainObject = bll.GetObjectByRevision(managementUnitAndBusinessUnitLinkIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get ManagementUnitAndBusinessUnitLink Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetManagementUnitAndBusinessUnitLinkPropertyRevisionByDateRange")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetManagementUnitAndBusinessUnitLinkPropertyRevisionByDateRange(GetManagementUnitAndBusinessUnitLinkPropertyRevisionByDateRangeAutoRequest getManagementUnitAndBusinessUnitLinkPropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getManagementUnitAndBusinessUnitLinkPropertyRevisionByDateRangeAutoRequest.period;
            string propertyName = getManagementUnitAndBusinessUnitLinkPropertyRevisionByDateRangeAutoRequest.propertyName;
            SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkIdentityDTO managementUnitAndBusinessUnitLinkIdentity = getManagementUnitAndBusinessUnitLinkPropertyRevisionByDateRangeAutoRequest.managementUnitAndBusinessUnitLinkIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetManagementUnitAndBusinessUnitLinkPropertyRevisionByDateRangeInternal(managementUnitAndBusinessUnitLinkIdentity, propertyName, period, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetManagementUnitAndBusinessUnitLinkPropertyRevisionByDateRangeInternal(SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkIdentityDTO managementUnitAndBusinessUnitLinkIdentity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitAndBusinessUnitLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndBusinessUnitLinkFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, SampleSystem.BLL.ISampleSystemSecurityService, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.ManagementUnitAndBusinessUnitLink>(managementUnitAndBusinessUnitLinkIdentity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get ManagementUnitAndBusinessUnitLink Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetManagementUnitAndBusinessUnitLinkPropertyRevisions")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetManagementUnitAndBusinessUnitLinkPropertyRevisions(GetManagementUnitAndBusinessUnitLinkPropertyRevisionsAutoRequest getManagementUnitAndBusinessUnitLinkPropertyRevisionsAutoRequest)
        {
            string propertyName = getManagementUnitAndBusinessUnitLinkPropertyRevisionsAutoRequest.propertyName;
            SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkIdentityDTO managementUnitAndBusinessUnitLinkIdentity = getManagementUnitAndBusinessUnitLinkPropertyRevisionsAutoRequest.managementUnitAndBusinessUnitLinkIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetManagementUnitAndBusinessUnitLinkPropertyRevisionsInternal(managementUnitAndBusinessUnitLinkIdentity, propertyName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetManagementUnitAndBusinessUnitLinkPropertyRevisionsInternal(SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkIdentityDTO managementUnitAndBusinessUnitLinkIdentity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitAndBusinessUnitLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndBusinessUnitLinkFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, SampleSystem.BLL.ISampleSystemSecurityService, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.ManagementUnitAndBusinessUnitLink>(managementUnitAndBusinessUnitLinkIdentity.Id, propertyName);
        }
        
        /// <summary>
        /// Get ManagementUnitAndBusinessUnitLink revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetManagementUnitAndBusinessUnitLinkRevisions")]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetManagementUnitAndBusinessUnitLinkRevisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkIdentityDTO managementUnitAndBusinessUnitLinkIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetManagementUnitAndBusinessUnitLinkRevisionsInternal(managementUnitAndBusinessUnitLinkIdentity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetManagementUnitAndBusinessUnitLinkRevisionsInternal(SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkIdentityDTO managementUnitAndBusinessUnitLinkIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitAndBusinessUnitLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndBusinessUnitLinkFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(managementUnitAndBusinessUnitLinkIdentity.Id));
        }
        
        /// <summary>
        /// Get ManagementUnitAndBusinessUnitLink (RichDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichManagementUnitAndBusinessUnitLinkWithRevision")]
        public virtual SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkRichDTO GetRichManagementUnitAndBusinessUnitLinkWithRevision(GetRichManagementUnitAndBusinessUnitLinkWithRevisionAutoRequest getRichManagementUnitAndBusinessUnitLinkWithRevisionAutoRequest)
        {
            long revision = getRichManagementUnitAndBusinessUnitLinkWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkIdentityDTO managementUnitAndBusinessUnitLinkIdentity = getRichManagementUnitAndBusinessUnitLinkWithRevisionAutoRequest.managementUnitAndBusinessUnitLinkIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichManagementUnitAndBusinessUnitLinkWithRevisionInternal(managementUnitAndBusinessUnitLinkIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkRichDTO GetRichManagementUnitAndBusinessUnitLinkWithRevisionInternal(SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkIdentityDTO managementUnitAndBusinessUnitLinkIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitAndBusinessUnitLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndBusinessUnitLinkFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.ManagementUnitAndBusinessUnitLink domainObject = bll.GetObjectByRevision(managementUnitAndBusinessUnitLinkIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get ManagementUnitAndBusinessUnitLink (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleManagementUnitAndBusinessUnitLinkWithRevision")]
        public virtual SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkSimpleDTO GetSimpleManagementUnitAndBusinessUnitLinkWithRevision(GetSimpleManagementUnitAndBusinessUnitLinkWithRevisionAutoRequest getSimpleManagementUnitAndBusinessUnitLinkWithRevisionAutoRequest)
        {
            long revision = getSimpleManagementUnitAndBusinessUnitLinkWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkIdentityDTO managementUnitAndBusinessUnitLinkIdentity = getSimpleManagementUnitAndBusinessUnitLinkWithRevisionAutoRequest.managementUnitAndBusinessUnitLinkIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleManagementUnitAndBusinessUnitLinkWithRevisionInternal(managementUnitAndBusinessUnitLinkIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkSimpleDTO GetSimpleManagementUnitAndBusinessUnitLinkWithRevisionInternal(SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkIdentityDTO managementUnitAndBusinessUnitLinkIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitAndBusinessUnitLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndBusinessUnitLinkFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.ManagementUnitAndBusinessUnitLink domainObject = bll.GetObjectByRevision(managementUnitAndBusinessUnitLinkIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetFullManagementUnitAndBusinessUnitLinkWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkIdentityDTO managementUnitAndBusinessUnitLinkIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetManagementUnitAndBusinessUnitLinkPropertyRevisionByDateRangeAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkIdentityDTO managementUnitAndBusinessUnitLinkIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=2)]
        public Framework.Core.Period? period;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetManagementUnitAndBusinessUnitLinkPropertyRevisionsAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkIdentityDTO managementUnitAndBusinessUnitLinkIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetRichManagementUnitAndBusinessUnitLinkWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkIdentityDTO managementUnitAndBusinessUnitLinkIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetSimpleManagementUnitAndBusinessUnitLinkWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkIdentityDTO managementUnitAndBusinessUnitLinkIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
}
