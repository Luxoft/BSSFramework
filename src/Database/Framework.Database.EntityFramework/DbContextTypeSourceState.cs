using System.Collections.Concurrent;

namespace Framework.Database.EntityFramework;

public record DbContextTypeSourceState
{
    public ConcurrentDictionary<Type, Type> Dictionary { get; } = [];
}
