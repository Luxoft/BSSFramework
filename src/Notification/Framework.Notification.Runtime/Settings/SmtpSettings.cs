namespace Framework.Notification.Settings;

public class SmtpSettings
{
    /// <summary>
    /// false to not send notifications to the smtp server (mainly for development needs)
    /// </summary>
    public bool SmtpEnabled { get; set; }

    public string? OutputFolder { get; set; }

    public string? Server { get; set; }

    public int Port { get; set; } = 25;

    /// <summary>
    /// Mailbox to which notifications without recipients will be redirected (notifications for which having no recipients is allowed at all)
    /// </summary>
    public string[] DefaultReceiverEmails { get; set; } = [];

    /// <summary>
    /// Test mailbox to which notifications from test environments (where !environment.IsProduction()) will be redirected
    /// </summary>
    public string[] TestEmails { get; set; } = [];

    public int MaxSubjectLength { get; set; } = int.MaxValue;
}
