using System;
using System.Linq;

using Configuration.WebApi.Controllers;

using Framework.Authorization.Domain;
using Framework.Configuration.Generated.DTO;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.DomainDriven.ServiceModel.Service;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.Domain;
using SampleSystem.IntegrationTests.__Support.ServiceEnvironment;
using SampleSystem.WebApiCore.Controllers;

namespace SampleSystem.BLL.Test
{
    [TestClass]
    public class ConfigurationTests
    {
        [TestMethod]
        public void TestCreateModification()
        {
            var environment = SampleSystemTestServiceEnvironment.Default;

            environment.GetContextEvaluator().Evaluate(DBSessionMode.Write, context =>
            {
                var empl = context.Logics.Employee.GetUnsecureQueryable().FirstOrDefault();

                empl.Interphone = Guid.NewGuid().ToString().Take(25).Concat();

                context.Logics.Employee.Save(empl);

                return;
            });
        }

        [TestMethod]
        public void TestForceSendEvent()
        {
            var environment = SampleSystemTestServiceEnvironment.Default;

            var configFacade = environment.ServiceProvider.GetDefaultControllerEvaluator<ConfigSLJsonController>();

            //var mainFacade = new Facade(environment);

            var domainType = configFacade.Evaluate(c => c.GetRichDomainTypeByName(nameof(BusinessUnit)));

            var operation = domainType.EventOperations.Single(op => op.Name == "Save");

            //var bu = mainFacade.GetVisualBusinessUnitByName("BU1");

            configFacade.Evaluate(c => c.ForceDomainTypeEvent(new DomainTypeEventModelStrictDTO
            {
                Operation = operation.Identity,

                //DomainObjectId = new Guid("AA57E4AF-3BE6-42BD-B6F7-6691E6CCF9AA")// bu.Id
            }));

            return;
        }
    }
}
