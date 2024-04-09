using Framework.Authorization;
using Framework.Core;
using Framework.Projection;
using Framework.Projection.Lambda;
using Framework.Security;

using SampleSystem.Domain;
using SampleSystem.Domain.Models.Filters;

namespace SampleSystem.CodeGenerate;

public class SampleSystemProjectionSource : ProjectionSource
{
    public SampleSystemProjectionSource()
    {
        this.TestEmployee = new Projection<Employee>(() => this.TestEmployee, true)
                            .Property(employee => employee.Login)
                            .Property(employee => employee.Role.Name)
                            .Property(employee => employee.Role.Id)

                            .Property(employee => employee.NameEng.FirstName)

                            .Property(employee => employee.Ppm.NameNative.MiddleName)

                            .Property(employee => employee.CoreBusinessUnit.Period.EndDate, "BuEndDate")

                            .Property(employee => employee.CoreBusinessUnit, () => this.BusinessUnitIdentity)

                            .Property(employee => employee.CoreBusinessUnit.Name)
                            .Property(employee => employee.Position.Name)
                            .Property(employee => employee.CoreBusinessUnit.Projects, () => this.VisualProject)

                            .Filter<TestEmployeeFilter>(ProjectionFilterTargets.OData | ProjectionFilterTargets.Collection)
                            .Filter<EmployeeFilterModel>(ProjectionFilterTargets.Collection)
                            .Filter<SingleEmployeeFilterModel>(ProjectionFilterTargets.Single);

        this.TestBusinessUnit = new Projection<BusinessUnit>(() => this.TestBusinessUnit, true)
                                .Property(bu => bu.Name)
                                //.Property(bu => bu.Order)
                                .Property(bu => bu.Period.EndDate)

                                .Property(bu => bu.Parent.Period.StartDate)

                                .Property(bu => bu, () => this.HerBusinessUnit, "Her", ignoreSerialization: true)
                                .CustomProperty<string>("HerBusinessUnit_Full") // Расчётное свойство типа "string"

                                .Property(bu => bu.BusinessUnitEmployeeRoles, () => this.MiniBusinessUnitEmployeeRole, ignoreSerialization: true)
                                .CustomProperty<string>("Employees", fetchs: new[] { "BusinessUnitEmployeeRoles.Employee" }) // Расчётное свойство типа "string" и подгрузка для него из базы "BusinessUnitEmployeeRoles.Employee"

                                .CustomProperty<string>("CalcProp", writable: true) // Расчётное свойство типа "string" с разрешенной записью в него
                                .CustomProperty<string[][]>("CalcMatrix") // Расчётное свойство типа "string[][]"
                                .CustomProperty("CalcProjectionProp", () => this.TestBusinessUnitType) // Расчётное свойство типа "TestBusinessUnitType"
                                .Filter<HierarchicalBusinessUnitFilterModel>(ProjectionFilterTargets.ODataTree);

        this.TestBusinessUnitType = new Projection<BusinessUnitType>(() => this.TestBusinessUnitType)
                                    .Property(employee => employee.Id, ignoreSerialization: true) // Id-поле исключается из сериализации, наследование этой ProjectionDTO будет производится от BaseAbstractDTO
                                    .Property(bu => bu.Name);

        this.TestLocation = new Projection<Location>(() => this.TestLocation, true)
                .Property(location => location.Name);

        this.TestDepartment = new Projection<HRDepartment>(() => this.TestDepartment, true)
                              .Property(department => department.Name)
                              .Property(department => department.Location, () => this.TestLocation)
                              .Property(department => department.Location.BinaryData);

        this.HerBusinessUnit = new Projection<BusinessUnit>(() => this.HerBusinessUnit)
                               .Property(bu => bu.Name)
                               .Property(bu => bu.Parent, () => this.HerBusinessUnit);

        this.MiniBusinessUnitEmployeeRole = new Projection<BusinessUnitEmployeeRole>(() => this.MiniBusinessUnitEmployeeRole)
                .Property(link => link.Employee, () => this.VisualEmployee);

        this.VisualEmployee = new Projection<Employee>(() => this.VisualEmployee)
                              .Property(emp => emp.NameEng, ignoreSerialization: true)
                              .Property(emp => emp.NameEng.FirstName);

        this.TestLocationCollectionProperties = new Projection<Location>(() => this.TestLocationCollectionProperties, true)
                                                .Property(location => location.Name)
                                                .Property(x => x.Children, () => this.TestLocation)
                                                .CustomProperty<Guid[]>("Child_Identities")
                                                .CustomProperty<Period[]>("Child_Periods")
                                                .CustomProperty<DateTime[]>("Date_Intervals")
                                                .CustomProperty<string[]>("Security_Codes");

        this.TestIMRequest = new Projection<IMRequest>(() => this.TestIMRequest, true)
                             .Property(request => request.Message)
                             .Property(request => request.OneToOneDetail, () => this.TestIMRequestDetail);

        this.TestIMRequestDetail = new Projection<IMRequestDetail>(() => this.TestIMRequestDetail);

        this.CustomCompanyLegalEntity = new Projection<CompanyLegalEntity>(() => this.CustomCompanyLegalEntity, true)
                                        .Attribute(new ExampleCustomProjectionAttribute()) // Добавлям кастомный атрибут в проекцию
                                        .Attribute(new ViewDomainObjectAttribute(AuthorizationSecurityRule.AuthorizationImpersonate)) // Подменяем атрибут доступа проекции
                                        .Property(legalEntity => legalEntity.Code, propertyAttributes: new Attribute[] { new ViewDomainObjectAttribute(SampleSystemSecurityOperation.CompanyLegalEntityView) }) // Добавляем свойство и атрибут доступа к нему
                                        .Property(legalEntity => legalEntity.Name)
                                        .Property(legalEntity => legalEntity.NameEnglish)
                                        .Property(legalEntity => legalEntity.CurrentObj, () => this.CustomTestObjForNested)
                                        .Property(legalEntity => legalEntity.BaseObj, () => this.CustomTestObjForNested)
                                        .Property(obj => obj.AribaStatus.Type)
                                        .Property(obj => obj.AribaStatus.Description);

        this.CustomTestObjForNested = new Projection<TestObjForNested>(() => this.CustomTestObjForNested, false)
                                      .Property(obj => obj.Period.StartDate, "PeriodStartDateXXX")
                                      .Property(obj => obj.Name);

        this.UnpersitentContainer = new Projection<DomainObjectBase>(() => this.UnpersitentContainer)
                                    .CustomProperty<string>("TestString", true)
                                    .CustomProperty("TestBU", true, () => this.TestBusinessUnit)
                                    .CustomManyProperty<Period>("PeriodArray", true, null, typeof(Array))
                                    .CustomManyProperty("Locations", true, () => this.TestLocation, typeof(List<>));

        this.VisualProject = new Projection<Project>(() => this.VisualProject)
                .Property(proj => proj.Code);

        this.BusinessUnitIdentity = new Projection<BusinessUnit>(() => this.BusinessUnitIdentity);

        this.BusinessUnitProgramClass =
                new Projection<BusinessUnit>(() => this.BusinessUnitProgramClass, true)
                        .Property(z => z.Name)
                        .Property(z => z.IsNewBusiness)
                        .CustomProperty<string>("VirtualValue")
                        .CustomProperty<string>("VirtualName")
                        .Property(z => z.Period.EndDate, ignoreSerialization: true)
                        .Property(z => z.BusinessUnitType.Id, ignoreSerialization: true)
                        .Filter<BusinessUnitProgramClassFilterModel>();

        this.TestSecurityObjItemProjection =
                new Projection<TestSecurityObjItem>(() => this.TestSecurityObjItemProjection, true)
                        .Property(item => item.Name);

        this.TestCustomContextSecurityObjProjection =
                new Projection<TestCustomContextSecurityObj>(() => this.TestCustomContextSecurityObjProjection, true)
                        .Property(item => item.Name);
    }

    public Projection<TestCustomContextSecurityObj> TestCustomContextSecurityObjProjection { get; }

    public Projection<BusinessUnit> BusinessUnitProgramClass { get; }

    public Projection<Employee> TestEmployee { get; }

    public Projection<IMRequest> TestIMRequest { get; }

    public Projection<IMRequestDetail> TestIMRequestDetail { get; }

    public Projection<BusinessUnitEmployeeRole> MiniBusinessUnitEmployeeRole { get; }

    public Projection<Employee> VisualEmployee { get; }

    public Projection<BusinessUnit> TestBusinessUnit { get; }

    public Projection<BusinessUnitType> TestBusinessUnitType { get; }

    public Projection<BusinessUnit> HerBusinessUnit { get; }

    public Projection<BusinessUnit> BusinessUnitIdentity { get; }

    public Projection<Location> TestLocation { get; }

    public Projection<HRDepartment> TestDepartment { get; }

    public Projection<Location> TestLocationCollectionProperties { get; }

    public Projection<CompanyLegalEntity> CustomCompanyLegalEntity { get; }

    public Projection<TestObjForNested> CustomTestObjForNested { get; }

    public Projection<DomainObjectBase> UnpersitentContainer { get; }

    public Projection<Project> VisualProject { get; }

    public Projection<TestSecurityObjItem> TestSecurityObjItemProjection { get; }
}
