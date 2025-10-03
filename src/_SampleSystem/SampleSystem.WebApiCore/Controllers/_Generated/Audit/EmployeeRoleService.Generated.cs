namespace SampleSystem.WebApiCore.Controllers.Audit
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/[controller]/[action]")]
    public partial class EmployeeRoleController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get EmployeeRole Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetEmployeeRolePropertyRevisionByDateRange(GetEmployeeRolePropertyRevisionByDateRangeAutoRequest getEmployeeRolePropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getEmployeeRolePropertyRevisionByDateRangeAutoRequest.Period;
            string propertyName = getEmployeeRolePropertyRevisionByDateRangeAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.EmployeeRoleIdentityDTO employeeRoleIdentity = getEmployeeRolePropertyRevisionByDateRangeAutoRequest.EmployeeRoleIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetEmployeeRolePropertyRevisionByDateRangeInternal(employeeRoleIdentity, propertyName, period, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetEmployeeRolePropertyRevisionByDateRangeInternal(SampleSystem.Generated.DTO.EmployeeRoleIdentityDTO employeeRoleIdentity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRoleBLL bll = evaluateData.Context.Logics.EmployeeRoleFactory.Create(SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.EmployeeRole>(employeeRoleIdentity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get EmployeeRole Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetEmployeeRolePropertyRevisions(GetEmployeeRolePropertyRevisionsAutoRequest getEmployeeRolePropertyRevisionsAutoRequest)
        {
            string propertyName = getEmployeeRolePropertyRevisionsAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.EmployeeRoleIdentityDTO employeeRoleIdentity = getEmployeeRolePropertyRevisionsAutoRequest.EmployeeRoleIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetEmployeeRolePropertyRevisionsInternal(employeeRoleIdentity, propertyName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetEmployeeRolePropertyRevisionsInternal(SampleSystem.Generated.DTO.EmployeeRoleIdentityDTO employeeRoleIdentity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRoleBLL bll = evaluateData.Context.Logics.EmployeeRoleFactory.Create(SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.EmployeeRole>(employeeRoleIdentity.Id, propertyName);
        }
        
        /// <summary>
        /// Get EmployeeRole revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetEmployeeRoleRevisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeeRoleIdentityDTO employeeRoleIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetEmployeeRoleRevisionsInternal(employeeRoleIdentity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetEmployeeRoleRevisionsInternal(SampleSystem.Generated.DTO.EmployeeRoleIdentityDTO employeeRoleIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRoleBLL bll = evaluateData.Context.Logics.EmployeeRoleFactory.Create(SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(employeeRoleIdentity.Id));
        }
        
        /// <summary>
        /// Get EmployeeRole (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.EmployeeRoleFullDTO GetFullEmployeeRoleWithRevision(GetFullEmployeeRoleWithRevisionAutoRequest getFullEmployeeRoleWithRevisionAutoRequest)
        {
            long revision = getFullEmployeeRoleWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.EmployeeRoleIdentityDTO employeeRoleIdentity = getFullEmployeeRoleWithRevisionAutoRequest.EmployeeRoleIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullEmployeeRoleWithRevisionInternal(employeeRoleIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeRoleFullDTO GetFullEmployeeRoleWithRevisionInternal(SampleSystem.Generated.DTO.EmployeeRoleIdentityDTO employeeRoleIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRoleBLL bll = evaluateData.Context.Logics.EmployeeRoleFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.EmployeeRole domainObject = bll.GetObjectByRevision(employeeRoleIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get EmployeeRole (RichDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.EmployeeRoleRichDTO GetRichEmployeeRoleWithRevision(GetRichEmployeeRoleWithRevisionAutoRequest getRichEmployeeRoleWithRevisionAutoRequest)
        {
            long revision = getRichEmployeeRoleWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.EmployeeRoleIdentityDTO employeeRoleIdentity = getRichEmployeeRoleWithRevisionAutoRequest.EmployeeRoleIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichEmployeeRoleWithRevisionInternal(employeeRoleIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeRoleRichDTO GetRichEmployeeRoleWithRevisionInternal(SampleSystem.Generated.DTO.EmployeeRoleIdentityDTO employeeRoleIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRoleBLL bll = evaluateData.Context.Logics.EmployeeRoleFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.EmployeeRole domainObject = bll.GetObjectByRevision(employeeRoleIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get EmployeeRole (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.EmployeeRoleSimpleDTO GetSimpleEmployeeRoleWithRevision(GetSimpleEmployeeRoleWithRevisionAutoRequest getSimpleEmployeeRoleWithRevisionAutoRequest)
        {
            long revision = getSimpleEmployeeRoleWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.EmployeeRoleIdentityDTO employeeRoleIdentity = getSimpleEmployeeRoleWithRevisionAutoRequest.EmployeeRoleIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleEmployeeRoleWithRevisionInternal(employeeRoleIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeRoleSimpleDTO GetSimpleEmployeeRoleWithRevisionInternal(SampleSystem.Generated.DTO.EmployeeRoleIdentityDTO employeeRoleIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRoleBLL bll = evaluateData.Context.Logics.EmployeeRoleFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.EmployeeRole domainObject = bll.GetObjectByRevision(employeeRoleIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get EmployeeRole (VisualDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.EmployeeRoleVisualDTO GetVisualEmployeeRoleWithRevision(GetVisualEmployeeRoleWithRevisionAutoRequest getVisualEmployeeRoleWithRevisionAutoRequest)
        {
            long revision = getVisualEmployeeRoleWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.EmployeeRoleIdentityDTO employeeRoleIdentity = getVisualEmployeeRoleWithRevisionAutoRequest.EmployeeRoleIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualEmployeeRoleWithRevisionInternal(employeeRoleIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeRoleVisualDTO GetVisualEmployeeRoleWithRevisionInternal(SampleSystem.Generated.DTO.EmployeeRoleIdentityDTO employeeRoleIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRoleBLL bll = evaluateData.Context.Logics.EmployeeRoleFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.EmployeeRole domainObject = bll.GetObjectByRevision(employeeRoleIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetEmployeeRolePropertyRevisionByDateRangeAutoRequest
    {
        
        private SampleSystem.Generated.DTO.EmployeeRoleIdentityDTO employeeRoleIdentity;
        
        private string propertyName;
        
        private Framework.Core.Period? period;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.EmployeeRoleIdentityDTO EmployeeRoleIdentity
        {
            get
            {
                return this.employeeRoleIdentity;
            }
            set
            {
                this.employeeRoleIdentity = value;
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
    public partial class GetEmployeeRolePropertyRevisionsAutoRequest
    {
        
        private SampleSystem.Generated.DTO.EmployeeRoleIdentityDTO employeeRoleIdentity;
        
        private string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.EmployeeRoleIdentityDTO EmployeeRoleIdentity
        {
            get
            {
                return this.employeeRoleIdentity;
            }
            set
            {
                this.employeeRoleIdentity = value;
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
    public partial class GetFullEmployeeRoleWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.EmployeeRoleIdentityDTO employeeRoleIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.EmployeeRoleIdentityDTO EmployeeRoleIdentity
        {
            get
            {
                return this.employeeRoleIdentity;
            }
            set
            {
                this.employeeRoleIdentity = value;
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
    public partial class GetRichEmployeeRoleWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.EmployeeRoleIdentityDTO employeeRoleIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.EmployeeRoleIdentityDTO EmployeeRoleIdentity
        {
            get
            {
                return this.employeeRoleIdentity;
            }
            set
            {
                this.employeeRoleIdentity = value;
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
    public partial class GetSimpleEmployeeRoleWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.EmployeeRoleIdentityDTO employeeRoleIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.EmployeeRoleIdentityDTO EmployeeRoleIdentity
        {
            get
            {
                return this.employeeRoleIdentity;
            }
            set
            {
                this.employeeRoleIdentity = value;
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
    public partial class GetVisualEmployeeRoleWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.EmployeeRoleIdentityDTO employeeRoleIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.EmployeeRoleIdentityDTO EmployeeRoleIdentity
        {
            get
            {
                return this.employeeRoleIdentity;
            }
            set
            {
                this.employeeRoleIdentity = value;
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
