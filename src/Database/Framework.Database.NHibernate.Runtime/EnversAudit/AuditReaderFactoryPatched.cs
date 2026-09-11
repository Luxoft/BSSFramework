using Framework.Core;

namespace Framework.Database.NHibernate.Envers;

public static class AuditReaderFactoryPatched
{
    public static IAuditReaderPatched NotImplemented { get; } =
        LazyInterfaceImplementHelper.CreateNotImplemented<IAuditReaderPatched>("Audit not supported");

    internal static async Task SafeInitCurrentRevisionAsync(this IAuditReaderPatched auditReader, CancellationToken ct)
    {
        if (auditReader == NotImplemented)
        {
            return;
        }

        await auditReader.GetCurrentRevisionAsync(true, ct);
    }
}
