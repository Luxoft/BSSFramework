﻿using Automation.ServiceEnvironment;
using Automation.Utils;

using Framework.Configuration.Domain;
using Framework.Configuration.Generated.DTO;
using Framework.Core;
using Framework.DomainDriven.BLL;

using SampleSystem.Domain;
using SampleSystem.Domain.Inline;
using SampleSystem.Generated.DTO;
using SampleSystem.WebApiCore.Controllers.Main;

namespace SampleSystem.IntegrationTests.__Support.TestData.Helpers;

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
        name = name ?? TextRandomizer.UniqueString("Country");
        nativeName = nativeName ?? TextRandomizer.UniqueString("Country");
        code = code ?? TextRandomizer.UniqueString("Code");
        culture = culture ?? TextRandomizer.UniqueString("Culture");

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
        name = name ?? TextRandomizer.UniqueString("Location");
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
        name = name ?? TextRandomizer.UniqueString("Type");

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

        var businessUnitTypeController = this.RootServiceProvider.GetDefaultControllerEvaluator<BusinessUnitTypeController>();
        var buTypeStrict = new BusinessUnitTypeStrictDTO(businessUnitTypeController.Evaluate(c => c.GetFullBusinessUnitType(type.ToIdentityDTO())));

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


        businessUnitTypeController.Evaluate(c => c.SaveBusinessUnitType(buTypeStrict));

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
            bool allowedForFilterRole = false,
            bool active = true)
    {
        name = name ?? TextRandomizer.UniqueString("BusinessUnit");

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
                                      period = period ?? new Period(this.FinancialYearService.GetCurrentFinancialYear().StartDate.AddYears(-1));
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
                                                          Period = period.Value,
                                                          AllowedForFilterRole = allowedForFilterRole
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
        name = name ?? TextRandomizer.UniqueString("ManagementUnit");

        var parentId = parent != null
                               ? ((ManagementUnitIdentityDTO)parent).Id
                               : DefaultConstants.MANAGEMENT_UNIT_PARENT_COMPANY_ID;

        return this.EvaluateWrite(
                                  context =>
                                  {
                                      unit = context.Logics.ManagementUnit.GetById(this.GetGuid(id));
                                      period = period ?? new Period(this.FinancialYearService.GetCurrentFinancialYear().StartDate);
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
        name = name ?? TextRandomizer.UniqueString("Department");
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

        var employeeId = employee?.Id ?? DefaultConstants.HRDepartment_DEFAULT_HEAD_EMPLOYEE_ID;

        return this.EvaluateWrite(
                                  context =>
                                  {
                                      var department = context.Logics.HRDepartment.GetById(this.GetGuid(id));

                                      if (department == null)
                                      {
                                          var head = context.Logics.Employee.GetById(employeeId, true);

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
        name = name ?? TextRandomizer.UniqueString("Legal");
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
}
