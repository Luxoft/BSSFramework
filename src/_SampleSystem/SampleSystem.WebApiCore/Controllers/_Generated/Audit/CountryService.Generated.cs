namespace SampleSystem.WebApiCore.Controllers.Audit
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/[controller]/[action]")]
    public partial class CountryController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get Country Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetCountryPropertyRevisionByDateRange(GetCountryPropertyRevisionByDateRangeAutoRequest getCountryPropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getCountryPropertyRevisionByDateRangeAutoRequest.Period;
            string propertyName = getCountryPropertyRevisionByDateRangeAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.CountryIdentityDTO countryIdentity = getCountryPropertyRevisionByDateRangeAutoRequest.CountryIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetCountryPropertyRevisionByDateRangeInternal(countryIdentity, propertyName, period, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetCountryPropertyRevisionByDateRangeInternal(SampleSystem.Generated.DTO.CountryIdentityDTO countryIdentity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICountryBLL bll = evaluateData.Context.Logics.CountryFactory.Create(SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.Country>(countryIdentity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get Country Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetCountryPropertyRevisions(GetCountryPropertyRevisionsAutoRequest getCountryPropertyRevisionsAutoRequest)
        {
            string propertyName = getCountryPropertyRevisionsAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.CountryIdentityDTO countryIdentity = getCountryPropertyRevisionsAutoRequest.CountryIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetCountryPropertyRevisionsInternal(countryIdentity, propertyName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetCountryPropertyRevisionsInternal(SampleSystem.Generated.DTO.CountryIdentityDTO countryIdentity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICountryBLL bll = evaluateData.Context.Logics.CountryFactory.Create(SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.Country>(countryIdentity.Id, propertyName);
        }
        
        /// <summary>
        /// Get Country revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetCountryRevisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.CountryIdentityDTO countryIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetCountryRevisionsInternal(countryIdentity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetCountryRevisionsInternal(SampleSystem.Generated.DTO.CountryIdentityDTO countryIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICountryBLL bll = evaluateData.Context.Logics.CountryFactory.Create(SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(countryIdentity.Id));
        }
        
        /// <summary>
        /// Get Country (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.CountryFullDTO GetFullCountryWithRevision(GetFullCountryWithRevisionAutoRequest getFullCountryWithRevisionAutoRequest)
        {
            long revision = getFullCountryWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.CountryIdentityDTO countryIdentity = getFullCountryWithRevisionAutoRequest.CountryIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullCountryWithRevisionInternal(countryIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.CountryFullDTO GetFullCountryWithRevisionInternal(SampleSystem.Generated.DTO.CountryIdentityDTO countryIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICountryBLL bll = evaluateData.Context.Logics.CountryFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Country domainObject = bll.GetObjectByRevision(countryIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Country (RichDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.CountryRichDTO GetRichCountryWithRevision(GetRichCountryWithRevisionAutoRequest getRichCountryWithRevisionAutoRequest)
        {
            long revision = getRichCountryWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.CountryIdentityDTO countryIdentity = getRichCountryWithRevisionAutoRequest.CountryIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichCountryWithRevisionInternal(countryIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.CountryRichDTO GetRichCountryWithRevisionInternal(SampleSystem.Generated.DTO.CountryIdentityDTO countryIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICountryBLL bll = evaluateData.Context.Logics.CountryFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Country domainObject = bll.GetObjectByRevision(countryIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Country (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.CountrySimpleDTO GetSimpleCountryWithRevision(GetSimpleCountryWithRevisionAutoRequest getSimpleCountryWithRevisionAutoRequest)
        {
            long revision = getSimpleCountryWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.CountryIdentityDTO countryIdentity = getSimpleCountryWithRevisionAutoRequest.CountryIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleCountryWithRevisionInternal(countryIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.CountrySimpleDTO GetSimpleCountryWithRevisionInternal(SampleSystem.Generated.DTO.CountryIdentityDTO countryIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICountryBLL bll = evaluateData.Context.Logics.CountryFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Country domainObject = bll.GetObjectByRevision(countryIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Country (VisualDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.CountryVisualDTO GetVisualCountryWithRevision(GetVisualCountryWithRevisionAutoRequest getVisualCountryWithRevisionAutoRequest)
        {
            long revision = getVisualCountryWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.CountryIdentityDTO countryIdentity = getVisualCountryWithRevisionAutoRequest.CountryIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualCountryWithRevisionInternal(countryIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.CountryVisualDTO GetVisualCountryWithRevisionInternal(SampleSystem.Generated.DTO.CountryIdentityDTO countryIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ICountryBLL bll = evaluateData.Context.Logics.CountryFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Country domainObject = bll.GetObjectByRevision(countryIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetCountryPropertyRevisionByDateRangeAutoRequest
    {
        
        private SampleSystem.Generated.DTO.CountryIdentityDTO countryIdentity;
        
        private string propertyName;
        
        private Framework.Core.Period? period;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.CountryIdentityDTO CountryIdentity
        {
            get
            {
                return this.countryIdentity;
            }
            set
            {
                this.countryIdentity = value;
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
    public partial class GetCountryPropertyRevisionsAutoRequest
    {
        
        private SampleSystem.Generated.DTO.CountryIdentityDTO countryIdentity;
        
        private string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.CountryIdentityDTO CountryIdentity
        {
            get
            {
                return this.countryIdentity;
            }
            set
            {
                this.countryIdentity = value;
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
    public partial class GetFullCountryWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.CountryIdentityDTO countryIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.CountryIdentityDTO CountryIdentity
        {
            get
            {
                return this.countryIdentity;
            }
            set
            {
                this.countryIdentity = value;
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
    public partial class GetRichCountryWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.CountryIdentityDTO countryIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.CountryIdentityDTO CountryIdentity
        {
            get
            {
                return this.countryIdentity;
            }
            set
            {
                this.countryIdentity = value;
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
    public partial class GetSimpleCountryWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.CountryIdentityDTO countryIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.CountryIdentityDTO CountryIdentity
        {
            get
            {
                return this.countryIdentity;
            }
            set
            {
                this.countryIdentity = value;
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
    public partial class GetVisualCountryWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.CountryIdentityDTO countryIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.CountryIdentityDTO CountryIdentity
        {
            get
            {
                return this.countryIdentity;
            }
            set
            {
                this.countryIdentity = value;
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
