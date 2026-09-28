using System.Net.Mail;
using System.Text;

using Framework.Notification.MailMessageModifier;

namespace Framework.Notification.Tests;

public class GdprMailMessageModifierTests
{
    private readonly GdprMailMessageModifier service = new();

    private static MailMessage GetMessage(string body) => new () { Body = body };

    [Fact]
    public void Cleanup_SentMessage_RemoveFewGdprBlocks()
    {
        // Arrange
        var raw = new StringBuilder();
        raw.AppendLine("<html>");

        raw.AppendLine("<!--GDPR-->");
        raw.AppendLine("Personal Data");
        raw.AppendLine("<!--/GDPR-->");

        raw.AppendLine("General Data");

        raw.AppendLine("<!--GDPR-->");
        raw.AppendLine("Personal Data");
        raw.AppendLine("<!--/GDPR-->");

        raw.AppendLine("</html>");

        var message = GetMessage(raw.ToString());

        var cleaned = new StringBuilder();
        cleaned.AppendLine("<html>");
        cleaned.AppendLine(string.Empty);
        cleaned.AppendLine("General Data");
        cleaned.AppendLine(string.Empty);
        cleaned.AppendLine("</html>");

        // Act
        this.service.Modify(message);

        // Assert
        Assert.Equal(cleaned.ToString(), message.Body);
    }

    [Fact]
    public void Cleanup_SentMessage_RemoveGdprBlock()
    {
        // Arrange
        var message = GetMessage("<!--GDPR-->Personal Data<!--/GDPR-->");

        // Act
        this.service.Modify(message);

        // Assert
        Assert.Equal(string.Empty, message.Body);
    }

    [Fact]
    public void Cleanup_SentMessage_RemoveNestedGdprBlocks()
    {
        // Arrange
        var raw = new StringBuilder();
        raw.AppendLine("<html>");

        raw.AppendLine("<!--GDPR-->");
        raw.AppendLine("Personal Data");

        raw.AppendLine("<!--GDPR-->");
        raw.AppendLine("Personal Data");
        raw.AppendLine("<!--/GDPR-->");

        raw.AppendLine("Personal Data");
        raw.AppendLine("<!--/GDPR-->");

        raw.AppendLine("</html>");

        var message = GetMessage(raw.ToString());

        var cleaned = new StringBuilder();
        cleaned.AppendLine("<html>");
        cleaned.AppendLine(string.Empty);
        cleaned.AppendLine("</html>");

        // Act
        this.service.Modify(message);

        // Assert
        Assert.Equal(cleaned.ToString(), message.Body);
    }
}
