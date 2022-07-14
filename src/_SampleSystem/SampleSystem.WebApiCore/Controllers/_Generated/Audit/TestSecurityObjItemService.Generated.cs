namespace SampleSystem.WebApiCore.Controllers.Audit
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/v{version:apiVersion}/[controller]")]
    public partial class TestSecurityObjItemController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment, SampleSystem.BLL.ISampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>>
    {
        
        public TestSecurityObjItemController(Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment serviceEnvironment, Framework.Exceptions.IExceptionProcessor exceptionProcessor) : 
                base(serviceEnvironment, exceptionProcessor)
        {
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, SampleSystem.BLL.ISampleSystemBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>(session, context, new SampleSystemServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get TestSecurityObjItem (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullTestSecurityObjItemWithRevision")]
        public virtual SampleSystem.Generated.DTO.TestSecurityObjItemFullDTO GetFullTestSecurityObjItemWithRevision(GetFullTestSecurityObjItemWithRevisionAutoRequest getFullTestSecurityObjItemWithRevisionAutoRequest)
        {
            long revision = getFullTestSecurityObjItemWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO testSecurityObjItemIdentity = getFullTestSecurityObjItemWithRevisionAutoRequest.testSecurityObjItemIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullTestSecurityObjItemWithRevisionInternal(testSecurityObjItemIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestSecurityObjItemFullDTO GetFullTestSecurityObjItemWithRevisionInternal(SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO testSecurityObjItemIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecurityObjItemBLL bll = evaluateData.Context.Logics.TestSecurityObjItemFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestSecurityObjItem domainObject = bll.GetObjectByRevision(testSecurityObjItemIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestSecurityObjItem (RichDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichTestSecurityObjItemWithRevision")]
        public virtual SampleSystem.Generated.DTO.TestSecurityObjItemRichDTO GetRichTestSecurityObjItemWithRevision(GetRichTestSecurityObjItemWithRevisionAutoRequest getRichTestSecurityObjItemWithRevisionAutoRequest)
        {
            long revision = getRichTestSecurityObjItemWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO testSecurityObjItemIdentity = getRichTestSecurityObjItemWithRevisionAutoRequest.testSecurityObjItemIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichTestSecurityObjItemWithRevisionInternal(testSecurityObjItemIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestSecurityObjItemRichDTO GetRichTestSecurityObjItemWithRevisionInternal(SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO testSecurityObjItemIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecurityObjItemBLL bll = evaluateData.Context.Logics.TestSecurityObjItemFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestSecurityObjItem domainObject = bll.GetObjectByRevision(testSecurityObjItemIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestSecurityObjItem (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleTestSecurityObjItemWithRevision")]
        public virtual SampleSystem.Generated.DTO.TestSecurityObjItemSimpleDTO GetSimpleTestSecurityObjItemWithRevision(GetSimpleTestSecurityObjItemWithRevisionAutoRequest getSimpleTestSecurityObjItemWithRevisionAutoRequest)
        {
            long revision = getSimpleTestSecurityObjItemWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO testSecurityObjItemIdentity = getSimpleTestSecurityObjItemWithRevisionAutoRequest.testSecurityObjItemIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleTestSecurityObjItemWithRevisionInternal(testSecurityObjItemIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestSecurityObjItemSimpleDTO GetSimpleTestSecurityObjItemWithRevisionInternal(SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO testSecurityObjItemIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecurityObjItemBLL bll = evaluateData.Context.Logics.TestSecurityObjItemFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestSecurityObjItem domainObject = bll.GetObjectByRevision(testSecurityObjItemIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestSecurityObjItem Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetTestSecurityObjItemPropertyRevisionByDateRange")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestSecurityObjItemPropertyRevisionByDateRange(GetTestSecurityObjItemPropertyRevisionByDateRangeAutoRequest getTestSecurityObjItemPropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getTestSecurityObjItemPropertyRevisionByDateRangeAutoRequest.period;
            string propertyName = getTestSecurityObjItemPropertyRevisionByDateRangeAutoRequest.propertyName;
            SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO testSecurityObjItemIdentity = getTestSecurityObjItemPropertyRevisionByDateRangeAutoRequest.testSecurityObjItemIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetTestSecurityObjItemPropertyRevisionByDateRangeInternal(testSecurityObjItemIdentity, propertyName, period, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestSecurityObjItemPropertyRevisionByDateRangeInternal(SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO testSecurityObjItemIdentity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecurityObjItemBLL bll = evaluateData.Context.Logics.TestSecurityObjItemFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, SampleSystem.BLL.ISampleSystemSecurityService, SampleSystem.SampleSystemSecurityOperationCode, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.TestSecurityObjItem>(testSecurityObjItemIdentity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get TestSecurityObjItem Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetTestSecurityObjItemPropertyRevisions")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestSecurityObjItemPropertyRevisions(GetTestSecurityObjItemPropertyRevisionsAutoRequest getTestSecurityObjItemPropertyRevisionsAutoRequest)
        {
            string propertyName = getTestSecurityObjItemPropertyRevisionsAutoRequest.propertyName;
            SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO testSecurityObjItemIdentity = getTestSecurityObjItemPropertyRevisionsAutoRequest.testSecurityObjItemIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetTestSecurityObjItemPropertyRevisionsInternal(testSecurityObjItemIdentity, propertyName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestSecurityObjItemPropertyRevisionsInternal(SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO testSecurityObjItemIdentity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecurityObjItemBLL bll = evaluateData.Context.Logics.TestSecurityObjItemFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, SampleSystem.BLL.ISampleSystemSecurityService, SampleSystem.SampleSystemSecurityOperationCode, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.TestSecurityObjItem>(testSecurityObjItemIdentity.Id, propertyName);
        }
        
        /// <summary>
        /// Get TestSecurityObjItem revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetTestSecurityObjItemRevisions")]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetTestSecurityObjItemRevisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO testSecurityObjItemIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetTestSecurityObjItemRevisionsInternal(testSecurityObjItemIdentity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetTestSecurityObjItemRevisionsInternal(SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO testSecurityObjItemIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecurityObjItemBLL bll = evaluateData.Context.Logics.TestSecurityObjItemFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(testSecurityObjItemIdentity.Id));
        }
        
        /// <summary>
        /// Get TestSecurityObjItem (VisualDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualTestSecurityObjItemWithRevision")]
        public virtual SampleSystem.Generated.DTO.TestSecurityObjItemVisualDTO GetVisualTestSecurityObjItemWithRevision(GetVisualTestSecurityObjItemWithRevisionAutoRequest getVisualTestSecurityObjItemWithRevisionAutoRequest)
        {
            long revision = getVisualTestSecurityObjItemWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO testSecurityObjItemIdentity = getVisualTestSecurityObjItemWithRevisionAutoRequest.testSecurityObjItemIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualTestSecurityObjItemWithRevisionInternal(testSecurityObjItemIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestSecurityObjItemVisualDTO GetVisualTestSecurityObjItemWithRevisionInternal(SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO testSecurityObjItemIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecurityObjItemBLL bll = evaluateData.Context.Logics.TestSecurityObjItemFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestSecurityObjItem domainObject = bll.GetObjectByRevision(testSecurityObjItemIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetFullTestSecurityObjItemWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO testSecurityObjItemIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetRichTestSecurityObjItemWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO testSecurityObjItemIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetSimpleTestSecurityObjItemWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO testSecurityObjItemIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetTestSecurityObjItemPropertyRevisionByDateRangeAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO testSecurityObjItemIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=2)]
        public Framework.Core.Period? period;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetTestSecurityObjItemPropertyRevisionsAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO testSecurityObjItemIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetVisualTestSecurityObjItemWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO testSecurityObjItemIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
}
