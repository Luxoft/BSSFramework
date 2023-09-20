namespace SampleSystem.WebApiCore.Controllers.Audit
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/v{version:apiVersion}/[controller]")]
    public partial class CompanyLegalEntityController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>>
    {
        
        /// <summary>
        /// Get CompanyLegalEntity Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetCompanyLegalEntityPropertyRevisionByDateRange")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetCompanyLegalEntityPropertyRevisionByDateRange(GetCompanyLegalEntityPropertyRevisionByDateRangeAutoRequest getCompanyLegalEntityPropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getCompanyLegalEntityPropertyRevisionByDateRangeAutoRequest.period;
            string propertyName = getCompanyLegalEntityPropertyRevisionByDateRangeAutoRequest.propertyName;
            SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity = getCompanyLegalEntityPropertyRevisionByDateRangeAutoRequest.companyLegalEntityIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetCompanyLegalEntityPropertyRevisionByDateRangeInternal(companyLegalEntityIdentity, propertyName, period, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetCompanyLegalEntityPropertyRevisionByDateRangeInternal(SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.CompanyLegalEntity>(companyLegalEntityIdentity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get CompanyLegalEntity Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetCompanyLegalEntityPropertyRevisions")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetCompanyLegalEntityPropertyRevisions(GetCompanyLegalEntityPropertyRevisionsAutoRequest getCompanyLegalEntityPropertyRevisionsAutoRequest)
        {
            string propertyName = getCompanyLegalEntityPropertyRevisionsAutoRequest.propertyName;
            SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity = getCompanyLegalEntityPropertyRevisionsAutoRequest.companyLegalEntityIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetCompanyLegalEntityPropertyRevisionsInternal(companyLegalEntityIdentity, propertyName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetCompanyLegalEntityPropertyRevisionsInternal(SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.CompanyLegalEntity>(companyLegalEntityIdentity.Id, propertyName);
        }
        
        /// <summary>
        /// Get CompanyLegalEntity revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetCompanyLegalEntityRevisions")]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetCompanyLegalEntityRevisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetCompanyLegalEntityRevisionsInternal(companyLegalEntityIdentity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetCompanyLegalEntityRevisionsInternal(SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(companyLegalEntityIdentity.Id));
        }
        
        /// <summary>
        /// Get CompanyLegalEntity (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullCompanyLegalEntityWithRevision")]
        public virtual SampleSystem.Generated.DTO.CompanyLegalEntityFullDTO GetFullCompanyLegalEntityWithRevision(GetFullCompanyLegalEntityWithRevisionAutoRequest getFullCompanyLegalEntityWithRevisionAutoRequest)
        {
            long revision = getFullCompanyLegalEntityWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity = getFullCompanyLegalEntityWithRevisionAutoRequest.companyLegalEntityIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullCompanyLegalEntityWithRevisionInternal(companyLegalEntityIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.CompanyLegalEntityFullDTO GetFullCompanyLegalEntityWithRevisionInternal(SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.CompanyLegalEntity domainObject = bll.GetObjectByRevision(companyLegalEntityIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get CompanyLegalEntity (RichDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichCompanyLegalEntityWithRevision")]
        public virtual SampleSystem.Generated.DTO.CompanyLegalEntityRichDTO GetRichCompanyLegalEntityWithRevision(GetRichCompanyLegalEntityWithRevisionAutoRequest getRichCompanyLegalEntityWithRevisionAutoRequest)
        {
            long revision = getRichCompanyLegalEntityWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity = getRichCompanyLegalEntityWithRevisionAutoRequest.companyLegalEntityIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichCompanyLegalEntityWithRevisionInternal(companyLegalEntityIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.CompanyLegalEntityRichDTO GetRichCompanyLegalEntityWithRevisionInternal(SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.CompanyLegalEntity domainObject = bll.GetObjectByRevision(companyLegalEntityIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get CompanyLegalEntity (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleCompanyLegalEntityWithRevision")]
        public virtual SampleSystem.Generated.DTO.CompanyLegalEntitySimpleDTO GetSimpleCompanyLegalEntityWithRevision(GetSimpleCompanyLegalEntityWithRevisionAutoRequest getSimpleCompanyLegalEntityWithRevisionAutoRequest)
        {
            long revision = getSimpleCompanyLegalEntityWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity = getSimpleCompanyLegalEntityWithRevisionAutoRequest.companyLegalEntityIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleCompanyLegalEntityWithRevisionInternal(companyLegalEntityIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.CompanyLegalEntitySimpleDTO GetSimpleCompanyLegalEntityWithRevisionInternal(SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.CompanyLegalEntity domainObject = bll.GetObjectByRevision(companyLegalEntityIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get CompanyLegalEntity (VisualDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualCompanyLegalEntityWithRevision")]
        public virtual SampleSystem.Generated.DTO.CompanyLegalEntityVisualDTO GetVisualCompanyLegalEntityWithRevision(GetVisualCompanyLegalEntityWithRevisionAutoRequest getVisualCompanyLegalEntityWithRevisionAutoRequest)
        {
            long revision = getVisualCompanyLegalEntityWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity = getVisualCompanyLegalEntityWithRevisionAutoRequest.companyLegalEntityIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualCompanyLegalEntityWithRevisionInternal(companyLegalEntityIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.CompanyLegalEntityVisualDTO GetVisualCompanyLegalEntityWithRevisionInternal(SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.CompanyLegalEntity domainObject = bll.GetObjectByRevision(companyLegalEntityIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetCompanyLegalEntityPropertyRevisionByDateRangeAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=2)]
        public Framework.Core.Period? period;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetCompanyLegalEntityPropertyRevisionsAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetFullCompanyLegalEntityWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetRichCompanyLegalEntityWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetSimpleCompanyLegalEntityWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetVisualCompanyLegalEntityWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
}
