namespace SampleSystem.WebApiCore.Controllers.Audit
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/[controller]/[action]")]
    public partial class TestImmutableObjController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get TestImmutableObj (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullTestImmutableObjWithRevision")]
        public virtual SampleSystem.Generated.DTO.TestImmutableObjFullDTO GetFullTestImmutableObjWithRevision(GetFullTestImmutableObjWithRevisionAutoRequest getFullTestImmutableObjWithRevisionAutoRequest)
        {
            long revision = getFullTestImmutableObjWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.TestImmutableObjIdentityDTO testImmutableObjIdentity = getFullTestImmutableObjWithRevisionAutoRequest.TestImmutableObjIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullTestImmutableObjWithRevisionInternal(testImmutableObjIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestImmutableObjFullDTO GetFullTestImmutableObjWithRevisionInternal(SampleSystem.Generated.DTO.TestImmutableObjIdentityDTO testImmutableObjIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestImmutableObjBLL bll = evaluateData.Context.Logics.TestImmutableObjFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestImmutableObj domainObject = bll.GetObjectByRevision(testImmutableObjIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestImmutableObj (RichDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichTestImmutableObjWithRevision")]
        public virtual SampleSystem.Generated.DTO.TestImmutableObjRichDTO GetRichTestImmutableObjWithRevision(GetRichTestImmutableObjWithRevisionAutoRequest getRichTestImmutableObjWithRevisionAutoRequest)
        {
            long revision = getRichTestImmutableObjWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.TestImmutableObjIdentityDTO testImmutableObjIdentity = getRichTestImmutableObjWithRevisionAutoRequest.TestImmutableObjIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichTestImmutableObjWithRevisionInternal(testImmutableObjIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestImmutableObjRichDTO GetRichTestImmutableObjWithRevisionInternal(SampleSystem.Generated.DTO.TestImmutableObjIdentityDTO testImmutableObjIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestImmutableObjBLL bll = evaluateData.Context.Logics.TestImmutableObjFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestImmutableObj domainObject = bll.GetObjectByRevision(testImmutableObjIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestImmutableObj (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleTestImmutableObjWithRevision")]
        public virtual SampleSystem.Generated.DTO.TestImmutableObjSimpleDTO GetSimpleTestImmutableObjWithRevision(GetSimpleTestImmutableObjWithRevisionAutoRequest getSimpleTestImmutableObjWithRevisionAutoRequest)
        {
            long revision = getSimpleTestImmutableObjWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.TestImmutableObjIdentityDTO testImmutableObjIdentity = getSimpleTestImmutableObjWithRevisionAutoRequest.TestImmutableObjIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleTestImmutableObjWithRevisionInternal(testImmutableObjIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestImmutableObjSimpleDTO GetSimpleTestImmutableObjWithRevisionInternal(SampleSystem.Generated.DTO.TestImmutableObjIdentityDTO testImmutableObjIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestImmutableObjBLL bll = evaluateData.Context.Logics.TestImmutableObjFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestImmutableObj domainObject = bll.GetObjectByRevision(testImmutableObjIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestImmutableObj Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetTestImmutableObjPropertyRevisionByDateRange")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestImmutableObjPropertyRevisionByDateRange(GetTestImmutableObjPropertyRevisionByDateRangeAutoRequest getTestImmutableObjPropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getTestImmutableObjPropertyRevisionByDateRangeAutoRequest.Period;
            string propertyName = getTestImmutableObjPropertyRevisionByDateRangeAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.TestImmutableObjIdentityDTO testImmutableObjIdentity = getTestImmutableObjPropertyRevisionByDateRangeAutoRequest.TestImmutableObjIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetTestImmutableObjPropertyRevisionByDateRangeInternal(testImmutableObjIdentity, propertyName, period, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestImmutableObjPropertyRevisionByDateRangeInternal(SampleSystem.Generated.DTO.TestImmutableObjIdentityDTO testImmutableObjIdentity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestImmutableObjBLL bll = evaluateData.Context.Logics.TestImmutableObjFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.TestImmutableObj>(testImmutableObjIdentity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get TestImmutableObj Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetTestImmutableObjPropertyRevisions")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestImmutableObjPropertyRevisions(GetTestImmutableObjPropertyRevisionsAutoRequest getTestImmutableObjPropertyRevisionsAutoRequest)
        {
            string propertyName = getTestImmutableObjPropertyRevisionsAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.TestImmutableObjIdentityDTO testImmutableObjIdentity = getTestImmutableObjPropertyRevisionsAutoRequest.TestImmutableObjIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetTestImmutableObjPropertyRevisionsInternal(testImmutableObjIdentity, propertyName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestImmutableObjPropertyRevisionsInternal(SampleSystem.Generated.DTO.TestImmutableObjIdentityDTO testImmutableObjIdentity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestImmutableObjBLL bll = evaluateData.Context.Logics.TestImmutableObjFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.TestImmutableObj>(testImmutableObjIdentity.Id, propertyName);
        }
        
        /// <summary>
        /// Get TestImmutableObj revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetTestImmutableObjRevisions")]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetTestImmutableObjRevisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestImmutableObjIdentityDTO testImmutableObjIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetTestImmutableObjRevisionsInternal(testImmutableObjIdentity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetTestImmutableObjRevisionsInternal(SampleSystem.Generated.DTO.TestImmutableObjIdentityDTO testImmutableObjIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestImmutableObjBLL bll = evaluateData.Context.Logics.TestImmutableObjFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(testImmutableObjIdentity.Id));
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetFullTestImmutableObjWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.TestImmutableObjIdentityDTO testImmutableObjIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.TestImmutableObjIdentityDTO TestImmutableObjIdentity
        {
            get
            {
                return this.testImmutableObjIdentity;
            }
            set
            {
                this.testImmutableObjIdentity = value;
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
    public partial class GetRichTestImmutableObjWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.TestImmutableObjIdentityDTO testImmutableObjIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.TestImmutableObjIdentityDTO TestImmutableObjIdentity
        {
            get
            {
                return this.testImmutableObjIdentity;
            }
            set
            {
                this.testImmutableObjIdentity = value;
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
    public partial class GetSimpleTestImmutableObjWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.TestImmutableObjIdentityDTO testImmutableObjIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.TestImmutableObjIdentityDTO TestImmutableObjIdentity
        {
            get
            {
                return this.testImmutableObjIdentity;
            }
            set
            {
                this.testImmutableObjIdentity = value;
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
    public partial class GetTestImmutableObjPropertyRevisionByDateRangeAutoRequest
    {
        
        private SampleSystem.Generated.DTO.TestImmutableObjIdentityDTO testImmutableObjIdentity;
        
        private string propertyName;
        
        private Framework.Core.Period? period;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.TestImmutableObjIdentityDTO TestImmutableObjIdentity
        {
            get
            {
                return this.testImmutableObjIdentity;
            }
            set
            {
                this.testImmutableObjIdentity = value;
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
    public partial class GetTestImmutableObjPropertyRevisionsAutoRequest
    {
        
        private SampleSystem.Generated.DTO.TestImmutableObjIdentityDTO testImmutableObjIdentity;
        
        private string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.TestImmutableObjIdentityDTO TestImmutableObjIdentity
        {
            get
            {
                return this.testImmutableObjIdentity;
            }
            set
            {
                this.testImmutableObjIdentity = value;
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
