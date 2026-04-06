using Framework.Subscriptions.Domain;

namespace Framework.Subscriptions.Old;

public record Recipient(string Login, string Email) : IEmployee;
