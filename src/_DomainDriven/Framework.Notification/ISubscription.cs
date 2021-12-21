namespace Framework.Notification
{
    public interface ISubscription : IMailAddressContainer
    {
        bool IncludeAttachments { get; set; }

        bool SendIndividualLetters { get; set; }
    }
}