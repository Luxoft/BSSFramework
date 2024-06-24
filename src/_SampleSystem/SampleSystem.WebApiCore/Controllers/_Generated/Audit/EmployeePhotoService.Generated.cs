namespace SampleSystem.WebApiCore.Controllers.Audit
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/[controller]")]
    public partial class EmployeePhotoController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get EmployeePhoto Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetEmployeePhotoPropertyRevisionByDateRange")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetEmployeePhotoPropertyRevisionByDateRange(GetEmployeePhotoPropertyRevisionByDateRangeAutoRequest getEmployeePhotoPropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getEmployeePhotoPropertyRevisionByDateRangeAutoRequest.Period;
            string propertyName = getEmployeePhotoPropertyRevisionByDateRangeAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.EmployeePhotoIdentityDTO employeePhotoIdentity = getEmployeePhotoPropertyRevisionByDateRangeAutoRequest.EmployeePhotoIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetEmployeePhotoPropertyRevisionByDateRangeInternal(employeePhotoIdentity, propertyName, period, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetEmployeePhotoPropertyRevisionByDateRangeInternal(SampleSystem.Generated.DTO.EmployeePhotoIdentityDTO employeePhotoIdentity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeePhotoBLL bll = evaluateData.Context.Logics.EmployeePhotoFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.EmployeePhoto>(employeePhotoIdentity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get EmployeePhoto Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetEmployeePhotoPropertyRevisions")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetEmployeePhotoPropertyRevisions(GetEmployeePhotoPropertyRevisionsAutoRequest getEmployeePhotoPropertyRevisionsAutoRequest)
        {
            string propertyName = getEmployeePhotoPropertyRevisionsAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.EmployeePhotoIdentityDTO employeePhotoIdentity = getEmployeePhotoPropertyRevisionsAutoRequest.EmployeePhotoIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetEmployeePhotoPropertyRevisionsInternal(employeePhotoIdentity, propertyName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetEmployeePhotoPropertyRevisionsInternal(SampleSystem.Generated.DTO.EmployeePhotoIdentityDTO employeePhotoIdentity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeePhotoBLL bll = evaluateData.Context.Logics.EmployeePhotoFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.EmployeePhoto>(employeePhotoIdentity.Id, propertyName);
        }
        
        /// <summary>
        /// Get EmployeePhoto revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetEmployeePhotoRevisions")]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetEmployeePhotoRevisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeePhotoIdentityDTO employeePhotoIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetEmployeePhotoRevisionsInternal(employeePhotoIdentity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetEmployeePhotoRevisionsInternal(SampleSystem.Generated.DTO.EmployeePhotoIdentityDTO employeePhotoIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeePhotoBLL bll = evaluateData.Context.Logics.EmployeePhotoFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(employeePhotoIdentity.Id));
        }
        
        /// <summary>
        /// Get EmployeePhoto (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullEmployeePhotoWithRevision")]
        public virtual SampleSystem.Generated.DTO.EmployeePhotoFullDTO GetFullEmployeePhotoWithRevision(GetFullEmployeePhotoWithRevisionAutoRequest getFullEmployeePhotoWithRevisionAutoRequest)
        {
            long revision = getFullEmployeePhotoWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.EmployeePhotoIdentityDTO employeePhotoIdentity = getFullEmployeePhotoWithRevisionAutoRequest.EmployeePhotoIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullEmployeePhotoWithRevisionInternal(employeePhotoIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeePhotoFullDTO GetFullEmployeePhotoWithRevisionInternal(SampleSystem.Generated.DTO.EmployeePhotoIdentityDTO employeePhotoIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeePhotoBLL bll = evaluateData.Context.Logics.EmployeePhotoFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.EmployeePhoto domainObject = bll.GetObjectByRevision(employeePhotoIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get EmployeePhoto (RichDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichEmployeePhotoWithRevision")]
        public virtual SampleSystem.Generated.DTO.EmployeePhotoRichDTO GetRichEmployeePhotoWithRevision(GetRichEmployeePhotoWithRevisionAutoRequest getRichEmployeePhotoWithRevisionAutoRequest)
        {
            long revision = getRichEmployeePhotoWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.EmployeePhotoIdentityDTO employeePhotoIdentity = getRichEmployeePhotoWithRevisionAutoRequest.EmployeePhotoIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichEmployeePhotoWithRevisionInternal(employeePhotoIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeePhotoRichDTO GetRichEmployeePhotoWithRevisionInternal(SampleSystem.Generated.DTO.EmployeePhotoIdentityDTO employeePhotoIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeePhotoBLL bll = evaluateData.Context.Logics.EmployeePhotoFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.EmployeePhoto domainObject = bll.GetObjectByRevision(employeePhotoIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get EmployeePhoto (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleEmployeePhotoWithRevision")]
        public virtual SampleSystem.Generated.DTO.EmployeePhotoSimpleDTO GetSimpleEmployeePhotoWithRevision(GetSimpleEmployeePhotoWithRevisionAutoRequest getSimpleEmployeePhotoWithRevisionAutoRequest)
        {
            long revision = getSimpleEmployeePhotoWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.EmployeePhotoIdentityDTO employeePhotoIdentity = getSimpleEmployeePhotoWithRevisionAutoRequest.EmployeePhotoIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleEmployeePhotoWithRevisionInternal(employeePhotoIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeePhotoSimpleDTO GetSimpleEmployeePhotoWithRevisionInternal(SampleSystem.Generated.DTO.EmployeePhotoIdentityDTO employeePhotoIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeePhotoBLL bll = evaluateData.Context.Logics.EmployeePhotoFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.EmployeePhoto domainObject = bll.GetObjectByRevision(employeePhotoIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetEmployeePhotoPropertyRevisionByDateRangeAutoRequest
    {
        
        private SampleSystem.Generated.DTO.EmployeePhotoIdentityDTO employeePhotoIdentity;
        
        private string propertyName;
        
        private Framework.Core.Period? period;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.EmployeePhotoIdentityDTO EmployeePhotoIdentity
        {
            get
            {
                return this.employeePhotoIdentity;
            }
            set
            {
                this.employeePhotoIdentity = value;
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
    public partial class GetEmployeePhotoPropertyRevisionsAutoRequest
    {
        
        private SampleSystem.Generated.DTO.EmployeePhotoIdentityDTO employeePhotoIdentity;
        
        private string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.EmployeePhotoIdentityDTO EmployeePhotoIdentity
        {
            get
            {
                return this.employeePhotoIdentity;
            }
            set
            {
                this.employeePhotoIdentity = value;
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
    public partial class GetFullEmployeePhotoWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.EmployeePhotoIdentityDTO employeePhotoIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.EmployeePhotoIdentityDTO EmployeePhotoIdentity
        {
            get
            {
                return this.employeePhotoIdentity;
            }
            set
            {
                this.employeePhotoIdentity = value;
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
    public partial class GetRichEmployeePhotoWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.EmployeePhotoIdentityDTO employeePhotoIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.EmployeePhotoIdentityDTO EmployeePhotoIdentity
        {
            get
            {
                return this.employeePhotoIdentity;
            }
            set
            {
                this.employeePhotoIdentity = value;
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
    public partial class GetSimpleEmployeePhotoWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.EmployeePhotoIdentityDTO employeePhotoIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.EmployeePhotoIdentityDTO EmployeePhotoIdentity
        {
            get
            {
                return this.employeePhotoIdentity;
            }
            set
            {
                this.employeePhotoIdentity = value;
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
