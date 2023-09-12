namespace SampleSystem.WebApiCore.Controllers.Main
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("api/v{version:apiVersion}/[controller]")]
    public partial class SqlParserTestObjController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>>
    {
        
        /// <summary>
        /// Check SqlParserTestObj access
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CheckSqlParserTestObjAccess")]
        public virtual void CheckSqlParserTestObjAccess(CheckSqlParserTestObjAccessAutoRequest checkSqlParserTestObjAccessAutoRequest)
        {
            SampleSystem.SampleSystemSecurityOperationCode securityOperationCode = checkSqlParserTestObjAccessAutoRequest.securityOperationCode;
            SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdent = checkSqlParserTestObjAccessAutoRequest.sqlParserTestObjIdent;
            this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.CheckSqlParserTestObjAccessInternal(sqlParserTestObjIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual void CheckSqlParserTestObjAccessInternal(SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdent, SampleSystem.SampleSystemSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISqlParserTestObjBLL bll = evaluateData.Context.Logics.SqlParserTestObj;
            Framework.Security.SecurityOperationParser.Check(securityOperationCode);
            SampleSystem.Domain.SqlParserTestObj domainObject = bll.GetById(sqlParserTestObjIdent.Id, true);
            Framework.SecuritySystem.SecurityProviderExtensions.CheckAccess(evaluateData.Context.SecurityService.GetSecurityProvider<SampleSystem.Domain.SqlParserTestObj>(securityOperationCode), domainObject, evaluateData.Context.AccessDeniedExceptionService);
        }
        
        /// <summary>
        /// Get SqlParserTestObj (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullSqlParserTestObj")]
        public virtual SampleSystem.Generated.DTO.SqlParserTestObjFullDTO GetFullSqlParserTestObj([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullSqlParserTestObjInternal(sqlParserTestObjIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SqlParserTestObjFullDTO GetFullSqlParserTestObjInternal(SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISqlParserTestObjBLL bll = evaluateData.Context.Logics.SqlParserTestObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.SqlParserTestObj domainObject = bll.GetById(sqlParserTestObjIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.SqlParserTestObj>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of SqlParserTestObjs (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullSqlParserTestObjs")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.SqlParserTestObjFullDTO> GetFullSqlParserTestObjs()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullSqlParserTestObjsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get SqlParserTestObjs (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullSqlParserTestObjsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.SqlParserTestObjFullDTO> GetFullSqlParserTestObjsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO[] sqlParserTestObjIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullSqlParserTestObjsByIdentsInternal(sqlParserTestObjIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.SqlParserTestObjFullDTO> GetFullSqlParserTestObjsByIdentsInternal(SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO[] sqlParserTestObjIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISqlParserTestObjBLL bll = evaluateData.Context.Logics.SqlParserTestObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(sqlParserTestObjIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.SqlParserTestObj>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.SqlParserTestObjFullDTO> GetFullSqlParserTestObjsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISqlParserTestObjBLL bll = evaluateData.Context.Logics.SqlParserTestObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.SqlParserTestObj>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get SqlParserTestObj (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleSqlParserTestObj")]
        public virtual SampleSystem.Generated.DTO.SqlParserTestObjSimpleDTO GetSimpleSqlParserTestObj([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleSqlParserTestObjInternal(sqlParserTestObjIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SqlParserTestObjSimpleDTO GetSimpleSqlParserTestObjInternal(SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISqlParserTestObjBLL bll = evaluateData.Context.Logics.SqlParserTestObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.SqlParserTestObj domainObject = bll.GetById(sqlParserTestObjIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.SqlParserTestObj>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of SqlParserTestObjs (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleSqlParserTestObjs")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.SqlParserTestObjSimpleDTO> GetSimpleSqlParserTestObjs()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleSqlParserTestObjsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get SqlParserTestObjs (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleSqlParserTestObjsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.SqlParserTestObjSimpleDTO> GetSimpleSqlParserTestObjsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO[] sqlParserTestObjIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleSqlParserTestObjsByIdentsInternal(sqlParserTestObjIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.SqlParserTestObjSimpleDTO> GetSimpleSqlParserTestObjsByIdentsInternal(SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO[] sqlParserTestObjIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISqlParserTestObjBLL bll = evaluateData.Context.Logics.SqlParserTestObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(sqlParserTestObjIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.SqlParserTestObj>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.SqlParserTestObjSimpleDTO> GetSimpleSqlParserTestObjsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISqlParserTestObjBLL bll = evaluateData.Context.Logics.SqlParserTestObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.SqlParserTestObj>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Check access for SqlParserTestObj
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("HasSqlParserTestObjAccess")]
        public virtual bool HasSqlParserTestObjAccess(HasSqlParserTestObjAccessAutoRequest hasSqlParserTestObjAccessAutoRequest)
        {
            SampleSystem.SampleSystemSecurityOperationCode securityOperationCode = hasSqlParserTestObjAccessAutoRequest.securityOperationCode;
            SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdent = hasSqlParserTestObjAccessAutoRequest.sqlParserTestObjIdent;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.HasSqlParserTestObjAccessInternal(sqlParserTestObjIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual bool HasSqlParserTestObjAccessInternal(SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdent, SampleSystem.SampleSystemSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISqlParserTestObjBLL bll = evaluateData.Context.Logics.SqlParserTestObj;
            Framework.Security.SecurityOperationParser.Check(securityOperationCode);
            SampleSystem.Domain.SqlParserTestObj domainObject = bll.GetById(sqlParserTestObjIdent.Id, true);
            return evaluateData.Context.SecurityService.GetSecurityProvider<SampleSystem.Domain.SqlParserTestObj>(securityOperationCode).HasAccess(domainObject);
        }
        
        /// <summary>
        /// Remove SqlParserTestObj
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("RemoveSqlParserTestObj")]
        public virtual void RemoveSqlParserTestObj([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdent)
        {
            this.Evaluate(Framework.DomainDriven.DBSessionMode.Write, evaluateData => this.RemoveSqlParserTestObjInternal(sqlParserTestObjIdent, evaluateData));
        }
        
        protected virtual void RemoveSqlParserTestObjInternal(SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdent, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISqlParserTestObjBLL bll = evaluateData.Context.Logics.SqlParserTestObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            this.RemoveSqlParserTestObjInternal(sqlParserTestObjIdent, evaluateData, bll);
        }
        
        protected virtual void RemoveSqlParserTestObjInternal(SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdent, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData, SampleSystem.BLL.ISqlParserTestObjBLL bll)
        {
            SampleSystem.Domain.SqlParserTestObj domainObject = bll.GetById(sqlParserTestObjIdent.Id, true);
            bll.Remove(domainObject);
        }
        
        /// <summary>
        /// Save SqlParserTestObjs
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("SaveSqlParserTestObj")]
        public virtual SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO SaveSqlParserTestObj([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.SqlParserTestObjStrictDTO sqlParserTestObjStrict)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Write, evaluateData => this.SaveSqlParserTestObjInternal(sqlParserTestObjStrict, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO SaveSqlParserTestObjInternal(SampleSystem.Generated.DTO.SqlParserTestObjStrictDTO sqlParserTestObjStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISqlParserTestObjBLL bll = evaluateData.Context.Logics.SqlParserTestObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            return this.SaveSqlParserTestObjInternal(sqlParserTestObjStrict, evaluateData, bll);
        }
        
        protected virtual SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO SaveSqlParserTestObjInternal(SampleSystem.Generated.DTO.SqlParserTestObjStrictDTO sqlParserTestObjStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData, SampleSystem.BLL.ISqlParserTestObjBLL bll)
        {
            SampleSystem.Domain.SqlParserTestObj domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByIdOrCreate(bll, sqlParserTestObjStrict.Id);
            sqlParserTestObjStrict.MapToDomainObject(evaluateData.MappingService, domainObject);
            bll.Save(domainObject);
            return SampleSystem.Generated.DTO.LambdaHelper.ToIdentityDTO(domainObject);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class CheckSqlParserTestObjAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public SampleSystem.SampleSystemSecurityOperationCode securityOperationCode;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class HasSqlParserTestObjAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public SampleSystem.SampleSystemSecurityOperationCode securityOperationCode;
    }
}
