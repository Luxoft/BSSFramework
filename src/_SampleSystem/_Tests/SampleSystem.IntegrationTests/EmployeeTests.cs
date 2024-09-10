using System.Data.SqlTypes;

using Automation.Utils.DatabaseUtils;
using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.Domain;
using SampleSystem.IntegrationTests.__Support.TestData;

using Framework.Configuration.Generated.DTO;

using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.DAL.Revisions;
using Framework.DomainDriven.NHibernate;
using Framework.Events;
using Framework.OData;
using Framework.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

using NHibernate.Impl;

using SampleSystem.Generated.DTO;

namespace SampleSystem.IntegrationTests;

[TestClass]
public class EmployeeTests : TestBase
{
    [TestMethod]
    public void GetEmployeeFromDB_FilterByAge_ReturnNotNulRecords()
    {
        /*
         * дефект в старых версих nhibernate.
         * Linq в конструкциях сравнений (например q.Age == 10) дополнительно генерирует включение null полей (например `or employee0_.[age] is null`))
         */

        // Arrange
        this.DataHelper.SaveEmployee(Guid.NewGuid(), age: 10);
        CoreDatabaseUtil.ExecuteSql(
            this.DatabaseContext.Main.ConnectionString,
            "INSERT INTO [app].[Employee] ([id], age) VALUES (NewId(), null)");

        // Act, IntegrationNamespace
        var actual = this.Evaluate(
            DBSessionMode.Read,
            ctx => ctx.Logics.Employee.GetUnsecureQueryable().Where(q => q.Age == 10).ToList());

        // Assert
        actual.Count().Should().Be(1);
        actual.Select(z => z.Age).All(z => z == 10).Should().BeTrue();
    }

    [TestMethod]
    public void AddNewEmployee_CheckEmployeeSaved()
    {
        // Arrange
        var employeeController = this.MainWebApi.Employee;
        var employeeIdentity = this.DataHelper.SaveEmployee(Guid.NewGuid());

        // Act
        var employees = employeeController.Evaluate(c => c.GetSimpleEmployees());

        // Assert
        employees.Should().Contain(e => e.Id == employeeIdentity.Id);
    }

    [TestMethod]
    public void GetEmployeeByOData_ContainsForNumberProperty_OnlyRequestedDataInTheResult()
    {
        // Arrange
        var employeeController = this.MainWebApi.Employee;
        var employeeQueryController = this.GetControllerEvaluator<WebApiCore.Controllers.MainQuery.EmployeeQueryController>();

        foreach (var pin in new[] { 123, 456 })
        {
            var employeeIdentity = this.DataHelper.SaveEmployee(Guid.NewGuid());
            var employee = employeeController.Evaluate(c => c.GetSimpleEmployee(employeeIdentity));
            employee.Pin = pin;
            employeeController.Evaluate(c => c.SaveEmployee(employee.ToStrict()));
        }

        // Act
        var query = "$top=30&$filter=substringof('23',Pin)";
        var result = employeeQueryController.Evaluate(c => c.GetSimpleEmployeesByODataQueryString(query));

        // Assert
        var pins = result.Items.Select(x => x.Pin).ToArray();
        pins.Should().Contain(123);
        pins.Should().NotContain(456);
    }

    [TestMethod]
    public void GetEmployeeByOData_TakeTestWithoutSorting_OnlyRequestedDataInTheResult()
    {
        // Arrange
        var employeeController = this.MainWebApi.Employee;
        var employeeQueryController = this.GetControllerEvaluator<WebApiCore.Controllers.MainQuery.EmployeeQueryController>();

        var idToPinMap = new Dictionary<Guid, int> { { Guid.NewGuid(), 123 }, { Guid.NewGuid(), 456 } };

        /*
         https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql/comparing-guid-and-uniqueidentifier-values
         Both Guid and SqlGuid have a CompareTo method for comparing different GUID values. However, System.Guid.CompareTo and SqlTypes.SqlGuid.CompareTo are implemented differently.
         SqlGuid implements CompareTo using SQL Server behavior, in the last six bytes of a value are most significant. Guid evaluates all 16 bytes.
         */
        var sqlGuids = idToPinMap.Keys.Select(x => new SqlGuid(x.ToString())).ToList();
        sqlGuids.Sort();

        foreach (var item in idToPinMap)
        {
            var employeeIdentity = this.DataHelper.SaveEmployee(item.Key);
            var employee = employeeController.Evaluate(c => c.GetSimpleEmployee(employeeIdentity));
            employee.Pin = item.Value;
            employeeController.Evaluate(c => c.SaveEmployee(employee.ToStrict()));
        }

        // Act
        var result = employeeQueryController.Evaluate(
            c => c.GetSimpleEmployeesByODataQueryString("$top=1&$filter=Pin eq 123 or Pin eq 456"));

        // Assert
        var minId = sqlGuids[0].Value;
        var maxId = sqlGuids[1].Value;

        var firstPin = idToPinMap[minId];
        var secondPin = idToPinMap[maxId];

        var pins = result.Items.Select(x => x.Pin).ToArray();
        pins.Should().Contain(firstPin);
        pins.Should().NotContain(secondPin);
    }

    [TestMethod]
    public void GetEmployeeByOData_TakeAndSkipTestWithoutSorting_OnlyRequestedDataInTheResult()
    {
        // Arrange
        var employeeController = this.MainWebApi.Employee;
        var employeeQueryController = this.GetControllerEvaluator<WebApiCore.Controllers.MainQuery.EmployeeQueryController>();

        var idToPinMap = new Dictionary<Guid, int> { { Guid.NewGuid(), 123 }, { Guid.NewGuid(), 456 } };

        /*
         https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql/comparing-guid-and-uniqueidentifier-values
         Both Guid and SqlGuid have a CompareTo method for comparing different GUID values. However, System.Guid.CompareTo and SqlTypes.SqlGuid.CompareTo are implemented differently.
         SqlGuid implements CompareTo using SQL Server behavior, in the last six bytes of a value are most significant. Guid evaluates all 16 bytes.
         */
        var sqlGuids = idToPinMap.Keys.Select(x => new SqlGuid(x.ToString())).ToList();
        sqlGuids.Sort();

        foreach (var item in idToPinMap)
        {
            var employeeIdentity = this.DataHelper.SaveEmployee(item.Key);
            var employee = employeeController.Evaluate(c => c.GetSimpleEmployee(employeeIdentity));
            employee.Pin = item.Value;
            employeeController.Evaluate(c => c.SaveEmployee(employee.ToStrict()));
        }

        // Act
        var result = employeeQueryController.Evaluate(
            c => c.GetSimpleEmployeesByODataQueryString("$top=1&$skip=1&$filter=Pin eq 123 or Pin eq 456"));

        // Assert
        var minId = sqlGuids[0].Value;
        var maxId = sqlGuids[1].Value;

        var firstPin = idToPinMap[minId];
        var secondPin = idToPinMap[maxId];

        var pins = result.Items.Select(x => x.Pin).ToArray();
        pins.Should().Contain(secondPin);
        pins.Should().NotContain(firstPin);
    }

    [TestMethod]
    public void GetEmployeeByOData_TakeAndSkipTestWithSorting_OnlyRequestedDataInTheResult()
    {
        // Arrange
        var employeeController = this.MainWebApi.Employee;
        var employeeQueryController = this.GetControllerEvaluator<WebApiCore.Controllers.MainQuery.EmployeeQueryController>();

        foreach (var pin in new[] { 123, 456 })
        {
            var employeeIdentity = this.DataHelper.SaveEmployee(Guid.NewGuid());
            var employee = employeeController.Evaluate(c => c.GetSimpleEmployee(employeeIdentity));
            employee.Pin = pin;
            employeeController.Evaluate(c => c.SaveEmployee(employee.ToStrict()));
        }

        // Act
        var result = employeeQueryController.Evaluate(
            c => c.GetSimpleEmployeesByODataQueryString("$top=1&$skip=1&$filter=Pin eq 123 or Pin eq 456&$orderby=Pin desc"));

        // Assert
        var pins = result.Items.Select(x => x.Pin).ToArray();
        pins.Should().Contain(123);
        pins.Should().NotContain(456);
    }

    [TestMethod]
    public void ForceDomainTypeEvent_ForceEmployeeSaveEvent_ContainsEventEmployee()
    {
        // Arrange
        var employeeIdentity = this.DataHelper.SaveEmployee(Guid.NewGuid());

        var configFacade = this.GetConfigurationControllerEvaluator();

        var domainType = configFacade.Evaluate(c => c.GetRichDomainTypeByName(nameof(Employee)));

        var operation = domainType.EventOperations.Single(op => op.Name == EventOperation.Save.Name);

        this.ClearIntegrationEvents();

        // Act
        configFacade.Evaluate(
            c => c.ForceDomainTypeEvent(
                new DomainTypeEventModelStrictDTO
                {
                    Operation = operation.Identity, DomainObjectIdents = new List<Guid> { employeeIdentity.Id }
                }));

        // Assert
        this.GetIntegrationEvents<EmployeeSaveEventDTO>().Should().ContainSingle(dto => dto.Employee.Id == employeeIdentity.Id);
        this.GetIntegrationEvents<EmployeeCustomEventModelSaveEventDTO>().Should()
            .ContainSingle(dto => dto.EmployeeCustomEventModel.Id == employeeIdentity.Id);
    }

    [TestMethod]
    public void ChangeEmployee_ProcessModifications_ContainsNotification()
    {
        // Arrange
        var employeeController = this.MainWebApi.Employee;

        var employeeIdentity = this.DataHelper.SaveEmployee(Guid.NewGuid());
        var employeeVersion = employeeController.Evaluate(c => c.GetSimpleEmployee(employeeIdentity)).Version;

        this.ClearNotifications();
        this.ClearModifications();

        employeeController.Evaluate(
            c => c.UpdateEmployee(
                new EmployeeUpdateDTO { Id = employeeIdentity.Id, Interphone = new Just<string>("1234"), Version = employeeVersion }));

        var restFacade = this.GetConfigurationControllerEvaluator();

        // Act
        var processedModCount = restFacade
                                .WithImpersonate(DefaultConstants.INTEGRATION_BUS)
                                .Evaluate(c => c.ProcessModifications(1000));

        // Assert
        var modifications = this.GetModifications();
        var notifications = this.GetNotifications();

        processedModCount.Should().BeGreaterThan(0);

        modifications.Should().ContainSingle(dto => dto.ModificationType == ModificationType.Save && dto.Identity == employeeIdentity.Id);
        notifications.Should().ContainSingle(
            dto => dto.From == "SampleSystem@luxoft.com"
                   && dto.Message.Message.Contains("Hi there!!!")
                   && dto.TechnicalInformation.ContextObjectId == employeeIdentity.Id);
    }

    [TestMethod]
    public void ChangeEmployee_ProcessModifications_ChangedUnprocessedCount()
    {
        // Arrange
        var employeeController = this.MainWebApi.Employee;
        var employeeIdentity = this.DataHelper.SaveEmployee(Guid.NewGuid());
        var employeeVersion = employeeController.Evaluate(c => c.GetSimpleEmployee(employeeIdentity)).Version;

        this.ClearNotifications();
        this.ClearModifications();

        employeeController.Evaluate(
            c => c.UpdateEmployee(
                new EmployeeUpdateDTO { Id = employeeIdentity.Id, Interphone = new Just<string>("1234"), Version = employeeVersion }));

        var restFacade = this.GetConfigurationControllerEvaluator();

        // Act
        var preProcessedModificationState = restFacade.Evaluate(c => c.GetModificationQueueProcessingState());
        var preProcessedNotificationState = restFacade.Evaluate(c => c.GetNotificationQueueProcessingState());

        restFacade.WithImpersonate(DefaultConstants.INTEGRATION_BUS).Evaluate(c => c.ProcessModifications(1000));

        var postProcessedModificationState = restFacade.Evaluate(c => c.GetModificationQueueProcessingState());
        var postProcessedNotificationState = restFacade.Evaluate(c => c.GetNotificationQueueProcessingState());

        // Assert
        preProcessedModificationState.UnprocessedCount.Should().Be(1);
        preProcessedNotificationState.UnprocessedCount.Should().Be(0);

        postProcessedModificationState.UnprocessedCount.Should().Be(0);
        postProcessedNotificationState.UnprocessedCount.Should().BeGreaterOrEqualTo(1);
    }

    [TestMethod]
    public void ChangeEmployee_ContainsAribaEvent()
    {
        // Arrange
        var employeeController = this.MainWebApi.Employee;
        var employeeIdentity = this.DataHelper.SaveEmployee(Guid.NewGuid());
        var employeeVersion = employeeController.Evaluate(c => c.GetSimpleEmployee(employeeIdentity)).Version;

        this.ClearIntegrationEvents();

        // Act
        employeeController.Evaluate(
            c => c.UpdateEmployee(
                new EmployeeUpdateDTO { Id = employeeIdentity.Id, Interphone = new Just<string>("1234"), Version = employeeVersion }));

        // Assert
        this.GetIntegrationEvents<EmployeeSaveEventDTO>("ariba").Should().ContainSingle(dto => dto.Employee.Id == employeeIdentity.Id);
    }

    [TestMethod]
    [Ignore]
    public void EventListenerTest()
    {
        this.Evaluate(
            DBSessionMode.Write,
            bllContext =>
            {
                var dbSession = bllContext.ServiceProvider.GetRequiredService<IDBSession>();
                var writeNhibSession = dbSession as WriteNHibSession;
                var impl = writeNhibSession.NativeSession as SessionImpl;
                return;
            });
    }



    [TestMethod]
    public void ChangeEmployeeWithoutVersionInfo_RaisedStateException()
    {
        // Arrange
        var employeeController = this.MainWebApi.Employee;
        var employeeIdentity = this.DataHelper.SaveEmployee(Guid.NewGuid());

        // Act
        var call = new Action(
            () => employeeController.Evaluate(
                c => c.UpdateEmployee(new EmployeeUpdateDTO { Id = employeeIdentity.Id, Interphone = new Just<string>("1234") })));

        // Assert
        call.Should().Throw<Exception>().WithMessage($"Object '{nameof(Employee)}' was updated or deleted by another transaction");
    }

    [TestMethod]
    public void CreateEmployeeFilter_IsNotVirtual()
    {
        // Arrange
        var buIdentity = this.DataHelper.SaveBusinessUnit();

        // Act
        var isVirtualResult = this.Evaluate(
            DBSessionMode.Read,
            ctx =>
            {
                var filter = new TestEmployeeFilter { BusinessUnit = ctx.Logics.BusinessUnit.GetById(buIdentity.Id, true) };

                var operation = SelectOperation<Employee>.Default.AddFilter(e => e.CoreBusinessUnit.Id == filter.BusinessUnit.Id);

                return operation.IsVirtual;
            });

        // Assert
        isVirtualResult.Should().Be(false);
    }

    [TestMethod]
    public void LoadEmployeeCellPhoneByDependencySecurity_ObjectLoaded()
    {
        // Arrange

        // Act
        var notNull = this.Evaluate(
            DBSessionMode.Read,
            ctx =>
            {
                var objects = ctx.Logics.Default.Create<EmployeeCellPhone>(SecurityRule.View).GetFullList();

                return objects != null;
            });

        // Assert
        notNull.Should().Be(true);
    }
}
