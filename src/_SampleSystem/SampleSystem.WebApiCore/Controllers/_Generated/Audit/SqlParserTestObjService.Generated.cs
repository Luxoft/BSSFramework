namespace SampleSystem.WebApiCore.Controllers.Audit
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/[controller]/[action]")]
    public partial class SqlParserTestObjController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get SqlParserTestObj (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullSqlParserTestObjWithRevision")]
        public virtual SampleSystem.Generated.DTO.SqlParserTestObjFullDTO GetFullSqlParserTestObjWithRevision(GetFullSqlParserTestObjWithRevisionAutoRequest getFullSqlParserTestObjWithRevisionAutoRequest)
        {
            long revision = getFullSqlParserTestObjWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdentity = getFullSqlParserTestObjWithRevisionAutoRequest.SqlParserTestObjIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullSqlParserTestObjWithRevisionInternal(sqlParserTestObjIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SqlParserTestObjFullDTO GetFullSqlParserTestObjWithRevisionInternal(SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISqlParserTestObjBLL bll = evaluateData.Context.Logics.SqlParserTestObjFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.SqlParserTestObj domainObject = bll.GetObjectByRevision(sqlParserTestObjIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get SqlParserTestObj (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleSqlParserTestObjWithRevision")]
        public virtual SampleSystem.Generated.DTO.SqlParserTestObjSimpleDTO GetSimpleSqlParserTestObjWithRevision(GetSimpleSqlParserTestObjWithRevisionAutoRequest getSimpleSqlParserTestObjWithRevisionAutoRequest)
        {
            long revision = getSimpleSqlParserTestObjWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdentity = getSimpleSqlParserTestObjWithRevisionAutoRequest.SqlParserTestObjIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleSqlParserTestObjWithRevisionInternal(sqlParserTestObjIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SqlParserTestObjSimpleDTO GetSimpleSqlParserTestObjWithRevisionInternal(SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISqlParserTestObjBLL bll = evaluateData.Context.Logics.SqlParserTestObjFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.SqlParserTestObj domainObject = bll.GetObjectByRevision(sqlParserTestObjIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get SqlParserTestObj Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSqlParserTestObjPropertyRevisionByDateRange")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetSqlParserTestObjPropertyRevisionByDateRange(GetSqlParserTestObjPropertyRevisionByDateRangeAutoRequest getSqlParserTestObjPropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getSqlParserTestObjPropertyRevisionByDateRangeAutoRequest.Period;
            string propertyName = getSqlParserTestObjPropertyRevisionByDateRangeAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdentity = getSqlParserTestObjPropertyRevisionByDateRangeAutoRequest.SqlParserTestObjIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSqlParserTestObjPropertyRevisionByDateRangeInternal(sqlParserTestObjIdentity, propertyName, period, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetSqlParserTestObjPropertyRevisionByDateRangeInternal(SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdentity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISqlParserTestObjBLL bll = evaluateData.Context.Logics.SqlParserTestObjFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.SqlParserTestObj>(sqlParserTestObjIdentity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get SqlParserTestObj Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSqlParserTestObjPropertyRevisions")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetSqlParserTestObjPropertyRevisions(GetSqlParserTestObjPropertyRevisionsAutoRequest getSqlParserTestObjPropertyRevisionsAutoRequest)
        {
            string propertyName = getSqlParserTestObjPropertyRevisionsAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdentity = getSqlParserTestObjPropertyRevisionsAutoRequest.SqlParserTestObjIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSqlParserTestObjPropertyRevisionsInternal(sqlParserTestObjIdentity, propertyName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetSqlParserTestObjPropertyRevisionsInternal(SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdentity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISqlParserTestObjBLL bll = evaluateData.Context.Logics.SqlParserTestObjFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.SqlParserTestObj>(sqlParserTestObjIdentity.Id, propertyName);
        }
        
        /// <summary>
        /// Get SqlParserTestObj revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSqlParserTestObjRevisions")]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetSqlParserTestObjRevisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSqlParserTestObjRevisionsInternal(sqlParserTestObjIdentity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetSqlParserTestObjRevisionsInternal(SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISqlParserTestObjBLL bll = evaluateData.Context.Logics.SqlParserTestObjFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(sqlParserTestObjIdentity.Id));
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetFullSqlParserTestObjWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO SqlParserTestObjIdentity
        {
            get
            {
                return this.sqlParserTestObjIdentity;
            }
            set
            {
                this.sqlParserTestObjIdentity = value;
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
    public partial class GetSimpleSqlParserTestObjWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO SqlParserTestObjIdentity
        {
            get
            {
                return this.sqlParserTestObjIdentity;
            }
            set
            {
                this.sqlParserTestObjIdentity = value;
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
    public partial class GetSqlParserTestObjPropertyRevisionByDateRangeAutoRequest
    {
        
        private SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdentity;
        
        private string propertyName;
        
        private Framework.Core.Period? period;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO SqlParserTestObjIdentity
        {
            get
            {
                return this.sqlParserTestObjIdentity;
            }
            set
            {
                this.sqlParserTestObjIdentity = value;
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
    public partial class GetSqlParserTestObjPropertyRevisionsAutoRequest
    {
        
        private SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdentity;
        
        private string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO SqlParserTestObjIdentity
        {
            get
            {
                return this.sqlParserTestObjIdentity;
            }
            set
            {
                this.sqlParserTestObjIdentity = value;
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
