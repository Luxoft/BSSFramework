namespace SampleSystem.WebApiCore.Controllers.Audit
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/[controller]/[action]")]
    public partial class HRDepartmentController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get HRDepartment (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullHRDepartmentWithRevision")]
        public virtual SampleSystem.Generated.DTO.HRDepartmentFullDTO GetFullHRDepartmentWithRevision(GetFullHRDepartmentWithRevisionAutoRequest getFullHRDepartmentWithRevisionAutoRequest)
        {
            long revision = getFullHRDepartmentWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity = getFullHRDepartmentWithRevisionAutoRequest.HRDepartmentIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullHRDepartmentWithRevisionInternal(hRDepartmentIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.HRDepartmentFullDTO GetFullHRDepartmentWithRevisionInternal(SampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.HRDepartment domainObject = bll.GetObjectByRevision(hRDepartmentIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get HRDepartment Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetHRDepartmentPropertyRevisionByDateRange")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetHRDepartmentPropertyRevisionByDateRange(GetHRDepartmentPropertyRevisionByDateRangeAutoRequest getHRDepartmentPropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getHRDepartmentPropertyRevisionByDateRangeAutoRequest.Period;
            string propertyName = getHRDepartmentPropertyRevisionByDateRangeAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity = getHRDepartmentPropertyRevisionByDateRangeAutoRequest.HRDepartmentIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetHRDepartmentPropertyRevisionByDateRangeInternal(hRDepartmentIdentity, propertyName, period, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetHRDepartmentPropertyRevisionByDateRangeInternal(SampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.HRDepartment>(hRDepartmentIdentity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get HRDepartment Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetHRDepartmentPropertyRevisions")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetHRDepartmentPropertyRevisions(GetHRDepartmentPropertyRevisionsAutoRequest getHRDepartmentPropertyRevisionsAutoRequest)
        {
            string propertyName = getHRDepartmentPropertyRevisionsAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity = getHRDepartmentPropertyRevisionsAutoRequest.HRDepartmentIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetHRDepartmentPropertyRevisionsInternal(hRDepartmentIdentity, propertyName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetHRDepartmentPropertyRevisionsInternal(SampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.HRDepartment>(hRDepartmentIdentity.Id, propertyName);
        }
        
        /// <summary>
        /// Get HRDepartment revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetHRDepartmentRevisions")]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetHRDepartmentRevisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetHRDepartmentRevisionsInternal(hRDepartmentIdentity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetHRDepartmentRevisionsInternal(SampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(hRDepartmentIdentity.Id));
        }
        
        /// <summary>
        /// Get HRDepartment (RichDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichHRDepartmentWithRevision")]
        public virtual SampleSystem.Generated.DTO.HRDepartmentRichDTO GetRichHRDepartmentWithRevision(GetRichHRDepartmentWithRevisionAutoRequest getRichHRDepartmentWithRevisionAutoRequest)
        {
            long revision = getRichHRDepartmentWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity = getRichHRDepartmentWithRevisionAutoRequest.HRDepartmentIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichHRDepartmentWithRevisionInternal(hRDepartmentIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.HRDepartmentRichDTO GetRichHRDepartmentWithRevisionInternal(SampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.HRDepartment domainObject = bll.GetObjectByRevision(hRDepartmentIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get HRDepartment (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleHRDepartmentWithRevision")]
        public virtual SampleSystem.Generated.DTO.HRDepartmentSimpleDTO GetSimpleHRDepartmentWithRevision(GetSimpleHRDepartmentWithRevisionAutoRequest getSimpleHRDepartmentWithRevisionAutoRequest)
        {
            long revision = getSimpleHRDepartmentWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity = getSimpleHRDepartmentWithRevisionAutoRequest.HRDepartmentIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleHRDepartmentWithRevisionInternal(hRDepartmentIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.HRDepartmentSimpleDTO GetSimpleHRDepartmentWithRevisionInternal(SampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.HRDepartment domainObject = bll.GetObjectByRevision(hRDepartmentIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get HRDepartment (VisualDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualHRDepartmentWithRevision")]
        public virtual SampleSystem.Generated.DTO.HRDepartmentVisualDTO GetVisualHRDepartmentWithRevision(GetVisualHRDepartmentWithRevisionAutoRequest getVisualHRDepartmentWithRevisionAutoRequest)
        {
            long revision = getVisualHRDepartmentWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity = getVisualHRDepartmentWithRevisionAutoRequest.HRDepartmentIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualHRDepartmentWithRevisionInternal(hRDepartmentIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.HRDepartmentVisualDTO GetVisualHRDepartmentWithRevisionInternal(SampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.HRDepartment domainObject = bll.GetObjectByRevision(hRDepartmentIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetFullHRDepartmentWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.HRDepartmentIdentityDTO HRDepartmentIdentity
        {
            get
            {
                return this.hRDepartmentIdentity;
            }
            set
            {
                this.hRDepartmentIdentity = value;
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
    public partial class GetHRDepartmentPropertyRevisionByDateRangeAutoRequest
    {
        
        private SampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity;
        
        private string propertyName;
        
        private Framework.Core.Period? period;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.HRDepartmentIdentityDTO HRDepartmentIdentity
        {
            get
            {
                return this.hRDepartmentIdentity;
            }
            set
            {
                this.hRDepartmentIdentity = value;
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
    public partial class GetHRDepartmentPropertyRevisionsAutoRequest
    {
        
        private SampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity;
        
        private string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.HRDepartmentIdentityDTO HRDepartmentIdentity
        {
            get
            {
                return this.hRDepartmentIdentity;
            }
            set
            {
                this.hRDepartmentIdentity = value;
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
    public partial class GetRichHRDepartmentWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.HRDepartmentIdentityDTO HRDepartmentIdentity
        {
            get
            {
                return this.hRDepartmentIdentity;
            }
            set
            {
                this.hRDepartmentIdentity = value;
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
    public partial class GetSimpleHRDepartmentWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.HRDepartmentIdentityDTO HRDepartmentIdentity
        {
            get
            {
                return this.hRDepartmentIdentity;
            }
            set
            {
                this.hRDepartmentIdentity = value;
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
    public partial class GetVisualHRDepartmentWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.HRDepartmentIdentityDTO HRDepartmentIdentity
        {
            get
            {
                return this.hRDepartmentIdentity;
            }
            set
            {
                this.hRDepartmentIdentity = value;
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
