using System.Data.SqlTypes;

using Anch.Core;

using Framework.Application;
using Framework.Application.Events;
using Framework.AutomationCore.Utils.DatabaseUtils;
using Framework.Configuration.Generated.DTO;
using Framework.Database;
using Framework.Database.NHibernate.Sessions;

using Anch.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

using NHibernate.Impl;

using SampleSystem.Domain.Employee;
using SampleSystem.Generated.DTO;
using SampleSystem.IntegrationTests.__Support.TestData;

namespace SampleSystem.IntegrationTests;

public class EmployeeTests : TestBase
{
    [Fact]
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
        Assert.Single(actual);
        Assert.True(actual.Select(z => z.Age).All(z => z == 10));
    }

    [Fact]
    public void AddNewEmployee_CheckEmployeeSaved()
    {
        // Arrange
        var employeeController = this.MainWebApi.Employee;
        var employeeIdentity = this.DataHelper.SaveEmployee(Guid.NewGuid());

        // Act
        var employees = employeeController.Evaluate(c => c.GetSimpleEmployees());

        // Assert
        Assert.Contains(employees, e => e.Id == employeeIdentity.Id);
    }

    [Fact]
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
        Assert.Contains(123, pins);
        Assert.DoesNotContain(456, pins);
    }

    [Fact]
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
        Assert.Contains(firstPin, pins);
        Assert.DoesNotContain(secondPin, pins);
    }

    [Fact]
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
        Assert.Contains(secondPin, pins);
        Assert.DoesNotContain(firstPin, pins);
    }

    [Fact]
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
        Assert.Contains(123, pins);
        Assert.DoesNotContain(456, pins);
    }

    [Fact]
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
        Assert.Single(this.GetIntegrationEvents<EmployeeSaveEventDTO>(), dto => dto.Employee.Id == employeeIdentity.Id);
        Assert.Single(this.GetIntegrationEvents<EmployeeCustomEventModelSaveEventDTO>(), dto => dto.EmployeeCustomEventModel.Id == employeeIdentity.Id);
    }

    [Fact]
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
                new EmployeeUpdateDTO { Id = employeeIdentity.Id, Interphone = Maybe.Return("1234"), Version = employeeVersion }));

        var restFacade = this.GetConfigurationControllerEvaluator();

        // Act
        var processedModCount = restFacade
                                .WithImpersonate(DefaultConstants.INTEGRATION_BUS)
                                .Evaluate(c => c.ProcessModifications(1000));

        // Assert
        var modifications = this.GetModifications();
        var notifications = this.GetNotifications();

        Assert.True(processedModCount > 0);

        Assert.Single(modifications, dto => dto.ModificationType == ModificationType.Save && dto.Identity == employeeIdentity.Id);
        Assert.Single(
            notifications,
            dto => dto.From == "SampleSystem@luxoft.com"
                   && dto.Message.Message.Contains("Hi there!!!")
                   && dto.TechnicalInformation.ContextObjectId == employeeIdentity.Id);
    }

    [Fact]
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
                new EmployeeUpdateDTO { Id = employeeIdentity.Id, Interphone = Maybe.Return("1234"), Version = employeeVersion }));

        var restFacade = this.GetConfigurationControllerEvaluator();

        // Act
        var preProcessedModificationState = restFacade.Evaluate(c => c.GetModificationQueueProcessingState());
        var preProcessedNotificationState = restFacade.Evaluate(c => c.GetNotificationQueueProcessingState());

        restFacade.WithImpersonate(DefaultConstants.INTEGRATION_BUS).Evaluate(c => c.ProcessModifications(1000));

        var postProcessedModificationState = restFacade.Evaluate(c => c.GetModificationQueueProcessingState());
        var postProcessedNotificationState = restFacade.Evaluate(c => c.GetNotificationQueueProcessingState());

        // Assert
        Assert.Equal(1, preProcessedModificationState.UnprocessedCount);
        Assert.Equal(0, preProcessedNotificationState.UnprocessedCount);

        Assert.Equal(0, postProcessedModificationState.UnprocessedCount);
        Assert.True(postProcessedNotificationState.UnprocessedCount >= 1);
    }

    [Fact]
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
                new EmployeeUpdateDTO { Id = employeeIdentity.Id, Interphone = Maybe.Return("1234"), Version = employeeVersion }));

        // Assert
        Assert.Single(this.GetIntegrationEvents<EmployeeSaveEventDTO>("ariba"), dto => dto.Employee.Id == employeeIdentity.Id);
    }

    [Fact(Skip = "Skip")]
    public void EventListenerTest() =>
        this.Evaluate(
            DBSessionMode.Write,
            bllContext =>
            {
                var dbSession = bllContext.ServiceProvider.GetRequiredService<IDBSession>();
                var writeNhibSession = dbSession as WriteNHibSession;
                var impl = writeNhibSession.NativeSession as SessionImpl;
                return;
            });

    [Fact]
    public void ChangeEmployeeWithoutVersionInfo_RaisedStateException()
    {
        // Arrange
        var employeeController = this.MainWebApi.Employee;
        var employeeIdentity = this.DataHelper.SaveEmployee(Guid.NewGuid());

        // Act
        var call = new Action(
            () => employeeController.Evaluate(
                c => c.UpdateEmployee(new EmployeeUpdateDTO { Id = employeeIdentity.Id, Interphone = Maybe.Return("1234") })));

        // Assert
        Assert.Equal($"Object '{nameof(Employee)}' was updated or deleted by another transaction", Assert.Throws<StaleDomainObjectStateException>(call).Message);
    }

    [Fact]
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
        Assert.True(notNull);
    }
}
