using System;

using Automation.Utils;

using Framework.Core;

using AttachmentsSampleSystem.Domain;
using AttachmentsSampleSystem.Domain.Inline;
using AttachmentsSampleSystem.Generated.DTO;
using AttachmentsSampleSystem.IntegrationTests.__Support.Utils.Framework;

namespace AttachmentsSampleSystem.IntegrationTests.__Support.TestData.Helpers
{
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
                FirstName = StringUtil.RandomString("FirstName", 15),
                LastName = StringUtil.RandomString("LastName", 15)
            };
            var nameTemp = nameEng;
            nameTemp.MiddleName = StringUtil.RandomString("MiddleName", 15);

            nameNative = nameNative ?? nameTemp;
            nameRussian = nameRussian ?? nameTemp;

            email = email ?? $"{nameEng.FirstName}{DefaultConstants.FAKE_MAIL}";

            if (login == null)
            {
                login = $"{ConfigUtil.ComputerName}\\{nameEng.FirstName}";
            }
            else if (login.Equals(DefaultConstants.EMPLOYEE_MY_LOGIN))
            {
                login = this.AuthHelper.GetCurrentUserLogin();
            }

            var hrDepartmentId = hrDepartment != null
                ? ((HRDepartmentIdentityDTO)hrDepartment).Id
                : location == null
                ? DefaultConstants.HRDEPARTMENT_PARENT_ID
                : this.SaveHRDepartment(location: location).Id;

            birthDate = birthDate ?? new DateTime(1990, 2, 15);

            var rnd = new Random();
            pin = pin ?? rnd.Next(100000);

            return this.EvaluateWrite(
                context =>
                {
                    if (!saveEmployeeWithNullHireDate)
                    {
                        hireDate = hireDate ?? context.DateTimeService.CurrentMonth.StartDate;
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
        public EmployeeIdentityDTO GetEmployeeByLogin(string login)
        {
            return this.EvaluateRead(context =>
            {
                return context.Logics.Employee.GetObjectBy(e => e.Login == login).ToIdentityDTO();
            });
        }

        public EmployeeSimpleDTO GetCurrentEmployee()
        {
            return this.EvaluateRead(context =>
                context.Logics.Employee.GetObjectBy(e => e.Login == context.Authorization.CurrentPrincipalName).ToSimpleDTO(new AttachmentsSampleSystemServerPrimitiveDTOMappingService(context)));
        }
    }
}
