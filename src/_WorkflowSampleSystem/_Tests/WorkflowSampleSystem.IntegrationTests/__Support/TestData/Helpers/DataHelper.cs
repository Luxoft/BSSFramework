﻿using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Configuration.Domain;
using Framework.Configuration.Domain.Reports;
using Framework.Configuration.Generated.DTO;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.WebApiNetCore;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using WorkflowSampleSystem.Domain;
using WorkflowSampleSystem.Domain.Inline;
using WorkflowSampleSystem.Generated.DTO;
using WorkflowSampleSystem.IntegrationTests.__Support.ServiceEnvironment;
using WorkflowSampleSystem.IntegrationTests.__Support.Utils.Framework;
using WorkflowSampleSystem.WebApiCore.Controllers.Main;

namespace WorkflowSampleSystem.IntegrationTests.__Support.TestData.Helpers
{
    public partial class DataHelper
    {
        public ExceptionMessageIdentityDTO SaveExceptionMessage(
            Guid? id = null,
            string message = null)
        {
            return this.EvaluateWrite(
                context =>
                {
                    var bll = context.Configuration.Logics.ExceptionMessage;
                    var exceptionMessage = bll.GetById(this.GetGuid(id));

                    if (exceptionMessage == null)
                    {
                        exceptionMessage = new ExceptionMessage()
                        {
                            Message = message
                        };

                        bll.Insert(exceptionMessage, this.GetGuid(id));
                    }

                    return exceptionMessage.ToIdentityDTO();
                });
        }

        public LocationIdentityDTO SaveLocation(
            Guid? id = null,
            string name = null,
            LocationIdentityDTO? parent = null,
            bool isFinancial = true,
            int code = 1,
            bool active = true,
            int closeDate = 20)
        {
            name = name ?? StringUtil.UniqueString("Location");
            var parentId = parent != null ? ((LocationIdentityDTO)parent).Id : DefaultConstants.LOCATION_PARENT_ID;

            return this.EvaluateWrite(
                context =>
                {
                    var location = context.Logics.Location.GetById(this.GetGuid(id));

                    if (location == null)
                    {
                        location = new Location
                        {
                            Active = active,
                            Name = name,
                            Parent = context.Logics.Location.GetById(parentId),
                        };

                        context.Logics.Location.Insert(location, this.GetGuid(id));
                    }

                    return location.ToIdentityDTO();
                });
        }

        public BusinessUnitIdentityDTO SaveBusinessUnit(
            Guid? id = null,
            string name = null,
            BusinessUnitIdentityDTO? parent = null,
            bool parentIsNeeded = true,
            Period? period = null,
            bool isPool = true,
            bool isNewBusiness = false,
            decimal commision = 0,
            bool isSpecialCommision = false,
            bool isProduction = true,
            int newBusinessStatusLeft = 0,
            int rank = 1,
            bool active = true)
        {
            name = name ?? StringUtil.UniqueString("BusinessUnit");

            var parentId = parent != null
                ? ((BusinessUnitIdentityDTO)parent).Id
                : DefaultConstants.BUSINESS_UNIT_PARENT_COMPANY_ID;

            BusinessUnit businessUnit;

            return this.EvaluateWrite(
                context =>
                {
                    period = period ?? new Period(context.DateTimeService.CurrentFinancialYear.StartDate.AddYears(-1));
                    businessUnit = context.Logics.BusinessUnit.GetById(this.GetGuid(id));

                    if (businessUnit == null)
                    {
                        businessUnit =
                            new BusinessUnit(parentIsNeeded ? context.Logics.BusinessUnit.GetById(parentId) : null)
                            {
                                Active = active,
                                Name = name,
                                Parent = context.Logics.BusinessUnit.GetById(parentId),
                                Period = period.Value
                            };

                        context.Logics.BusinessUnit.Insert(businessUnit, this.GetGuid(id));
                    }

                    return businessUnit.ToIdentityDTO();
                });
        }

        public HRDepartmentIdentityDTO SaveHRDepartment(
            Guid? id = null,
            string name = null,
            string nameNative = null,
            HRDepartmentIdentityDTO? parent = null,
            EmployeeIdentityDTO? employee = null,
            string code = null,
            string codeNative = null,
            LocationIdentityDTO? location = null,
            bool isLegal = true,
            bool isProduction = true,
            long externalId = 1,
            bool active = true)
        {
            name = name ?? StringUtil.UniqueString("Department");
            nameNative = nameNative ?? name;
            code = code ?? name;
            codeNative = codeNative ?? name;

            var locationId = location != null
                ? ((LocationIdentityDTO)location).Id
                : DefaultConstants.LOCATION_PARENT_ID;

            var parentId = parent != null
                ? ((HRDepartmentIdentityDTO)parent).Id
                : DefaultConstants.HRDEPARTMENT_PARENT_ID;

            return this.EvaluateWrite(
                context =>
                {
                    var department = context.Logics.HRDepartment.GetById(this.GetGuid(id));

                    if (department == null)
                    {
                        var head = employee != null
                            ? context.Logics.Employee.GetById(((EmployeeIdentityDTO)employee).Id)
                            : context.Logics.Employee.GetObjectBy(e => e.Login == this.AuthHelper.GetCurrentUserLogin());

                        department = new HRDepartment
                        {
                            Active = active,
                            Name = name,
                            NameNative = nameNative,
                            Location = context.Logics.Location.GetById(locationId),
                            Parent = context.Logics.HRDepartment.GetById(parentId),
                            Head = head,
                            Code = code,
                            CodeNative = codeNative,
                            IsLegal = isLegal,
                            IsProduction = isProduction,
                            ExternalId = externalId
                        };

                        context.Logics.HRDepartment.Insert(department, this.GetGuid(id));
                    }

                    return department.ToIdentityDTO();
                });
        }
        public Employee SaveEmployee(string name, string login = null)
        {
            var result = this.EvaluateWrite(context =>
            {
                var employee = new Employee() { CellPhone = "2123" };
                employee.NameNative = new Fio { FirstName = name };
                employee.WorkPeriod = new Period(DateTime.Now.AddDays(-1), DateTime.Now);
                employee.EducationDuration = new Period(DateTime.Now.AddYears(-5), DateTime.Now.AddYears(-2));

                if (login != null)
                {
                    employee.Login = login;
                }

                var bll = context.Logics.EmployeeFactory.Create();
                bll.Save(employee);

                return employee;
            });

            return result;
        }

        public ReportIdentityDTO SaveReport(
                Guid? id = null,
                string name = null,
                string description = "",
                string domainTypeName = nameof(Employee),
                string owner = "")
        {
            return this.EvaluateWrite(
                eval =>
                {
                    var report = new Report()
                    {
                        Name = name ?? StringUtil.UniqueString(nameof(Report)),
                        Description = description,
                        DomainTypeName = domainTypeName,
                        Owner = owner
                    };
                    eval.Configuration.Logics.Report.Insert(report, id ?? Guid.NewGuid());

                    return report.ToIdentityDTO();
                });
        }

        public ReportParameterIdentityDTO SaveReportParameter(
                ReportIdentityDTO reportIdentity,
                Guid? id = null,
                string name = null,
                string typeName = nameof(Location),
                string displayValueProperty = nameof(Location.Name))
        {
            return this.EvaluateWrite(
                eval =>
                {
                    var report = eval.Configuration.Logics.Report.GetById(reportIdentity.Id, true);

                    var parameter = new ReportParameter(report)
                    {
                        Name = name ?? StringUtil.UniqueString(nameof(ReportParameter)),
                        TypeName = typeName,
                        DisplayValueProperty = displayValueProperty
                    };

                    eval.Configuration.Logics.ReportParameter.Insert(parameter, id ?? Guid.NewGuid());

                    return parameter.ToIdentityDTO();
                });
        }

        public void SaveReportProperty(
                ReportIdentityDTO reportIdentity,
                string propertyPath,
                Guid? id = null,
                string alias = null,
                string formula = null,
                int order = 0,
                int sortOrdered = 0,
                int sortType = 0)
        {
            this.EvaluateWrite(
                eval =>
                {
                    var report = eval.Configuration.Logics.Report.GetById(reportIdentity.Id, true);

                    var property = new ReportProperty(report)
                    {
                        PropertyPath = propertyPath,
                        Alias = alias ?? propertyPath,
                        Formula = formula,
                        Order = order,
                        SortOrdered = sortOrdered,
                        SortType = sortType
                    };

                    eval.Configuration.Logics.ReportProperty.Insert(property, id ?? Guid.NewGuid());
                });
        }

        public void SaveReportFilter(
                ReportIdentityDTO reportIdentity,
                ReportParameterIdentityDTO parameterIdentity,
                Guid? id = null,
                string property = nameof(Employee.Location),
                string filterOperator = "eq")
        {
            this.EvaluateWrite(
                eval =>
                {
                    var report = eval.Configuration.Logics.Report.GetById(reportIdentity.Id, true);
                    var parameter = eval.Configuration.Logics.ReportParameter.GetById(parameterIdentity.Id, true);

                    eval.Configuration.Logics.ReportFilter.Save(new ReportFilter(report)
                    {
                        IsValueFromParameters = true,
                        Value = parameter.Name,
                        Property = property,
                        FilterOperator = filterOperator
                    });
                });

        }

        public TController GetController<TController>(string principalName = null)
            where TController : ControllerBase, IApiControllerBase
        {
            var scope = this.Environment.ServiceProvider.CreateScope();

            var controller = scope.ServiceProvider.GetRequiredService<TController>();

            controller.ServiceProvider = scope.ServiceProvider;
            controller.PrincipalName = principalName;

            return controller;
        }
    }

    public static class ServiceProviderControllerExtensions
    {
        public static IServiceCollection RegisterControllers(this IServiceCollection services)
        {
            var asms = new[]
                       {
                               typeof(WorkflowSampleSystem.WebApiCore.Controllers.Main.EmployeeController).Assembly,
                       };

            var exceptControllers = new Type[]
                                    {
                                    };


            foreach (var controllerType in asms.SelectMany(a => a.GetTypes()).Except(exceptControllers).Where(t => !t.IsAbstract && typeof(IApiControllerBase).IsAssignableFrom(t) && typeof(ControllerBase).IsAssignableFrom(t)))
            {
                services.AddScoped(controllerType);

                services.AddSingleton(typeof(ControllerEvaluator<>).MakeGenericType(controllerType));
            }

            return services;
        }
    }
}
