namespace SampleSystem.WebApiCore.Controllers.Audit
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/[controller]/[action]")]
    public partial class TestSecuritySubObjItem3Controller : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get TestSecuritySubObjItem3 (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullTestSecuritySubObjItem3WithRevision")]
        public virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem3FullDTO GetFullTestSecuritySubObjItem3WithRevision(GetFullTestSecuritySubObjItem3WithRevisionAutoRequest getFullTestSecuritySubObjItem3WithRevisionAutoRequest)
        {
            long revision = getFullTestSecuritySubObjItem3WithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity = getFullTestSecuritySubObjItem3WithRevisionAutoRequest.TestSecuritySubObjItem3Identity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullTestSecuritySubObjItem3WithRevisionInternal(testSecuritySubObjItem3Identity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem3FullDTO GetFullTestSecuritySubObjItem3WithRevisionInternal(SampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItem3BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem3Factory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestSecuritySubObjItem3 domainObject = bll.GetObjectByRevision(testSecuritySubObjItem3Identity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestSecuritySubObjItem3 (RichDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichTestSecuritySubObjItem3WithRevision")]
        public virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem3RichDTO GetRichTestSecuritySubObjItem3WithRevision(GetRichTestSecuritySubObjItem3WithRevisionAutoRequest getRichTestSecuritySubObjItem3WithRevisionAutoRequest)
        {
            long revision = getRichTestSecuritySubObjItem3WithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity = getRichTestSecuritySubObjItem3WithRevisionAutoRequest.TestSecuritySubObjItem3Identity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichTestSecuritySubObjItem3WithRevisionInternal(testSecuritySubObjItem3Identity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem3RichDTO GetRichTestSecuritySubObjItem3WithRevisionInternal(SampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItem3BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem3Factory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestSecuritySubObjItem3 domainObject = bll.GetObjectByRevision(testSecuritySubObjItem3Identity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestSecuritySubObjItem3 (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleTestSecuritySubObjItem3WithRevision")]
        public virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem3SimpleDTO GetSimpleTestSecuritySubObjItem3WithRevision(GetSimpleTestSecuritySubObjItem3WithRevisionAutoRequest getSimpleTestSecuritySubObjItem3WithRevisionAutoRequest)
        {
            long revision = getSimpleTestSecuritySubObjItem3WithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity = getSimpleTestSecuritySubObjItem3WithRevisionAutoRequest.TestSecuritySubObjItem3Identity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleTestSecuritySubObjItem3WithRevisionInternal(testSecuritySubObjItem3Identity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem3SimpleDTO GetSimpleTestSecuritySubObjItem3WithRevisionInternal(SampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItem3BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem3Factory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestSecuritySubObjItem3 domainObject = bll.GetObjectByRevision(testSecuritySubObjItem3Identity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestSecuritySubObjItem3 Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetTestSecuritySubObjItem3PropertyRevisionByDateRange")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestSecuritySubObjItem3PropertyRevisionByDateRange(GetTestSecuritySubObjItem3PropertyRevisionByDateRangeAutoRequest getTestSecuritySubObjItem3PropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getTestSecuritySubObjItem3PropertyRevisionByDateRangeAutoRequest.Period;
            string propertyName = getTestSecuritySubObjItem3PropertyRevisionByDateRangeAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity = getTestSecuritySubObjItem3PropertyRevisionByDateRangeAutoRequest.TestSecuritySubObjItem3Identity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetTestSecuritySubObjItem3PropertyRevisionByDateRangeInternal(testSecuritySubObjItem3Identity, propertyName, period, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestSecuritySubObjItem3PropertyRevisionByDateRangeInternal(SampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItem3BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem3Factory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.TestSecuritySubObjItem3>(testSecuritySubObjItem3Identity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get TestSecuritySubObjItem3 Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetTestSecuritySubObjItem3PropertyRevisions")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestSecuritySubObjItem3PropertyRevisions(GetTestSecuritySubObjItem3PropertyRevisionsAutoRequest getTestSecuritySubObjItem3PropertyRevisionsAutoRequest)
        {
            string propertyName = getTestSecuritySubObjItem3PropertyRevisionsAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity = getTestSecuritySubObjItem3PropertyRevisionsAutoRequest.TestSecuritySubObjItem3Identity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetTestSecuritySubObjItem3PropertyRevisionsInternal(testSecuritySubObjItem3Identity, propertyName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestSecuritySubObjItem3PropertyRevisionsInternal(SampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItem3BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem3Factory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.TestSecuritySubObjItem3>(testSecuritySubObjItem3Identity.Id, propertyName);
        }
        
        /// <summary>
        /// Get TestSecuritySubObjItem3 revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetTestSecuritySubObjItem3Revisions")]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetTestSecuritySubObjItem3Revisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetTestSecuritySubObjItem3RevisionsInternal(testSecuritySubObjItem3Identity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetTestSecuritySubObjItem3RevisionsInternal(SampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItem3BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem3Factory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(testSecuritySubObjItem3Identity.Id));
        }
        
        /// <summary>
        /// Get TestSecuritySubObjItem3 (VisualDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualTestSecuritySubObjItem3WithRevision")]
        public virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem3VisualDTO GetVisualTestSecuritySubObjItem3WithRevision(GetVisualTestSecuritySubObjItem3WithRevisionAutoRequest getVisualTestSecuritySubObjItem3WithRevisionAutoRequest)
        {
            long revision = getVisualTestSecuritySubObjItem3WithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity = getVisualTestSecuritySubObjItem3WithRevisionAutoRequest.TestSecuritySubObjItem3Identity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualTestSecuritySubObjItem3WithRevisionInternal(testSecuritySubObjItem3Identity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem3VisualDTO GetVisualTestSecuritySubObjItem3WithRevisionInternal(SampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItem3BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem3Factory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestSecuritySubObjItem3 domainObject = bll.GetObjectByRevision(testSecuritySubObjItem3Identity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetFullTestSecuritySubObjItem3WithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO TestSecuritySubObjItem3Identity
        {
            get
            {
                return this.testSecuritySubObjItem3Identity;
            }
            set
            {
                this.testSecuritySubObjItem3Identity = value;
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
    public partial class GetRichTestSecuritySubObjItem3WithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO TestSecuritySubObjItem3Identity
        {
            get
            {
                return this.testSecuritySubObjItem3Identity;
            }
            set
            {
                this.testSecuritySubObjItem3Identity = value;
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
    public partial class GetSimpleTestSecuritySubObjItem3WithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO TestSecuritySubObjItem3Identity
        {
            get
            {
                return this.testSecuritySubObjItem3Identity;
            }
            set
            {
                this.testSecuritySubObjItem3Identity = value;
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
    public partial class GetTestSecuritySubObjItem3PropertyRevisionByDateRangeAutoRequest
    {
        
        private SampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity;
        
        private string propertyName;
        
        private Framework.Core.Period? period;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO TestSecuritySubObjItem3Identity
        {
            get
            {
                return this.testSecuritySubObjItem3Identity;
            }
            set
            {
                this.testSecuritySubObjItem3Identity = value;
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
    public partial class GetTestSecuritySubObjItem3PropertyRevisionsAutoRequest
    {
        
        private SampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity;
        
        private string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO TestSecuritySubObjItem3Identity
        {
            get
            {
                return this.testSecuritySubObjItem3Identity;
            }
            set
            {
                this.testSecuritySubObjItem3Identity = value;
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
    public partial class GetVisualTestSecuritySubObjItem3WithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO TestSecuritySubObjItem3Identity
        {
            get
            {
                return this.testSecuritySubObjItem3Identity;
            }
            set
            {
                this.testSecuritySubObjItem3Identity = value;
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
