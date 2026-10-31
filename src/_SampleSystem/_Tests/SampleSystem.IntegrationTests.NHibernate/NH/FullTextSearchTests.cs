using Anch.Testing.Xunit;

using Framework.AutomationCore.RootServiceProviderContainer;

using SampleSystem.IntegrationTests._Environment.TestData;

namespace SampleSystem.IntegrationTests.NH;

public class FullTextSearchTests(IServiceProvider rootServiceProvider) : TestBase(rootServiceProvider)
{
    /// <summary>
    /// The test is intended to verify that the NHibernate dialect works,
    /// using search by the SQL Server Full Text Catalog.
    /// The test is deliberately disabled because the duration of building the catalog is indeterminate,
    /// and the test uses a wait so that the catalog is (presumably) built.
    /// Nevertheless, the test itself is worth keeping for a quick check of the dialect.
    /// To run the test correctly, in the file
    /// "/src/_SampleSystem/_Tests/SampleSystem.IntegrationTests/__Support/Scripts/SampleSystem/Sample.sql"
    /// uncomment the SQL code with the header "Create and populate Full Text Catalog".
    /// </summary>
    [AnchFact(Skip = "Skip")]
    public void FullTextContainsFunctionWorksCorrect(CancellationToken ct)
    {
        Task.Delay(10000, ct).Wait(ct);

        this.EvaluateRead(context =>
                          {
                              var bll = context.Logics.Employee;
                              var employees = bll.GetUnsecureQueryable().Where(e => e.Email.FullTextContains("admin")).ToList();
                              Assert.Single(employees);
                          });
    }
}
