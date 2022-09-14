using System.Linq;
using System.Threading.Tasks;

using Automation.ServiceEnvironment;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SampleSystem.IntegrationTests.__Support.TestData;

namespace SampleSystem.IntegrationTests.NH
{
    [TestClass]
    public class FullTextSearchTests : TestBase
    {
        /// <summary>
        /// Тест предназначен для проверки работоспособности диалекта NHibernate,
        /// использующего поиск по SQL Server Full Text Catalog.
        /// Тест специально отключен, поскольку длительность построения каталога является неопределенной,
        /// и тест использует ожидание для того, чтобы каталог (передположительно) был построен.
        /// Тем не менее, сам тест хочется сохранить для оперативной проверки работы диалекта.
        /// Для правильного прогона теста, необходимо в файле
        /// "/src/_SampleSystem/_Tests/SampleSystem.IntegrationTests/__Support/Scripts/SampleSystem/Sample.sql"
        /// раскоментировать SQL код с заголовком "Create and populate Full Text Catalog".
        /// </summary>
        [TestMethod]
        [Ignore]
        public void FullTextContainsFunctionWorksCorrect()
        {
            Task.Delay(10000).Wait();

            this.EvaluateRead(context =>
            {
                var bll = context.Logics.Employee;
                var employees = bll.GetUnsecureQueryable().Where(e => e.Email.FullTextContains("admin")).ToList();
                employees.Should().HaveCount(1);
            });
        }
    }
}
