namespace SampleSystem.WebApiCore.Controllers.Audit
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/[controller]/[action]")]
    public partial class BusinessUnitTypeController : Framework.Infrastructure.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get BusinessUnitType Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetBusinessUnitTypePropertyRevisionByDateRange(GetBusinessUnitTypePropertyRevisionByDateRangeAutoRequest getBusinessUnitTypePropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getBusinessUnitTypePropertyRevisionByDateRangeAutoRequest.Period;
            string propertyName = getBusinessUnitTypePropertyRevisionByDateRangeAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdentity = getBusinessUnitTypePropertyRevisionByDateRangeAutoRequest.BusinessUnitTypeIdentity;
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetBusinessUnitTypePropertyRevisionByDateRangeInternal(businessUnitTypeIdentity, propertyName, period, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetBusinessUnitTypePropertyRevisionByDateRangeInternal(SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdentity, string propertyName, Framework.Core.Period? period, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitTypeBLL bll = evaluateData.Context.Logics.BusinessUnitTypeFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            return new Framework.Infrastructure.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.BLL.Services.IRootSecurityService, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.Directories.BusinessUnitType>(businessUnitTypeIdentity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get BusinessUnitType Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetBusinessUnitTypePropertyRevisions(GetBusinessUnitTypePropertyRevisionsAutoRequest getBusinessUnitTypePropertyRevisionsAutoRequest)
        {
            string propertyName = getBusinessUnitTypePropertyRevisionsAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdentity = getBusinessUnitTypePropertyRevisionsAutoRequest.BusinessUnitTypeIdentity;
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetBusinessUnitTypePropertyRevisionsInternal(businessUnitTypeIdentity, propertyName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetBusinessUnitTypePropertyRevisionsInternal(SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdentity, string propertyName, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitTypeBLL bll = evaluateData.Context.Logics.BusinessUnitTypeFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            return new Framework.Infrastructure.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.BLL.Services.IRootSecurityService, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.Directories.BusinessUnitType>(businessUnitTypeIdentity.Id, propertyName);
        }
        
        /// <summary>
        /// Get BusinessUnitType revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.BLL.DTOMapping.Domain.DefaultDomainObjectRevisionDTO GetBusinessUnitTypeRevisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdentity)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetBusinessUnitTypeRevisionsInternal(businessUnitTypeIdentity, evaluateData));
        }
        
        protected virtual Framework.BLL.DTOMapping.Domain.DefaultDomainObjectRevisionDTO GetBusinessUnitTypeRevisionsInternal(SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdentity, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitTypeBLL bll = evaluateData.Context.Logics.BusinessUnitTypeFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            return new Framework.BLL.DTOMapping.Domain.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(businessUnitTypeIdentity.Id));
        }
        
        /// <summary>
        /// Get BusinessUnitType (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.BusinessUnitTypeFullDTO GetFullBusinessUnitTypeWithRevision(GetFullBusinessUnitTypeWithRevisionAutoRequest getFullBusinessUnitTypeWithRevisionAutoRequest)
        {
            long revision = getFullBusinessUnitTypeWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdentity = getFullBusinessUnitTypeWithRevisionAutoRequest.BusinessUnitTypeIdentity;
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetFullBusinessUnitTypeWithRevisionInternal(businessUnitTypeIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.BusinessUnitTypeFullDTO GetFullBusinessUnitTypeWithRevisionInternal(SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdentity, long revision, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitTypeBLL bll = evaluateData.Context.Logics.BusinessUnitTypeFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Directories.BusinessUnitType domainObject = bll.GetObjectByRevision(businessUnitTypeIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get BusinessUnitType (RichDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.BusinessUnitTypeRichDTO GetRichBusinessUnitTypeWithRevision(GetRichBusinessUnitTypeWithRevisionAutoRequest getRichBusinessUnitTypeWithRevisionAutoRequest)
        {
            long revision = getRichBusinessUnitTypeWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdentity = getRichBusinessUnitTypeWithRevisionAutoRequest.BusinessUnitTypeIdentity;
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetRichBusinessUnitTypeWithRevisionInternal(businessUnitTypeIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.BusinessUnitTypeRichDTO GetRichBusinessUnitTypeWithRevisionInternal(SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdentity, long revision, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitTypeBLL bll = evaluateData.Context.Logics.BusinessUnitTypeFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Directories.BusinessUnitType domainObject = bll.GetObjectByRevision(businessUnitTypeIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get BusinessUnitType (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.BusinessUnitTypeSimpleDTO GetSimpleBusinessUnitTypeWithRevision(GetSimpleBusinessUnitTypeWithRevisionAutoRequest getSimpleBusinessUnitTypeWithRevisionAutoRequest)
        {
            long revision = getSimpleBusinessUnitTypeWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdentity = getSimpleBusinessUnitTypeWithRevisionAutoRequest.BusinessUnitTypeIdentity;
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetSimpleBusinessUnitTypeWithRevisionInternal(businessUnitTypeIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.BusinessUnitTypeSimpleDTO GetSimpleBusinessUnitTypeWithRevisionInternal(SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdentity, long revision, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitTypeBLL bll = evaluateData.Context.Logics.BusinessUnitTypeFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Directories.BusinessUnitType domainObject = bll.GetObjectByRevision(businessUnitTypeIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get BusinessUnitType (VisualDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.BusinessUnitTypeVisualDTO GetVisualBusinessUnitTypeWithRevision(GetVisualBusinessUnitTypeWithRevisionAutoRequest getVisualBusinessUnitTypeWithRevisionAutoRequest)
        {
            long revision = getVisualBusinessUnitTypeWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdentity = getVisualBusinessUnitTypeWithRevisionAutoRequest.BusinessUnitTypeIdentity;
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetVisualBusinessUnitTypeWithRevisionInternal(businessUnitTypeIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.BusinessUnitTypeVisualDTO GetVisualBusinessUnitTypeWithRevisionInternal(SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdentity, long revision, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitTypeBLL bll = evaluateData.Context.Logics.BusinessUnitTypeFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Directories.BusinessUnitType domainObject = bll.GetObjectByRevision(businessUnitTypeIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.BLL.DTOMapping.Domain.AutoRequestAttribute()]
    public partial class GetBusinessUnitTypePropertyRevisionByDateRangeAutoRequest
    {
        
        private SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdentity;
        
        private string propertyName;
        
        private Framework.Core.Period? period;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.BLL.DTOMapping.Domain.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO BusinessUnitTypeIdentity
        {
            get
            {
                return this.businessUnitTypeIdentity;
            }
            set
            {
                this.businessUnitTypeIdentity = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.BLL.DTOMapping.Domain.AutoRequestPropertyAttribute(OrderIndex=1)]
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
        [Framework.BLL.DTOMapping.Domain.AutoRequestPropertyAttribute(OrderIndex=2)]
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
    [Framework.BLL.DTOMapping.Domain.AutoRequestAttribute()]
    public partial class GetBusinessUnitTypePropertyRevisionsAutoRequest
    {
        
        private SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdentity;
        
        private string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.BLL.DTOMapping.Domain.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO BusinessUnitTypeIdentity
        {
            get
            {
                return this.businessUnitTypeIdentity;
            }
            set
            {
                this.businessUnitTypeIdentity = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.BLL.DTOMapping.Domain.AutoRequestPropertyAttribute(OrderIndex=1)]
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
    [Framework.BLL.DTOMapping.Domain.AutoRequestAttribute()]
    public partial class GetFullBusinessUnitTypeWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.BLL.DTOMapping.Domain.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO BusinessUnitTypeIdentity
        {
            get
            {
                return this.businessUnitTypeIdentity;
            }
            set
            {
                this.businessUnitTypeIdentity = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.BLL.DTOMapping.Domain.AutoRequestPropertyAttribute(OrderIndex=1)]
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
    [Framework.BLL.DTOMapping.Domain.AutoRequestAttribute()]
    public partial class GetRichBusinessUnitTypeWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.BLL.DTOMapping.Domain.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO BusinessUnitTypeIdentity
        {
            get
            {
                return this.businessUnitTypeIdentity;
            }
            set
            {
                this.businessUnitTypeIdentity = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.BLL.DTOMapping.Domain.AutoRequestPropertyAttribute(OrderIndex=1)]
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
    [Framework.BLL.DTOMapping.Domain.AutoRequestAttribute()]
    public partial class GetSimpleBusinessUnitTypeWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.BLL.DTOMapping.Domain.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO BusinessUnitTypeIdentity
        {
            get
            {
                return this.businessUnitTypeIdentity;
            }
            set
            {
                this.businessUnitTypeIdentity = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.BLL.DTOMapping.Domain.AutoRequestPropertyAttribute(OrderIndex=1)]
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
    [Framework.BLL.DTOMapping.Domain.AutoRequestAttribute()]
    public partial class GetVisualBusinessUnitTypeWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO businessUnitTypeIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.BLL.DTOMapping.Domain.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.BusinessUnitTypeIdentityDTO BusinessUnitTypeIdentity
        {
            get
            {
                return this.businessUnitTypeIdentity;
            }
            set
            {
                this.businessUnitTypeIdentity = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.BLL.DTOMapping.Domain.AutoRequestPropertyAttribute(OrderIndex=1)]
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
