using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentAssertions;

using Framework.Configuration.Domain;
using Framework.Core;
using Framework.DomainDriven.DAL.Revisions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.Domain;
using SampleSystem.IntegrationTests.__Support.ServiceEnvironment.IntegrationTests;
using SampleSystem.IntegrationTests.__Support.TestData;

namespace SampleSystem.IntegrationTests
{
    [TestClass]
    public class SubscriptionWithSecurityItemTests : TestBase
    {
        [TestMethod]
        public void SubscriptionWithSecurityItem_SubscriptionShouldBeProcessed()
        {
            // Arrange
            this.CreateSubscription();
            var location = this.DataHelper.SaveLocation();
            this.DataHelper.SaveHRDepartment(location: location);

            var dto = this.GetModifications().Single(omi => omi.TypeInfoDescription.Name == "HRDepartment");

            var typeInfoDescription = new TypeInfoDescription();
            typeInfoDescription.Name = dto.TypeInfoDescription.Name;
            typeInfoDescription.NameSpace = dto.TypeInfoDescription.NameSpace;

            var change = new ObjectModificationInfo<Guid>(
                                                          dto.Identity,
                                                          typeInfoDescription,
                                                          dto.ModificationType,
                                                          dto.Revision);

            // Act
            var result = this.DataHelper.ProcessChangedObjectInfo(change);

            // Assert
            result.GetErrors().Should().BeEmpty();
        }

        private void CreateSubscription(string code = null)
        {
            var messageTemplate = this.DataHelper.SaveMessageTemplate("T000");

            var domainType = this.DataHelper.GetDomainType(typeof(HRDepartment));

            var conditionLambda = this.DataHelper.SaveSubscriptionLambda(
                                                                         "CL000",
                                                                         SubscriptionLambdaType.Condition,
                                                                         domainType,
                                                                         "(prev, next) => true",
                                                                         false);

            var generationLambda = this.DataHelper.SaveSubscriptionLambda(
                                                                          "GL000",
                                                                          SubscriptionLambdaType.Generation,
                                                                          domainType,
                                                                          $"(p, c) => new List<NotificationMessageGenerationInfo>();",
                                                                          false);

            var copyGenerationLambda = this.DataHelper.SaveSubscriptionLambda(
                                                                              "GL001",
                                                                              SubscriptionLambdaType.Generation,
                                                                              domainType,
                                                                              $"(p, c) => new List<NotificationMessageGenerationInfo>();",
                                                                              false);

            var subscription = this.DataHelper.SaveSubscription(
                                                                code ?? "S000",
                                                                true,
                                                                domainType,
                                                                messageTemplate,
                                                                conditionLambda,
                                                                generationLambda,
                                                                copyGenerationLambda);

            this.DataHelper.SaveSecurityItem(subscription, domainType, typeof(Location), "(p, c) => new [] { c.Location }");
        }
    }
}
