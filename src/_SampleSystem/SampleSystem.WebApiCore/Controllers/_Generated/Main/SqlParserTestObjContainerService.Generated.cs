namespace SampleSystem.WebApiCore.Controllers.Main
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("api/v{version:apiVersion}/[controller]")]
    public partial class SqlParserTestObjContainerController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>>
    {
        
        /// <summary>
        /// Get SqlParserTestObjContainer (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullSqlParserTestObjContainer")]
        public virtual SampleSystem.Generated.DTO.SqlParserTestObjContainerFullDTO GetFullSqlParserTestObjContainer([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.SqlParserTestObjContainerIdentityDTO sqlParserTestObjContainerIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullSqlParserTestObjContainerInternal(sqlParserTestObjContainerIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SqlParserTestObjContainerFullDTO GetFullSqlParserTestObjContainerInternal(SampleSystem.Generated.DTO.SqlParserTestObjContainerIdentityDTO sqlParserTestObjContainerIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISqlParserTestObjContainerBLL bll = evaluateData.Context.Logics.SqlParserTestObjContainerFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.SqlParserTestObjContainer domainObject = bll.GetById(sqlParserTestObjContainerIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.SqlParserTestObjContainer>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of SqlParserTestObjContainers (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullSqlParserTestObjContainers")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.SqlParserTestObjContainerFullDTO> GetFullSqlParserTestObjContainers()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullSqlParserTestObjContainersInternal(evaluateData));
        }
        
        /// <summary>
        /// Get SqlParserTestObjContainers (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullSqlParserTestObjContainersByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.SqlParserTestObjContainerFullDTO> GetFullSqlParserTestObjContainersByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.SqlParserTestObjContainerIdentityDTO[] sqlParserTestObjContainerIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullSqlParserTestObjContainersByIdentsInternal(sqlParserTestObjContainerIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.SqlParserTestObjContainerFullDTO> GetFullSqlParserTestObjContainersByIdentsInternal(SampleSystem.Generated.DTO.SqlParserTestObjContainerIdentityDTO[] sqlParserTestObjContainerIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISqlParserTestObjContainerBLL bll = evaluateData.Context.Logics.SqlParserTestObjContainerFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(sqlParserTestObjContainerIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.SqlParserTestObjContainer>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.SqlParserTestObjContainerFullDTO> GetFullSqlParserTestObjContainersInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISqlParserTestObjContainerBLL bll = evaluateData.Context.Logics.SqlParserTestObjContainerFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.SqlParserTestObjContainer>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get SqlParserTestObjContainer (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleSqlParserTestObjContainer")]
        public virtual SampleSystem.Generated.DTO.SqlParserTestObjContainerSimpleDTO GetSimpleSqlParserTestObjContainer([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.SqlParserTestObjContainerIdentityDTO sqlParserTestObjContainerIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleSqlParserTestObjContainerInternal(sqlParserTestObjContainerIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SqlParserTestObjContainerSimpleDTO GetSimpleSqlParserTestObjContainerInternal(SampleSystem.Generated.DTO.SqlParserTestObjContainerIdentityDTO sqlParserTestObjContainerIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISqlParserTestObjContainerBLL bll = evaluateData.Context.Logics.SqlParserTestObjContainerFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.SqlParserTestObjContainer domainObject = bll.GetById(sqlParserTestObjContainerIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.SqlParserTestObjContainer>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of SqlParserTestObjContainers (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleSqlParserTestObjContainers")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.SqlParserTestObjContainerSimpleDTO> GetSimpleSqlParserTestObjContainers()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleSqlParserTestObjContainersInternal(evaluateData));
        }
        
        /// <summary>
        /// Get SqlParserTestObjContainers (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleSqlParserTestObjContainersByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.SqlParserTestObjContainerSimpleDTO> GetSimpleSqlParserTestObjContainersByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.SqlParserTestObjContainerIdentityDTO[] sqlParserTestObjContainerIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleSqlParserTestObjContainersByIdentsInternal(sqlParserTestObjContainerIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.SqlParserTestObjContainerSimpleDTO> GetSimpleSqlParserTestObjContainersByIdentsInternal(SampleSystem.Generated.DTO.SqlParserTestObjContainerIdentityDTO[] sqlParserTestObjContainerIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISqlParserTestObjContainerBLL bll = evaluateData.Context.Logics.SqlParserTestObjContainerFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(sqlParserTestObjContainerIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.SqlParserTestObjContainer>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.SqlParserTestObjContainerSimpleDTO> GetSimpleSqlParserTestObjContainersInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISqlParserTestObjContainerBLL bll = evaluateData.Context.Logics.SqlParserTestObjContainerFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.SqlParserTestObjContainer>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Save SqlParserTestObjContainers
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("SaveSqlParserTestObjContainer")]
        public virtual SampleSystem.Generated.DTO.SqlParserTestObjContainerIdentityDTO SaveSqlParserTestObjContainer([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.SqlParserTestObjContainerStrictDTO sqlParserTestObjContainerStrict)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Write, evaluateData => this.SaveSqlParserTestObjContainerInternal(sqlParserTestObjContainerStrict, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SqlParserTestObjContainerIdentityDTO SaveSqlParserTestObjContainerInternal(SampleSystem.Generated.DTO.SqlParserTestObjContainerStrictDTO sqlParserTestObjContainerStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISqlParserTestObjContainerBLL bll = evaluateData.Context.Logics.SqlParserTestObjContainerFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            return this.SaveSqlParserTestObjContainerInternal(sqlParserTestObjContainerStrict, evaluateData, bll);
        }
        
        protected virtual SampleSystem.Generated.DTO.SqlParserTestObjContainerIdentityDTO SaveSqlParserTestObjContainerInternal(SampleSystem.Generated.DTO.SqlParserTestObjContainerStrictDTO sqlParserTestObjContainerStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData, SampleSystem.BLL.ISqlParserTestObjContainerBLL bll)
        {
            SampleSystem.Domain.SqlParserTestObjContainer domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByIdOrCreate(bll, sqlParserTestObjContainerStrict.Id);
            sqlParserTestObjContainerStrict.MapToDomainObject(evaluateData.MappingService, domainObject);
            bll.Save(domainObject);
            return SampleSystem.Generated.DTO.LambdaHelper.ToIdentityDTO(domainObject);
        }
    }
}
