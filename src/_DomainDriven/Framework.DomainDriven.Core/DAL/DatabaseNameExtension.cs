namespace Framework.DomainDriven
{
    public static class DatabaseNameExtension
    {
        /// <summary>
        /// Создание дефолтного инстанса <see cref="AuditDatabaseName"/>
        /// В котором для хранения аудита испльзуется схема appAudit
        /// </summary>
        public static AuditDatabaseName ToDefaultAudit(this DatabaseName source) => new AuditDatabaseName(source.Name, source.Schema + nameof(Audit), "appAudit");
    }
}
