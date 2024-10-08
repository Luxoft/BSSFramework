﻿namespace SampleSystem.WebApiCore.Controllers.Audit
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/[controller]/[action]")]
    public partial class WorkingCalendar1676Controller : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get WorkingCalendar1676 (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.WorkingCalendar1676FullDTO GetFullWorkingCalendar1676WithRevision(GetFullWorkingCalendar1676WithRevisionAutoRequest getFullWorkingCalendar1676WithRevisionAutoRequest)
        {
            long revision = getFullWorkingCalendar1676WithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.WorkingCalendar1676IdentityDTO workingCalendar1676Identity = getFullWorkingCalendar1676WithRevisionAutoRequest.WorkingCalendar1676Identity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullWorkingCalendar1676WithRevisionInternal(workingCalendar1676Identity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.WorkingCalendar1676FullDTO GetFullWorkingCalendar1676WithRevisionInternal(SampleSystem.Generated.DTO.WorkingCalendar1676IdentityDTO workingCalendar1676Identity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IWorkingCalendar1676BLL bll = evaluateData.Context.Logics.WorkingCalendar1676Factory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.EnversBug1676.WorkingCalendar1676 domainObject = bll.GetObjectByRevision(workingCalendar1676Identity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get WorkingCalendar1676 (RichDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.WorkingCalendar1676RichDTO GetRichWorkingCalendar1676WithRevision(GetRichWorkingCalendar1676WithRevisionAutoRequest getRichWorkingCalendar1676WithRevisionAutoRequest)
        {
            long revision = getRichWorkingCalendar1676WithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.WorkingCalendar1676IdentityDTO workingCalendar1676Identity = getRichWorkingCalendar1676WithRevisionAutoRequest.WorkingCalendar1676Identity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichWorkingCalendar1676WithRevisionInternal(workingCalendar1676Identity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.WorkingCalendar1676RichDTO GetRichWorkingCalendar1676WithRevisionInternal(SampleSystem.Generated.DTO.WorkingCalendar1676IdentityDTO workingCalendar1676Identity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IWorkingCalendar1676BLL bll = evaluateData.Context.Logics.WorkingCalendar1676Factory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.EnversBug1676.WorkingCalendar1676 domainObject = bll.GetObjectByRevision(workingCalendar1676Identity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get WorkingCalendar1676 (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.WorkingCalendar1676SimpleDTO GetSimpleWorkingCalendar1676WithRevision(GetSimpleWorkingCalendar1676WithRevisionAutoRequest getSimpleWorkingCalendar1676WithRevisionAutoRequest)
        {
            long revision = getSimpleWorkingCalendar1676WithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.WorkingCalendar1676IdentityDTO workingCalendar1676Identity = getSimpleWorkingCalendar1676WithRevisionAutoRequest.WorkingCalendar1676Identity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleWorkingCalendar1676WithRevisionInternal(workingCalendar1676Identity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.WorkingCalendar1676SimpleDTO GetSimpleWorkingCalendar1676WithRevisionInternal(SampleSystem.Generated.DTO.WorkingCalendar1676IdentityDTO workingCalendar1676Identity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IWorkingCalendar1676BLL bll = evaluateData.Context.Logics.WorkingCalendar1676Factory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.EnversBug1676.WorkingCalendar1676 domainObject = bll.GetObjectByRevision(workingCalendar1676Identity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get WorkingCalendar1676 (VisualDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.WorkingCalendar1676VisualDTO GetVisualWorkingCalendar1676WithRevision(GetVisualWorkingCalendar1676WithRevisionAutoRequest getVisualWorkingCalendar1676WithRevisionAutoRequest)
        {
            long revision = getVisualWorkingCalendar1676WithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.WorkingCalendar1676IdentityDTO workingCalendar1676Identity = getVisualWorkingCalendar1676WithRevisionAutoRequest.WorkingCalendar1676Identity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualWorkingCalendar1676WithRevisionInternal(workingCalendar1676Identity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.WorkingCalendar1676VisualDTO GetVisualWorkingCalendar1676WithRevisionInternal(SampleSystem.Generated.DTO.WorkingCalendar1676IdentityDTO workingCalendar1676Identity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IWorkingCalendar1676BLL bll = evaluateData.Context.Logics.WorkingCalendar1676Factory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.EnversBug1676.WorkingCalendar1676 domainObject = bll.GetObjectByRevision(workingCalendar1676Identity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get WorkingCalendar1676 Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetWorkingCalendar1676PropertyRevisionByDateRange(GetWorkingCalendar1676PropertyRevisionByDateRangeAutoRequest getWorkingCalendar1676PropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getWorkingCalendar1676PropertyRevisionByDateRangeAutoRequest.Period;
            string propertyName = getWorkingCalendar1676PropertyRevisionByDateRangeAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.WorkingCalendar1676IdentityDTO workingCalendar1676Identity = getWorkingCalendar1676PropertyRevisionByDateRangeAutoRequest.WorkingCalendar1676Identity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetWorkingCalendar1676PropertyRevisionByDateRangeInternal(workingCalendar1676Identity, propertyName, period, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetWorkingCalendar1676PropertyRevisionByDateRangeInternal(SampleSystem.Generated.DTO.WorkingCalendar1676IdentityDTO workingCalendar1676Identity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IWorkingCalendar1676BLL bll = evaluateData.Context.Logics.WorkingCalendar1676Factory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.EnversBug1676.WorkingCalendar1676>(workingCalendar1676Identity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get WorkingCalendar1676 Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetWorkingCalendar1676PropertyRevisions(GetWorkingCalendar1676PropertyRevisionsAutoRequest getWorkingCalendar1676PropertyRevisionsAutoRequest)
        {
            string propertyName = getWorkingCalendar1676PropertyRevisionsAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.WorkingCalendar1676IdentityDTO workingCalendar1676Identity = getWorkingCalendar1676PropertyRevisionsAutoRequest.WorkingCalendar1676Identity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetWorkingCalendar1676PropertyRevisionsInternal(workingCalendar1676Identity, propertyName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetWorkingCalendar1676PropertyRevisionsInternal(SampleSystem.Generated.DTO.WorkingCalendar1676IdentityDTO workingCalendar1676Identity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IWorkingCalendar1676BLL bll = evaluateData.Context.Logics.WorkingCalendar1676Factory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.EnversBug1676.WorkingCalendar1676>(workingCalendar1676Identity.Id, propertyName);
        }
        
        /// <summary>
        /// Get WorkingCalendar1676 revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetWorkingCalendar1676Revisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.WorkingCalendar1676IdentityDTO workingCalendar1676Identity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetWorkingCalendar1676RevisionsInternal(workingCalendar1676Identity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetWorkingCalendar1676RevisionsInternal(SampleSystem.Generated.DTO.WorkingCalendar1676IdentityDTO workingCalendar1676Identity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IWorkingCalendar1676BLL bll = evaluateData.Context.Logics.WorkingCalendar1676Factory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(workingCalendar1676Identity.Id));
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetFullWorkingCalendar1676WithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.WorkingCalendar1676IdentityDTO workingCalendar1676Identity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.WorkingCalendar1676IdentityDTO WorkingCalendar1676Identity
        {
            get
            {
                return this.workingCalendar1676Identity;
            }
            set
            {
                this.workingCalendar1676Identity = value;
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
    public partial class GetRichWorkingCalendar1676WithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.WorkingCalendar1676IdentityDTO workingCalendar1676Identity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.WorkingCalendar1676IdentityDTO WorkingCalendar1676Identity
        {
            get
            {
                return this.workingCalendar1676Identity;
            }
            set
            {
                this.workingCalendar1676Identity = value;
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
    public partial class GetSimpleWorkingCalendar1676WithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.WorkingCalendar1676IdentityDTO workingCalendar1676Identity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.WorkingCalendar1676IdentityDTO WorkingCalendar1676Identity
        {
            get
            {
                return this.workingCalendar1676Identity;
            }
            set
            {
                this.workingCalendar1676Identity = value;
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
    public partial class GetVisualWorkingCalendar1676WithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.WorkingCalendar1676IdentityDTO workingCalendar1676Identity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.WorkingCalendar1676IdentityDTO WorkingCalendar1676Identity
        {
            get
            {
                return this.workingCalendar1676Identity;
            }
            set
            {
                this.workingCalendar1676Identity = value;
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
    public partial class GetWorkingCalendar1676PropertyRevisionByDateRangeAutoRequest
    {
        
        private SampleSystem.Generated.DTO.WorkingCalendar1676IdentityDTO workingCalendar1676Identity;
        
        private string propertyName;
        
        private Framework.Core.Period? period;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.WorkingCalendar1676IdentityDTO WorkingCalendar1676Identity
        {
            get
            {
                return this.workingCalendar1676Identity;
            }
            set
            {
                this.workingCalendar1676Identity = value;
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
    public partial class GetWorkingCalendar1676PropertyRevisionsAutoRequest
    {
        
        private SampleSystem.Generated.DTO.WorkingCalendar1676IdentityDTO workingCalendar1676Identity;
        
        private string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.WorkingCalendar1676IdentityDTO WorkingCalendar1676Identity
        {
            get
            {
                return this.workingCalendar1676Identity;
            }
            set
            {
                this.workingCalendar1676Identity = value;
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
}
