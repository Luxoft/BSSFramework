using CommonFramework;

using Framework.Core;
using Framework.Database.Domain;

using SampleSystem.Domain.Employee;
using SampleSystem.Domain.Inline;
using SampleSystem.Generated.DTO;
using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.WebApiCore.Controllers.Audit;
using SampleSystem.WebApiCore.Controllers.Main;

using BusinessUnitController = SampleSystem.WebApiCore.Controllers.Audit.BusinessUnitController;

namespace SampleSystem.IntegrationTests;

public class AuditTests : TestBase
{
    [Fact]
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

        Assert.Equal(testCount + 1, actualRevesionCount.RevisionInfos.Count());
    }

    [Fact]
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
        Assert.Equal($"{expected}", lastEmployeeState.NameEng.FirstName);
    }

    [Fact]
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

        Assert.Single(propertyRevisions.RevisionInfos);
    }

    [Fact]
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
        Assert.Equal($"{expected}{emailTail}", checkPropertyRevision?.Value);
    }

    [Fact]
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
        Assert.Equal(AuditRevisionType.Added, firstRevision.RevisionType);
    }

    [Fact]
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

        Assert.Single(afterFirstRevisions);
        Assert.Equal(AuditRevisionType.Modified, afterFirstRevisions.First());
    }


    [Fact]
    public void CrateNewBu_AuditBuLoadedFromCustomMapping()
    {
        // Arrange
        var testUser = nameof(this.CrateNewBu_AuditBuLoadedFromCustomMapping);
        this.AuthManager.For(testUser).LoginAs();

        // Act
        var newBu = this.DataHelper.SaveBusinessUnit();

        // Assert

        var newBuRevInfo = this.GetControllerEvaluator<BusinessUnitController>(testUser)
                               .Evaluate(c => c.GetBusinessUnitRevisions(newBu))
                               .RevisionInfos
                               .Single();

        var auditBu = this.GetControllerEvaluator<BusinessUnitAuditController>(testUser)
                          .Evaluate(c => c.LoadFromCustomAuditMapping(newBu, newBuRevInfo.RevisionNumber));


        Assert.Equal(newBuRevInfo.RevisionNumber, auditBu.Revision);
        Assert.Equal(testUser, auditBu.Author);
        Assert.Equal(testUser, newBuRevInfo.Author);

        Assert.Equal(newBu, auditBu.BuIdent);
    }
}
