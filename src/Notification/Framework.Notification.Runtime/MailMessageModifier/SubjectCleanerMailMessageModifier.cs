using System.Net.Mail;

using Framework.Core;
using Framework.Notification.Settings;

using Microsoft.Extensions.Options;

namespace Framework.Notification.MailMessageModifier;

public class SubjectCleanerMailMessageModifier(IOptions<SmtpSettings> smtpSettings) : IMailMessageModifier
{
    public void Modify(MailMessage message) => message.Subject = this.Clean(message.Subject);

    public string Clean(string subject) => this.CutSubjectOnRight(this.ReplaceUnsupportedCharactersForSubject(subject));

    private string CutSubjectOnRight(string subject)
    {
        return subject.SubStringUnsafe(smtpSettings.Value.MaxSubjectLength);
    }

    private string ReplaceUnsupportedCharactersForSubject(string subject) => subject.Replace(Environment.NewLine, " ").Replace('\r', ' ').Replace('\n', ' ');
}
