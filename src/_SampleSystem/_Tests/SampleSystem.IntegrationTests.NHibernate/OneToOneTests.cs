using Framework.Application;
using Framework.Database;

using SampleSystem.Domain.Employee;
using SampleSystem.IntegrationTests._Environment.TestData;
using SampleSystem.WebApiCore.Controllers.MainQuery;

namespace SampleSystem.IntegrationTests;

public class OneToOneTests(IServiceProvider rootServiceProvider) : TestBase(rootServiceProvider)
{
    [Fact]
    public void GetRequestProjection_ContainsOneToOneDetail_Initialized()
    {
        // Arrange
        var iMRequestQueryController = this.GetControllerEvaluator<IMRequestQueryController>();

        var idents = this.Evaluate(
                                   DBSessionMode.Write,
                                   context =>
                                   {
                                       var bll = context.Logics.Default.Create<IMRequest>();

                                       var request = new IMRequest { Name = $"TestRequestName_{Guid.NewGuid()}" };
                                       request.OneToOneDetail = new IMRequestDetail(request);

                                       bll.Save(request);

                                       return new
                                              {
                                                      RequestId = request.Id,

                                                      DetailId = request.OneToOneDetail.Id
                                              };
                                   });

        // Act
        var result = iMRequestQueryController.Evaluate(c => c.GetTestIMRequestsByODataQueryString(""));

        // Assert
        Assert.Contains(result.Items, request => request.Id == idents.RequestId && request.OneToOneDetail.Id == idents.DetailId);
    }
}
