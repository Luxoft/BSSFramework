﻿namespace SampleSystem.WebApiCore.Controllers.Audit
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/[controller]/[action]")]
    public partial class TestRootSecurityObjController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get TestRootSecurityObj (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestRootSecurityObjFullDTO GetFullTestRootSecurityObjWithRevision(GetFullTestRootSecurityObjWithRevisionAutoRequest getFullTestRootSecurityObjWithRevisionAutoRequest)
        {
            long revision = getFullTestRootSecurityObjWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.TestRootSecurityObjIdentityDTO testRootSecurityObjIdentity = getFullTestRootSecurityObjWithRevisionAutoRequest.TestRootSecurityObjIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullTestRootSecurityObjWithRevisionInternal(testRootSecurityObjIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestRootSecurityObjFullDTO GetFullTestRootSecurityObjWithRevisionInternal(SampleSystem.Generated.DTO.TestRootSecurityObjIdentityDTO testRootSecurityObjIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestRootSecurityObjBLL bll = evaluateData.Context.Logics.TestRootSecurityObjFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestRootSecurityObj domainObject = bll.GetObjectByRevision(testRootSecurityObjIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestRootSecurityObj (RichDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestRootSecurityObjRichDTO GetRichTestRootSecurityObjWithRevision(GetRichTestRootSecurityObjWithRevisionAutoRequest getRichTestRootSecurityObjWithRevisionAutoRequest)
        {
            long revision = getRichTestRootSecurityObjWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.TestRootSecurityObjIdentityDTO testRootSecurityObjIdentity = getRichTestRootSecurityObjWithRevisionAutoRequest.TestRootSecurityObjIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichTestRootSecurityObjWithRevisionInternal(testRootSecurityObjIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestRootSecurityObjRichDTO GetRichTestRootSecurityObjWithRevisionInternal(SampleSystem.Generated.DTO.TestRootSecurityObjIdentityDTO testRootSecurityObjIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestRootSecurityObjBLL bll = evaluateData.Context.Logics.TestRootSecurityObjFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestRootSecurityObj domainObject = bll.GetObjectByRevision(testRootSecurityObjIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestRootSecurityObj (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestRootSecurityObjSimpleDTO GetSimpleTestRootSecurityObjWithRevision(GetSimpleTestRootSecurityObjWithRevisionAutoRequest getSimpleTestRootSecurityObjWithRevisionAutoRequest)
        {
            long revision = getSimpleTestRootSecurityObjWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.TestRootSecurityObjIdentityDTO testRootSecurityObjIdentity = getSimpleTestRootSecurityObjWithRevisionAutoRequest.TestRootSecurityObjIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleTestRootSecurityObjWithRevisionInternal(testRootSecurityObjIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestRootSecurityObjSimpleDTO GetSimpleTestRootSecurityObjWithRevisionInternal(SampleSystem.Generated.DTO.TestRootSecurityObjIdentityDTO testRootSecurityObjIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestRootSecurityObjBLL bll = evaluateData.Context.Logics.TestRootSecurityObjFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestRootSecurityObj domainObject = bll.GetObjectByRevision(testRootSecurityObjIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestRootSecurityObj Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestRootSecurityObjPropertyRevisionByDateRange(GetTestRootSecurityObjPropertyRevisionByDateRangeAutoRequest getTestRootSecurityObjPropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getTestRootSecurityObjPropertyRevisionByDateRangeAutoRequest.Period;
            string propertyName = getTestRootSecurityObjPropertyRevisionByDateRangeAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.TestRootSecurityObjIdentityDTO testRootSecurityObjIdentity = getTestRootSecurityObjPropertyRevisionByDateRangeAutoRequest.TestRootSecurityObjIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetTestRootSecurityObjPropertyRevisionByDateRangeInternal(testRootSecurityObjIdentity, propertyName, period, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestRootSecurityObjPropertyRevisionByDateRangeInternal(SampleSystem.Generated.DTO.TestRootSecurityObjIdentityDTO testRootSecurityObjIdentity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestRootSecurityObjBLL bll = evaluateData.Context.Logics.TestRootSecurityObjFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.TestRootSecurityObj>(testRootSecurityObjIdentity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get TestRootSecurityObj Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestRootSecurityObjPropertyRevisions(GetTestRootSecurityObjPropertyRevisionsAutoRequest getTestRootSecurityObjPropertyRevisionsAutoRequest)
        {
            string propertyName = getTestRootSecurityObjPropertyRevisionsAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.TestRootSecurityObjIdentityDTO testRootSecurityObjIdentity = getTestRootSecurityObjPropertyRevisionsAutoRequest.TestRootSecurityObjIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetTestRootSecurityObjPropertyRevisionsInternal(testRootSecurityObjIdentity, propertyName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestRootSecurityObjPropertyRevisionsInternal(SampleSystem.Generated.DTO.TestRootSecurityObjIdentityDTO testRootSecurityObjIdentity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestRootSecurityObjBLL bll = evaluateData.Context.Logics.TestRootSecurityObjFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.TestRootSecurityObj>(testRootSecurityObjIdentity.Id, propertyName);
        }
        
        /// <summary>
        /// Get TestRootSecurityObj revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetTestRootSecurityObjRevisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestRootSecurityObjIdentityDTO testRootSecurityObjIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetTestRootSecurityObjRevisionsInternal(testRootSecurityObjIdentity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetTestRootSecurityObjRevisionsInternal(SampleSystem.Generated.DTO.TestRootSecurityObjIdentityDTO testRootSecurityObjIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestRootSecurityObjBLL bll = evaluateData.Context.Logics.TestRootSecurityObjFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(testRootSecurityObjIdentity.Id));
        }
        
        /// <summary>
        /// Get TestRootSecurityObj (VisualDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestRootSecurityObjVisualDTO GetVisualTestRootSecurityObjWithRevision(GetVisualTestRootSecurityObjWithRevisionAutoRequest getVisualTestRootSecurityObjWithRevisionAutoRequest)
        {
            long revision = getVisualTestRootSecurityObjWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.TestRootSecurityObjIdentityDTO testRootSecurityObjIdentity = getVisualTestRootSecurityObjWithRevisionAutoRequest.TestRootSecurityObjIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualTestRootSecurityObjWithRevisionInternal(testRootSecurityObjIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestRootSecurityObjVisualDTO GetVisualTestRootSecurityObjWithRevisionInternal(SampleSystem.Generated.DTO.TestRootSecurityObjIdentityDTO testRootSecurityObjIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestRootSecurityObjBLL bll = evaluateData.Context.Logics.TestRootSecurityObjFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestRootSecurityObj domainObject = bll.GetObjectByRevision(testRootSecurityObjIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetFullTestRootSecurityObjWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.TestRootSecurityObjIdentityDTO testRootSecurityObjIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.TestRootSecurityObjIdentityDTO TestRootSecurityObjIdentity
        {
            get
            {
                return this.testRootSecurityObjIdentity;
            }
            set
            {
                this.testRootSecurityObjIdentity = value;
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
    public partial class GetRichTestRootSecurityObjWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.TestRootSecurityObjIdentityDTO testRootSecurityObjIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.TestRootSecurityObjIdentityDTO TestRootSecurityObjIdentity
        {
            get
            {
                return this.testRootSecurityObjIdentity;
            }
            set
            {
                this.testRootSecurityObjIdentity = value;
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
    public partial class GetSimpleTestRootSecurityObjWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.TestRootSecurityObjIdentityDTO testRootSecurityObjIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.TestRootSecurityObjIdentityDTO TestRootSecurityObjIdentity
        {
            get
            {
                return this.testRootSecurityObjIdentity;
            }
            set
            {
                this.testRootSecurityObjIdentity = value;
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
    public partial class GetTestRootSecurityObjPropertyRevisionByDateRangeAutoRequest
    {
        
        private SampleSystem.Generated.DTO.TestRootSecurityObjIdentityDTO testRootSecurityObjIdentity;
        
        private string propertyName;
        
        private Framework.Core.Period? period;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.TestRootSecurityObjIdentityDTO TestRootSecurityObjIdentity
        {
            get
            {
                return this.testRootSecurityObjIdentity;
            }
            set
            {
                this.testRootSecurityObjIdentity = value;
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
    public partial class GetTestRootSecurityObjPropertyRevisionsAutoRequest
    {
        
        private SampleSystem.Generated.DTO.TestRootSecurityObjIdentityDTO testRootSecurityObjIdentity;
        
        private string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.TestRootSecurityObjIdentityDTO TestRootSecurityObjIdentity
        {
            get
            {
                return this.testRootSecurityObjIdentity;
            }
            set
            {
                this.testRootSecurityObjIdentity = value;
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
    public partial class GetVisualTestRootSecurityObjWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.TestRootSecurityObjIdentityDTO testRootSecurityObjIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.TestRootSecurityObjIdentityDTO TestRootSecurityObjIdentity
        {
            get
            {
                return this.testRootSecurityObjIdentity;
            }
            set
            {
                this.testRootSecurityObjIdentity = value;
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
