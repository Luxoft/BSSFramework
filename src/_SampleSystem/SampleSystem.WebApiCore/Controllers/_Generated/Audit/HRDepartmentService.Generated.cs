namespace SampleSystem.WebApiCore.Controllers.Audit
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/v{version:apiVersion}/[controller]")]
    public partial class HRDepartmentController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<SampleSystem.BLL.ISampleSystemBLLContext>, SampleSystem.BLL.ISampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>>
    {
        
        public HRDepartmentController(Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<SampleSystem.BLL.ISampleSystemBLLContext> serviceEnvironment, Framework.Exceptions.IExceptionProcessor exceptionProcessor) : 
                base(serviceEnvironment, exceptionProcessor)
        {
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, SampleSystem.BLL.ISampleSystemBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>(session, context, new SampleSystemServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get HRDepartment (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullHRDepartmentWithRevision")]
        public virtual SampleSystem.Generated.DTO.HRDepartmentFullDTO GetFullHRDepartmentWithRevision(GetFullHRDepartmentWithRevisionAutoRequest getFullHRDepartmentWithRevisionAutoRequest)
        {
            long revision = getFullHRDepartmentWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity = getFullHRDepartmentWithRevisionAutoRequest.hRDepartmentIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullHRDepartmentWithRevisionInternal(hRDepartmentIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.HRDepartmentFullDTO GetFullHRDepartmentWithRevisionInternal(SampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
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
            Framework.Core.Period? period = getHRDepartmentPropertyRevisionByDateRangeAutoRequest.period;
            string propertyName = getHRDepartmentPropertyRevisionByDateRangeAutoRequest.propertyName;
            SampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity = getHRDepartmentPropertyRevisionByDateRangeAutoRequest.hRDepartmentIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetHRDepartmentPropertyRevisionByDateRangeInternal(hRDepartmentIdentity, propertyName, period, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetHRDepartmentPropertyRevisionByDateRangeInternal(SampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, SampleSystem.BLL.ISampleSystemSecurityService, SampleSystem.SampleSystemSecurityOperationCode, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.HRDepartment>(hRDepartmentIdentity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get HRDepartment Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetHRDepartmentPropertyRevisions")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetHRDepartmentPropertyRevisions(GetHRDepartmentPropertyRevisionsAutoRequest getHRDepartmentPropertyRevisionsAutoRequest)
        {
            string propertyName = getHRDepartmentPropertyRevisionsAutoRequest.propertyName;
            SampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity = getHRDepartmentPropertyRevisionsAutoRequest.hRDepartmentIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetHRDepartmentPropertyRevisionsInternal(hRDepartmentIdentity, propertyName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetHRDepartmentPropertyRevisionsInternal(SampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, SampleSystem.BLL.ISampleSystemSecurityService, SampleSystem.SampleSystemSecurityOperationCode, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.HRDepartment>(hRDepartmentIdentity.Id, propertyName);
        }
        
        /// <summary>
        /// Get HRDepartment revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetHRDepartmentRevisions")]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetHRDepartmentRevisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetHRDepartmentRevisionsInternal(hRDepartmentIdentity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetHRDepartmentRevisionsInternal(SampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(hRDepartmentIdentity.Id));
        }
        
        /// <summary>
        /// Get HRDepartment (RichDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichHRDepartmentWithRevision")]
        public virtual SampleSystem.Generated.DTO.HRDepartmentRichDTO GetRichHRDepartmentWithRevision(GetRichHRDepartmentWithRevisionAutoRequest getRichHRDepartmentWithRevisionAutoRequest)
        {
            long revision = getRichHRDepartmentWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity = getRichHRDepartmentWithRevisionAutoRequest.hRDepartmentIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichHRDepartmentWithRevisionInternal(hRDepartmentIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.HRDepartmentRichDTO GetRichHRDepartmentWithRevisionInternal(SampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
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
            long revision = getSimpleHRDepartmentWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity = getSimpleHRDepartmentWithRevisionAutoRequest.hRDepartmentIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleHRDepartmentWithRevisionInternal(hRDepartmentIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.HRDepartmentSimpleDTO GetSimpleHRDepartmentWithRevisionInternal(SampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
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
            long revision = getVisualHRDepartmentWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity = getVisualHRDepartmentWithRevisionAutoRequest.hRDepartmentIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualHRDepartmentWithRevisionInternal(hRDepartmentIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.HRDepartmentVisualDTO GetVisualHRDepartmentWithRevisionInternal(SampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.HRDepartment domainObject = bll.GetObjectByRevision(hRDepartmentIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetFullHRDepartmentWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetHRDepartmentPropertyRevisionByDateRangeAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=2)]
        public Framework.Core.Period? period;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetHRDepartmentPropertyRevisionsAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetRichHRDepartmentWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetSimpleHRDepartmentWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetVisualHRDepartmentWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
}
