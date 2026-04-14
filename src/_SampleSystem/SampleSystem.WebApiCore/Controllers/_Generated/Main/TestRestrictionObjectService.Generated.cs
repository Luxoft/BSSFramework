namespace SampleSystem.WebApiCore.Controllers.Main
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("api/[controller]/[action]")]
    public partial class TestRestrictionObjectController : Framework.Infrastructure.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get TestRestrictionObject (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestRestrictionObjectFullDTO GetFullTestRestrictionObject([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestRestrictionObjectIdentityDTO testRestrictionObjectIdentity)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetFullTestRestrictionObjectInternal(testRestrictionObjectIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestRestrictionObjectFullDTO GetFullTestRestrictionObjectInternal(SampleSystem.Generated.DTO.TestRestrictionObjectIdentityDTO testRestrictionObjectIdentity, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestRestrictionObjectBLL bll = evaluateData.Context.Logics.TestRestrictionObjectFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Employee.TestRestrictionObject domainObject = bll.GetById(testRestrictionObjectIdentity.Id, true, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.Employee.TestRestrictionObject>(Framework.BLL.Domain.DTO.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of TestRestrictionObjects (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestRestrictionObjectFullDTO> GetFullTestRestrictionObjects()
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetFullTestRestrictionObjectsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get TestRestrictionObjects (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestRestrictionObjectFullDTO> GetFullTestRestrictionObjectsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestRestrictionObjectIdentityDTO[] testRestrictionObjectIdents)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetFullTestRestrictionObjectsByIdentsInternal(testRestrictionObjectIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestRestrictionObjectFullDTO> GetFullTestRestrictionObjectsByIdentsInternal(SampleSystem.Generated.DTO.TestRestrictionObjectIdentityDTO[] testRestrictionObjectIdents, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestRestrictionObjectBLL bll = evaluateData.Context.Logics.TestRestrictionObjectFactory.Create(SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(testRestrictionObjectIdents, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.Employee.TestRestrictionObject>(Framework.BLL.Domain.DTO.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestRestrictionObjectFullDTO> GetFullTestRestrictionObjectsInternal(Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestRestrictionObjectBLL bll = evaluateData.Context.Logics.TestRestrictionObjectFactory.Create(SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(new Framework.BLL.DTOFetchRule<SampleSystem.Domain.Employee.TestRestrictionObject>(Framework.BLL.Domain.DTO.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestRestrictionObject (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestRestrictionObjectRichDTO GetRichTestRestrictionObject([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestRestrictionObjectIdentityDTO testRestrictionObjectIdentity)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetRichTestRestrictionObjectInternal(testRestrictionObjectIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestRestrictionObjectRichDTO GetRichTestRestrictionObjectInternal(SampleSystem.Generated.DTO.TestRestrictionObjectIdentityDTO testRestrictionObjectIdentity, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestRestrictionObjectBLL bll = evaluateData.Context.Logics.TestRestrictionObjectFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Employee.TestRestrictionObject domainObject = bll.GetById(testRestrictionObjectIdentity.Id, true, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.Employee.TestRestrictionObject>(Framework.BLL.Domain.DTO.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestRestrictionObject (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestRestrictionObjectSimpleDTO GetSimpleTestRestrictionObject([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestRestrictionObjectIdentityDTO testRestrictionObjectIdentity)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetSimpleTestRestrictionObjectInternal(testRestrictionObjectIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestRestrictionObjectSimpleDTO GetSimpleTestRestrictionObjectInternal(SampleSystem.Generated.DTO.TestRestrictionObjectIdentityDTO testRestrictionObjectIdentity, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestRestrictionObjectBLL bll = evaluateData.Context.Logics.TestRestrictionObjectFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Employee.TestRestrictionObject domainObject = bll.GetById(testRestrictionObjectIdentity.Id, true, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.Employee.TestRestrictionObject>(Framework.BLL.Domain.DTO.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of TestRestrictionObjects (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestRestrictionObjectSimpleDTO> GetSimpleTestRestrictionObjects()
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetSimpleTestRestrictionObjectsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get TestRestrictionObjects (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestRestrictionObjectSimpleDTO> GetSimpleTestRestrictionObjectsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestRestrictionObjectIdentityDTO[] testRestrictionObjectIdents)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetSimpleTestRestrictionObjectsByIdentsInternal(testRestrictionObjectIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestRestrictionObjectSimpleDTO> GetSimpleTestRestrictionObjectsByIdentsInternal(SampleSystem.Generated.DTO.TestRestrictionObjectIdentityDTO[] testRestrictionObjectIdents, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestRestrictionObjectBLL bll = evaluateData.Context.Logics.TestRestrictionObjectFactory.Create(SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(testRestrictionObjectIdents, new Framework.BLL.DTOFetchRule<SampleSystem.Domain.Employee.TestRestrictionObject>(Framework.BLL.Domain.DTO.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.TestRestrictionObjectSimpleDTO> GetSimpleTestRestrictionObjectsInternal(Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestRestrictionObjectBLL bll = evaluateData.Context.Logics.TestRestrictionObjectFactory.Create(SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(new Framework.BLL.DTOFetchRule<SampleSystem.Domain.Employee.TestRestrictionObject>(Framework.BLL.Domain.DTO.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
    }
}
