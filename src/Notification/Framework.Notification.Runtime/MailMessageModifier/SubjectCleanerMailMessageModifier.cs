using System.Net.Mail;

using Framework.Core;

namespace Framework.Notification.MailMessageModifier;

public class SubjectCleanerMailMessageModifier : IMailMessageModifier
{
    public void Modify(MailMessage message) => message.Subject = this.Clean(message.Subject);

    public string Clean(string subject) => this.CutSubjectOnRight(this.ReplaceUnsupportedCharactersForSubject(subject));

    private string CutSubjectOnRight(string subject)
    {
        var recommendationLimitCharactersInSubject = 78;
        return subject.SubStringUnsafe(recommendationLimitCharactersInSubject);
    }

    private string ReplaceUnsupportedCharactersForSubject(string subject) => subject.Replace(Environment.NewLine, " ").Replace('\r', ' ').Replace('\n', ' ');
}
