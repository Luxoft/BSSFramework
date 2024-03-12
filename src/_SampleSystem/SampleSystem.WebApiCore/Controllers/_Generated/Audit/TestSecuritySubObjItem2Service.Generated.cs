namespace SampleSystem.WebApiCore.Controllers.Audit
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/v{version:apiVersion}/[controller]")]
    public partial class TestSecuritySubObjItem2Controller : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get TestSecuritySubObjItem2 (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullTestSecuritySubObjItem2WithRevision")]
        public virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem2FullDTO GetFullTestSecuritySubObjItem2WithRevision(GetFullTestSecuritySubObjItem2WithRevisionAutoRequest getFullTestSecuritySubObjItem2WithRevisionAutoRequest)
        {
            long revision = getFullTestSecuritySubObjItem2WithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.TestSecuritySubObjItem2IdentityDTO testSecuritySubObjItem2Identity = getFullTestSecuritySubObjItem2WithRevisionAutoRequest.testSecuritySubObjItem2Identity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullTestSecuritySubObjItem2WithRevisionInternal(testSecuritySubObjItem2Identity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem2FullDTO GetFullTestSecuritySubObjItem2WithRevisionInternal(SampleSystem.Generated.DTO.TestSecuritySubObjItem2IdentityDTO testSecuritySubObjItem2Identity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItem2BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem2Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestSecuritySubObjItem2 domainObject = bll.GetObjectByRevision(testSecuritySubObjItem2Identity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestSecuritySubObjItem2 (RichDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichTestSecuritySubObjItem2WithRevision")]
        public virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem2RichDTO GetRichTestSecuritySubObjItem2WithRevision(GetRichTestSecuritySubObjItem2WithRevisionAutoRequest getRichTestSecuritySubObjItem2WithRevisionAutoRequest)
        {
            long revision = getRichTestSecuritySubObjItem2WithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.TestSecuritySubObjItem2IdentityDTO testSecuritySubObjItem2Identity = getRichTestSecuritySubObjItem2WithRevisionAutoRequest.testSecuritySubObjItem2Identity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichTestSecuritySubObjItem2WithRevisionInternal(testSecuritySubObjItem2Identity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem2RichDTO GetRichTestSecuritySubObjItem2WithRevisionInternal(SampleSystem.Generated.DTO.TestSecuritySubObjItem2IdentityDTO testSecuritySubObjItem2Identity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItem2BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem2Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestSecuritySubObjItem2 domainObject = bll.GetObjectByRevision(testSecuritySubObjItem2Identity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestSecuritySubObjItem2 (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleTestSecuritySubObjItem2WithRevision")]
        public virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem2SimpleDTO GetSimpleTestSecuritySubObjItem2WithRevision(GetSimpleTestSecuritySubObjItem2WithRevisionAutoRequest getSimpleTestSecuritySubObjItem2WithRevisionAutoRequest)
        {
            long revision = getSimpleTestSecuritySubObjItem2WithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.TestSecuritySubObjItem2IdentityDTO testSecuritySubObjItem2Identity = getSimpleTestSecuritySubObjItem2WithRevisionAutoRequest.testSecuritySubObjItem2Identity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleTestSecuritySubObjItem2WithRevisionInternal(testSecuritySubObjItem2Identity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem2SimpleDTO GetSimpleTestSecuritySubObjItem2WithRevisionInternal(SampleSystem.Generated.DTO.TestSecuritySubObjItem2IdentityDTO testSecuritySubObjItem2Identity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItem2BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem2Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestSecuritySubObjItem2 domainObject = bll.GetObjectByRevision(testSecuritySubObjItem2Identity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestSecuritySubObjItem2 Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetTestSecuritySubObjItem2PropertyRevisionByDateRange")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestSecuritySubObjItem2PropertyRevisionByDateRange(GetTestSecuritySubObjItem2PropertyRevisionByDateRangeAutoRequest getTestSecuritySubObjItem2PropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getTestSecuritySubObjItem2PropertyRevisionByDateRangeAutoRequest.period;
            string propertyName = getTestSecuritySubObjItem2PropertyRevisionByDateRangeAutoRequest.propertyName;
            SampleSystem.Generated.DTO.TestSecuritySubObjItem2IdentityDTO testSecuritySubObjItem2Identity = getTestSecuritySubObjItem2PropertyRevisionByDateRangeAutoRequest.testSecuritySubObjItem2Identity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetTestSecuritySubObjItem2PropertyRevisionByDateRangeInternal(testSecuritySubObjItem2Identity, propertyName, period, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestSecuritySubObjItem2PropertyRevisionByDateRangeInternal(SampleSystem.Generated.DTO.TestSecuritySubObjItem2IdentityDTO testSecuritySubObjItem2Identity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItem2BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem2Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.TestSecuritySubObjItem2>(testSecuritySubObjItem2Identity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get TestSecuritySubObjItem2 Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetTestSecuritySubObjItem2PropertyRevisions")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestSecuritySubObjItem2PropertyRevisions(GetTestSecuritySubObjItem2PropertyRevisionsAutoRequest getTestSecuritySubObjItem2PropertyRevisionsAutoRequest)
        {
            string propertyName = getTestSecuritySubObjItem2PropertyRevisionsAutoRequest.propertyName;
            SampleSystem.Generated.DTO.TestSecuritySubObjItem2IdentityDTO testSecuritySubObjItem2Identity = getTestSecuritySubObjItem2PropertyRevisionsAutoRequest.testSecuritySubObjItem2Identity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetTestSecuritySubObjItem2PropertyRevisionsInternal(testSecuritySubObjItem2Identity, propertyName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestSecuritySubObjItem2PropertyRevisionsInternal(SampleSystem.Generated.DTO.TestSecuritySubObjItem2IdentityDTO testSecuritySubObjItem2Identity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItem2BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem2Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.TestSecuritySubObjItem2>(testSecuritySubObjItem2Identity.Id, propertyName);
        }
        
        /// <summary>
        /// Get TestSecuritySubObjItem2 revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetTestSecuritySubObjItem2Revisions")]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetTestSecuritySubObjItem2Revisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestSecuritySubObjItem2IdentityDTO testSecuritySubObjItem2Identity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetTestSecuritySubObjItem2RevisionsInternal(testSecuritySubObjItem2Identity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetTestSecuritySubObjItem2RevisionsInternal(SampleSystem.Generated.DTO.TestSecuritySubObjItem2IdentityDTO testSecuritySubObjItem2Identity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItem2BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem2Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(testSecuritySubObjItem2Identity.Id));
        }
        
        /// <summary>
        /// Get TestSecuritySubObjItem2 (VisualDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualTestSecuritySubObjItem2WithRevision")]
        public virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem2VisualDTO GetVisualTestSecuritySubObjItem2WithRevision(GetVisualTestSecuritySubObjItem2WithRevisionAutoRequest getVisualTestSecuritySubObjItem2WithRevisionAutoRequest)
        {
            long revision = getVisualTestSecuritySubObjItem2WithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.TestSecuritySubObjItem2IdentityDTO testSecuritySubObjItem2Identity = getVisualTestSecuritySubObjItem2WithRevisionAutoRequest.testSecuritySubObjItem2Identity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualTestSecuritySubObjItem2WithRevisionInternal(testSecuritySubObjItem2Identity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem2VisualDTO GetVisualTestSecuritySubObjItem2WithRevisionInternal(SampleSystem.Generated.DTO.TestSecuritySubObjItem2IdentityDTO testSecuritySubObjItem2Identity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItem2BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem2Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestSecuritySubObjItem2 domainObject = bll.GetObjectByRevision(testSecuritySubObjItem2Identity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetFullTestSecuritySubObjItem2WithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.TestSecuritySubObjItem2IdentityDTO testSecuritySubObjItem2Identity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetRichTestSecuritySubObjItem2WithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.TestSecuritySubObjItem2IdentityDTO testSecuritySubObjItem2Identity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetSimpleTestSecuritySubObjItem2WithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.TestSecuritySubObjItem2IdentityDTO testSecuritySubObjItem2Identity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetTestSecuritySubObjItem2PropertyRevisionByDateRangeAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.TestSecuritySubObjItem2IdentityDTO testSecuritySubObjItem2Identity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=2)]
        public Framework.Core.Period? period;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetTestSecuritySubObjItem2PropertyRevisionsAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.TestSecuritySubObjItem2IdentityDTO testSecuritySubObjItem2Identity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetVisualTestSecuritySubObjItem2WithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.TestSecuritySubObjItem2IdentityDTO testSecuritySubObjItem2Identity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
}
