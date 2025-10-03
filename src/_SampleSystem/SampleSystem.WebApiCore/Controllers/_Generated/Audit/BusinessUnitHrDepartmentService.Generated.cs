namespace SampleSystem.WebApiCore.Controllers.Audit
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/[controller]/[action]")]
    public partial class BusinessUnitHrDepartmentController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get BusinessUnitHrDepartment Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetBusinessUnitHrDepartmentPropertyRevisionByDateRange(GetBusinessUnitHrDepartmentPropertyRevisionByDateRangeAutoRequest getBusinessUnitHrDepartmentPropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getBusinessUnitHrDepartmentPropertyRevisionByDateRangeAutoRequest.Period;
            string propertyName = getBusinessUnitHrDepartmentPropertyRevisionByDateRangeAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO businessUnitHrDepartmentIdentity = getBusinessUnitHrDepartmentPropertyRevisionByDateRangeAutoRequest.BusinessUnitHrDepartmentIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetBusinessUnitHrDepartmentPropertyRevisionByDateRangeInternal(businessUnitHrDepartmentIdentity, propertyName, period, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetBusinessUnitHrDepartmentPropertyRevisionByDateRangeInternal(SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO businessUnitHrDepartmentIdentity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitHrDepartmentBLL bll = evaluateData.Context.Logics.BusinessUnitHrDepartmentFactory.Create(SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.BusinessUnitHrDepartment>(businessUnitHrDepartmentIdentity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get BusinessUnitHrDepartment Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetBusinessUnitHrDepartmentPropertyRevisions(GetBusinessUnitHrDepartmentPropertyRevisionsAutoRequest getBusinessUnitHrDepartmentPropertyRevisionsAutoRequest)
        {
            string propertyName = getBusinessUnitHrDepartmentPropertyRevisionsAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO businessUnitHrDepartmentIdentity = getBusinessUnitHrDepartmentPropertyRevisionsAutoRequest.BusinessUnitHrDepartmentIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetBusinessUnitHrDepartmentPropertyRevisionsInternal(businessUnitHrDepartmentIdentity, propertyName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetBusinessUnitHrDepartmentPropertyRevisionsInternal(SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO businessUnitHrDepartmentIdentity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitHrDepartmentBLL bll = evaluateData.Context.Logics.BusinessUnitHrDepartmentFactory.Create(SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.BusinessUnitHrDepartment>(businessUnitHrDepartmentIdentity.Id, propertyName);
        }
        
        /// <summary>
        /// Get BusinessUnitHrDepartment revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetBusinessUnitHrDepartmentRevisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO businessUnitHrDepartmentIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetBusinessUnitHrDepartmentRevisionsInternal(businessUnitHrDepartmentIdentity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetBusinessUnitHrDepartmentRevisionsInternal(SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO businessUnitHrDepartmentIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitHrDepartmentBLL bll = evaluateData.Context.Logics.BusinessUnitHrDepartmentFactory.Create(SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(businessUnitHrDepartmentIdentity.Id));
        }
        
        /// <summary>
        /// Get BusinessUnitHrDepartment (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.BusinessUnitHrDepartmentFullDTO GetFullBusinessUnitHrDepartmentWithRevision(GetFullBusinessUnitHrDepartmentWithRevisionAutoRequest getFullBusinessUnitHrDepartmentWithRevisionAutoRequest)
        {
            long revision = getFullBusinessUnitHrDepartmentWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO businessUnitHrDepartmentIdentity = getFullBusinessUnitHrDepartmentWithRevisionAutoRequest.BusinessUnitHrDepartmentIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullBusinessUnitHrDepartmentWithRevisionInternal(businessUnitHrDepartmentIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.BusinessUnitHrDepartmentFullDTO GetFullBusinessUnitHrDepartmentWithRevisionInternal(SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO businessUnitHrDepartmentIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitHrDepartmentBLL bll = evaluateData.Context.Logics.BusinessUnitHrDepartmentFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.BusinessUnitHrDepartment domainObject = bll.GetObjectByRevision(businessUnitHrDepartmentIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get BusinessUnitHrDepartment (RichDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.BusinessUnitHrDepartmentRichDTO GetRichBusinessUnitHrDepartmentWithRevision(GetRichBusinessUnitHrDepartmentWithRevisionAutoRequest getRichBusinessUnitHrDepartmentWithRevisionAutoRequest)
        {
            long revision = getRichBusinessUnitHrDepartmentWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO businessUnitHrDepartmentIdentity = getRichBusinessUnitHrDepartmentWithRevisionAutoRequest.BusinessUnitHrDepartmentIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichBusinessUnitHrDepartmentWithRevisionInternal(businessUnitHrDepartmentIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.BusinessUnitHrDepartmentRichDTO GetRichBusinessUnitHrDepartmentWithRevisionInternal(SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO businessUnitHrDepartmentIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitHrDepartmentBLL bll = evaluateData.Context.Logics.BusinessUnitHrDepartmentFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.BusinessUnitHrDepartment domainObject = bll.GetObjectByRevision(businessUnitHrDepartmentIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get BusinessUnitHrDepartment (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.BusinessUnitHrDepartmentSimpleDTO GetSimpleBusinessUnitHrDepartmentWithRevision(GetSimpleBusinessUnitHrDepartmentWithRevisionAutoRequest getSimpleBusinessUnitHrDepartmentWithRevisionAutoRequest)
        {
            long revision = getSimpleBusinessUnitHrDepartmentWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO businessUnitHrDepartmentIdentity = getSimpleBusinessUnitHrDepartmentWithRevisionAutoRequest.BusinessUnitHrDepartmentIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleBusinessUnitHrDepartmentWithRevisionInternal(businessUnitHrDepartmentIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.BusinessUnitHrDepartmentSimpleDTO GetSimpleBusinessUnitHrDepartmentWithRevisionInternal(SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO businessUnitHrDepartmentIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IBusinessUnitHrDepartmentBLL bll = evaluateData.Context.Logics.BusinessUnitHrDepartmentFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.BusinessUnitHrDepartment domainObject = bll.GetObjectByRevision(businessUnitHrDepartmentIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetBusinessUnitHrDepartmentPropertyRevisionByDateRangeAutoRequest
    {
        
        private SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO businessUnitHrDepartmentIdentity;
        
        private string propertyName;
        
        private Framework.Core.Period? period;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO BusinessUnitHrDepartmentIdentity
        {
            get
            {
                return this.businessUnitHrDepartmentIdentity;
            }
            set
            {
                this.businessUnitHrDepartmentIdentity = value;
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
    public partial class GetBusinessUnitHrDepartmentPropertyRevisionsAutoRequest
    {
        
        private SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO businessUnitHrDepartmentIdentity;
        
        private string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO BusinessUnitHrDepartmentIdentity
        {
            get
            {
                return this.businessUnitHrDepartmentIdentity;
            }
            set
            {
                this.businessUnitHrDepartmentIdentity = value;
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
    public partial class GetFullBusinessUnitHrDepartmentWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO businessUnitHrDepartmentIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO BusinessUnitHrDepartmentIdentity
        {
            get
            {
                return this.businessUnitHrDepartmentIdentity;
            }
            set
            {
                this.businessUnitHrDepartmentIdentity = value;
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
    public partial class GetRichBusinessUnitHrDepartmentWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO businessUnitHrDepartmentIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO BusinessUnitHrDepartmentIdentity
        {
            get
            {
                return this.businessUnitHrDepartmentIdentity;
            }
            set
            {
                this.businessUnitHrDepartmentIdentity = value;
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
    public partial class GetSimpleBusinessUnitHrDepartmentWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO businessUnitHrDepartmentIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.BusinessUnitHrDepartmentIdentityDTO BusinessUnitHrDepartmentIdentity
        {
            get
            {
                return this.businessUnitHrDepartmentIdentity;
            }
            set
            {
                this.businessUnitHrDepartmentIdentity = value;
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
