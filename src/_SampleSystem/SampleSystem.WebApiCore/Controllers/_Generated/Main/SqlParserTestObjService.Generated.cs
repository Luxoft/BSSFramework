namespace SampleSystem.WebApiCore.Controllers.Main
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("api/[controller]/[action]")]
    public partial class SqlParserTestObjController : Framework.Infrastructure.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get SqlParserTestObj (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.SqlParserTestObjFullDTO GetFullSqlParserTestObj([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdentity)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetFullSqlParserTestObjInternal(sqlParserTestObjIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SqlParserTestObjFullDTO GetFullSqlParserTestObjInternal(SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdentity, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISqlParserTestObjBLL bll = evaluateData.Context.Logics.SqlParserTestObjFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.SqlParserTestObj domainObject = bll.GetById(sqlParserTestObjIdentity.Id, true, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.SqlParserTestObj>(Framework.BLL.Domain.DTO.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of SqlParserTestObjs (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.SqlParserTestObjFullDTO> GetFullSqlParserTestObjs()
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetFullSqlParserTestObjsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get SqlParserTestObjs (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.SqlParserTestObjFullDTO> GetFullSqlParserTestObjsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO[] sqlParserTestObjIdents)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetFullSqlParserTestObjsByIdentsInternal(sqlParserTestObjIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.SqlParserTestObjFullDTO> GetFullSqlParserTestObjsByIdentsInternal(SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO[] sqlParserTestObjIdents, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISqlParserTestObjBLL bll = evaluateData.Context.Logics.SqlParserTestObjFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(sqlParserTestObjIdents, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.SqlParserTestObj>(Framework.BLL.Domain.DTO.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.SqlParserTestObjFullDTO> GetFullSqlParserTestObjsInternal(Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISqlParserTestObjBLL bll = evaluateData.Context.Logics.SqlParserTestObjFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(new Framework.BLL.DTOFetchRule<SampleSystem.Domain.SqlParserTestObj>(Framework.BLL.Domain.DTO.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get SqlParserTestObj (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.SqlParserTestObjSimpleDTO GetSimpleSqlParserTestObj([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdentity)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetSimpleSqlParserTestObjInternal(sqlParserTestObjIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SqlParserTestObjSimpleDTO GetSimpleSqlParserTestObjInternal(SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdentity, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISqlParserTestObjBLL bll = evaluateData.Context.Logics.SqlParserTestObjFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.SqlParserTestObj domainObject = bll.GetById(sqlParserTestObjIdentity.Id, true, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.SqlParserTestObj>(Framework.BLL.Domain.DTO.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of SqlParserTestObjs (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.SqlParserTestObjSimpleDTO> GetSimpleSqlParserTestObjs()
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetSimpleSqlParserTestObjsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get SqlParserTestObjs (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.SqlParserTestObjSimpleDTO> GetSimpleSqlParserTestObjsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO[] sqlParserTestObjIdents)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetSimpleSqlParserTestObjsByIdentsInternal(sqlParserTestObjIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.SqlParserTestObjSimpleDTO> GetSimpleSqlParserTestObjsByIdentsInternal(SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO[] sqlParserTestObjIdents, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISqlParserTestObjBLL bll = evaluateData.Context.Logics.SqlParserTestObjFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(sqlParserTestObjIdents, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.SqlParserTestObj>(Framework.BLL.Domain.DTO.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.SqlParserTestObjSimpleDTO> GetSimpleSqlParserTestObjsInternal(Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISqlParserTestObjBLL bll = evaluateData.Context.Logics.SqlParserTestObjFactory.Create(Anch.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(new Framework.BLL.DTOFetchRule<SampleSystem.Domain.SqlParserTestObj>(Framework.BLL.Domain.DTO.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Remove SqlParserTestObj
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual void RemoveSqlParserTestObj([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdent)
        {
            this.Evaluate(Framework.Database.DBSessionMode.Write, evaluateData => this.RemoveSqlParserTestObjInternal(sqlParserTestObjIdent, evaluateData));
        }
        
        protected virtual void RemoveSqlParserTestObjInternal(SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdent, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISqlParserTestObjBLL bll = evaluateData.Context.Logics.SqlParserTestObjFactory.Create(Anch.SecuritySystem.SecurityRule.Edit);
            this.RemoveSqlParserTestObjInternal(sqlParserTestObjIdent, evaluateData, bll);
        }
        
        protected virtual void RemoveSqlParserTestObjInternal(SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdent, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData, SampleSystem.BLL.ISqlParserTestObjBLL bll)
        {
            SampleSystem.Domain.SqlParserTestObj domainObject = bll.GetById(sqlParserTestObjIdent.Id, true);
            bll.Remove(domainObject);
        }
        
        /// <summary>
        /// Save SqlParserTestObjs
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO SaveSqlParserTestObj([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.SqlParserTestObjStrictDTO sqlParserTestObjStrict)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Write, evaluateData => this.SaveSqlParserTestObjInternal(sqlParserTestObjStrict, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO SaveSqlParserTestObjInternal(SampleSystem.Generated.DTO.SqlParserTestObjStrictDTO sqlParserTestObjStrict, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISqlParserTestObjBLL bll = evaluateData.Context.Logics.SqlParserTestObjFactory.Create(Anch.SecuritySystem.SecurityRule.Edit);
            return this.SaveSqlParserTestObjInternal(sqlParserTestObjStrict, evaluateData, bll);
        }
        
        protected virtual SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO SaveSqlParserTestObjInternal(SampleSystem.Generated.DTO.SqlParserTestObjStrictDTO sqlParserTestObjStrict, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData, SampleSystem.BLL.ISqlParserTestObjBLL bll)
        {
            SampleSystem.Domain.SqlParserTestObj domainObject = Framework.BLL.DefaultDomainBLLBaseExtensions.GetByIdOrCreate(bll, sqlParserTestObjStrict.Id);
            sqlParserTestObjStrict.MapToDomainObject(evaluateData.MappingService, domainObject);
            bll.Save(domainObject);
            return SampleSystem.Generated.DTO.LambdaHelper.ToIdentityDTO(domainObject);
        }
    }
}
