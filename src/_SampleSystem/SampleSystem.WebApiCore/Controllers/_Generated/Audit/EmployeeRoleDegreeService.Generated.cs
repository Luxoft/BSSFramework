namespace SampleSystem.WebApiCore.Controllers.Audit
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/v{version:apiVersion}/[controller]")]
    public partial class EmployeeRoleDegreeController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment, SampleSystem.BLL.ISampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>>
    {
        
        public EmployeeRoleDegreeController(Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment serviceEnvironment, Framework.Exceptions.IExceptionProcessor exceptionProcessor) : 
                base(serviceEnvironment, exceptionProcessor)
        {
        }
        
        /// <summary>
        /// Get EmployeeRoleDegree Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetEmployeeRoleDegreePropertyRevisionByDateRange")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetEmployeeRoleDegreePropertyRevisionByDateRange(GetEmployeeRoleDegreePropertyRevisionByDateRangeAutoRequest getEmployeeRoleDegreePropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getEmployeeRoleDegreePropertyRevisionByDateRangeAutoRequest.period;
            string propertyName = getEmployeeRoleDegreePropertyRevisionByDateRangeAutoRequest.propertyName;
            SampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdentity = getEmployeeRoleDegreePropertyRevisionByDateRangeAutoRequest.employeeRoleDegreeIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetEmployeeRoleDegreePropertyRevisionByDateRangeInternal(employeeRoleDegreeIdentity, propertyName, period, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetEmployeeRoleDegreePropertyRevisionByDateRangeInternal(SampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdentity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRoleDegreeBLL bll = evaluateData.Context.Logics.EmployeeRoleDegreeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, SampleSystem.BLL.ISampleSystemSecurityService, SampleSystem.SampleSystemSecurityOperationCode, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.EmployeeRoleDegree>(employeeRoleDegreeIdentity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get EmployeeRoleDegree Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetEmployeeRoleDegreePropertyRevisions")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetEmployeeRoleDegreePropertyRevisions(GetEmployeeRoleDegreePropertyRevisionsAutoRequest getEmployeeRoleDegreePropertyRevisionsAutoRequest)
        {
            string propertyName = getEmployeeRoleDegreePropertyRevisionsAutoRequest.propertyName;
            SampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdentity = getEmployeeRoleDegreePropertyRevisionsAutoRequest.employeeRoleDegreeIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetEmployeeRoleDegreePropertyRevisionsInternal(employeeRoleDegreeIdentity, propertyName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetEmployeeRoleDegreePropertyRevisionsInternal(SampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdentity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRoleDegreeBLL bll = evaluateData.Context.Logics.EmployeeRoleDegreeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, SampleSystem.BLL.ISampleSystemSecurityService, SampleSystem.SampleSystemSecurityOperationCode, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.EmployeeRoleDegree>(employeeRoleDegreeIdentity.Id, propertyName);
        }
        
        /// <summary>
        /// Get EmployeeRoleDegree revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetEmployeeRoleDegreeRevisions")]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetEmployeeRoleDegreeRevisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetEmployeeRoleDegreeRevisionsInternal(employeeRoleDegreeIdentity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetEmployeeRoleDegreeRevisionsInternal(SampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRoleDegreeBLL bll = evaluateData.Context.Logics.EmployeeRoleDegreeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(employeeRoleDegreeIdentity.Id));
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, SampleSystem.BLL.ISampleSystemBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>(session, context, new SampleSystemServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get EmployeeRoleDegree (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullEmployeeRoleDegreeWithRevision")]
        public virtual SampleSystem.Generated.DTO.EmployeeRoleDegreeFullDTO GetFullEmployeeRoleDegreeWithRevision(GetFullEmployeeRoleDegreeWithRevisionAutoRequest getFullEmployeeRoleDegreeWithRevisionAutoRequest)
        {
            long revision = getFullEmployeeRoleDegreeWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdentity = getFullEmployeeRoleDegreeWithRevisionAutoRequest.employeeRoleDegreeIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullEmployeeRoleDegreeWithRevisionInternal(employeeRoleDegreeIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeRoleDegreeFullDTO GetFullEmployeeRoleDegreeWithRevisionInternal(SampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRoleDegreeBLL bll = evaluateData.Context.Logics.EmployeeRoleDegreeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.EmployeeRoleDegree domainObject = bll.GetObjectByRevision(employeeRoleDegreeIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get EmployeeRoleDegree (RichDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichEmployeeRoleDegreeWithRevision")]
        public virtual SampleSystem.Generated.DTO.EmployeeRoleDegreeRichDTO GetRichEmployeeRoleDegreeWithRevision(GetRichEmployeeRoleDegreeWithRevisionAutoRequest getRichEmployeeRoleDegreeWithRevisionAutoRequest)
        {
            long revision = getRichEmployeeRoleDegreeWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdentity = getRichEmployeeRoleDegreeWithRevisionAutoRequest.employeeRoleDegreeIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichEmployeeRoleDegreeWithRevisionInternal(employeeRoleDegreeIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeRoleDegreeRichDTO GetRichEmployeeRoleDegreeWithRevisionInternal(SampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRoleDegreeBLL bll = evaluateData.Context.Logics.EmployeeRoleDegreeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.EmployeeRoleDegree domainObject = bll.GetObjectByRevision(employeeRoleDegreeIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get EmployeeRoleDegree (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleEmployeeRoleDegreeWithRevision")]
        public virtual SampleSystem.Generated.DTO.EmployeeRoleDegreeSimpleDTO GetSimpleEmployeeRoleDegreeWithRevision(GetSimpleEmployeeRoleDegreeWithRevisionAutoRequest getSimpleEmployeeRoleDegreeWithRevisionAutoRequest)
        {
            long revision = getSimpleEmployeeRoleDegreeWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdentity = getSimpleEmployeeRoleDegreeWithRevisionAutoRequest.employeeRoleDegreeIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleEmployeeRoleDegreeWithRevisionInternal(employeeRoleDegreeIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeRoleDegreeSimpleDTO GetSimpleEmployeeRoleDegreeWithRevisionInternal(SampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRoleDegreeBLL bll = evaluateData.Context.Logics.EmployeeRoleDegreeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.EmployeeRoleDegree domainObject = bll.GetObjectByRevision(employeeRoleDegreeIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get EmployeeRoleDegree (VisualDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualEmployeeRoleDegreeWithRevision")]
        public virtual SampleSystem.Generated.DTO.EmployeeRoleDegreeVisualDTO GetVisualEmployeeRoleDegreeWithRevision(GetVisualEmployeeRoleDegreeWithRevisionAutoRequest getVisualEmployeeRoleDegreeWithRevisionAutoRequest)
        {
            long revision = getVisualEmployeeRoleDegreeWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdentity = getVisualEmployeeRoleDegreeWithRevisionAutoRequest.employeeRoleDegreeIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualEmployeeRoleDegreeWithRevisionInternal(employeeRoleDegreeIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeRoleDegreeVisualDTO GetVisualEmployeeRoleDegreeWithRevisionInternal(SampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRoleDegreeBLL bll = evaluateData.Context.Logics.EmployeeRoleDegreeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.EmployeeRoleDegree domainObject = bll.GetObjectByRevision(employeeRoleDegreeIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetEmployeeRoleDegreePropertyRevisionByDateRangeAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=2)]
        public Framework.Core.Period? period;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetEmployeeRoleDegreePropertyRevisionsAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetFullEmployeeRoleDegreeWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetRichEmployeeRoleDegreeWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetSimpleEmployeeRoleDegreeWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetVisualEmployeeRoleDegreeWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
}
