using System.Linq;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AttachmentsSampleSystem.Domain;
using AttachmentsSampleSystem.IntegrationTests.__Support.TestData;

using Framework.Attachments.Generated.DTO;

namespace AttachmentsSampleSystem.IntegrationTests
{
    [TestClass]
    public class AttachmentTests : TestBase
    {
        [TestMethod]
        public void SaveAttachment_AttachmentExists()
        {
            // Arrange
            var employeeIdent = this.DataHelper.SaveEmployee();

            var sourceAttachment = new AttachmentStrictDTO
            {
                Name = "TestFile",
                Content = new byte[] { 1, 2, 3 }
            };

            var attachmentController = this.GetAttachmentControllerEvaluator();

            // Act
            var attachmentIdentity = attachmentController.Evaluate(c => c.SaveAttachment(nameof(Employee), employeeIdent.Id, sourceAttachment));

            var reloadedAttachment = attachmentController.Evaluate(c => c.GetRichAttachment(attachmentIdentity));

            // Assert
            reloadedAttachment.Name.SequenceEqual(sourceAttachment.Name).Should().Be(true);
            reloadedAttachment.Content.SequenceEqual(sourceAttachment.Content).Should().Be(true);
        }
    }
}
