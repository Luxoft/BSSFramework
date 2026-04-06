using System.Collections.Immutable;

namespace Framework.Subscriptions.Old;

public record RecipientBag(ImmutableHashSet<string> To, ImmutableHashSet<string> CopyTo, ImmutableHashSet<string> ReplyTo);
