namespace SampleSystem.IntegrationTests;

//[TestClass]
//public class SpecificationTests : TestBase
//{
//    [TestMethod]
//    public void SpecificationWithProjection()
//    {
//        // Arrange
//        this.DataHelper.SaveCountry(name: "test1");
//        this.DataHelper.SaveCountry(name: "test2");

//        // Act

//        var c = this.EvaluateWrite(context => context.Logics.Country.Single(new Test1NameSpecification()));

//        // Assert
//        c.Name.Should().Be("test1");
//    }

//    [TestMethod]
//    public void GetQueryWithFetch()
//    {
//        // Arrange
//        var dep1 = this.DataHelper.SaveHRDepartment(name: "dep1");
//        this.DataHelper.SaveEmployee(login: "emp1", hrDepartment: dep1);

//        // Act
//        var employee =
//                this.EvaluateWrite(context => context.Logics.Employee.Single(new EmployeeWithFetchSpecification()));

//        // Assert
//        employee.Login.Should().Be("emp1");
//        NHibernateUtil.IsInitialized(employee.HRDepartment).Should().BeTrue();
//    }

//    private class EmployeeWithFetchSpecification : Specification<Employee>
//    {
//        public EmployeeWithFetchSpecification()
//        {
//            this.Query = q => q.Where(x => x.Login == "emp1");
//            this.AddFetch(q => q.Fetch(f => f.HRDepartment));
//        }
//    }

//    private class Test1NameSpecification : Specification<Country, CountryProjection>
//    {
//        public Test1NameSpecification() =>
//                this.Query = q => q.Where(x => x.Name == "test1").Select(x => new CountryProjection { Name = x.Name });
//    }

//    private class CountryProjection
//    {
//        public string Name
//        {
//            get;
//            set;
//        }
//    }
//}
