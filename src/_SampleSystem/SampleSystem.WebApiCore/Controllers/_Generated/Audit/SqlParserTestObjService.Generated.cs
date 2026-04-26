namespace SampleSystem.WebApiCore.Controllers.Audit
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/[controller]/[action]")]
    public partial class SqlParserTestObjController : Framework.Infrastructure.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get SqlParserTestObj (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.SqlParserTestObjFullDTO GetFullSqlParserTestObjWithRevision(GetFullSqlParserTestObjWithRevisionAutoRequest getFullSqlParserTestObjWithRevisionAutoRequest)
        {
            long revision = getFullSqlParserTestObjWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdentity = getFullSqlParserTestObjWithRevisionAutoRequest.SqlParserTestObjIdentity;
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetFullSqlParserTestObjWithRevisionInternal(sqlParserTestObjIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SqlParserTestObjFullDTO GetFullSqlParserTestObjWithRevisionInternal(SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdentity, long revision, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISqlParserTestObjBLL bll = evaluateData.Context.Logics.SqlParserTestObjFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.SqlParserTestObj domainObject = bll.GetObjectByRevision(sqlParserTestObjIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get SqlParserTestObj (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.SqlParserTestObjSimpleDTO GetSimpleSqlParserTestObjWithRevision(GetSimpleSqlParserTestObjWithRevisionAutoRequest getSimpleSqlParserTestObjWithRevisionAutoRequest)
        {
            long revision = getSimpleSqlParserTestObjWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdentity = getSimpleSqlParserTestObjWithRevisionAutoRequest.SqlParserTestObjIdentity;
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetSimpleSqlParserTestObjWithRevisionInternal(sqlParserTestObjIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SqlParserTestObjSimpleDTO GetSimpleSqlParserTestObjWithRevisionInternal(SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdentity, long revision, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISqlParserTestObjBLL bll = evaluateData.Context.Logics.SqlParserTestObjFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.SqlParserTestObj domainObject = bll.GetObjectByRevision(sqlParserTestObjIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get SqlParserTestObj Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetSqlParserTestObjPropertyRevisionByDateRange(GetSqlParserTestObjPropertyRevisionByDateRangeAutoRequest getSqlParserTestObjPropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getSqlParserTestObjPropertyRevisionByDateRangeAutoRequest.Period;
            string propertyName = getSqlParserTestObjPropertyRevisionByDateRangeAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdentity = getSqlParserTestObjPropertyRevisionByDateRangeAutoRequest.SqlParserTestObjIdentity;
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetSqlParserTestObjPropertyRevisionByDateRangeInternal(sqlParserTestObjIdentity, propertyName, period, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetSqlParserTestObjPropertyRevisionByDateRangeInternal(SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdentity, string propertyName, Framework.Core.Period? period, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISqlParserTestObjBLL bll = evaluateData.Context.Logics.SqlParserTestObjFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            return new Framework.Infrastructure.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.BLL.Services.IRootSecurityService, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.SqlParserTestObj>(sqlParserTestObjIdentity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get SqlParserTestObj Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetSqlParserTestObjPropertyRevisions(GetSqlParserTestObjPropertyRevisionsAutoRequest getSqlParserTestObjPropertyRevisionsAutoRequest)
        {
            string propertyName = getSqlParserTestObjPropertyRevisionsAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdentity = getSqlParserTestObjPropertyRevisionsAutoRequest.SqlParserTestObjIdentity;
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetSqlParserTestObjPropertyRevisionsInternal(sqlParserTestObjIdentity, propertyName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetSqlParserTestObjPropertyRevisionsInternal(SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdentity, string propertyName, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISqlParserTestObjBLL bll = evaluateData.Context.Logics.SqlParserTestObjFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            return new Framework.Infrastructure.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.BLL.Services.IRootSecurityService, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.SqlParserTestObj>(sqlParserTestObjIdentity.Id, propertyName);
        }
        
        /// <summary>
        /// Get SqlParserTestObj revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.BLL.DTOMapping.Domain.DefaultDomainObjectRevisionDTO GetSqlParserTestObjRevisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdentity)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetSqlParserTestObjRevisionsInternal(sqlParserTestObjIdentity, evaluateData));
        }
        
        protected virtual Framework.BLL.DTOMapping.Domain.DefaultDomainObjectRevisionDTO GetSqlParserTestObjRevisionsInternal(SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdentity, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISqlParserTestObjBLL bll = evaluateData.Context.Logics.SqlParserTestObjFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            return new Framework.BLL.DTOMapping.Domain.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(sqlParserTestObjIdentity.Id));
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.BLL.DTOMapping.Domain.AutoRequestAttribute()]
    public partial class GetFullSqlParserTestObjWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.BLL.DTOMapping.Domain.AutoRequestPropertyAttribute(OrderIndex=0)]
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
        [Framework.BLL.DTOMapping.Domain.AutoRequestPropertyAttribute(OrderIndex=1)]
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
    [Framework.BLL.DTOMapping.Domain.AutoRequestAttribute()]
    public partial class GetSimpleSqlParserTestObjWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.BLL.DTOMapping.Domain.AutoRequestPropertyAttribute(OrderIndex=0)]
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
        [Framework.BLL.DTOMapping.Domain.AutoRequestPropertyAttribute(OrderIndex=1)]
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
    [Framework.BLL.DTOMapping.Domain.AutoRequestAttribute()]
    public partial class GetSqlParserTestObjPropertyRevisionByDateRangeAutoRequest
    {
        
        private SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdentity;
        
        private string propertyName;
        
        private Framework.Core.Period? period;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.BLL.DTOMapping.Domain.AutoRequestPropertyAttribute(OrderIndex=0)]
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
        [Framework.BLL.DTOMapping.Domain.AutoRequestPropertyAttribute(OrderIndex=1)]
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
        [Framework.BLL.DTOMapping.Domain.AutoRequestPropertyAttribute(OrderIndex=2)]
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
    [Framework.BLL.DTOMapping.Domain.AutoRequestAttribute()]
    public partial class GetSqlParserTestObjPropertyRevisionsAutoRequest
    {
        
        private SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdentity;
        
        private string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.BLL.DTOMapping.Domain.AutoRequestPropertyAttribute(OrderIndex=0)]
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
        [Framework.BLL.DTOMapping.Domain.AutoRequestPropertyAttribute(OrderIndex=1)]
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
