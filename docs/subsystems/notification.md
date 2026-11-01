# Notifications and subscriptions

Two related subsystems:

- **Notification** (`src/Notification`) — sending emails: queue, processing of recipients and test environments, SMTP.
- **Subscriptions** (`src/Subscriptions`) — rules of the form "when domain object X changes, send email Y": to whom, with which template and under what condition.

Subscriptions are described in code (code-first) and react to changes through the DAL listener `SubscriptionDALListener`. A ready notification goes to Notification.

## Subscription

A subscription is a class inheriting `Subscription<TDomainObject, TRenderingObject>` (`Framework.Subscriptions.Metadata`). `TRenderingObject` is the model for the email template. Often it is the domain object itself.

Example `SampleSystem.Subscriptions.Metadata/Country/Create/CountryCreateSubscription.cs`:

```csharp
public class CountryCreateSubscription : Subscription<Domain.Directories.Country, _Country_Create_MessageTemplate_cshtml>
{
    public override DomainObjectChangeType DomainObjectChangeType { get; } = DomainObjectChangeType.Create;

    public override bool InlineAttachments { get; } = false;

    public override async IAsyncEnumerable<NotificationMessageGenerationInfo<Domain.Directories.Country>> GetTo(
        IServiceProvider serviceProvider, DomainObjectVersions<Domain.Directories.Country> versions)
    {
        yield return new("tester@luxoft.com", versions);
    }

    public override async IAsyncEnumerable<NotificationMessageGenerationInfo<Domain.Directories.Country>> GetCopyTo(
        IServiceProvider serviceProvider, DomainObjectVersions<Domain.Directories.Country> versions)
    {
        yield return new("tester@luxoft.com", versions);
    }
}
```

### What can be overridden

The contract `ISubscription<TDomainObject, TRenderingObject>`:

| Member | Purpose |
|---|---|
| `DomainObjectChangeType` | When the subscription fires: `Create`, `Update`, `Delete` or `Any` (flags) |
| `IsProcessed(...)` | An extra condition: should the subscription be processed for these object versions? |
| `ConvertToRenderingObject(...)` | Converts the domain object into the template model |
| `GetTo` / `GetCopyTo` / `GetReplyTo` | Recipients: primary, copy, Reply-To |
| `GetNotificationFilterGroups(...)` | Recipient filter groups by security roles |
| `GetMessage(...)` | Email subject and body (from the template by default) |
| `GetAttachments(...)` | Attachments |
| `Sender` | Sender address |
| `SendIndividualLetters` | `true` — a separate email for each To / Copy recipient instead of a shared one. ReplyTo is added to every email |
| `InlineAttachments` | Embed attachments from the template |
| `AllowEmptyListOfRecipients` | Allow sending without recipients (for example, for tests) |
| `RecipientMergeType` | How to combine recipients from different sources: `Union`, `Intersect`, `LeftExceptRight`, `RightExceptLeft` |
| `SecurityRule` | Security rule for selecting recipients by roles |

Object versions arrive in `DomainObjectVersions<T>` (previous and current): compare them to decide whether an email is needed.

Recipients can be specified as a string of emails separated by commas or semicolons (`new("a@x.com; b@x.com", versions)`): addresses are cleaned and deduplicated case-insensitively. For other recipient types there is `NotificationMessageGenerationInfo<TRenderingObject, TRecipient>`.

### Email template

Next to the subscription lie a Razor template `MessageTemplate.cshtml` and the `MessageTemplate.generated.cs` generated from it (`RazorTemplate`). The template class is what is passed as the second parameter of `Subscription<…>`. The `.generated.cs` file is created by the Razor generation tool and must not be edited by hand.

### Metadata project structure

```
SampleSystem.Subscriptions.Metadata/
  Country/Create/
    CountryCreateSubscription.cs
    MessageTemplate.cshtml
    MessageTemplate.generated.cs
  DataModel/Create/...
  DomainChangedByRecipients/NotPersistentCustomModel/...
```

The convention is to group by domain type and change type: `<Type>/<Create|Update|…>/`.

## Wiring

In `AddGeneralDependencyInjection`:

```csharp
.AddSubscriptions(
    (Employee e) => e.Email,                                              // how to get an employee's email
    [typeof(EmployeeUpdateSubscription).Assembly],                        // assemblies with subscriptions
    new("SampleSystem@luxoft.com", "SampleSystem"))                       // default sender address and name
.AddSubscriptionManagers()                                                // see below
.AddLegacyNotification(configuration, hostEnvironment)
```

The `SubscriptionDALListener` listener is additionally registered:

```csharp
settings.AddListener<SubscriptionDALListener>()
```

In SampleSystem, `AddLegacyNotification` calls `settings.AddNotification(configuration, isProduction)` and replaces the sender with `LocalDbNotificationMessageSender`, which puts emails into a local DB instead of actually sending them. For production, keep the default implementation (SMTP).

## Sending an email

The chain in `Framework.Notification.Runtime`:

1. `RootNotificationMessageSender` receives a `Notification`. If there are no recipients, it logs a warning and redirects the email to support.
2. `IActualNotificationProcessor` → `ActualMailMessageProcessor`: clones the message and applies the chain of `IMailMessageModifier` **in DI registration order**. Each sees the result of the previous one, so order matters (for example, redirecting to a test address must come before anything that relies on the final recipients).
3. The inner sender (by key `RootNotificationMessageSender.InnerKey`): SMTP (`SmtpNotificationMessageSender`) or your own.
4. `ISentNotificationLogger` records the fact of sending.

Ready-made modifiers (`MailMessageModifier/`):

| Modifier | What it does |
|---|---|
| `RedirectToSupportMailMessageModifier` | If there is no recipient at all (To / CC / Bcc), adds `DefaultReceiverEmails`. Must come after modifiers that change the recipient list |
| `RedirectToTestAddress` | If `TestEmails` is set, removes all real recipients (To / CC / Bcc / ReplyTo) and sends only to the test addresses. Inserts the original recipients at the start of the body (the body is treated as HTML) |
| `RewriteReceiversMailMessageModifier` | Replaces addresses by the `RewriteReceiversSettings` rules. **If at least one rule is set, recipients without a matching rule are dropped.** Without rules the email is unchanged |
| `SubjectCleanerMailMessageModifier` | Replaces line breaks in the subject with spaces and truncates the subject to `MaxSubjectLength` |
| `GdprMailMessageModifier` | Cuts `<!--GDPR-->…<!--/GDPR-->` sections out of the body (case-insensitive, nested sections are processed from the inside). An unclosed section stays in the text |
| `HtmlMarkerMessageModifier` | Sets `IsBodyHtml = true` if the body starts with `<html` |

For your own modifier: implement `IMailMessageModifier` and register it in DI (its position in the registration order determines when it is called).

> To keep a template section out of the email, wrap it in `<!--GDPR-->…<!--/GDPR-->`, provided `GdprMailMessageModifier` is registered in your application.

## Configuration

Example from SampleSystem's `appsettings.json`:

```json
"SmtpSettings": {
  "SmtpEnabled": "false",
  "OutputFolder": "C:\\Logs\\SampleSystem\\emails",
  "Server": "puppy",
  "Port": 25,
  "DefaultReceiverEmails": [ "test3@luxoft.fake" ],
  "TestEmails": [ "IadTest@luxoft.fake" ]
},
"RewriteReceiversSettings": {
  "RewriteRules": [
    { "From": "test1@luxoft.fake", "To": [ "test2@luxoft.fake" ] }
  ]
}
```

`SmtpSettings`:

| Parameter | Meaning |
|---|---|
| `SmtpEnabled` | `false` — do not send to the SMTP server (for development) |
| `OutputFolder` | Directory where emails are saved |
| `Server`, `Port` | SMTP server, default port 25 |
| `DefaultReceiverEmails` | Addresses for emails without recipients (when an empty list is allowed) |
| `TestEmails` | Test addresses for non-production environments |
| `MaxSubjectLength` | Maximum subject length, unlimited by default |

`RewriteReceiversSettings.RewriteRules`: a list of `From` → `To[]` rules.

## Subscription managers

`AddSubscriptionManager<T>()` registers the application's "subscription managers" (in SampleSystem: `ExampleSampleSystemEventsSubscriptionManager`, `ExampleSampleSystemAribaEventsSubscriptionManager`). They belong to the old API (the `// Legacy` block in the DI composition, see [legacy.md](../legacy.md)). For new subscriptions use `Subscription<,>` classes.

## Scheduled sending

The job `ISendNotificationsJob` (`Framework.Configuration.BLL.Jobs`) calls `DomainObjectModification.ProcessAsync(1000, ct)`, i.e. it processes accumulated domain object changes in batches of 1000. In the host it is registered with `Cron.Never()`: it is run manually or on the schedule from `Hangfire:JobTimings` (see [jobs.md](jobs.md)).

## Tests

- Unit tests: `src/Notification/Framework.Notification.Tests`.
- Integration: `SubscriptionCustomNotPersistentModelTests`, `NotificationCountryTests`, `GetNotificationPrincipalsByHierarchicalContextTests`, and the `Subscriptions_Metadata` directory in `SampleSystem.IntegrationTests`.
