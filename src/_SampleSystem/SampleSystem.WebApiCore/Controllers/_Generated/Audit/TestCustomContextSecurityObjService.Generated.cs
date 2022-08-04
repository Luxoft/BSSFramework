namespace SampleSystem.WebApiCore.Controllers.Audit
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/v{version:apiVersion}/[controller]")]
    public partial class TestCustomContextSecurityObjController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>>
    {
        
        /// <summary>
        /// Get TestCustomContextSecurityObj (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullTestCustomContextSecurityObjWithRevision")]
        public virtual SampleSystem.Generated.DTO.TestCustomContextSecurityObjFullDTO GetFullTestCustomContextSecurityObjWithRevision(GetFullTestCustomContextSecurityObjWithRevisionAutoRequest getFullTestCustomContextSecurityObjWithRevisionAutoRequest)
        {
            long revision = getFullTestCustomContextSecurityObjWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity = getFullTestCustomContextSecurityObjWithRevisionAutoRequest.testCustomContextSecurityObjIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullTestCustomContextSecurityObjWithRevisionInternal(testCustomContextSecurityObjIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestCustomContextSecurityObjFullDTO GetFullTestCustomContextSecurityObjWithRevisionInternal(SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestCustomContextSecurityObjBLL bll = evaluateData.Context.Logics.TestCustomContextSecurityObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestCustomContextSecurityObj domainObject = bll.GetObjectByRevision(testCustomContextSecurityObjIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestCustomContextSecurityObj (RichDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichTestCustomContextSecurityObjWithRevision")]
        public virtual SampleSystem.Generated.DTO.TestCustomContextSecurityObjRichDTO GetRichTestCustomContextSecurityObjWithRevision(GetRichTestCustomContextSecurityObjWithRevisionAutoRequest getRichTestCustomContextSecurityObjWithRevisionAutoRequest)
        {
            long revision = getRichTestCustomContextSecurityObjWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity = getRichTestCustomContextSecurityObjWithRevisionAutoRequest.testCustomContextSecurityObjIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichTestCustomContextSecurityObjWithRevisionInternal(testCustomContextSecurityObjIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestCustomContextSecurityObjRichDTO GetRichTestCustomContextSecurityObjWithRevisionInternal(SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestCustomContextSecurityObjBLL bll = evaluateData.Context.Logics.TestCustomContextSecurityObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestCustomContextSecurityObj domainObject = bll.GetObjectByRevision(testCustomContextSecurityObjIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestCustomContextSecurityObj (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleTestCustomContextSecurityObjWithRevision")]
        public virtual SampleSystem.Generated.DTO.TestCustomContextSecurityObjSimpleDTO GetSimpleTestCustomContextSecurityObjWithRevision(GetSimpleTestCustomContextSecurityObjWithRevisionAutoRequest getSimpleTestCustomContextSecurityObjWithRevisionAutoRequest)
        {
            long revision = getSimpleTestCustomContextSecurityObjWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity = getSimpleTestCustomContextSecurityObjWithRevisionAutoRequest.testCustomContextSecurityObjIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleTestCustomContextSecurityObjWithRevisionInternal(testCustomContextSecurityObjIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestCustomContextSecurityObjSimpleDTO GetSimpleTestCustomContextSecurityObjWithRevisionInternal(SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestCustomContextSecurityObjBLL bll = evaluateData.Context.Logics.TestCustomContextSecurityObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestCustomContextSecurityObj domainObject = bll.GetObjectByRevision(testCustomContextSecurityObjIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestCustomContextSecurityObj Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetTestCustomContextSecurityObjPropertyRevisionByDateRange")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestCustomContextSecurityObjPropertyRevisionByDateRange(GetTestCustomContextSecurityObjPropertyRevisionByDateRangeAutoRequest getTestCustomContextSecurityObjPropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getTestCustomContextSecurityObjPropertyRevisionByDateRangeAutoRequest.period;
            string propertyName = getTestCustomContextSecurityObjPropertyRevisionByDateRangeAutoRequest.propertyName;
            SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity = getTestCustomContextSecurityObjPropertyRevisionByDateRangeAutoRequest.testCustomContextSecurityObjIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetTestCustomContextSecurityObjPropertyRevisionByDateRangeInternal(testCustomContextSecurityObjIdentity, propertyName, period, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestCustomContextSecurityObjPropertyRevisionByDateRangeInternal(SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestCustomContextSecurityObjBLL bll = evaluateData.Context.Logics.TestCustomContextSecurityObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, SampleSystem.BLL.ISampleSystemSecurityService, SampleSystem.SampleSystemSecurityOperationCode, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.TestCustomContextSecurityObj>(testCustomContextSecurityObjIdentity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get TestCustomContextSecurityObj Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetTestCustomContextSecurityObjPropertyRevisions")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestCustomContextSecurityObjPropertyRevisions(GetTestCustomContextSecurityObjPropertyRevisionsAutoRequest getTestCustomContextSecurityObjPropertyRevisionsAutoRequest)
        {
            string propertyName = getTestCustomContextSecurityObjPropertyRevisionsAutoRequest.propertyName;
            SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity = getTestCustomContextSecurityObjPropertyRevisionsAutoRequest.testCustomContextSecurityObjIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetTestCustomContextSecurityObjPropertyRevisionsInternal(testCustomContextSecurityObjIdentity, propertyName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestCustomContextSecurityObjPropertyRevisionsInternal(SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestCustomContextSecurityObjBLL bll = evaluateData.Context.Logics.TestCustomContextSecurityObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, SampleSystem.BLL.ISampleSystemSecurityService, SampleSystem.SampleSystemSecurityOperationCode, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.TestCustomContextSecurityObj>(testCustomContextSecurityObjIdentity.Id, propertyName);
        }
        
        /// <summary>
        /// Get TestCustomContextSecurityObj revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetTestCustomContextSecurityObjRevisions")]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetTestCustomContextSecurityObjRevisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetTestCustomContextSecurityObjRevisionsInternal(testCustomContextSecurityObjIdentity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetTestCustomContextSecurityObjRevisionsInternal(SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestCustomContextSecurityObjBLL bll = evaluateData.Context.Logics.TestCustomContextSecurityObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(testCustomContextSecurityObjIdentity.Id));
        }
        
        /// <summary>
        /// Get TestCustomContextSecurityObj (VisualDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualTestCustomContextSecurityObjWithRevision")]
        public virtual SampleSystem.Generated.DTO.TestCustomContextSecurityObjVisualDTO GetVisualTestCustomContextSecurityObjWithRevision(GetVisualTestCustomContextSecurityObjWithRevisionAutoRequest getVisualTestCustomContextSecurityObjWithRevisionAutoRequest)
        {
            long revision = getVisualTestCustomContextSecurityObjWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity = getVisualTestCustomContextSecurityObjWithRevisionAutoRequest.testCustomContextSecurityObjIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualTestCustomContextSecurityObjWithRevisionInternal(testCustomContextSecurityObjIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestCustomContextSecurityObjVisualDTO GetVisualTestCustomContextSecurityObjWithRevisionInternal(SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestCustomContextSecurityObjBLL bll = evaluateData.Context.Logics.TestCustomContextSecurityObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestCustomContextSecurityObj domainObject = bll.GetObjectByRevision(testCustomContextSecurityObjIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetFullTestCustomContextSecurityObjWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetRichTestCustomContextSecurityObjWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetSimpleTestCustomContextSecurityObjWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetTestCustomContextSecurityObjPropertyRevisionByDateRangeAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=2)]
        public Framework.Core.Period? period;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetTestCustomContextSecurityObjPropertyRevisionsAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetVisualTestCustomContextSecurityObjWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
}
