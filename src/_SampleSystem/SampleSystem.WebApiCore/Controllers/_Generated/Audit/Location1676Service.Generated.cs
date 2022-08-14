namespace SampleSystem.WebApiCore.Controllers.Audit
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/v{version:apiVersion}/[controller]")]
    public partial class Location1676Controller : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>>
    {
        
        /// <summary>
        /// Get Location1676 (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullLocation1676WithRevision")]
        public virtual SampleSystem.Generated.DTO.Location1676FullDTO GetFullLocation1676WithRevision(GetFullLocation1676WithRevisionAutoRequest getFullLocation1676WithRevisionAutoRequest)
        {
            long revision = getFullLocation1676WithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.Location1676IdentityDTO location1676Identity = getFullLocation1676WithRevisionAutoRequest.location1676Identity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullLocation1676WithRevisionInternal(location1676Identity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.Location1676FullDTO GetFullLocation1676WithRevisionInternal(SampleSystem.Generated.DTO.Location1676IdentityDTO location1676Identity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ILocation1676BLL bll = evaluateData.Context.Logics.Location1676Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.EnversBug1676.Location1676 domainObject = bll.GetObjectByRevision(location1676Identity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Location1676 Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetLocation1676PropertyRevisionByDateRange")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetLocation1676PropertyRevisionByDateRange(GetLocation1676PropertyRevisionByDateRangeAutoRequest getLocation1676PropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getLocation1676PropertyRevisionByDateRangeAutoRequest.period;
            string propertyName = getLocation1676PropertyRevisionByDateRangeAutoRequest.propertyName;
            SampleSystem.Generated.DTO.Location1676IdentityDTO location1676Identity = getLocation1676PropertyRevisionByDateRangeAutoRequest.location1676Identity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetLocation1676PropertyRevisionByDateRangeInternal(location1676Identity, propertyName, period, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetLocation1676PropertyRevisionByDateRangeInternal(SampleSystem.Generated.DTO.Location1676IdentityDTO location1676Identity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ILocation1676BLL bll = evaluateData.Context.Logics.Location1676Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, SampleSystem.BLL.ISampleSystemSecurityService, SampleSystem.SampleSystemSecurityOperationCode, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.EnversBug1676.Location1676>(location1676Identity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get Location1676 Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetLocation1676PropertyRevisions")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetLocation1676PropertyRevisions(GetLocation1676PropertyRevisionsAutoRequest getLocation1676PropertyRevisionsAutoRequest)
        {
            string propertyName = getLocation1676PropertyRevisionsAutoRequest.propertyName;
            SampleSystem.Generated.DTO.Location1676IdentityDTO location1676Identity = getLocation1676PropertyRevisionsAutoRequest.location1676Identity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetLocation1676PropertyRevisionsInternal(location1676Identity, propertyName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetLocation1676PropertyRevisionsInternal(SampleSystem.Generated.DTO.Location1676IdentityDTO location1676Identity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ILocation1676BLL bll = evaluateData.Context.Logics.Location1676Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, SampleSystem.BLL.ISampleSystemSecurityService, SampleSystem.SampleSystemSecurityOperationCode, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.EnversBug1676.Location1676>(location1676Identity.Id, propertyName);
        }
        
        /// <summary>
        /// Get Location1676 revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetLocation1676Revisions")]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetLocation1676Revisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.Location1676IdentityDTO location1676Identity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetLocation1676RevisionsInternal(location1676Identity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetLocation1676RevisionsInternal(SampleSystem.Generated.DTO.Location1676IdentityDTO location1676Identity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ILocation1676BLL bll = evaluateData.Context.Logics.Location1676Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(location1676Identity.Id));
        }
        
        /// <summary>
        /// Get Location1676 (RichDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichLocation1676WithRevision")]
        public virtual SampleSystem.Generated.DTO.Location1676RichDTO GetRichLocation1676WithRevision(GetRichLocation1676WithRevisionAutoRequest getRichLocation1676WithRevisionAutoRequest)
        {
            long revision = getRichLocation1676WithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.Location1676IdentityDTO location1676Identity = getRichLocation1676WithRevisionAutoRequest.location1676Identity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichLocation1676WithRevisionInternal(location1676Identity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.Location1676RichDTO GetRichLocation1676WithRevisionInternal(SampleSystem.Generated.DTO.Location1676IdentityDTO location1676Identity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ILocation1676BLL bll = evaluateData.Context.Logics.Location1676Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.EnversBug1676.Location1676 domainObject = bll.GetObjectByRevision(location1676Identity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Location1676 (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleLocation1676WithRevision")]
        public virtual SampleSystem.Generated.DTO.Location1676SimpleDTO GetSimpleLocation1676WithRevision(GetSimpleLocation1676WithRevisionAutoRequest getSimpleLocation1676WithRevisionAutoRequest)
        {
            long revision = getSimpleLocation1676WithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.Location1676IdentityDTO location1676Identity = getSimpleLocation1676WithRevisionAutoRequest.location1676Identity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleLocation1676WithRevisionInternal(location1676Identity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.Location1676SimpleDTO GetSimpleLocation1676WithRevisionInternal(SampleSystem.Generated.DTO.Location1676IdentityDTO location1676Identity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ILocation1676BLL bll = evaluateData.Context.Logics.Location1676Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.EnversBug1676.Location1676 domainObject = bll.GetObjectByRevision(location1676Identity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Location1676 (VisualDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualLocation1676WithRevision")]
        public virtual SampleSystem.Generated.DTO.Location1676VisualDTO GetVisualLocation1676WithRevision(GetVisualLocation1676WithRevisionAutoRequest getVisualLocation1676WithRevisionAutoRequest)
        {
            long revision = getVisualLocation1676WithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.Location1676IdentityDTO location1676Identity = getVisualLocation1676WithRevisionAutoRequest.location1676Identity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualLocation1676WithRevisionInternal(location1676Identity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.Location1676VisualDTO GetVisualLocation1676WithRevisionInternal(SampleSystem.Generated.DTO.Location1676IdentityDTO location1676Identity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ILocation1676BLL bll = evaluateData.Context.Logics.Location1676Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.EnversBug1676.Location1676 domainObject = bll.GetObjectByRevision(location1676Identity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetFullLocation1676WithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.Location1676IdentityDTO location1676Identity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetLocation1676PropertyRevisionByDateRangeAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.Location1676IdentityDTO location1676Identity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=2)]
        public Framework.Core.Period? period;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetLocation1676PropertyRevisionsAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.Location1676IdentityDTO location1676Identity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetRichLocation1676WithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.Location1676IdentityDTO location1676Identity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetSimpleLocation1676WithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.Location1676IdentityDTO location1676Identity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetVisualLocation1676WithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.Location1676IdentityDTO location1676Identity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
}
