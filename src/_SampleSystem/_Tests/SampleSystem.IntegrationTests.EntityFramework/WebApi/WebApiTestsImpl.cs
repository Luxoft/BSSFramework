using SampleSystem.IntegrationTests.WebApi;

namespace SampleSystem.IntegrationTests;

public class WebApiTestsImpl(IServiceProvider rootServiceProvider) : WebApiTests(rootServiceProvider);
