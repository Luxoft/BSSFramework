using System;

using FluentAssertions;

using Framework.DomainDriven.BLL;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.Domain;
using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.WebApiCore.Controllers.MainQuery;

namespace SampleSystem.IntegrationTests
{
    [TestClass]
    public class OneToOneTests : TestBase
    {
        [TestMethod]
        public void GetRequestProjection_ContainsOneToOneDetail_Initialized()
        {
            // Arrange
            var iMRequestQueryController = this.GetController<IMRequestQueryController>();

            var idents = this.DataHelper.Environment.GetContextEvaluator().Evaluate(
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
            var result = iMRequestQueryController.GetTestIMRequestsByODataQueryString($"");

            // Assert
            result.Items.Should().Contain(request => request.Id == idents.RequestId && request.OneToOneDetail.Id == idents.DetailId);
        }
    }
}
