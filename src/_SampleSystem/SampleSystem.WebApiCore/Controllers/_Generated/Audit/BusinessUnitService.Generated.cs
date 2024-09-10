namespace SampleSystem.WebApiCore.Controllers.Audit
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/[controller]/[action]")]
    public partial class BusinessUnitController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get BusinessUnit Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetBusinessUnitPropertyRevisionByDateRange(GetBusinessUnitPropertyRevisionByDateRangeAutoRequest getBusinessUnitPropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getBusinessUnitPropertyRevisionByDateRangeAutoRequest.Period;
            string propertyName = getBusinessUnitPropertyRevisionByDateRangeAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.BusinessUnitIdentityDTO businessUnitIdentity = getBusinessUnitPropertyRevisionByDateRangeAutoRequest.BusinessUnitIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetBusinessUnitPropertyRevisionByDateRangeInternal(businessUnitIdentity, propertyName, period, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetBusinessUnitPropertyRevisionByDateRangeInternal(SampleSystem.Generated.DTO.BusinessUnitIdentityDTO businessUnitIdentity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitBLL bll = evaluateData.Context.Logics.BusinessUnitFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.BusinessUnit>(businessUnitIdentity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get BusinessUnit Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetBusinessUnitPropertyRevisions(GetBusinessUnitPropertyRevisionsAutoRequest getBusinessUnitPropertyRevisionsAutoRequest)
        {
            string propertyName = getBusinessUnitPropertyRevisionsAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.BusinessUnitIdentityDTO businessUnitIdentity = getBusinessUnitPropertyRevisionsAutoRequest.BusinessUnitIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetBusinessUnitPropertyRevisionsInternal(businessUnitIdentity, propertyName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetBusinessUnitPropertyRevisionsInternal(SampleSystem.Generated.DTO.BusinessUnitIdentityDTO businessUnitIdentity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitBLL bll = evaluateData.Context.Logics.BusinessUnitFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.BusinessUnit>(businessUnitIdentity.Id, propertyName);
        }
        
        /// <summary>
        /// Get BusinessUnit revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetBusinessUnitRevisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.BusinessUnitIdentityDTO businessUnitIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetBusinessUnitRevisionsInternal(businessUnitIdentity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetBusinessUnitRevisionsInternal(SampleSystem.Generated.DTO.BusinessUnitIdentityDTO businessUnitIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitBLL bll = evaluateData.Context.Logics.BusinessUnitFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(businessUnitIdentity.Id));
        }
        
        /// <summary>
        /// Get BusinessUnit (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.BusinessUnitFullDTO GetFullBusinessUnitWithRevision(GetFullBusinessUnitWithRevisionAutoRequest getFullBusinessUnitWithRevisionAutoRequest)
        {
            long revision = getFullBusinessUnitWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.BusinessUnitIdentityDTO businessUnitIdentity = getFullBusinessUnitWithRevisionAutoRequest.BusinessUnitIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullBusinessUnitWithRevisionInternal(businessUnitIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.BusinessUnitFullDTO GetFullBusinessUnitWithRevisionInternal(SampleSystem.Generated.DTO.BusinessUnitIdentityDTO businessUnitIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitBLL bll = evaluateData.Context.Logics.BusinessUnitFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.BusinessUnit domainObject = bll.GetObjectByRevision(businessUnitIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get BusinessUnit (RichDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.BusinessUnitRichDTO GetRichBusinessUnitWithRevision(GetRichBusinessUnitWithRevisionAutoRequest getRichBusinessUnitWithRevisionAutoRequest)
        {
            long revision = getRichBusinessUnitWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.BusinessUnitIdentityDTO businessUnitIdentity = getRichBusinessUnitWithRevisionAutoRequest.BusinessUnitIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichBusinessUnitWithRevisionInternal(businessUnitIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.BusinessUnitRichDTO GetRichBusinessUnitWithRevisionInternal(SampleSystem.Generated.DTO.BusinessUnitIdentityDTO businessUnitIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitBLL bll = evaluateData.Context.Logics.BusinessUnitFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.BusinessUnit domainObject = bll.GetObjectByRevision(businessUnitIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get BusinessUnit (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.BusinessUnitSimpleDTO GetSimpleBusinessUnitWithRevision(GetSimpleBusinessUnitWithRevisionAutoRequest getSimpleBusinessUnitWithRevisionAutoRequest)
        {
            long revision = getSimpleBusinessUnitWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.BusinessUnitIdentityDTO businessUnitIdentity = getSimpleBusinessUnitWithRevisionAutoRequest.BusinessUnitIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleBusinessUnitWithRevisionInternal(businessUnitIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.BusinessUnitSimpleDTO GetSimpleBusinessUnitWithRevisionInternal(SampleSystem.Generated.DTO.BusinessUnitIdentityDTO businessUnitIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitBLL bll = evaluateData.Context.Logics.BusinessUnitFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.BusinessUnit domainObject = bll.GetObjectByRevision(businessUnitIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get BusinessUnit (VisualDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.BusinessUnitVisualDTO GetVisualBusinessUnitWithRevision(GetVisualBusinessUnitWithRevisionAutoRequest getVisualBusinessUnitWithRevisionAutoRequest)
        {
            long revision = getVisualBusinessUnitWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.BusinessUnitIdentityDTO businessUnitIdentity = getVisualBusinessUnitWithRevisionAutoRequest.BusinessUnitIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualBusinessUnitWithRevisionInternal(businessUnitIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.BusinessUnitVisualDTO GetVisualBusinessUnitWithRevisionInternal(SampleSystem.Generated.DTO.BusinessUnitIdentityDTO businessUnitIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitBLL bll = evaluateData.Context.Logics.BusinessUnitFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.BusinessUnit domainObject = bll.GetObjectByRevision(businessUnitIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetBusinessUnitPropertyRevisionByDateRangeAutoRequest
    {
        
        private SampleSystem.Generated.DTO.BusinessUnitIdentityDTO businessUnitIdentity;
        
        private string propertyName;
        
        private Framework.Core.Period? period;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.BusinessUnitIdentityDTO BusinessUnitIdentity
        {
            get
            {
                return this.businessUnitIdentity;
            }
            set
            {
                this.businessUnitIdentity = value;
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
    public partial class GetBusinessUnitPropertyRevisionsAutoRequest
    {
        
        private SampleSystem.Generated.DTO.BusinessUnitIdentityDTO businessUnitIdentity;
        
        private string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.BusinessUnitIdentityDTO BusinessUnitIdentity
        {
            get
            {
                return this.businessUnitIdentity;
            }
            set
            {
                this.businessUnitIdentity = value;
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
    public partial class GetFullBusinessUnitWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.BusinessUnitIdentityDTO businessUnitIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.BusinessUnitIdentityDTO BusinessUnitIdentity
        {
            get
            {
                return this.businessUnitIdentity;
            }
            set
            {
                this.businessUnitIdentity = value;
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
    public partial class GetRichBusinessUnitWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.BusinessUnitIdentityDTO businessUnitIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.BusinessUnitIdentityDTO BusinessUnitIdentity
        {
            get
            {
                return this.businessUnitIdentity;
            }
            set
            {
                this.businessUnitIdentity = value;
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
    public partial class GetSimpleBusinessUnitWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.BusinessUnitIdentityDTO businessUnitIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.BusinessUnitIdentityDTO BusinessUnitIdentity
        {
            get
            {
                return this.businessUnitIdentity;
            }
            set
            {
                this.businessUnitIdentity = value;
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
    public partial class GetVisualBusinessUnitWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.BusinessUnitIdentityDTO businessUnitIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.BusinessUnitIdentityDTO BusinessUnitIdentity
        {
            get
            {
                return this.businessUnitIdentity;
            }
            set
            {
                this.businessUnitIdentity = value;
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
