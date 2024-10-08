﻿namespace SampleSystem.WebApiCore.Controllers.Integration
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("integrationApi/[controller]/[action]")]
    public partial class EmployeeController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Save Employee
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.EmployeeIdentityDTO SaveEmployee([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeeIntegrationRichDTO employee)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Write, evaluateData => this.SaveEmployeeInternal(employee, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeIdentityDTO SaveEmployeeInternal(SampleSystem.Generated.DTO.EmployeeIntegrationRichDTO employee, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeBLL bll = evaluateData.Context.Logics.Employee;
            evaluateData.Context.Authorization.SecuritySystem.CheckAccess(Framework.SecuritySystem.SecurityRole.SystemIntegration);
            return this.SaveEmployeeInternal(employee, evaluateData, bll);
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeIdentityDTO SaveEmployeeInternal(SampleSystem.Generated.DTO.EmployeeIntegrationRichDTO employee, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData, SampleSystem.BLL.IEmployeeBLL bll)
        {
            SampleSystem.Domain.Employee domainObject = bll.GetById(employee.Id, false);
            if (object.ReferenceEquals(domainObject, null))
            {
                domainObject = new SampleSystem.Domain.Employee();
            }
            employee.MapToDomainObject(evaluateData.MappingService, domainObject);
            bll.Insert(domainObject, employee.Id);
            return SampleSystem.Generated.DTO.LambdaHelper.ToIdentityDTO(domainObject);
        }
    }
}
