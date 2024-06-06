namespace SampleSystem.WebApiCore.Controllers.Audit
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/[controller]")]
    public partial class EmployeeController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get Employee Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetEmployeePropertyRevisionByDateRange")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetEmployeePropertyRevisionByDateRange(GetEmployeePropertyRevisionByDateRangeAutoRequest getEmployeePropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getEmployeePropertyRevisionByDateRangeAutoRequest.Period;
            string propertyName = getEmployeePropertyRevisionByDateRangeAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.EmployeeIdentityDTO employeeIdentity = getEmployeePropertyRevisionByDateRangeAutoRequest.EmployeeIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetEmployeePropertyRevisionByDateRangeInternal(employeeIdentity, propertyName, period, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetEmployeePropertyRevisionByDateRangeInternal(SampleSystem.Generated.DTO.EmployeeIdentityDTO employeeIdentity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeBLL bll = evaluateData.Context.Logics.EmployeeFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.Employee>(employeeIdentity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get Employee Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetEmployeePropertyRevisions")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetEmployeePropertyRevisions(GetEmployeePropertyRevisionsAutoRequest getEmployeePropertyRevisionsAutoRequest)
        {
            string propertyName = getEmployeePropertyRevisionsAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.EmployeeIdentityDTO employeeIdentity = getEmployeePropertyRevisionsAutoRequest.EmployeeIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetEmployeePropertyRevisionsInternal(employeeIdentity, propertyName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetEmployeePropertyRevisionsInternal(SampleSystem.Generated.DTO.EmployeeIdentityDTO employeeIdentity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeBLL bll = evaluateData.Context.Logics.EmployeeFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.Employee>(employeeIdentity.Id, propertyName);
        }
        
        /// <summary>
        /// Get Employee revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetEmployeeRevisions")]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetEmployeeRevisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeeIdentityDTO employeeIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetEmployeeRevisionsInternal(employeeIdentity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetEmployeeRevisionsInternal(SampleSystem.Generated.DTO.EmployeeIdentityDTO employeeIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeBLL bll = evaluateData.Context.Logics.EmployeeFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(employeeIdentity.Id));
        }
        
        /// <summary>
        /// Get Employee (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullEmployeeWithRevision")]
        public virtual SampleSystem.Generated.DTO.EmployeeFullDTO GetFullEmployeeWithRevision(GetFullEmployeeWithRevisionAutoRequest getFullEmployeeWithRevisionAutoRequest)
        {
            long revision = getFullEmployeeWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.EmployeeIdentityDTO employeeIdentity = getFullEmployeeWithRevisionAutoRequest.EmployeeIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullEmployeeWithRevisionInternal(employeeIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeFullDTO GetFullEmployeeWithRevisionInternal(SampleSystem.Generated.DTO.EmployeeIdentityDTO employeeIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeBLL bll = evaluateData.Context.Logics.EmployeeFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Employee domainObject = bll.GetObjectByRevision(employeeIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Employee (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleEmployeeWithRevision")]
        public virtual SampleSystem.Generated.DTO.EmployeeSimpleDTO GetSimpleEmployeeWithRevision(GetSimpleEmployeeWithRevisionAutoRequest getSimpleEmployeeWithRevisionAutoRequest)
        {
            long revision = getSimpleEmployeeWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.EmployeeIdentityDTO employeeIdentity = getSimpleEmployeeWithRevisionAutoRequest.EmployeeIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleEmployeeWithRevisionInternal(employeeIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeSimpleDTO GetSimpleEmployeeWithRevisionInternal(SampleSystem.Generated.DTO.EmployeeIdentityDTO employeeIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeBLL bll = evaluateData.Context.Logics.EmployeeFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Employee domainObject = bll.GetObjectByRevision(employeeIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetEmployeePropertyRevisionByDateRangeAutoRequest
    {
        
        private SampleSystem.Generated.DTO.EmployeeIdentityDTO employeeIdentity;
        
        private string propertyName;
        
        private Framework.Core.Period? period;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.EmployeeIdentityDTO EmployeeIdentity
        {
            get
            {
                return this.employeeIdentity;
            }
            set
            {
                this.employeeIdentity = value;
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
    public partial class GetEmployeePropertyRevisionsAutoRequest
    {
        
        private SampleSystem.Generated.DTO.EmployeeIdentityDTO employeeIdentity;
        
        private string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.EmployeeIdentityDTO EmployeeIdentity
        {
            get
            {
                return this.employeeIdentity;
            }
            set
            {
                this.employeeIdentity = value;
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
    public partial class GetFullEmployeeWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.EmployeeIdentityDTO employeeIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.EmployeeIdentityDTO EmployeeIdentity
        {
            get
            {
                return this.employeeIdentity;
            }
            set
            {
                this.employeeIdentity = value;
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
    public partial class GetSimpleEmployeeWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.EmployeeIdentityDTO employeeIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.EmployeeIdentityDTO EmployeeIdentity
        {
            get
            {
                return this.employeeIdentity;
            }
            set
            {
                this.employeeIdentity = value;
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
