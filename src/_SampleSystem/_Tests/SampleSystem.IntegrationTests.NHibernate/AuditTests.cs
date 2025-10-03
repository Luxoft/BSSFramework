using CommonFramework;

using Framework.Core;
using Framework.DomainDriven.DAL.Revisions;

using SampleSystem.Domain;
using SampleSystem.Domain.Inline;
using SampleSystem.Generated.DTO;
using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.WebApiCore.Controllers.Audit;
using SampleSystem.WebApiCore.Controllers.Main;

using BusinessUnitController = SampleSystem.WebApiCore.Controllers.Audit.BusinessUnitController;

namespace SampleSystem.IntegrationTests;

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

        var employeeIdentity = employeeController.Evaluate(c => c.SaveEmployee(employeeStrictDto));

        for (int q = 0; q < testCount; q++)
        {
            var employeeFull = employeeController.Evaluate(c => c.GetFullEmployee(employeeIdentity));
            var employeeStrict = employeeFull.ToStrict();
            employeeStrict.NameEng = new FioShort() { FirstName = $"{q}" };
            employeeController.Evaluate(c => c.SaveEmployee(employeeStrict));
        }

        // Assert
        var skip = 3;

        var lastRevision = employeeAuditController.Evaluate(c => c.GetEmployeeRevisions(employeeIdentity))
                                                  .RevisionInfos
                                                  .EmptyIfNull()
                                                  .OrderBy(z => z.RevisionNumber)
                                                  .Skip(skip) ////
                                                  .FirstOrDefault();

        var lastEmployeeState = employeeAuditController.Evaluate(c => c.GetFullEmployeeWithRevision( new GetFullEmployeeWithRevisionAutoRequest
                                                                     {
                                                                             EmployeeIdentity = employeeIdentity,
                                                                             Revision = lastRevision.RevisionNumber
                                                                     }));

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

        var employeeIdentity = employeeController.Evaluate(c => c.SaveEmployee(employeeStrictDto));

        // Assert
        var propertyRevisions = employeeAuditController.Evaluate(c => c.GetEmployeePropertyRevisions(new GetEmployeePropertyRevisionsAutoRequest
                                                                     {
                                                                             EmployeeIdentity = employeeIdentity,
                                                                             PropertyName = $"{nameof(Employee.CoreBusinessUnit)}"
                                                                     }));

        propertyRevisions.RevisionInfos.Count().Should().Be(1);
    }

    [TestMethod]
    public void GetObjectPropertyRevisions_ChangePrimitiveProperty_CorrectRevisions()
    {
        // Act
        var employeeController = this.MainWebApi.Employee;
        var employeeAuditController = this.GetControllerEvaluator<WebApiCore.Controllers.Audit.EmployeeController>();
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

        var employeeIdentity = employeeController.Evaluate(c => c.SaveEmployee(employeeStrictDto));

        for (int q = 0; q < testCount; q++)
        {
            var employeeFull = employeeController.Evaluate(c => c.GetFullEmployee(employeeIdentity));
            var employeeStrict = employeeFull.ToStrict();
            employeeStrict.Email = $"{q}{emailTail}";
            employeeController.Evaluate(c => c.SaveEmployee(employeeStrict));
        }

        // Assert
        var skip = 3;
        var checkPropertyRevision = employeeAuditController.Evaluate(c => c.GetEmployeePropertyRevisions(new GetEmployeePropertyRevisionsAutoRequest
                                                                         {
                                                                                 EmployeeIdentity = employeeIdentity,
                                                                                 PropertyName = $"{nameof(Employee.Email)}"
                                                                         }))
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
        var employeeAuditController = this.GetControllerEvaluator<WebApiCore.Controllers.Audit.EmployeeController>();
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

        var employeeIdentity = employeeController.Evaluate(c => c.SaveEmployee(employeeStrictDto));

        for (int q = 0; q < testCount; q++)
        {
            var employeeFull = employeeController.Evaluate(c => c.GetFullEmployee(employeeIdentity));
            var employeeStrict = employeeFull.ToStrict();
            employeeStrict.Email = $"{q}{emailTail}";
            employeeController.Evaluate(c => c.SaveEmployee(employeeStrict));
        }

        // Assert
        var skip = 3;

        var firstRevision = employeeAuditController.Evaluate(c => c.GetEmployeePropertyRevisions(new GetEmployeePropertyRevisionsAutoRequest
                                                                 {
                                                                         EmployeeIdentity = employeeIdentity,
                                                                         PropertyName = $"{nameof(Employee.Email)}"
                                                                 }))
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
        var employeeAuditController = this.GetControllerEvaluator<WebApiCore.Controllers.Audit.EmployeeController>();
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

        var employeeIdentity = employeeController.Evaluate(c => c.SaveEmployee(employeeStrictDto));

        for (int q = 0; q < testCount; q++)
        {
            var employeeFull = employeeController.Evaluate(c => c.GetFullEmployee(employeeIdentity));
            var employeeStrict = employeeFull.ToStrict();
            employeeStrict.Email = $"{q}{emailTail}";
            employeeController.Evaluate(c => c.SaveEmployee(employeeStrict));
        }

        // Assert
        var afterFirstRevisions = employeeAuditController.Evaluate(c => c.GetEmployeePropertyRevisions(new GetEmployeePropertyRevisionsAutoRequest
                                                                       {
                                                                               EmployeeIdentity = employeeIdentity,
                                                                               PropertyName = $"{nameof(Employee.Email)}"
                                                                       }))
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


    [TestMethod]
    public void CrateNewBu_AuditBuLoadedFromCustomMapping()
    {
        // Arrange
        var newBu = this.DataHelper.SaveBusinessUnit();
        var newBuRevInfo = this.GetControllerEvaluator<BusinessUnitController>()
                           .Evaluate(c => c.GetBusinessUnitRevisions(newBu))
                           .RevisionInfos
                           .Single();

        // Act
        var auditBu = this.GetControllerEvaluator<BusinessUnitAuditController>()
                                .Evaluate(c => c.LoadFromCustomAuditMapping(newBu, newBuRevInfo.RevisionNumber));

        // Assert
        auditBu.Revision.Should().Be(newBuRevInfo.RevisionNumber);
        auditBu.Author.Should().Be(newBuRevInfo.Author);
        auditBu.BuIdent.Should().Be(newBu);
    }
}
