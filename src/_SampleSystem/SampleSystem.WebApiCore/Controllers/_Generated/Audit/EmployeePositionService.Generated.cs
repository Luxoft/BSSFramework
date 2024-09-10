namespace SampleSystem.WebApiCore.Controllers.Audit
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/[controller]/[action]")]
    public partial class EmployeePositionController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get EmployeePosition Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetEmployeePositionPropertyRevisionByDateRange(GetEmployeePositionPropertyRevisionByDateRangeAutoRequest getEmployeePositionPropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getEmployeePositionPropertyRevisionByDateRangeAutoRequest.Period;
            string propertyName = getEmployeePositionPropertyRevisionByDateRangeAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.EmployeePositionIdentityDTO employeePositionIdentity = getEmployeePositionPropertyRevisionByDateRangeAutoRequest.EmployeePositionIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetEmployeePositionPropertyRevisionByDateRangeInternal(employeePositionIdentity, propertyName, period, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetEmployeePositionPropertyRevisionByDateRangeInternal(SampleSystem.Generated.DTO.EmployeePositionIdentityDTO employeePositionIdentity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeePositionBLL bll = evaluateData.Context.Logics.EmployeePositionFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.EmployeePosition>(employeePositionIdentity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get EmployeePosition Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetEmployeePositionPropertyRevisions(GetEmployeePositionPropertyRevisionsAutoRequest getEmployeePositionPropertyRevisionsAutoRequest)
        {
            string propertyName = getEmployeePositionPropertyRevisionsAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.EmployeePositionIdentityDTO employeePositionIdentity = getEmployeePositionPropertyRevisionsAutoRequest.EmployeePositionIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetEmployeePositionPropertyRevisionsInternal(employeePositionIdentity, propertyName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetEmployeePositionPropertyRevisionsInternal(SampleSystem.Generated.DTO.EmployeePositionIdentityDTO employeePositionIdentity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeePositionBLL bll = evaluateData.Context.Logics.EmployeePositionFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.EmployeePosition>(employeePositionIdentity.Id, propertyName);
        }
        
        /// <summary>
        /// Get EmployeePosition revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetEmployeePositionRevisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeePositionIdentityDTO employeePositionIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetEmployeePositionRevisionsInternal(employeePositionIdentity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetEmployeePositionRevisionsInternal(SampleSystem.Generated.DTO.EmployeePositionIdentityDTO employeePositionIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeePositionBLL bll = evaluateData.Context.Logics.EmployeePositionFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(employeePositionIdentity.Id));
        }
        
        /// <summary>
        /// Get EmployeePosition (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.EmployeePositionFullDTO GetFullEmployeePositionWithRevision(GetFullEmployeePositionWithRevisionAutoRequest getFullEmployeePositionWithRevisionAutoRequest)
        {
            long revision = getFullEmployeePositionWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.EmployeePositionIdentityDTO employeePositionIdentity = getFullEmployeePositionWithRevisionAutoRequest.EmployeePositionIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullEmployeePositionWithRevisionInternal(employeePositionIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeePositionFullDTO GetFullEmployeePositionWithRevisionInternal(SampleSystem.Generated.DTO.EmployeePositionIdentityDTO employeePositionIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeePositionBLL bll = evaluateData.Context.Logics.EmployeePositionFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.EmployeePosition domainObject = bll.GetObjectByRevision(employeePositionIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get EmployeePosition (RichDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.EmployeePositionRichDTO GetRichEmployeePositionWithRevision(GetRichEmployeePositionWithRevisionAutoRequest getRichEmployeePositionWithRevisionAutoRequest)
        {
            long revision = getRichEmployeePositionWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.EmployeePositionIdentityDTO employeePositionIdentity = getRichEmployeePositionWithRevisionAutoRequest.EmployeePositionIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichEmployeePositionWithRevisionInternal(employeePositionIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeePositionRichDTO GetRichEmployeePositionWithRevisionInternal(SampleSystem.Generated.DTO.EmployeePositionIdentityDTO employeePositionIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeePositionBLL bll = evaluateData.Context.Logics.EmployeePositionFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.EmployeePosition domainObject = bll.GetObjectByRevision(employeePositionIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get EmployeePosition (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.EmployeePositionSimpleDTO GetSimpleEmployeePositionWithRevision(GetSimpleEmployeePositionWithRevisionAutoRequest getSimpleEmployeePositionWithRevisionAutoRequest)
        {
            long revision = getSimpleEmployeePositionWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.EmployeePositionIdentityDTO employeePositionIdentity = getSimpleEmployeePositionWithRevisionAutoRequest.EmployeePositionIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleEmployeePositionWithRevisionInternal(employeePositionIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeePositionSimpleDTO GetSimpleEmployeePositionWithRevisionInternal(SampleSystem.Generated.DTO.EmployeePositionIdentityDTO employeePositionIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeePositionBLL bll = evaluateData.Context.Logics.EmployeePositionFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.EmployeePosition domainObject = bll.GetObjectByRevision(employeePositionIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get EmployeePosition (VisualDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.EmployeePositionVisualDTO GetVisualEmployeePositionWithRevision(GetVisualEmployeePositionWithRevisionAutoRequest getVisualEmployeePositionWithRevisionAutoRequest)
        {
            long revision = getVisualEmployeePositionWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.EmployeePositionIdentityDTO employeePositionIdentity = getVisualEmployeePositionWithRevisionAutoRequest.EmployeePositionIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualEmployeePositionWithRevisionInternal(employeePositionIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeePositionVisualDTO GetVisualEmployeePositionWithRevisionInternal(SampleSystem.Generated.DTO.EmployeePositionIdentityDTO employeePositionIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeePositionBLL bll = evaluateData.Context.Logics.EmployeePositionFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.EmployeePosition domainObject = bll.GetObjectByRevision(employeePositionIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetEmployeePositionPropertyRevisionByDateRangeAutoRequest
    {
        
        private SampleSystem.Generated.DTO.EmployeePositionIdentityDTO employeePositionIdentity;
        
        private string propertyName;
        
        private Framework.Core.Period? period;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.EmployeePositionIdentityDTO EmployeePositionIdentity
        {
            get
            {
                return this.employeePositionIdentity;
            }
            set
            {
                this.employeePositionIdentity = value;
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
    public partial class GetEmployeePositionPropertyRevisionsAutoRequest
    {
        
        private SampleSystem.Generated.DTO.EmployeePositionIdentityDTO employeePositionIdentity;
        
        private string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.EmployeePositionIdentityDTO EmployeePositionIdentity
        {
            get
            {
                return this.employeePositionIdentity;
            }
            set
            {
                this.employeePositionIdentity = value;
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
    public partial class GetFullEmployeePositionWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.EmployeePositionIdentityDTO employeePositionIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.EmployeePositionIdentityDTO EmployeePositionIdentity
        {
            get
            {
                return this.employeePositionIdentity;
            }
            set
            {
                this.employeePositionIdentity = value;
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
    public partial class GetRichEmployeePositionWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.EmployeePositionIdentityDTO employeePositionIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.EmployeePositionIdentityDTO EmployeePositionIdentity
        {
            get
            {
                return this.employeePositionIdentity;
            }
            set
            {
                this.employeePositionIdentity = value;
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
    public partial class GetSimpleEmployeePositionWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.EmployeePositionIdentityDTO employeePositionIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.EmployeePositionIdentityDTO EmployeePositionIdentity
        {
            get
            {
                return this.employeePositionIdentity;
            }
            set
            {
                this.employeePositionIdentity = value;
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
    public partial class GetVisualEmployeePositionWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.EmployeePositionIdentityDTO employeePositionIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.EmployeePositionIdentityDTO EmployeePositionIdentity
        {
            get
            {
                return this.employeePositionIdentity;
            }
            set
            {
                this.employeePositionIdentity = value;
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
