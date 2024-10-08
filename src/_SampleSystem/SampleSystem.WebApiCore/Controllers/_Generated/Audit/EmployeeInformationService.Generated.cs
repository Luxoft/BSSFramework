﻿namespace SampleSystem.WebApiCore.Controllers.Audit
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/[controller]/[action]")]
    public partial class EmployeeInformationController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get EmployeeInformation Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetEmployeeInformationPropertyRevisionByDateRange(GetEmployeeInformationPropertyRevisionByDateRangeAutoRequest getEmployeeInformationPropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getEmployeeInformationPropertyRevisionByDateRangeAutoRequest.Period;
            string propertyName = getEmployeeInformationPropertyRevisionByDateRangeAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.EmployeeInformationIdentityDTO employeeInformationIdentity = getEmployeeInformationPropertyRevisionByDateRangeAutoRequest.EmployeeInformationIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetEmployeeInformationPropertyRevisionByDateRangeInternal(employeeInformationIdentity, propertyName, period, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetEmployeeInformationPropertyRevisionByDateRangeInternal(SampleSystem.Generated.DTO.EmployeeInformationIdentityDTO employeeInformationIdentity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeInformationBLL bll = evaluateData.Context.Logics.EmployeeInformationFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.EmployeeInformation>(employeeInformationIdentity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get EmployeeInformation Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetEmployeeInformationPropertyRevisions(GetEmployeeInformationPropertyRevisionsAutoRequest getEmployeeInformationPropertyRevisionsAutoRequest)
        {
            string propertyName = getEmployeeInformationPropertyRevisionsAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.EmployeeInformationIdentityDTO employeeInformationIdentity = getEmployeeInformationPropertyRevisionsAutoRequest.EmployeeInformationIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetEmployeeInformationPropertyRevisionsInternal(employeeInformationIdentity, propertyName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetEmployeeInformationPropertyRevisionsInternal(SampleSystem.Generated.DTO.EmployeeInformationIdentityDTO employeeInformationIdentity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeInformationBLL bll = evaluateData.Context.Logics.EmployeeInformationFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.EmployeeInformation>(employeeInformationIdentity.Id, propertyName);
        }
        
        /// <summary>
        /// Get EmployeeInformation revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetEmployeeInformationRevisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeeInformationIdentityDTO employeeInformationIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetEmployeeInformationRevisionsInternal(employeeInformationIdentity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetEmployeeInformationRevisionsInternal(SampleSystem.Generated.DTO.EmployeeInformationIdentityDTO employeeInformationIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeInformationBLL bll = evaluateData.Context.Logics.EmployeeInformationFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(employeeInformationIdentity.Id));
        }
        
        /// <summary>
        /// Get EmployeeInformation (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.EmployeeInformationFullDTO GetFullEmployeeInformationWithRevision(GetFullEmployeeInformationWithRevisionAutoRequest getFullEmployeeInformationWithRevisionAutoRequest)
        {
            long revision = getFullEmployeeInformationWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.EmployeeInformationIdentityDTO employeeInformationIdentity = getFullEmployeeInformationWithRevisionAutoRequest.EmployeeInformationIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullEmployeeInformationWithRevisionInternal(employeeInformationIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeInformationFullDTO GetFullEmployeeInformationWithRevisionInternal(SampleSystem.Generated.DTO.EmployeeInformationIdentityDTO employeeInformationIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeInformationBLL bll = evaluateData.Context.Logics.EmployeeInformationFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.EmployeeInformation domainObject = bll.GetObjectByRevision(employeeInformationIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get EmployeeInformation (RichDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.EmployeeInformationRichDTO GetRichEmployeeInformationWithRevision(GetRichEmployeeInformationWithRevisionAutoRequest getRichEmployeeInformationWithRevisionAutoRequest)
        {
            long revision = getRichEmployeeInformationWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.EmployeeInformationIdentityDTO employeeInformationIdentity = getRichEmployeeInformationWithRevisionAutoRequest.EmployeeInformationIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichEmployeeInformationWithRevisionInternal(employeeInformationIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeInformationRichDTO GetRichEmployeeInformationWithRevisionInternal(SampleSystem.Generated.DTO.EmployeeInformationIdentityDTO employeeInformationIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeInformationBLL bll = evaluateData.Context.Logics.EmployeeInformationFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.EmployeeInformation domainObject = bll.GetObjectByRevision(employeeInformationIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get EmployeeInformation (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.EmployeeInformationSimpleDTO GetSimpleEmployeeInformationWithRevision(GetSimpleEmployeeInformationWithRevisionAutoRequest getSimpleEmployeeInformationWithRevisionAutoRequest)
        {
            long revision = getSimpleEmployeeInformationWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.EmployeeInformationIdentityDTO employeeInformationIdentity = getSimpleEmployeeInformationWithRevisionAutoRequest.EmployeeInformationIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleEmployeeInformationWithRevisionInternal(employeeInformationIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeInformationSimpleDTO GetSimpleEmployeeInformationWithRevisionInternal(SampleSystem.Generated.DTO.EmployeeInformationIdentityDTO employeeInformationIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeInformationBLL bll = evaluateData.Context.Logics.EmployeeInformationFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.EmployeeInformation domainObject = bll.GetObjectByRevision(employeeInformationIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get EmployeeInformation (VisualDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.EmployeeInformationVisualDTO GetVisualEmployeeInformationWithRevision(GetVisualEmployeeInformationWithRevisionAutoRequest getVisualEmployeeInformationWithRevisionAutoRequest)
        {
            long revision = getVisualEmployeeInformationWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.EmployeeInformationIdentityDTO employeeInformationIdentity = getVisualEmployeeInformationWithRevisionAutoRequest.EmployeeInformationIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualEmployeeInformationWithRevisionInternal(employeeInformationIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeInformationVisualDTO GetVisualEmployeeInformationWithRevisionInternal(SampleSystem.Generated.DTO.EmployeeInformationIdentityDTO employeeInformationIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeInformationBLL bll = evaluateData.Context.Logics.EmployeeInformationFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.EmployeeInformation domainObject = bll.GetObjectByRevision(employeeInformationIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetEmployeeInformationPropertyRevisionByDateRangeAutoRequest
    {
        
        private SampleSystem.Generated.DTO.EmployeeInformationIdentityDTO employeeInformationIdentity;
        
        private string propertyName;
        
        private Framework.Core.Period? period;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.EmployeeInformationIdentityDTO EmployeeInformationIdentity
        {
            get
            {
                return this.employeeInformationIdentity;
            }
            set
            {
                this.employeeInformationIdentity = value;
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
    public partial class GetEmployeeInformationPropertyRevisionsAutoRequest
    {
        
        private SampleSystem.Generated.DTO.EmployeeInformationIdentityDTO employeeInformationIdentity;
        
        private string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.EmployeeInformationIdentityDTO EmployeeInformationIdentity
        {
            get
            {
                return this.employeeInformationIdentity;
            }
            set
            {
                this.employeeInformationIdentity = value;
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
    public partial class GetFullEmployeeInformationWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.EmployeeInformationIdentityDTO employeeInformationIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.EmployeeInformationIdentityDTO EmployeeInformationIdentity
        {
            get
            {
                return this.employeeInformationIdentity;
            }
            set
            {
                this.employeeInformationIdentity = value;
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
    public partial class GetRichEmployeeInformationWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.EmployeeInformationIdentityDTO employeeInformationIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.EmployeeInformationIdentityDTO EmployeeInformationIdentity
        {
            get
            {
                return this.employeeInformationIdentity;
            }
            set
            {
                this.employeeInformationIdentity = value;
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
    public partial class GetSimpleEmployeeInformationWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.EmployeeInformationIdentityDTO employeeInformationIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.EmployeeInformationIdentityDTO EmployeeInformationIdentity
        {
            get
            {
                return this.employeeInformationIdentity;
            }
            set
            {
                this.employeeInformationIdentity = value;
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
    public partial class GetVisualEmployeeInformationWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.EmployeeInformationIdentityDTO employeeInformationIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.EmployeeInformationIdentityDTO EmployeeInformationIdentity
        {
            get
            {
                return this.employeeInformationIdentity;
            }
            set
            {
                this.employeeInformationIdentity = value;
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
