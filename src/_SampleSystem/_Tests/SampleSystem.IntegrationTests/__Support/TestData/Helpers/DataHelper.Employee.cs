using Automation.ServiceEnvironment;
using Automation.Utils;

using Framework.Core;

using Microsoft.Extensions.DependencyInjection;

using SampleSystem.Domain;
using SampleSystem.Domain.Inline;
using SampleSystem.Generated.DTO;

namespace SampleSystem.IntegrationTests.__Support.TestData.Helpers;

public partial class DataHelper
{
    public EmployeeIdentityDTO SaveEmployee(
            Guid? id = null,
            string login = null,
            Fio nameEng = null,
            Fio nameNative = null,
            Fio nameRussian = null,
            BusinessUnitIdentityDTO? coreBusinessUnit = null,
            HRDepartmentIdentityDTO? hrDepartment = null,
            ManagementUnitIdentityDTO? managementUnit = null,
            EmployeePositionIdentityDTO? position = null,
            EmployeeRegistrationTypeIdentityDTO? registrationType = null,
            EmployeeRoleIdentityDTO? role = null,
            EmployeeRoleDegreeIdentityDTO? roleDegree = null,
            string email = null,
            int? pin = null,
            DateTime? birthDate = null,
            DateTime? hireDate = null,
            DateTime? dismissDate = null,
            long externalId = 1,
            bool active = true,
            bool isObjectRequired = true,
            bool saveEmployeeWithNullHireDate = false,
            Period? workPeriod = null,
            DateTime? nonValidateVirtualProp = null,
            DateTime? validateVirtualProp = null,
            string personalCellPhone = null,
            Period? educationDuration = null,
            string cellPhone = "3365",
            LocationIdentityDTO? location = null,
            int age = 0)
    {
        Employee employee;

        nameEng = nameEng ?? new Fio
                             {
                                     FirstName = TextRandomizer.RandomString("FirstName", 15),
                                     LastName = TextRandomizer.RandomString("LastName", 15)
                             };
        var nameTemp = nameEng;
        nameTemp.MiddleName = TextRandomizer.RandomString("MiddleName", 15);

        nameNative = nameNative ?? nameTemp;
        nameRussian = nameRussian ?? nameTemp;

        email = email ?? $"{nameEng.FirstName}{DefaultConstants.FAKE_MAIL}";

        if (login == null)
        {
            login = $"{Environment.MachineName}\\{nameEng.FirstName}";
        }

        var hrDepartmentId = hrDepartment != null
                                     ? ((HRDepartmentIdentityDTO)hrDepartment).Id
                                     : location == null
                                             ? DefaultConstants.HRDEPARTMENT_PARENT_ID
                                             : this.SaveHRDepartment(location: location).Id;

        var positionId = position != null
                                 ? ((EmployeePositionIdentityDTO)position).Id
                                 : DefaultConstants.EMPLOYEE_POSITION_TESTER_ID;

        var registrationId = registrationType != null
                                     ? ((EmployeeRegistrationTypeIdentityDTO)registrationType).Id
                                     : DefaultConstants.EMPLOYEE_REGISTRATION_TYPE_STAFF_ID;

        var roleId = role != null
                             ? ((EmployeeRoleIdentityDTO)role).Id
                             : DefaultConstants.EMPLOYEE_ROLE_TESTER_ID;

        var roleDegreeId = roleDegree != null
                                   ? ((EmployeeRoleDegreeIdentityDTO)roleDegree).Id
                                   : DefaultConstants.EMPLOYEE_ROLE_DEGREE_REGULAR_ID;

        birthDate = birthDate ?? new DateTime(1990, 2, 15);

        var rnd = new Random(Guid.NewGuid().GetHashCode());

        pin ??= 0.RangeInfinity()
                 .Select(_ => rnd.Next(10000))
                 .First(rndPin => !this.EvaluateRead(c => c.Logics.Employee.GetUnsecureQueryable().Any(e => e.Pin == rndPin)));

        return this.EvaluateWrite(
                                  context =>
                                  {
                                      if (!saveEmployeeWithNullHireDate)
                                      {
                                          hireDate = hireDate ?? this.TimeProvider.GetCurrentMonth().StartDate;
                                      }

                                      employee = new Employee
                                                 {
                                                         Active = active,
                                                         ExternalId = externalId,
                                                         BirthDate = birthDate,
                                                         HRDepartment = context.Logics.HRDepartment.GetById(hrDepartmentId, isObjectRequired),
                                                         DismissDate = dismissDate,
                                                         Email = email,
                                                         HireDate = hireDate,
                                                         PlannedHireDate = hireDate,
                                                         Login = login,
                                                         NameEng = nameEng,
                                                         NameNative = nameNative,
                                                         NameRussian = nameRussian,
                                                         Pin = pin,
                                                         ManagementUnit = null, //context.Logics.ManagementUnit.GetById(managementUnitId, isObjectRequired),
                                                         Position = context.Logics.EmployeePosition.GetById(positionId),
                                                         RegistrationType = context.Logics.EmployeeRegistrationType.GetById(registrationId),
                                                         Role = context.Logics.EmployeeRole.GetById(roleId),
                                                         RoleDegree = context.Logics.EmployeeRoleDegree.GetById(roleDegreeId),
                                                         CellPhone = cellPhone,
                                                         Interphone = "3365",
                                                         Landlinephone = "3365",
                                                         PersonalCellPhone = personalCellPhone,
                                                         WorkPeriod = workPeriod ?? new Period(DateTime.Now.Date.AddMonths(-1), DateTime.Now.Date),
                                                         EducationDuration = educationDuration ?? new Period(DateTime.Now.Date.AddYears(-5), DateTime.Now.Date.AddYears(-2)),
                                                         NonValidateVirtualProp = nonValidateVirtualProp ?? DateTime.Now,
                                                         ValidateVirtualProp = validateVirtualProp ?? DateTime.Now,
                                                         Age = age
                                      };

                                      if (coreBusinessUnit != null)
                                      {
                                          employee.CoreBusinessUnit = context.Logics.BusinessUnit.GetById(
                                           coreBusinessUnit.GetValueOrDefault().Id,
                                           isObjectRequired);
                                      }

                                      context.Logics.Employee.Insert(employee, this.GetGuid(id));
                                      return employee.ToIdentityDTO();
                                  });
    }

    public EmployeePositionIdentityDTO SaveEmployeePosition(
            Guid? id = null,
            string name = null,
            string englishName = null,
            LocationIdentityDTO? location = null,
            long externalId = 1,
            bool active = true)
    {
        EmployeePosition position;
        name = name ?? TextRandomizer.UniqueString("Position");
        englishName = englishName ?? name;

        var locationId = location != null ? ((LocationIdentityDTO)location).Id : DefaultConstants.LOCATION_PARENT_ID;

        return this.EvaluateWrite(
                                  context =>
                                  {
                                      position = context.Logics.EmployeePosition.GetById(this.GetGuid(id));

                                      if (position == null)
                                      {
                                          position = new EmployeePosition
                                                     {
                                                             Active = active,
                                                             Name = name,
                                                             EnglishName = englishName,
                                                             ExternalId = externalId,
                                                             Location = context.Logics.Location.GetById(locationId)
                                                     };

                                          context.Logics.EmployeePosition.Insert(position, this.GetGuid(id));
                                      }

                                      return position.ToIdentityDTO();
                                  });
    }

    public EmployeeRegistrationTypeIdentityDTO SaveEmployeeRegistrationType(
            Guid? id = null,
            string name = null,
            long externalId = 1,
            bool active = true)
    {
        EmployeeRegistrationType type;
        name = name ?? TextRandomizer.UniqueString("Type");

        return this.EvaluateWrite(
                                  context =>
                                  {
                                      type = context.Logics.EmployeeRegistrationType.GetById(this.GetGuid(id));

                                      if (type == null)
                                      {
                                          type = new EmployeeRegistrationType
                                                 {
                                                         Active = active,
                                                         Name = name,
                                                         ExternalId = externalId
                                                 };

                                          context.Logics.EmployeeRegistrationType.Insert(type, this.GetGuid(id));
                                      }

                                      return type.ToIdentityDTO();
                                  });
    }

    public EmployeeRoleIdentityDTO SaveEmployeeRole(
            Guid? id = null,
            string name = null,
            bool active = true)
    {
        EmployeeRole role;
        name = name ?? TextRandomizer.UniqueString("Role");

        return this.EvaluateWrite(
                                  context =>
                                  {
                                      role = context.Logics.EmployeeRole.GetById(this.GetGuid(id));

                                      if (role == null)
                                      {
                                          role = new EmployeeRole { Active = active, Name = name };

                                          context.Logics.EmployeeRole.Insert(role, this.GetGuid(id));
                                      }

                                      return role.ToIdentityDTO();
                                  });
    }

    public EmployeeRoleDegreeIdentityDTO SaveEmployeeRoleDegree(
            Guid? id = null,
            string name = null,
            bool active = true)
    {
        EmployeeRoleDegree roleDegree;
        name = name ?? TextRandomizer.UniqueString("RoleDegree");

        return this.EvaluateWrite(
                                  context =>
                                  {
                                      roleDegree = context.Logics.EmployeeRoleDegree.GetById(this.GetGuid(id));

                                      if (roleDegree == null)
                                      {
                                          roleDegree = new EmployeeRoleDegree { Active = active, Name = name };

                                          context.Logics.EmployeeRoleDegree.Insert(roleDegree, this.GetGuid(id));
                                      }

                                      return roleDegree.ToIdentityDTO();
                                  });
    }


    public EmployeeSimpleDTO GetCurrentEmployee()
    {
        return this.EvaluateRead(
            context =>
                context.Logics.Employee.GetObjectBy(e => e.Login == context.Authorization.CurrentPrincipalName)
                       .ToSimpleDTO(context.ServiceProvider.GetRequiredService<ISampleSystemDTOMappingService>()));
    }

    public EmployeeSpecializationIdentityDTO SaveSpecialization(Guid? id = null, string name = null)
    {
        return this.EvaluateWrite(
                                  context =>
                                  {
                                      var specialization = new EmployeeSpecialization
                                                           {
                                                                   Id = id ?? Guid.NewGuid(),
                                                                   Name = name ?? TextRandomizer.UniqueString("EmployeeSpecialization")
                                                           };

                                      context.Logics.EmployeeSpecialization.Insert(specialization, specialization.Id);

                                      return specialization.ToIdentityDTO();
                                  });
    }

    public void SaveRoleRoleDegreeLink(EmployeeRoleIdentityDTO employeeRoleIdentity, EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdentity, Guid? id = null)
    {
        this.EvaluateWrite(
                           context =>
                           {
                               var employeeRole = context.Logics.EmployeeRole.GetById(employeeRoleIdentity.Id, true);
                               var employeeRoleDegree = context.Logics.EmployeeRoleDegree.GetById(employeeRoleDegreeIdentity.Id, true);
                               var result = new RoleRoleDegreeLink
                                            {
                                                    Id = id ?? Guid.NewGuid(),
                                                    Role = employeeRole,
                                                    RoleDegree = employeeRoleDegree
                                            };

                               context.Logics.RoleRoleDegreeLink.Insert(result, result.Id);
                           });
    }
}
