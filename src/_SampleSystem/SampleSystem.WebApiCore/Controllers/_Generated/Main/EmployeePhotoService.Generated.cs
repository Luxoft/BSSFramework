namespace SampleSystem.WebApiCore.Controllers.Main
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("api/[controller]/[action]")]
    public partial class EmployeePhotoController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get EmployeePhoto (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.EmployeePhotoFullDTO GetFullEmployeePhoto([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeePhotoIdentityDTO employeePhotoIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullEmployeePhotoInternal(employeePhotoIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeePhotoFullDTO GetFullEmployeePhotoInternal(SampleSystem.Generated.DTO.EmployeePhotoIdentityDTO employeePhotoIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeePhotoBLL bll = evaluateData.Context.Logics.EmployeePhotoFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.EmployeePhoto domainObject = bll.GetById(employeePhotoIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeePhoto>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of EmployeePhotos (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeePhotoFullDTO> GetFullEmployeePhotos()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullEmployeePhotosInternal(evaluateData));
        }
        
        /// <summary>
        /// Get EmployeePhotos (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeePhotoFullDTO> GetFullEmployeePhotosByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeePhotoIdentityDTO[] employeePhotoIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullEmployeePhotosByIdentsInternal(employeePhotoIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeePhotoFullDTO> GetFullEmployeePhotosByIdentsInternal(SampleSystem.Generated.DTO.EmployeePhotoIdentityDTO[] employeePhotoIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeePhotoBLL bll = evaluateData.Context.Logics.EmployeePhotoFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(employeePhotoIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeePhoto>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get EmployeePhotos (FullDTO) by operation
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeePhotoFullDTO> GetFullEmployeePhotosByOperation([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.SecuritySystem.DomainSecurityRule.ClientSecurityRule securityRule)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullEmployeePhotosByOperationInternal(securityRule, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeePhotoFullDTO> GetFullEmployeePhotosByOperationInternal(Framework.SecuritySystem.DomainSecurityRule.ClientSecurityRule securityRule, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeePhotoBLL bll = evaluateData.Context.Logics.EmployeePhotoFactory.Create(securityRule);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeePhoto>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeePhotoFullDTO> GetFullEmployeePhotosInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeePhotoBLL bll = evaluateData.Context.Logics.EmployeePhotoFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeePhoto>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get EmployeePhoto (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.EmployeePhotoRichDTO GetRichEmployeePhoto([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeePhotoIdentityDTO employeePhotoIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichEmployeePhotoInternal(employeePhotoIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeePhotoRichDTO GetRichEmployeePhotoInternal(SampleSystem.Generated.DTO.EmployeePhotoIdentityDTO employeePhotoIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeePhotoBLL bll = evaluateData.Context.Logics.EmployeePhotoFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.EmployeePhoto domainObject = bll.GetById(employeePhotoIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeePhoto>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get EmployeePhoto (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.EmployeePhotoSimpleDTO GetSimpleEmployeePhoto([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeePhotoIdentityDTO employeePhotoIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleEmployeePhotoInternal(employeePhotoIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeePhotoSimpleDTO GetSimpleEmployeePhotoInternal(SampleSystem.Generated.DTO.EmployeePhotoIdentityDTO employeePhotoIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeePhotoBLL bll = evaluateData.Context.Logics.EmployeePhotoFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.EmployeePhoto domainObject = bll.GetById(employeePhotoIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeePhoto>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of EmployeePhotos (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeePhotoSimpleDTO> GetSimpleEmployeePhotos()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleEmployeePhotosInternal(evaluateData));
        }
        
        /// <summary>
        /// Get EmployeePhotos (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeePhotoSimpleDTO> GetSimpleEmployeePhotosByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeePhotoIdentityDTO[] employeePhotoIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleEmployeePhotosByIdentsInternal(employeePhotoIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeePhotoSimpleDTO> GetSimpleEmployeePhotosByIdentsInternal(SampleSystem.Generated.DTO.EmployeePhotoIdentityDTO[] employeePhotoIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeePhotoBLL bll = evaluateData.Context.Logics.EmployeePhotoFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(employeePhotoIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeePhoto>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get EmployeePhotos (SimpleDTO) by operation
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeePhotoSimpleDTO> GetSimpleEmployeePhotosByOperation([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.SecuritySystem.DomainSecurityRule.ClientSecurityRule securityRule)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleEmployeePhotosByOperationInternal(securityRule, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeePhotoSimpleDTO> GetSimpleEmployeePhotosByOperationInternal(Framework.SecuritySystem.DomainSecurityRule.ClientSecurityRule securityRule, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeePhotoBLL bll = evaluateData.Context.Logics.EmployeePhotoFactory.Create(securityRule);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeePhoto>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeePhotoSimpleDTO> GetSimpleEmployeePhotosInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeePhotoBLL bll = evaluateData.Context.Logics.EmployeePhotoFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeePhoto>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
    }
}
