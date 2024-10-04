﻿namespace SampleSystem.WebApiCore.Controllers.Audit
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/[controller]/[action]")]
    public partial class TestJobObjectController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get TestJobObject (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestJobObjectFullDTO GetFullTestJobObjectWithRevision(GetFullTestJobObjectWithRevisionAutoRequest getFullTestJobObjectWithRevisionAutoRequest)
        {
            long revision = getFullTestJobObjectWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.TestJobObjectIdentityDTO testJobObjectIdentity = getFullTestJobObjectWithRevisionAutoRequest.TestJobObjectIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullTestJobObjectWithRevisionInternal(testJobObjectIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestJobObjectFullDTO GetFullTestJobObjectWithRevisionInternal(SampleSystem.Generated.DTO.TestJobObjectIdentityDTO testJobObjectIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestJobObjectBLL bll = evaluateData.Context.Logics.TestJobObjectFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestJobObject domainObject = bll.GetObjectByRevision(testJobObjectIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestJobObject (RichDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestJobObjectRichDTO GetRichTestJobObjectWithRevision(GetRichTestJobObjectWithRevisionAutoRequest getRichTestJobObjectWithRevisionAutoRequest)
        {
            long revision = getRichTestJobObjectWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.TestJobObjectIdentityDTO testJobObjectIdentity = getRichTestJobObjectWithRevisionAutoRequest.TestJobObjectIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichTestJobObjectWithRevisionInternal(testJobObjectIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestJobObjectRichDTO GetRichTestJobObjectWithRevisionInternal(SampleSystem.Generated.DTO.TestJobObjectIdentityDTO testJobObjectIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestJobObjectBLL bll = evaluateData.Context.Logics.TestJobObjectFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestJobObject domainObject = bll.GetObjectByRevision(testJobObjectIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestJobObject (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestJobObjectSimpleDTO GetSimpleTestJobObjectWithRevision(GetSimpleTestJobObjectWithRevisionAutoRequest getSimpleTestJobObjectWithRevisionAutoRequest)
        {
            long revision = getSimpleTestJobObjectWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.TestJobObjectIdentityDTO testJobObjectIdentity = getSimpleTestJobObjectWithRevisionAutoRequest.TestJobObjectIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleTestJobObjectWithRevisionInternal(testJobObjectIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestJobObjectSimpleDTO GetSimpleTestJobObjectWithRevisionInternal(SampleSystem.Generated.DTO.TestJobObjectIdentityDTO testJobObjectIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestJobObjectBLL bll = evaluateData.Context.Logics.TestJobObjectFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestJobObject domainObject = bll.GetObjectByRevision(testJobObjectIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestJobObject Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestJobObjectPropertyRevisionByDateRange(GetTestJobObjectPropertyRevisionByDateRangeAutoRequest getTestJobObjectPropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getTestJobObjectPropertyRevisionByDateRangeAutoRequest.Period;
            string propertyName = getTestJobObjectPropertyRevisionByDateRangeAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.TestJobObjectIdentityDTO testJobObjectIdentity = getTestJobObjectPropertyRevisionByDateRangeAutoRequest.TestJobObjectIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetTestJobObjectPropertyRevisionByDateRangeInternal(testJobObjectIdentity, propertyName, period, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestJobObjectPropertyRevisionByDateRangeInternal(SampleSystem.Generated.DTO.TestJobObjectIdentityDTO testJobObjectIdentity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestJobObjectBLL bll = evaluateData.Context.Logics.TestJobObjectFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.TestJobObject>(testJobObjectIdentity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get TestJobObject Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestJobObjectPropertyRevisions(GetTestJobObjectPropertyRevisionsAutoRequest getTestJobObjectPropertyRevisionsAutoRequest)
        {
            string propertyName = getTestJobObjectPropertyRevisionsAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.TestJobObjectIdentityDTO testJobObjectIdentity = getTestJobObjectPropertyRevisionsAutoRequest.TestJobObjectIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetTestJobObjectPropertyRevisionsInternal(testJobObjectIdentity, propertyName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestJobObjectPropertyRevisionsInternal(SampleSystem.Generated.DTO.TestJobObjectIdentityDTO testJobObjectIdentity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestJobObjectBLL bll = evaluateData.Context.Logics.TestJobObjectFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.TestJobObject>(testJobObjectIdentity.Id, propertyName);
        }
        
        /// <summary>
        /// Get TestJobObject revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetTestJobObjectRevisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestJobObjectIdentityDTO testJobObjectIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetTestJobObjectRevisionsInternal(testJobObjectIdentity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetTestJobObjectRevisionsInternal(SampleSystem.Generated.DTO.TestJobObjectIdentityDTO testJobObjectIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestJobObjectBLL bll = evaluateData.Context.Logics.TestJobObjectFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(testJobObjectIdentity.Id));
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetFullTestJobObjectWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.TestJobObjectIdentityDTO testJobObjectIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.TestJobObjectIdentityDTO TestJobObjectIdentity
        {
            get
            {
                return this.testJobObjectIdentity;
            }
            set
            {
                this.testJobObjectIdentity = value;
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
    public partial class GetRichTestJobObjectWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.TestJobObjectIdentityDTO testJobObjectIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.TestJobObjectIdentityDTO TestJobObjectIdentity
        {
            get
            {
                return this.testJobObjectIdentity;
            }
            set
            {
                this.testJobObjectIdentity = value;
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
    public partial class GetSimpleTestJobObjectWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.TestJobObjectIdentityDTO testJobObjectIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.TestJobObjectIdentityDTO TestJobObjectIdentity
        {
            get
            {
                return this.testJobObjectIdentity;
            }
            set
            {
                this.testJobObjectIdentity = value;
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
    public partial class GetTestJobObjectPropertyRevisionByDateRangeAutoRequest
    {
        
        private SampleSystem.Generated.DTO.TestJobObjectIdentityDTO testJobObjectIdentity;
        
        private string propertyName;
        
        private Framework.Core.Period? period;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.TestJobObjectIdentityDTO TestJobObjectIdentity
        {
            get
            {
                return this.testJobObjectIdentity;
            }
            set
            {
                this.testJobObjectIdentity = value;
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
    public partial class GetTestJobObjectPropertyRevisionsAutoRequest
    {
        
        private SampleSystem.Generated.DTO.TestJobObjectIdentityDTO testJobObjectIdentity;
        
        private string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.TestJobObjectIdentityDTO TestJobObjectIdentity
        {
            get
            {
                return this.testJobObjectIdentity;
            }
            set
            {
                this.testJobObjectIdentity = value;
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
