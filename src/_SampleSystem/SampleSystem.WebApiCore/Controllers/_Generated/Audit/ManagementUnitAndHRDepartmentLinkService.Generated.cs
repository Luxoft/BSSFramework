namespace SampleSystem.WebApiCore.Controllers.Audit
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/[controller]/[action]")]
    public partial class ManagementUnitAndHRDepartmentLinkController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get ManagementUnitAndHRDepartmentLink (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkFullDTO GetFullManagementUnitAndHRDepartmentLinkWithRevision(GetFullManagementUnitAndHRDepartmentLinkWithRevisionAutoRequest getFullManagementUnitAndHRDepartmentLinkWithRevisionAutoRequest)
        {
            long revision = getFullManagementUnitAndHRDepartmentLinkWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO managementUnitAndHRDepartmentLinkIdentity = getFullManagementUnitAndHRDepartmentLinkWithRevisionAutoRequest.ManagementUnitAndHRDepartmentLinkIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullManagementUnitAndHRDepartmentLinkWithRevisionInternal(managementUnitAndHRDepartmentLinkIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkFullDTO GetFullManagementUnitAndHRDepartmentLinkWithRevisionInternal(SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO managementUnitAndHRDepartmentLinkIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitAndHRDepartmentLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndHRDepartmentLinkFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.ManagementUnitAndHRDepartmentLink domainObject = bll.GetObjectByRevision(managementUnitAndHRDepartmentLinkIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get ManagementUnitAndHRDepartmentLink Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetManagementUnitAndHRDepartmentLinkPropertyRevisionByDateRange(GetManagementUnitAndHRDepartmentLinkPropertyRevisionByDateRangeAutoRequest getManagementUnitAndHRDepartmentLinkPropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getManagementUnitAndHRDepartmentLinkPropertyRevisionByDateRangeAutoRequest.Period;
            string propertyName = getManagementUnitAndHRDepartmentLinkPropertyRevisionByDateRangeAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO managementUnitAndHRDepartmentLinkIdentity = getManagementUnitAndHRDepartmentLinkPropertyRevisionByDateRangeAutoRequest.ManagementUnitAndHRDepartmentLinkIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetManagementUnitAndHRDepartmentLinkPropertyRevisionByDateRangeInternal(managementUnitAndHRDepartmentLinkIdentity, propertyName, period, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetManagementUnitAndHRDepartmentLinkPropertyRevisionByDateRangeInternal(SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO managementUnitAndHRDepartmentLinkIdentity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitAndHRDepartmentLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndHRDepartmentLinkFactory.Create(SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.ManagementUnitAndHRDepartmentLink>(managementUnitAndHRDepartmentLinkIdentity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get ManagementUnitAndHRDepartmentLink Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetManagementUnitAndHRDepartmentLinkPropertyRevisions(GetManagementUnitAndHRDepartmentLinkPropertyRevisionsAutoRequest getManagementUnitAndHRDepartmentLinkPropertyRevisionsAutoRequest)
        {
            string propertyName = getManagementUnitAndHRDepartmentLinkPropertyRevisionsAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO managementUnitAndHRDepartmentLinkIdentity = getManagementUnitAndHRDepartmentLinkPropertyRevisionsAutoRequest.ManagementUnitAndHRDepartmentLinkIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetManagementUnitAndHRDepartmentLinkPropertyRevisionsInternal(managementUnitAndHRDepartmentLinkIdentity, propertyName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetManagementUnitAndHRDepartmentLinkPropertyRevisionsInternal(SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO managementUnitAndHRDepartmentLinkIdentity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitAndHRDepartmentLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndHRDepartmentLinkFactory.Create(SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.ManagementUnitAndHRDepartmentLink>(managementUnitAndHRDepartmentLinkIdentity.Id, propertyName);
        }
        
        /// <summary>
        /// Get ManagementUnitAndHRDepartmentLink revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetManagementUnitAndHRDepartmentLinkRevisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO managementUnitAndHRDepartmentLinkIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetManagementUnitAndHRDepartmentLinkRevisionsInternal(managementUnitAndHRDepartmentLinkIdentity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetManagementUnitAndHRDepartmentLinkRevisionsInternal(SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO managementUnitAndHRDepartmentLinkIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitAndHRDepartmentLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndHRDepartmentLinkFactory.Create(SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(managementUnitAndHRDepartmentLinkIdentity.Id));
        }
        
        /// <summary>
        /// Get ManagementUnitAndHRDepartmentLink (RichDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkRichDTO GetRichManagementUnitAndHRDepartmentLinkWithRevision(GetRichManagementUnitAndHRDepartmentLinkWithRevisionAutoRequest getRichManagementUnitAndHRDepartmentLinkWithRevisionAutoRequest)
        {
            long revision = getRichManagementUnitAndHRDepartmentLinkWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO managementUnitAndHRDepartmentLinkIdentity = getRichManagementUnitAndHRDepartmentLinkWithRevisionAutoRequest.ManagementUnitAndHRDepartmentLinkIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichManagementUnitAndHRDepartmentLinkWithRevisionInternal(managementUnitAndHRDepartmentLinkIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkRichDTO GetRichManagementUnitAndHRDepartmentLinkWithRevisionInternal(SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO managementUnitAndHRDepartmentLinkIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitAndHRDepartmentLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndHRDepartmentLinkFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.ManagementUnitAndHRDepartmentLink domainObject = bll.GetObjectByRevision(managementUnitAndHRDepartmentLinkIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get ManagementUnitAndHRDepartmentLink (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkSimpleDTO GetSimpleManagementUnitAndHRDepartmentLinkWithRevision(GetSimpleManagementUnitAndHRDepartmentLinkWithRevisionAutoRequest getSimpleManagementUnitAndHRDepartmentLinkWithRevisionAutoRequest)
        {
            long revision = getSimpleManagementUnitAndHRDepartmentLinkWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO managementUnitAndHRDepartmentLinkIdentity = getSimpleManagementUnitAndHRDepartmentLinkWithRevisionAutoRequest.ManagementUnitAndHRDepartmentLinkIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleManagementUnitAndHRDepartmentLinkWithRevisionInternal(managementUnitAndHRDepartmentLinkIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkSimpleDTO GetSimpleManagementUnitAndHRDepartmentLinkWithRevisionInternal(SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO managementUnitAndHRDepartmentLinkIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitAndHRDepartmentLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndHRDepartmentLinkFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.ManagementUnitAndHRDepartmentLink domainObject = bll.GetObjectByRevision(managementUnitAndHRDepartmentLinkIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetFullManagementUnitAndHRDepartmentLinkWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO managementUnitAndHRDepartmentLinkIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO ManagementUnitAndHRDepartmentLinkIdentity
        {
            get
            {
                return this.managementUnitAndHRDepartmentLinkIdentity;
            }
            set
            {
                this.managementUnitAndHRDepartmentLinkIdentity = value;
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
    public partial class GetManagementUnitAndHRDepartmentLinkPropertyRevisionByDateRangeAutoRequest
    {
        
        private SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO managementUnitAndHRDepartmentLinkIdentity;
        
        private string propertyName;
        
        private Framework.Core.Period? period;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO ManagementUnitAndHRDepartmentLinkIdentity
        {
            get
            {
                return this.managementUnitAndHRDepartmentLinkIdentity;
            }
            set
            {
                this.managementUnitAndHRDepartmentLinkIdentity = value;
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
    public partial class GetManagementUnitAndHRDepartmentLinkPropertyRevisionsAutoRequest
    {
        
        private SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO managementUnitAndHRDepartmentLinkIdentity;
        
        private string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO ManagementUnitAndHRDepartmentLinkIdentity
        {
            get
            {
                return this.managementUnitAndHRDepartmentLinkIdentity;
            }
            set
            {
                this.managementUnitAndHRDepartmentLinkIdentity = value;
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
    public partial class GetRichManagementUnitAndHRDepartmentLinkWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO managementUnitAndHRDepartmentLinkIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO ManagementUnitAndHRDepartmentLinkIdentity
        {
            get
            {
                return this.managementUnitAndHRDepartmentLinkIdentity;
            }
            set
            {
                this.managementUnitAndHRDepartmentLinkIdentity = value;
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
    public partial class GetSimpleManagementUnitAndHRDepartmentLinkWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO managementUnitAndHRDepartmentLinkIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.ManagementUnitAndHRDepartmentLinkIdentityDTO ManagementUnitAndHRDepartmentLinkIdentity
        {
            get
            {
                return this.managementUnitAndHRDepartmentLinkIdentity;
            }
            set
            {
                this.managementUnitAndHRDepartmentLinkIdentity = value;
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
