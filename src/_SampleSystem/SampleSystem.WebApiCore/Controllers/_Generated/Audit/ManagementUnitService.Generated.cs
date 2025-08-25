namespace SampleSystem.WebApiCore.Controllers.Audit
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/[controller]/[action]")]
    public partial class ManagementUnitController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get ManagementUnit (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.ManagementUnitFullDTO GetFullManagementUnitWithRevision(GetFullManagementUnitWithRevisionAutoRequest getFullManagementUnitWithRevisionAutoRequest)
        {
            long revision = getFullManagementUnitWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdentity = getFullManagementUnitWithRevisionAutoRequest.ManagementUnitIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullManagementUnitWithRevisionInternal(managementUnitIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.ManagementUnitFullDTO GetFullManagementUnitWithRevisionInternal(SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitBLL bll = evaluateData.Context.Logics.ManagementUnitFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.ManagementUnit domainObject = bll.GetObjectByRevision(managementUnitIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get ManagementUnit Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetManagementUnitPropertyRevisionByDateRange(GetManagementUnitPropertyRevisionByDateRangeAutoRequest getManagementUnitPropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getManagementUnitPropertyRevisionByDateRangeAutoRequest.Period;
            string propertyName = getManagementUnitPropertyRevisionByDateRangeAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdentity = getManagementUnitPropertyRevisionByDateRangeAutoRequest.ManagementUnitIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetManagementUnitPropertyRevisionByDateRangeInternal(managementUnitIdentity, propertyName, period, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetManagementUnitPropertyRevisionByDateRangeInternal(SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdentity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitBLL bll = evaluateData.Context.Logics.ManagementUnitFactory.Create(SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.ManagementUnit>(managementUnitIdentity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get ManagementUnit Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetManagementUnitPropertyRevisions(GetManagementUnitPropertyRevisionsAutoRequest getManagementUnitPropertyRevisionsAutoRequest)
        {
            string propertyName = getManagementUnitPropertyRevisionsAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdentity = getManagementUnitPropertyRevisionsAutoRequest.ManagementUnitIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetManagementUnitPropertyRevisionsInternal(managementUnitIdentity, propertyName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetManagementUnitPropertyRevisionsInternal(SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdentity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitBLL bll = evaluateData.Context.Logics.ManagementUnitFactory.Create(SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.ManagementUnit>(managementUnitIdentity.Id, propertyName);
        }
        
        /// <summary>
        /// Get ManagementUnit revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetManagementUnitRevisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetManagementUnitRevisionsInternal(managementUnitIdentity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetManagementUnitRevisionsInternal(SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitBLL bll = evaluateData.Context.Logics.ManagementUnitFactory.Create(SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(managementUnitIdentity.Id));
        }
        
        /// <summary>
        /// Get ManagementUnit (RichDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.ManagementUnitRichDTO GetRichManagementUnitWithRevision(GetRichManagementUnitWithRevisionAutoRequest getRichManagementUnitWithRevisionAutoRequest)
        {
            long revision = getRichManagementUnitWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdentity = getRichManagementUnitWithRevisionAutoRequest.ManagementUnitIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichManagementUnitWithRevisionInternal(managementUnitIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.ManagementUnitRichDTO GetRichManagementUnitWithRevisionInternal(SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitBLL bll = evaluateData.Context.Logics.ManagementUnitFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.ManagementUnit domainObject = bll.GetObjectByRevision(managementUnitIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get ManagementUnit (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.ManagementUnitSimpleDTO GetSimpleManagementUnitWithRevision(GetSimpleManagementUnitWithRevisionAutoRequest getSimpleManagementUnitWithRevisionAutoRequest)
        {
            long revision = getSimpleManagementUnitWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdentity = getSimpleManagementUnitWithRevisionAutoRequest.ManagementUnitIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleManagementUnitWithRevisionInternal(managementUnitIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.ManagementUnitSimpleDTO GetSimpleManagementUnitWithRevisionInternal(SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitBLL bll = evaluateData.Context.Logics.ManagementUnitFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.ManagementUnit domainObject = bll.GetObjectByRevision(managementUnitIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get ManagementUnit (VisualDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.ManagementUnitVisualDTO GetVisualManagementUnitWithRevision(GetVisualManagementUnitWithRevisionAutoRequest getVisualManagementUnitWithRevisionAutoRequest)
        {
            long revision = getVisualManagementUnitWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdentity = getVisualManagementUnitWithRevisionAutoRequest.ManagementUnitIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualManagementUnitWithRevisionInternal(managementUnitIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.ManagementUnitVisualDTO GetVisualManagementUnitWithRevisionInternal(SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitBLL bll = evaluateData.Context.Logics.ManagementUnitFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.ManagementUnit domainObject = bll.GetObjectByRevision(managementUnitIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetFullManagementUnitWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.ManagementUnitIdentityDTO ManagementUnitIdentity
        {
            get
            {
                return this.managementUnitIdentity;
            }
            set
            {
                this.managementUnitIdentity = value;
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
    public partial class GetManagementUnitPropertyRevisionByDateRangeAutoRequest
    {
        
        private SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdentity;
        
        private string propertyName;
        
        private Framework.Core.Period? period;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.ManagementUnitIdentityDTO ManagementUnitIdentity
        {
            get
            {
                return this.managementUnitIdentity;
            }
            set
            {
                this.managementUnitIdentity = value;
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
    public partial class GetManagementUnitPropertyRevisionsAutoRequest
    {
        
        private SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdentity;
        
        private string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.ManagementUnitIdentityDTO ManagementUnitIdentity
        {
            get
            {
                return this.managementUnitIdentity;
            }
            set
            {
                this.managementUnitIdentity = value;
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
    public partial class GetRichManagementUnitWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.ManagementUnitIdentityDTO ManagementUnitIdentity
        {
            get
            {
                return this.managementUnitIdentity;
            }
            set
            {
                this.managementUnitIdentity = value;
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
    public partial class GetSimpleManagementUnitWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.ManagementUnitIdentityDTO ManagementUnitIdentity
        {
            get
            {
                return this.managementUnitIdentity;
            }
            set
            {
                this.managementUnitIdentity = value;
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
    public partial class GetVisualManagementUnitWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.ManagementUnitIdentityDTO ManagementUnitIdentity
        {
            get
            {
                return this.managementUnitIdentity;
            }
            set
            {
                this.managementUnitIdentity = value;
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
