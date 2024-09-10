namespace SampleSystem.WebApiCore.Controllers.Audit
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/[controller]/[action]")]
    public partial class BusinessUnitManagerCommissionLinkController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get BusinessUnitManagerCommissionLink Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetBusinessUnitManagerCommissionLinkPropertyRevisionByDateRange(GetBusinessUnitManagerCommissionLinkPropertyRevisionByDateRangeAutoRequest getBusinessUnitManagerCommissionLinkPropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getBusinessUnitManagerCommissionLinkPropertyRevisionByDateRangeAutoRequest.Period;
            string propertyName = getBusinessUnitManagerCommissionLinkPropertyRevisionByDateRangeAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkIdentityDTO businessUnitManagerCommissionLinkIdentity = getBusinessUnitManagerCommissionLinkPropertyRevisionByDateRangeAutoRequest.BusinessUnitManagerCommissionLinkIdentity;
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
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetBusinessUnitManagerCommissionLinkPropertyRevisions(GetBusinessUnitManagerCommissionLinkPropertyRevisionsAutoRequest getBusinessUnitManagerCommissionLinkPropertyRevisionsAutoRequest)
        {
            string propertyName = getBusinessUnitManagerCommissionLinkPropertyRevisionsAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkIdentityDTO businessUnitManagerCommissionLinkIdentity = getBusinessUnitManagerCommissionLinkPropertyRevisionsAutoRequest.BusinessUnitManagerCommissionLinkIdentity;
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
        public virtual SampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkFullDTO GetFullBusinessUnitManagerCommissionLinkWithRevision(GetFullBusinessUnitManagerCommissionLinkWithRevisionAutoRequest getFullBusinessUnitManagerCommissionLinkWithRevisionAutoRequest)
        {
            long revision = getFullBusinessUnitManagerCommissionLinkWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkIdentityDTO businessUnitManagerCommissionLinkIdentity = getFullBusinessUnitManagerCommissionLinkWithRevisionAutoRequest.BusinessUnitManagerCommissionLinkIdentity;
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
        public virtual SampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkRichDTO GetRichBusinessUnitManagerCommissionLinkWithRevision(GetRichBusinessUnitManagerCommissionLinkWithRevisionAutoRequest getRichBusinessUnitManagerCommissionLinkWithRevisionAutoRequest)
        {
            long revision = getRichBusinessUnitManagerCommissionLinkWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkIdentityDTO businessUnitManagerCommissionLinkIdentity = getRichBusinessUnitManagerCommissionLinkWithRevisionAutoRequest.BusinessUnitManagerCommissionLinkIdentity;
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
        public virtual SampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkSimpleDTO GetSimpleBusinessUnitManagerCommissionLinkWithRevision(GetSimpleBusinessUnitManagerCommissionLinkWithRevisionAutoRequest getSimpleBusinessUnitManagerCommissionLinkWithRevisionAutoRequest)
        {
            long revision = getSimpleBusinessUnitManagerCommissionLinkWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkIdentityDTO businessUnitManagerCommissionLinkIdentity = getSimpleBusinessUnitManagerCommissionLinkWithRevisionAutoRequest.BusinessUnitManagerCommissionLinkIdentity;
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
        
        private SampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkIdentityDTO businessUnitManagerCommissionLinkIdentity;
        
        private string propertyName;
        
        private Framework.Core.Period? period;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkIdentityDTO BusinessUnitManagerCommissionLinkIdentity
        {
            get
            {
                return this.businessUnitManagerCommissionLinkIdentity;
            }
            set
            {
                this.businessUnitManagerCommissionLinkIdentity = value;
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
    public partial class GetBusinessUnitManagerCommissionLinkPropertyRevisionsAutoRequest
    {
        
        private SampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkIdentityDTO businessUnitManagerCommissionLinkIdentity;
        
        private string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkIdentityDTO BusinessUnitManagerCommissionLinkIdentity
        {
            get
            {
                return this.businessUnitManagerCommissionLinkIdentity;
            }
            set
            {
                this.businessUnitManagerCommissionLinkIdentity = value;
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
    public partial class GetFullBusinessUnitManagerCommissionLinkWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkIdentityDTO businessUnitManagerCommissionLinkIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkIdentityDTO BusinessUnitManagerCommissionLinkIdentity
        {
            get
            {
                return this.businessUnitManagerCommissionLinkIdentity;
            }
            set
            {
                this.businessUnitManagerCommissionLinkIdentity = value;
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
    public partial class GetRichBusinessUnitManagerCommissionLinkWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkIdentityDTO businessUnitManagerCommissionLinkIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkIdentityDTO BusinessUnitManagerCommissionLinkIdentity
        {
            get
            {
                return this.businessUnitManagerCommissionLinkIdentity;
            }
            set
            {
                this.businessUnitManagerCommissionLinkIdentity = value;
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
    public partial class GetSimpleBusinessUnitManagerCommissionLinkWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkIdentityDTO businessUnitManagerCommissionLinkIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkIdentityDTO BusinessUnitManagerCommissionLinkIdentity
        {
            get
            {
                return this.businessUnitManagerCommissionLinkIdentity;
            }
            set
            {
                this.businessUnitManagerCommissionLinkIdentity = value;
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
