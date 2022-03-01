using System;
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
using WorkflowSampleSystem.IntegrationTests.__Support.Utils.Framework;
using WorkflowSampleSystem.WebApiCore.Controllers.Main;
using WorkflowSampleSystem.WebApiCore.Controllers.Report;

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

        public CountryIdentityDTO SaveCountry(
            Guid? id = null,
            string name = null,
            string nativeName = null,
            string code = null,
            string culture = null,
            bool active = true)
        {
            name = name ?? StringUtil.UniqueString("Country");
            nativeName = nativeName ?? StringUtil.UniqueString("Country");
            code = code ?? StringUtil.UniqueString("Code");
            culture = culture ?? StringUtil.UniqueString("Culture");

            return this.EvaluateWrite(
                context =>
                {
                    var countryBLL = context.Logics.Country;
                    var country = countryBLL.GetById(this.GetGuid(id));

                    if (country == null)
                    {
                        country = new Country
                        {
                            Active = active,
                            Code = code,
                            Name = name,
                            NameNative = nativeName,
                            Culture = culture,
                        };

                        countryBLL.Insert(country, this.GetGuid(id));
                    }

                    return country.ToIdentityDTO();
                });
        }

        public LocationIdentityDTO SaveLocation(
            Guid? id = null,
            string name = null,
            LocationIdentityDTO? parent = null,
            CountryIdentityDTO? country = null,
            bool isFinancial = true,
            LocationType locationType = LocationType.Country,
            int code = 1,
            bool active = true,
            int closeDate = 20)
        {
            name = name ?? StringUtil.UniqueString("Location");
            var parentId = parent != null ? ((LocationIdentityDTO)parent).Id : DefaultConstants.LOCATION_PARENT_ID;
            var countryId = country != null ? ((CountryIdentityDTO)country).Id : DefaultConstants.COUNTRY_RUSSIA_ID;

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
                            IsFinancial = isFinancial,
                            LocationType = locationType,
                            Parent = context.Logics.Location.GetById(parentId),
                            Country = context.Logics.Country.GetById(countryId),
                            CloseDate = closeDate,
                            Code = code
                        };

                        context.Logics.Location.Insert(location, this.GetGuid(id));
                    }

                    return location.ToIdentityDTO();
                });
        }

        public BusinessUnitTypeIdentityDTO SaveBusinessUnitType(
            Guid? id = null,
            string name = null,
            bool startConfirm = false,
            bool transferConfirm = false,
            bool billProjectAreNotAllowed = false,
            bool canBeIsCommission = false,
            bool canBeLinkedToDepartment = true,
            bool canBeNewBusiness = true,
            bool canBeResourcePool = true,
            bool isAdministrative = true,
            bool needVertical = true,
            PossibleStartDate possibleStartDate = 0,
            PossibleStartDate possibleTransferDate = 0,
            bool projectStartAllowed = true,
            bool startBOConfirm = true,
            bool transferBOConfirm = true,
            List<BusinessUnitTypeIdentityDTO> possibleParents = null,
            List<FinancialProjectType> possibleFinancialProjectTypes = null,
            List<BusinessUnitTypeIdentityDTO> transferTo = null,
            bool currentTypeInPossibleParents = false,
            bool currentTypeInTransferTo = false,
            bool active = true,
            bool canBeLinkedToClient = false)
        {
            name = name ?? StringUtil.UniqueString("Type");

            BusinessUnitType type = null;

            this.EvaluateWrite(
                context =>
                {
                    type = (context.Logics.BusinessUnitType.GetById(this.GetGuid(id)) ??
                            context.Logics.BusinessUnitType.GetByName(name)) ?? new BusinessUnitType();

                    type.Active = active;
                    type.Name = name;
                    type.AdditionalStartConfirm = startConfirm;
                    type.AdditionalTransferConfirm = transferConfirm;
                    type.BillingProjectAreNotAllowed = billProjectAreNotAllowed;
                    type.CanBeIsSpecialCommission = canBeIsCommission;
                    type.CanBeLinkedToDepartment = canBeLinkedToDepartment;
                    type.CanBeNewBusiness = canBeNewBusiness;
                    type.CanBeResourcePool = canBeResourcePool;
                    type.IsAdministrative = isAdministrative;
                    type.NeedVertical = needVertical;
                    type.PossibleStartDate = possibleStartDate;
                    type.PossibleTransferDate = possibleTransferDate;
                    type.ProjectStartAllowed = projectStartAllowed;
                    type.StartBOConfirm = startBOConfirm;
                    type.TransferBOConfirm = transferBOConfirm;
                    type.CanBeLinkedToClient = canBeLinkedToClient;

                    if (type.IsNew)
                    {
                        context.Logics.BusinessUnitType.Insert(type, this.GetGuid(id));
                    }
                    else
                    {
                        context.Logics.BusinessUnitType.Save(type);
                    }
                });

            if (possibleParents == null && transferTo == null && currentTypeInPossibleParents == false &&
                currentTypeInTransferTo == false)
            {
                return type.ToIdentityDTO();
            }

            var businessUnitTypeController = this.GetController<BusinessUnitTypeController>();
            var buTypeStrict = new BusinessUnitTypeStrictDTO(businessUnitTypeController.GetFullBusinessUnitType(type.ToIdentityDTO()));

            possibleParents = possibleParents ?? new List<BusinessUnitTypeIdentityDTO>();
            possibleParents.Add(type.ToIdentityDTO());

            var possibleParentsList =
                new List<BusinessUnitTypeLinkWithPossibleParentStrictDTO>
                {
                    new BusinessUnitTypeLinkWithPossibleParentStrictDTO
                    {
                        BusinessUnitType = type.ToIdentityDTO(), PossibleParent = type.ToIdentityDTO()
                    }
                };

            possibleParentsList.AddRange(
                possibleParents.Select(
                    possibleParent => new BusinessUnitTypeLinkWithPossibleParentStrictDTO
                    {
                        BusinessUnitType = type.ToIdentityDTO(),
                        PossibleParent = possibleParent
                    }));

            if (!currentTypeInPossibleParents)
            {
                possibleParentsList.RemoveBy(p => p.PossibleParent == type.ToIdentityDTO());
            }

            buTypeStrict.PossibleParents = possibleParentsList;

            if (transferTo != null)
            {
                var transferToList =
                    new List<BusinessUnitTypeLinkWithTransferToStrictDTO>
                    {
                        new BusinessUnitTypeLinkWithTransferToStrictDTO
                        {
                            BusinessUnitType = type.ToIdentityDTO(), TransferTo = type.ToIdentityDTO()
                        }
                    };

                transferToList.AddRange(
                    transferTo.Select(
                        transfer => new BusinessUnitTypeLinkWithTransferToStrictDTO
                        {
                            BusinessUnitType = type.ToIdentityDTO(),
                            TransferTo = transfer
                        }));

                if (!currentTypeInTransferTo)
                {
                    transferToList.RemoveBy(p => p.TransferTo == type.ToIdentityDTO());
                }

                buTypeStrict.TransferTo = transferToList;
            }

            if (possibleFinancialProjectTypes != null)
            {
                buTypeStrict.PossibleFinancialProjectTypes =
                    possibleFinancialProjectTypes.Select(
                        t =>
                            new BusinessUnitTypeLinkWithPossibleFinancialProjectTypeStrictDTO
                            {
                                FinancialProjectType = t,
                                BusinessUnitType = buTypeStrict.Identity
                            })
                        .ToList();
            }


            businessUnitTypeController.SaveBusinessUnitType(buTypeStrict);

            return type.ToIdentityDTO();
        }


        public BusinessUnitIdentityDTO SaveBusinessUnit(
            Guid? id = null,
            string name = null,
            BusinessUnitIdentityDTO? parent = null,
            BusinessUnitTypeIdentityDTO? type = null,
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

            var typeId = type != null
                ? ((BusinessUnitTypeIdentityDTO)type).Id
                : DefaultConstants.BUSINESS_UNIT_TYPE_PROGRAM_ID;

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
                                BusinessUnitForRent = null,
                                IsPool = isPool,
                                IsNewBusiness = isNewBusiness,
                                IsSpecialCommission = isSpecialCommision,
                                Commission = commision,
                                NewBusinessStatusLeft = newBusinessStatusLeft,
                                BusinessUnitStatus = BusinessUnitStatus.Current,
                                Options = BusinessUnitOptions.None,
                                Rank = rank,
                                IsProduction = isProduction,
                                BusinessUnitType = context.Logics.BusinessUnitType.GetById(typeId),
                                Parent = context.Logics.BusinessUnit.GetById(parentId),
                                Period = period.Value
                            };

                        context.Logics.BusinessUnit.Insert(businessUnit, this.GetGuid(id));
                    }

                    return businessUnit.ToIdentityDTO();
                });
        }

        public ManagementUnitIdentityDTO SaveManagementUnit(
            Guid? id = null,
            string name = null,
            ManagementUnitIdentityDTO? parent = null,
            bool parentIsNeeded = true,
            Period? period = null,
            bool isProduction = true,
            bool active = true)
        {
            ManagementUnit unit;
            name = name ?? StringUtil.UniqueString("ManagementUnit");

            var parentId = parent != null
                ? ((ManagementUnitIdentityDTO)parent).Id
                : DefaultConstants.MANAGEMENT_UNIT_PARENT_COMPANY_ID;

            return this.EvaluateWrite(
                context =>
                {
                    unit = context.Logics.ManagementUnit.GetById(this.GetGuid(id));
                    period = period ?? new Period(context.DateTimeService.CurrentFinancialYear.StartDate);
                    if (unit == null)
                    {
                        unit = new ManagementUnit
                        {
                            Active = active,
                            Name = name,
                            Parent = parentIsNeeded ? context.Logics.ManagementUnit.GetById(parentId) : null,
                            Period = (Period)period,
                            IsProduction = isProduction
                        };

                        context.Logics.ManagementUnit.Insert(unit, this.GetGuid(id));
                    }

                    return unit.ToIdentityDTO();
                });
        }


        public HRDepartmentIdentityDTO SaveHRDepartment(
            Guid? id = null,
            string name = null,
            string nameNative = null,
            HRDepartmentIdentityDTO? parent = null,
            EmployeeIdentityDTO? employee = null,
            CompanyLegalEntityIdentityDTO? companyLegalEntity = null,
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

            var companyLegalEntityId = companyLegalEntity != null
                ? ((CompanyLegalEntityIdentityDTO)companyLegalEntity).Id
                : DefaultConstants.COMPANY_LEGAL_ENTITY_ID;

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
                            CompanyLegalEntity = context.Logics.CompanyLegalEntity.GetById(companyLegalEntityId),
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

        public CompanyLegalEntityIdentityDTO SaveCompanyLegalEntity(
            Guid? id = null,
            string name = null,
            CompanyLegalEntityIdentityDTO? parent = null,
            string nameEnglish = null,
            string code = null,
            CompanyLegalEntityType? type = null,
            bool active = true)
        {
            name = name ?? StringUtil.UniqueString("Legal");
            nameEnglish = nameEnglish ?? name;
            code = code ?? name;
            type = type ?? CompanyLegalEntityType.LegalEntity;

            return this.EvaluateWrite(
                context =>
                {
                    var legal = context.Logics.CompanyLegalEntity.GetById(this.GetGuid(id));

                    if (legal == null)
                    {
                        CompanyLegalEntity parentDomainObject = (parent == null)
                            ? null
                            : context.Logics.CompanyLegalEntity.GetById(parent.Value.Id);

                        legal = new CompanyLegalEntity
                        {
                            Active = active,
                            Name = name,
                            NameEnglish = nameEnglish,
                            Code = code,
                            Type = (CompanyLegalEntityType)type,
                            Parent = parentDomainObject
                        };

                        context.Logics.CompanyLegalEntity.Insert(legal, this.GetGuid(id));
                    }

                    return legal.ToIdentityDTO();
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
            foreach (var controllerType in typeof(WorkflowSampleSystemGenericReportController).Assembly.GetTypes().Where(t => typeof(ControllerBase).IsAssignableFrom(t)))
            {
                services.AddScoped(controllerType);
            }

            return services;
        }
    }
}
