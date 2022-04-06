using System;
using System.Diagnostics;
using System.Linq;

using FluentAssertions;

using Framework.Core;
using Framework.DomainDriven.DAL.Revisions;

using Microsoft.SqlServer.Management.Smo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.Domain;
using SampleSystem.Domain.Inline;
using SampleSystem.Generated.DTO;
using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.WebApiCore.Controllers.Audit;

using EmployeeController = SampleSystem.WebApiCore.Controllers.Main.EmployeeController;

namespace SampleSystem.IntegrationTests
{
    [TestClass]
    public class AuditTests : TestBase
    {
        [TestMethod]
        public void GetObjectRevisions_CheckCount_Correct()
        {
            // Act
            var employeeController = this.MainWebApi.Employee;
            var employeeAuditController = this.MainAuditWebApi.Employee;
            var testCount = 10;

            var employeeStrictDto = new EmployeeStrictDTO
            {
                NameEng = new Fio() { FirstName = "firstName", LastName = "lastName" },
                WorkPeriod = new Period(DateTime.Now.AddDays(-1), DateTime.Now),
                ValidateVirtualProp = DateTime.Now,
                EducationDuration = new Period(DateTime.Now.AddYears(-5), DateTime.Now.AddYears(-2)),
                ExternalId = 1
            };

            var employeeIdentity = employeeController.Evaluate(c => c.SaveEmployee(employeeStrictDto));

            for (int q = 0; q < testCount; q++)
            {
                var employeeFull = employeeController.Evaluate(c => c.GetFullEmployee(employeeIdentity));
                var employeeStrict = employeeFull.ToStrict();
                employeeStrict.NameEng = new FioShort() { FirstName = $"{q}" };
                employeeController.Evaluate(c => c.SaveEmployee(employeeStrict));
            }

            // Assert
            var actualRevesionCount = employeeAuditController.Evaluate(c => c.GetEmployeeRevisions(employeeIdentity));

            actualRevesionCount.RevisionInfos.Count().Should().Be(testCount + 1);
        }

        [TestMethod]
        public void GetObjectByRevision_CheckState_Correct()
        {
            // Act
            var employeeController = this.MainWebApi.Employee;
            var employeeAuditController = this.MainAuditWebApi.Employee;
            var testCount = 10;

            var employeeStrictDto = new EmployeeStrictDTO
            {
                NameEng = new Fio() { FirstName = "firstName", LastName = "lastName" },
                WorkPeriod = new Period(DateTime.Now.AddDays(-1), DateTime.Now),
                ValidateVirtualProp = DateTime.Now,
                EducationDuration = new Period(DateTime.Now.AddYears(-5), DateTime.Now.AddYears(-2)),
                ExternalId = 1
            };

            var employeeIdentity = employeeController.SaveEmployee(employeeStrictDto);

            for (int q = 0; q < testCount; q++)
            {
                var employeeFull = employeeController.GetFullEmployee(employeeIdentity);
                var employeeStrict = employeeFull.ToStrict();
                employeeStrict.NameEng = new FioShort() { FirstName = $"{q}" };
                employeeController.SaveEmployee(employeeStrict);
            }

            // Assert
            var skip = 3;

            var lastRevision = employeeAuditController.GetEmployeeRevisions(employeeIdentity)
                                                      .RevisionInfos
                                                      .EmptyIfNull()
                                                      .OrderBy(z => z.RevisionNumber)
                                                      .Skip(skip) ////
                                                      .FirstOrDefault();

            var lastEmployeeState = employeeAuditController.GetFullEmployeeWithRevision( new GetFullEmployeeWithRevisionAutoRequest
                {
                    employeeIdentity = employeeIdentity,
                    revision = lastRevision.RevisionNumber
            });

            var expected = Enumerable.Range(-1, testCount).Skip(skip).First();
            lastEmployeeState.NameEng.FirstName.Should().Be($"{expected}");
        }

        [TestMethod]
        public void GetObjectPropertyRevisions_CallNotChangeProperty_RevisionsIsOne()
        {
            // Act
            var employeeController = this.MainWebApi.Employee;
            var employeeAuditController = this.MainAuditWebApi.Employee;
            var employeeStrictDto = new EmployeeStrictDTO
            {
                NameEng = new Fio() { FirstName = "firstName", LastName = "lastName" },
                WorkPeriod = new Period(DateTime.Now.AddDays(-1), DateTime.Now),
                ValidateVirtualProp = DateTime.Now,
                EducationDuration = new Period(DateTime.Now.AddYears(-5), DateTime.Now.AddYears(-2)),
                ExternalId = 1
            };

            var employeeIdentity = employeeController.SaveEmployee(employeeStrictDto);

            // Assert
            var propertyRevisions = employeeAuditController.GetEmployeePropertyRevisions(new GetEmployeePropertyRevisionsAutoRequest
                {
                    employeeIdentity = employeeIdentity,
                    propertyName = $"{nameof(Employee.CoreBusinessUnit)}"
                });

            propertyRevisions.RevisionInfos.Count().Should().Be(1);
        }

        [TestMethod]
        public void GetObjectPropertyRevisions_ChangePrimitiveProperty_CorrectRevisions()
        {
            // Act
            var employeeController = this.MainWebApi.Employee;
            var employeeAuditController = this.GetController<SampleSystem.WebApiCore.Controllers.Audit.EmployeeController>();
            var testCount = 10;
            var emailTail = "@email.email";

            var employeeStrictDto = new EmployeeStrictDTO
            {
                NameEng = new Fio() { FirstName = "firstName", LastName = "lastName" },
                WorkPeriod = new Period(DateTime.Now.AddDays(-1), DateTime.Now),
                ValidateVirtualProp = DateTime.Now,
                EducationDuration = new Period(DateTime.Now.AddYears(-5), DateTime.Now.AddYears(-2)),
                ExternalId = 1
            };

            var employeeIdentity = employeeController.SaveEmployee(employeeStrictDto);

            for (int q = 0; q < testCount; q++)
            {
                var employeeFull = employeeController.GetFullEmployee(employeeIdentity);
                var employeeStrict = employeeFull.ToStrict();
                employeeStrict.Email = $"{q}{emailTail}";
                employeeController.SaveEmployee(employeeStrict);
            }

            // Assert
            var skip = 3;
            var checkPropertyRevision = employeeAuditController.GetEmployeePropertyRevisions(new GetEmployeePropertyRevisionsAutoRequest
                                                                   {
                                                                       employeeIdentity = employeeIdentity,
                                                                       propertyName = $"{nameof(Employee.Email)}"
                                                                   })
                                                               .RevisionInfos
                                                               .EmptyIfNull()
                                                               .OrderBy(z => z.RevisionNumber)
                                                               .Skip(skip) ////
                                                               .FirstOrDefault() as SampleSystemPropertyRevisionDTO<string>;

            var expected = Enumerable.Range(-1, testCount).Skip(skip).First();
            checkPropertyRevision?.Value.Should().Be($"{expected}{emailTail}");
        }

        [TestMethod]
        public void GetObjectPropertyRevisions_CheckFirstRevisioins_HasAddedState()
        {
            // Act
            var employeeController = this.MainWebApi.Employee;
            var employeeAuditController = this.GetController<SampleSystem.WebApiCore.Controllers.Audit.EmployeeController>();
            var testCount = 10;
            var emailTail = "@email.email";

            var employeeStrictDto = new EmployeeStrictDTO
            {
                NameEng = new Fio() { FirstName = "firstName", LastName = "lastName" },
                WorkPeriod = new Period(DateTime.Now.AddDays(-1), DateTime.Now),
                ValidateVirtualProp = DateTime.Now,
                EducationDuration = new Period(DateTime.Now.AddYears(-5), DateTime.Now.AddYears(-2)),
                ExternalId = 1
            };

            var employeeIdentity = employeeController.SaveEmployee(employeeStrictDto);

            for (int q = 0; q < testCount; q++)
            {
                var employeeFull = employeeController.GetFullEmployee(employeeIdentity);
                var employeeStrict = employeeFull.ToStrict();
                employeeStrict.Email = $"{q}{emailTail}";
                employeeController.SaveEmployee(employeeStrict);
            }

            // Assert
            var skip = 3;

            var firstRevision = employeeAuditController.GetEmployeePropertyRevisions(new GetEmployeePropertyRevisionsAutoRequest
                                                           {
                                                               employeeIdentity = employeeIdentity,
                                                               propertyName = $"{nameof(Employee.Email)}"
                                                           })
                                                       .RevisionInfos
                                                       .EmptyIfNull()
                                                       .OrderBy(z => z.RevisionNumber)
                                                       .FirstOrDefault() as SampleSystemPropertyRevisionDTO<string>;

            var expected = Enumerable.Range(-1, testCount).Skip(skip).First();
            firstRevision.RevisionType.Should().Be(AuditRevisionType.Added);
        }

        [TestMethod]
        public void GetObjectPropertyRevisions_CheckAfterFirstRevisioins_AllModifiedState()
        {
            // Act
            var employeeController = this.MainWebApi.Employee;
            var employeeAuditController = this.GetController<SampleSystem.WebApiCore.Controllers.Audit.EmployeeController>();
            var testCount = 10;
            var emailTail = "@email.email";

            var employeeStrictDto = new EmployeeStrictDTO
            {
                NameEng = new Fio() { FirstName = "firstName", LastName = "lastName" },
                WorkPeriod = new Period(DateTime.Now.AddDays(-1), DateTime.Now),
                ValidateVirtualProp = DateTime.Now,
                EducationDuration = new Period(DateTime.Now.AddYears(-5), DateTime.Now.AddYears(-2)),
                ExternalId = 1
            };

            var employeeIdentity = employeeController.SaveEmployee(employeeStrictDto);

            for (int q = 0; q < testCount; q++)
            {
                var employeeFull = employeeController.GetFullEmployee(employeeIdentity);
                var employeeStrict = employeeFull.ToStrict();
                employeeStrict.Email = $"{q}{emailTail}";
                employeeController.SaveEmployee(employeeStrict);
            }

            // Assert
            var afterFirstRevisions = employeeAuditController.GetEmployeePropertyRevisions(new GetEmployeePropertyRevisionsAutoRequest
                                                                 {
                                                                     employeeIdentity = employeeIdentity,
                                                                     propertyName = $"{nameof(Employee.Email)}"
                                                                 })
                                                             .RevisionInfos
                                                             .EmptyIfNull()
                                                             .OrderBy(z => z.RevisionNumber)
                                                             .Skip(1)
                                                             .Select(z => z.RevisionType)
                                                             .Distinct()
                                                             .ToList();

            afterFirstRevisions.Count.Should().Be(1);
            afterFirstRevisions.First().Should().Be(AuditRevisionType.Modified);
        }
    }
}
