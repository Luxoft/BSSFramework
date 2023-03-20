using Framework.Projection.Lambda;

using SampleSystem.Domain;

namespace SampleSystem.CodeGenerate;

public class CustomSampleSystemProjectionSource : ProjectionSource
{
    public CustomSampleSystemProjectionSource()
    {
        this.TestEmployee = new Projection<Employee>(() => this.TestEmployee, true)
                            .Property(employee => employee.ExternalId)
                            .Property(employee => employee.CoreBusinessUnit.Name)
                //.Property(employee => employee.CoreBusinessUnit.Id)
                //.Property(employee => employee.CoreBusinessUnit.Period.EndDate, "BuEndDate")
                ;

        //this.TestEmployeeCellPhone = new Projection<EmployeeCellPhone>(() => this.TestEmployeeCellPhone)
        //    .Property(cellPhone => cellPhone.Number);

        //this.TestHRDepartmenty =
        //    new Projection<HRDepartment>(() => this.TestHRDepartmenty, true)
        //        .Property(dep => dep.Location.Children, () => this.TestSubLocation)
        //        .Property(dep => dep.CompanyLegalEntity.CurrentObj, () => this.TestObjForNestedIdentity1, "M1")
        //        .Property(dep => dep.CompanyLegalEntity.CurrentObj, () => this.TestObjForNestedIdentity2, "M2")
        //        .Property(dep => dep.CompanyLegalEntity.CurrentObj.Name)
        //    ;

        //this.TestObjForNestedIdentity1 = new Projection<TestObjForNested>(() => this.TestObjForNestedIdentity1)
        //    .Property(obj => obj.Active);

        //this.TestObjForNestedIdentity2 = new Projection<TestObjForNested>(() => this.TestObjForNestedIdentity2)
        //    .Property(obj => obj.ModifiedBy);

        //this.TestSubLocation = new Projection<Location>(() => this.TestSubLocation)
        //    .Property(prop => prop.Name);

        this.BusinessUnitIdentity = new Projection<BusinessUnit>(() => this.BusinessUnitIdentity);
    }

        

    public Projection<Employee> TestEmployee { get; }

    public Projection<BusinessUnit> BusinessUnitIdentity { get; }

    //public Projection<EmployeeCellPhone> TestEmployeeCellPhone { get; }

    //public Projection<HRDepartment> TestHRDepartmenty { get; }

    //public Projection<TestObjForNested> TestObjForNestedIdentity1 { get; }

    //public Projection<TestObjForNested> TestObjForNestedIdentity2 { get; }

    //public Projection<Location> TestSubLocation { get; }
}

//using Framework.Projection.Lambda;

//using SampleSystem.Domain;

//namespace SampleSystem.CodeGenerate
//{
//    public class CustomSampleSystemProjectionSource : ProjectionSource
//    {
//        public CustomSampleSystemProjectionSource()
//        {
//            this.TestEmployee =
//                new Projection<Employee>(() => this.TestEmployee, true)
//                    .Property(employee => employee.CoreBusinessUnit.BusinessUnitType, () => this.VisualBusinessUnitType, "M1")
//                    .Property(employee => employee.CoreBusinessUnit.BusinessUnitType.CanBeLinkedToClient)
//                    //.Property(employee => employee.CoreBusinessUnit.Projects, () => this.VisualProject)
//                ;

//            //this.VisualProject = new Projection<Project>(() => this.VisualProject)
//            //    .Property(proj => proj.Code);

//            this.VisualBusinessUnitType = new Projection<BusinessUnitType>(() => this.VisualBusinessUnitType)
//                .Property(t => t.Name);
//        }

//        public Projection<Employee> TestEmployee { get; }

//        //public Projection<Project> VisualProject { get; }

//        public Projection<BusinessUnitType> VisualBusinessUnitType { get; }
//    }
//}
