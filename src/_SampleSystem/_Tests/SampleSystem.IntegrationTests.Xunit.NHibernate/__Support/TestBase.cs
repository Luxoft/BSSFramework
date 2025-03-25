namespace SampleSystem.IntegrationTests.Xunit.NHibernate.__Support;

public class TestBase(IServiceProvider serviceProvider)
{
    protected IServiceProvider ServiceProvider { get; } = serviceProvider;
}
