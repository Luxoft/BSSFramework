using Framework.Core;

namespace Framework.Notification;

public class SubjectCleaner : ISubjectCleaner
{
    public string Clean(string subject) => this.CutSubjectOnRight(this.ReplaceUnsupportedCharactersForSubject(subject));

    private string CutSubjectOnRight(string subject)
    {
        var recommendationLimitCharactersInSubject = 78;
        return subject.SubStringUnsafe(recommendationLimitCharactersInSubject);
    }

    private string ReplaceUnsupportedCharactersForSubject(string subject) => subject.Replace(Environment.NewLine, " ").Replace('\r', ' ').Replace('\n', ' ');
}
