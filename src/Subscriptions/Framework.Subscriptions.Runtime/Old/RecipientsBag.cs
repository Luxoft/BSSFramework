using System.Collections.Immutable;

using Framework.Subscriptions.Domain;

namespace Framework.Subscriptions.Old;

public record RecipientsBag(ImmutableArray<IEmployee> To, ImmutableArray<IEmployee> CopyTo, ImmutableArray<IEmployee> ReplyTo);
