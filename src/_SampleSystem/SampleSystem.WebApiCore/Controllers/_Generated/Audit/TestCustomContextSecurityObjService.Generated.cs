namespace SampleSystem.WebApiCore.Controllers.Audit
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/[controller]/[action]")]
    public partial class TestCustomContextSecurityObjController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get TestCustomContextSecurityObj (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestCustomContextSecurityObjFullDTO GetFullTestCustomContextSecurityObjWithRevision(GetFullTestCustomContextSecurityObjWithRevisionAutoRequest getFullTestCustomContextSecurityObjWithRevisionAutoRequest)
        {
            long revision = getFullTestCustomContextSecurityObjWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity = getFullTestCustomContextSecurityObjWithRevisionAutoRequest.TestCustomContextSecurityObjIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullTestCustomContextSecurityObjWithRevisionInternal(testCustomContextSecurityObjIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestCustomContextSecurityObjFullDTO GetFullTestCustomContextSecurityObjWithRevisionInternal(SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestCustomContextSecurityObjBLL bll = evaluateData.Context.Logics.TestCustomContextSecurityObjFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestCustomContextSecurityObj domainObject = bll.GetObjectByRevision(testCustomContextSecurityObjIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestCustomContextSecurityObj (RichDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestCustomContextSecurityObjRichDTO GetRichTestCustomContextSecurityObjWithRevision(GetRichTestCustomContextSecurityObjWithRevisionAutoRequest getRichTestCustomContextSecurityObjWithRevisionAutoRequest)
        {
            long revision = getRichTestCustomContextSecurityObjWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity = getRichTestCustomContextSecurityObjWithRevisionAutoRequest.TestCustomContextSecurityObjIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichTestCustomContextSecurityObjWithRevisionInternal(testCustomContextSecurityObjIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestCustomContextSecurityObjRichDTO GetRichTestCustomContextSecurityObjWithRevisionInternal(SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestCustomContextSecurityObjBLL bll = evaluateData.Context.Logics.TestCustomContextSecurityObjFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestCustomContextSecurityObj domainObject = bll.GetObjectByRevision(testCustomContextSecurityObjIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestCustomContextSecurityObj (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestCustomContextSecurityObjSimpleDTO GetSimpleTestCustomContextSecurityObjWithRevision(GetSimpleTestCustomContextSecurityObjWithRevisionAutoRequest getSimpleTestCustomContextSecurityObjWithRevisionAutoRequest)
        {
            long revision = getSimpleTestCustomContextSecurityObjWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity = getSimpleTestCustomContextSecurityObjWithRevisionAutoRequest.TestCustomContextSecurityObjIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleTestCustomContextSecurityObjWithRevisionInternal(testCustomContextSecurityObjIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestCustomContextSecurityObjSimpleDTO GetSimpleTestCustomContextSecurityObjWithRevisionInternal(SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestCustomContextSecurityObjBLL bll = evaluateData.Context.Logics.TestCustomContextSecurityObjFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestCustomContextSecurityObj domainObject = bll.GetObjectByRevision(testCustomContextSecurityObjIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestCustomContextSecurityObj Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestCustomContextSecurityObjPropertyRevisionByDateRange(GetTestCustomContextSecurityObjPropertyRevisionByDateRangeAutoRequest getTestCustomContextSecurityObjPropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getTestCustomContextSecurityObjPropertyRevisionByDateRangeAutoRequest.Period;
            string propertyName = getTestCustomContextSecurityObjPropertyRevisionByDateRangeAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity = getTestCustomContextSecurityObjPropertyRevisionByDateRangeAutoRequest.TestCustomContextSecurityObjIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetTestCustomContextSecurityObjPropertyRevisionByDateRangeInternal(testCustomContextSecurityObjIdentity, propertyName, period, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestCustomContextSecurityObjPropertyRevisionByDateRangeInternal(SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestCustomContextSecurityObjBLL bll = evaluateData.Context.Logics.TestCustomContextSecurityObjFactory.Create(SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.TestCustomContextSecurityObj>(testCustomContextSecurityObjIdentity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get TestCustomContextSecurityObj Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestCustomContextSecurityObjPropertyRevisions(GetTestCustomContextSecurityObjPropertyRevisionsAutoRequest getTestCustomContextSecurityObjPropertyRevisionsAutoRequest)
        {
            string propertyName = getTestCustomContextSecurityObjPropertyRevisionsAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity = getTestCustomContextSecurityObjPropertyRevisionsAutoRequest.TestCustomContextSecurityObjIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetTestCustomContextSecurityObjPropertyRevisionsInternal(testCustomContextSecurityObjIdentity, propertyName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestCustomContextSecurityObjPropertyRevisionsInternal(SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestCustomContextSecurityObjBLL bll = evaluateData.Context.Logics.TestCustomContextSecurityObjFactory.Create(SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.TestCustomContextSecurityObj>(testCustomContextSecurityObjIdentity.Id, propertyName);
        }
        
        /// <summary>
        /// Get TestCustomContextSecurityObj revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetTestCustomContextSecurityObjRevisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetTestCustomContextSecurityObjRevisionsInternal(testCustomContextSecurityObjIdentity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetTestCustomContextSecurityObjRevisionsInternal(SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestCustomContextSecurityObjBLL bll = evaluateData.Context.Logics.TestCustomContextSecurityObjFactory.Create(SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(testCustomContextSecurityObjIdentity.Id));
        }
        
        /// <summary>
        /// Get TestCustomContextSecurityObj (VisualDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestCustomContextSecurityObjVisualDTO GetVisualTestCustomContextSecurityObjWithRevision(GetVisualTestCustomContextSecurityObjWithRevisionAutoRequest getVisualTestCustomContextSecurityObjWithRevisionAutoRequest)
        {
            long revision = getVisualTestCustomContextSecurityObjWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity = getVisualTestCustomContextSecurityObjWithRevisionAutoRequest.TestCustomContextSecurityObjIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualTestCustomContextSecurityObjWithRevisionInternal(testCustomContextSecurityObjIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestCustomContextSecurityObjVisualDTO GetVisualTestCustomContextSecurityObjWithRevisionInternal(SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestCustomContextSecurityObjBLL bll = evaluateData.Context.Logics.TestCustomContextSecurityObjFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestCustomContextSecurityObj domainObject = bll.GetObjectByRevision(testCustomContextSecurityObjIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetFullTestCustomContextSecurityObjWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO TestCustomContextSecurityObjIdentity
        {
            get
            {
                return this.testCustomContextSecurityObjIdentity;
            }
            set
            {
                this.testCustomContextSecurityObjIdentity = value;
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
    public partial class GetRichTestCustomContextSecurityObjWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO TestCustomContextSecurityObjIdentity
        {
            get
            {
                return this.testCustomContextSecurityObjIdentity;
            }
            set
            {
                this.testCustomContextSecurityObjIdentity = value;
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
    public partial class GetSimpleTestCustomContextSecurityObjWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO TestCustomContextSecurityObjIdentity
        {
            get
            {
                return this.testCustomContextSecurityObjIdentity;
            }
            set
            {
                this.testCustomContextSecurityObjIdentity = value;
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
    public partial class GetTestCustomContextSecurityObjPropertyRevisionByDateRangeAutoRequest
    {
        
        private SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity;
        
        private string propertyName;
        
        private Framework.Core.Period? period;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO TestCustomContextSecurityObjIdentity
        {
            get
            {
                return this.testCustomContextSecurityObjIdentity;
            }
            set
            {
                this.testCustomContextSecurityObjIdentity = value;
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
    public partial class GetTestCustomContextSecurityObjPropertyRevisionsAutoRequest
    {
        
        private SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity;
        
        private string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO TestCustomContextSecurityObjIdentity
        {
            get
            {
                return this.testCustomContextSecurityObjIdentity;
            }
            set
            {
                this.testCustomContextSecurityObjIdentity = value;
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
    public partial class GetVisualTestCustomContextSecurityObjWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO TestCustomContextSecurityObjIdentity
        {
            get
            {
                return this.testCustomContextSecurityObjIdentity;
            }
            set
            {
                this.testCustomContextSecurityObjIdentity = value;
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
