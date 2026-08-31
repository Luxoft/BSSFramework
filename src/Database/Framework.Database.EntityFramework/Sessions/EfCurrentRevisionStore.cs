using System.Runtime.CompilerServices;

using Microsoft.EntityFrameworkCore;

namespace Framework.Database.EntityFramework.Sessions;

/// <summary>
/// Tracks the last persisted audit revision id per <see cref="DbContext"/> instance.
/// Mirrors NHibernate/Envers behavior: returns 0 until the session has been flushed at least once,
/// and the actual revision id afterwards. The value is set by the audit flush interceptor
/// (Framework.Database.EntityFramework.Audit) right after a successful flush.
/// </summary>
public static class EfCurrentRevisionStore
{
    private static readonly ConditionalWeakTable<DbContext, StrongBox<long>> Storage = new();

    public static long GetCurrentRevision(DbContext context) =>
        Storage.TryGetValue(context, out var box) ? box.Value : 0;

    public static void SetCurrentRevision(DbContext context, long revision) =>
        Storage.GetOrCreateValue(context).Value = revision;
}
