namespace SampleSystem.WebApiCore.Controllers.Audit
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/[controller]")]
    public partial class CompanyLegalEntityController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get CompanyLegalEntity Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetCompanyLegalEntityPropertyRevisionByDateRange")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetCompanyLegalEntityPropertyRevisionByDateRange(GetCompanyLegalEntityPropertyRevisionByDateRangeAutoRequest getCompanyLegalEntityPropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getCompanyLegalEntityPropertyRevisionByDateRangeAutoRequest.Period;
            string propertyName = getCompanyLegalEntityPropertyRevisionByDateRangeAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity = getCompanyLegalEntityPropertyRevisionByDateRangeAutoRequest.CompanyLegalEntityIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetCompanyLegalEntityPropertyRevisionByDateRangeInternal(companyLegalEntityIdentity, propertyName, period, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetCompanyLegalEntityPropertyRevisionByDateRangeInternal(SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.CompanyLegalEntity>(companyLegalEntityIdentity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get CompanyLegalEntity Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetCompanyLegalEntityPropertyRevisions")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetCompanyLegalEntityPropertyRevisions(GetCompanyLegalEntityPropertyRevisionsAutoRequest getCompanyLegalEntityPropertyRevisionsAutoRequest)
        {
            string propertyName = getCompanyLegalEntityPropertyRevisionsAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity = getCompanyLegalEntityPropertyRevisionsAutoRequest.CompanyLegalEntityIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetCompanyLegalEntityPropertyRevisionsInternal(companyLegalEntityIdentity, propertyName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetCompanyLegalEntityPropertyRevisionsInternal(SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.SecurityRule.View);
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
            SampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(companyLegalEntityIdentity.Id));
        }
        
        /// <summary>
        /// Get CompanyLegalEntity (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullCompanyLegalEntityWithRevision")]
        public virtual SampleSystem.Generated.DTO.CompanyLegalEntityFullDTO GetFullCompanyLegalEntityWithRevision(GetFullCompanyLegalEntityWithRevisionAutoRequest getFullCompanyLegalEntityWithRevisionAutoRequest)
        {
            long revision = getFullCompanyLegalEntityWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity = getFullCompanyLegalEntityWithRevisionAutoRequest.CompanyLegalEntityIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullCompanyLegalEntityWithRevisionInternal(companyLegalEntityIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.CompanyLegalEntityFullDTO GetFullCompanyLegalEntityWithRevisionInternal(SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.SecurityRule.View);
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
            long revision = getRichCompanyLegalEntityWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity = getRichCompanyLegalEntityWithRevisionAutoRequest.CompanyLegalEntityIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichCompanyLegalEntityWithRevisionInternal(companyLegalEntityIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.CompanyLegalEntityRichDTO GetRichCompanyLegalEntityWithRevisionInternal(SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.SecurityRule.View);
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
            long revision = getSimpleCompanyLegalEntityWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity = getSimpleCompanyLegalEntityWithRevisionAutoRequest.CompanyLegalEntityIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleCompanyLegalEntityWithRevisionInternal(companyLegalEntityIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.CompanyLegalEntitySimpleDTO GetSimpleCompanyLegalEntityWithRevisionInternal(SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.SecurityRule.View);
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
            long revision = getVisualCompanyLegalEntityWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity = getVisualCompanyLegalEntityWithRevisionAutoRequest.CompanyLegalEntityIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualCompanyLegalEntityWithRevisionInternal(companyLegalEntityIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.CompanyLegalEntityVisualDTO GetVisualCompanyLegalEntityWithRevisionInternal(SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.CompanyLegalEntity domainObject = bll.GetObjectByRevision(companyLegalEntityIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetCompanyLegalEntityPropertyRevisionByDateRangeAutoRequest
    {
        
        private SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity;
        
        private string propertyName;
        
        private Framework.Core.Period? period;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO CompanyLegalEntityIdentity
        {
            get
            {
                return this.companyLegalEntityIdentity;
            }
            set
            {
                this.companyLegalEntityIdentity = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public virtual string PropertyName
        {
            get
            {
                return this.propertyName;
            }
            set
            {
                this.propertyName = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=2)]
        public virtual Framework.Core.Period? Period
        {
            get
            {
                return this.period;
            }
            set
            {
                this.period = value;
            }
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetCompanyLegalEntityPropertyRevisionsAutoRequest
    {
        
        private SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity;
        
        private string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO CompanyLegalEntityIdentity
        {
            get
            {
                return this.companyLegalEntityIdentity;
            }
            set
            {
                this.companyLegalEntityIdentity = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public virtual string PropertyName
        {
            get
            {
                return this.propertyName;
            }
            set
            {
                this.propertyName = value;
            }
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetFullCompanyLegalEntityWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO CompanyLegalEntityIdentity
        {
            get
            {
                return this.companyLegalEntityIdentity;
            }
            set
            {
                this.companyLegalEntityIdentity = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public virtual long Revision
        {
            get
            {
                return this.revision;
            }
            set
            {
                this.revision = value;
            }
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetRichCompanyLegalEntityWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO CompanyLegalEntityIdentity
        {
            get
            {
                return this.companyLegalEntityIdentity;
            }
            set
            {
                this.companyLegalEntityIdentity = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public virtual long Revision
        {
            get
            {
                return this.revision;
            }
            set
            {
                this.revision = value;
            }
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetSimpleCompanyLegalEntityWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO CompanyLegalEntityIdentity
        {
            get
            {
                return this.companyLegalEntityIdentity;
            }
            set
            {
                this.companyLegalEntityIdentity = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public virtual long Revision
        {
            get
            {
                return this.revision;
            }
            set
            {
                this.revision = value;
            }
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetVisualCompanyLegalEntityWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO CompanyLegalEntityIdentity
        {
            get
            {
                return this.companyLegalEntityIdentity;
            }
            set
            {
                this.companyLegalEntityIdentity = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public virtual long Revision
        {
            get
            {
                return this.revision;
            }
            set
            {
                this.revision = value;
            }
        }
    }
}
