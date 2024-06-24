namespace SampleSystem.WebApiCore.Controllers.Audit
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/[controller]")]
    public partial class EmployeeRoleDegreeController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get EmployeeRoleDegree Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetEmployeeRoleDegreePropertyRevisionByDateRange")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetEmployeeRoleDegreePropertyRevisionByDateRange(GetEmployeeRoleDegreePropertyRevisionByDateRangeAutoRequest getEmployeeRoleDegreePropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getEmployeeRoleDegreePropertyRevisionByDateRangeAutoRequest.Period;
            string propertyName = getEmployeeRoleDegreePropertyRevisionByDateRangeAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdentity = getEmployeeRoleDegreePropertyRevisionByDateRangeAutoRequest.EmployeeRoleDegreeIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetEmployeeRoleDegreePropertyRevisionByDateRangeInternal(employeeRoleDegreeIdentity, propertyName, period, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetEmployeeRoleDegreePropertyRevisionByDateRangeInternal(SampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdentity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRoleDegreeBLL bll = evaluateData.Context.Logics.EmployeeRoleDegreeFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.EmployeeRoleDegree>(employeeRoleDegreeIdentity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get EmployeeRoleDegree Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetEmployeeRoleDegreePropertyRevisions")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetEmployeeRoleDegreePropertyRevisions(GetEmployeeRoleDegreePropertyRevisionsAutoRequest getEmployeeRoleDegreePropertyRevisionsAutoRequest)
        {
            string propertyName = getEmployeeRoleDegreePropertyRevisionsAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdentity = getEmployeeRoleDegreePropertyRevisionsAutoRequest.EmployeeRoleDegreeIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetEmployeeRoleDegreePropertyRevisionsInternal(employeeRoleDegreeIdentity, propertyName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetEmployeeRoleDegreePropertyRevisionsInternal(SampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdentity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRoleDegreeBLL bll = evaluateData.Context.Logics.EmployeeRoleDegreeFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.EmployeeRoleDegree>(employeeRoleDegreeIdentity.Id, propertyName);
        }
        
        /// <summary>
        /// Get EmployeeRoleDegree revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetEmployeeRoleDegreeRevisions")]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetEmployeeRoleDegreeRevisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetEmployeeRoleDegreeRevisionsInternal(employeeRoleDegreeIdentity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetEmployeeRoleDegreeRevisionsInternal(SampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRoleDegreeBLL bll = evaluateData.Context.Logics.EmployeeRoleDegreeFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(employeeRoleDegreeIdentity.Id));
        }
        
        /// <summary>
        /// Get EmployeeRoleDegree (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullEmployeeRoleDegreeWithRevision")]
        public virtual SampleSystem.Generated.DTO.EmployeeRoleDegreeFullDTO GetFullEmployeeRoleDegreeWithRevision(GetFullEmployeeRoleDegreeWithRevisionAutoRequest getFullEmployeeRoleDegreeWithRevisionAutoRequest)
        {
            long revision = getFullEmployeeRoleDegreeWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdentity = getFullEmployeeRoleDegreeWithRevisionAutoRequest.EmployeeRoleDegreeIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullEmployeeRoleDegreeWithRevisionInternal(employeeRoleDegreeIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeRoleDegreeFullDTO GetFullEmployeeRoleDegreeWithRevisionInternal(SampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRoleDegreeBLL bll = evaluateData.Context.Logics.EmployeeRoleDegreeFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.EmployeeRoleDegree domainObject = bll.GetObjectByRevision(employeeRoleDegreeIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get EmployeeRoleDegree (RichDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichEmployeeRoleDegreeWithRevision")]
        public virtual SampleSystem.Generated.DTO.EmployeeRoleDegreeRichDTO GetRichEmployeeRoleDegreeWithRevision(GetRichEmployeeRoleDegreeWithRevisionAutoRequest getRichEmployeeRoleDegreeWithRevisionAutoRequest)
        {
            long revision = getRichEmployeeRoleDegreeWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdentity = getRichEmployeeRoleDegreeWithRevisionAutoRequest.EmployeeRoleDegreeIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichEmployeeRoleDegreeWithRevisionInternal(employeeRoleDegreeIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeRoleDegreeRichDTO GetRichEmployeeRoleDegreeWithRevisionInternal(SampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRoleDegreeBLL bll = evaluateData.Context.Logics.EmployeeRoleDegreeFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.EmployeeRoleDegree domainObject = bll.GetObjectByRevision(employeeRoleDegreeIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get EmployeeRoleDegree (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleEmployeeRoleDegreeWithRevision")]
        public virtual SampleSystem.Generated.DTO.EmployeeRoleDegreeSimpleDTO GetSimpleEmployeeRoleDegreeWithRevision(GetSimpleEmployeeRoleDegreeWithRevisionAutoRequest getSimpleEmployeeRoleDegreeWithRevisionAutoRequest)
        {
            long revision = getSimpleEmployeeRoleDegreeWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdentity = getSimpleEmployeeRoleDegreeWithRevisionAutoRequest.EmployeeRoleDegreeIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleEmployeeRoleDegreeWithRevisionInternal(employeeRoleDegreeIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeRoleDegreeSimpleDTO GetSimpleEmployeeRoleDegreeWithRevisionInternal(SampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRoleDegreeBLL bll = evaluateData.Context.Logics.EmployeeRoleDegreeFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.EmployeeRoleDegree domainObject = bll.GetObjectByRevision(employeeRoleDegreeIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get EmployeeRoleDegree (VisualDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualEmployeeRoleDegreeWithRevision")]
        public virtual SampleSystem.Generated.DTO.EmployeeRoleDegreeVisualDTO GetVisualEmployeeRoleDegreeWithRevision(GetVisualEmployeeRoleDegreeWithRevisionAutoRequest getVisualEmployeeRoleDegreeWithRevisionAutoRequest)
        {
            long revision = getVisualEmployeeRoleDegreeWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdentity = getVisualEmployeeRoleDegreeWithRevisionAutoRequest.EmployeeRoleDegreeIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualEmployeeRoleDegreeWithRevisionInternal(employeeRoleDegreeIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeRoleDegreeVisualDTO GetVisualEmployeeRoleDegreeWithRevisionInternal(SampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRoleDegreeBLL bll = evaluateData.Context.Logics.EmployeeRoleDegreeFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.EmployeeRoleDegree domainObject = bll.GetObjectByRevision(employeeRoleDegreeIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetEmployeeRoleDegreePropertyRevisionByDateRangeAutoRequest
    {
        
        private SampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdentity;
        
        private string propertyName;
        
        private Framework.Core.Period? period;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO EmployeeRoleDegreeIdentity
        {
            get
            {
                return this.employeeRoleDegreeIdentity;
            }
            set
            {
                this.employeeRoleDegreeIdentity = value;
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
    public partial class GetEmployeeRoleDegreePropertyRevisionsAutoRequest
    {
        
        private SampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdentity;
        
        private string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO EmployeeRoleDegreeIdentity
        {
            get
            {
                return this.employeeRoleDegreeIdentity;
            }
            set
            {
                this.employeeRoleDegreeIdentity = value;
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
    public partial class GetFullEmployeeRoleDegreeWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO EmployeeRoleDegreeIdentity
        {
            get
            {
                return this.employeeRoleDegreeIdentity;
            }
            set
            {
                this.employeeRoleDegreeIdentity = value;
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
    public partial class GetRichEmployeeRoleDegreeWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO EmployeeRoleDegreeIdentity
        {
            get
            {
                return this.employeeRoleDegreeIdentity;
            }
            set
            {
                this.employeeRoleDegreeIdentity = value;
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
    public partial class GetSimpleEmployeeRoleDegreeWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO EmployeeRoleDegreeIdentity
        {
            get
            {
                return this.employeeRoleDegreeIdentity;
            }
            set
            {
                this.employeeRoleDegreeIdentity = value;
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
    public partial class GetVisualEmployeeRoleDegreeWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO EmployeeRoleDegreeIdentity
        {
            get
            {
                return this.employeeRoleDegreeIdentity;
            }
            set
            {
                this.employeeRoleDegreeIdentity = value;
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
