using System;

using Framework.DomainDriven.DAL.Revisions;

using NHibernate.Envers;

namespace Framework.DomainDriven.NHibernate.Audit;

internal static class RevisionTypeExtension
{
    /// <summary>
    /// Toes the type of the audit revision.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    public static AuditRevisionType ToAuditRevisionType(this RevisionType source)
    {
        switch (source)
        {
            case RevisionType.Added: return AuditRevisionType.Added;
            case RevisionType.Modified: return AuditRevisionType.Modified;
            case RevisionType.Deleted: return AuditRevisionType.Deleted;
            default: throw new ArgumentException($"unknow {source}");
        }
    }
}
