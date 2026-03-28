using Framework.Database.NHibernate.DBGenerator.Contracts;
using Framework.Database.NHibernate.DBGenerator.ScriptGenerators.Support;

namespace Framework.Database.NHibernate.DBGenerator.ScriptGeneratorBuilder;

/// <summary>
/// Генерирует коллекцию скриптов для модификации основной базы данных с измененым именем, таких, чтобы она соответсвовала доменной медели
/// </summary>
internal class ReplaceDatabaseNameDecorator : IDatabaseScriptGenerator
{
    private readonly IDatabaseScriptGenerator source;
    private readonly Func<IDatabaseScriptGeneratorContext, DatabaseName> nextDatabaseNameFunc;

    public ReplaceDatabaseNameDecorator(Func<IDatabaseScriptGeneratorContext, DatabaseName> nextDatabaseNameFunc, IDatabaseScriptGenerator source)
    {
        if (nextDatabaseNameFunc == null)
        {
            throw new ArgumentNullException(nameof(nextDatabaseNameFunc));
        }

        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        this.nextDatabaseNameFunc = nextDatabaseNameFunc;
        this.source = source;
    }

    /// <summary>
    /// Генерирует sql скрипт, который создает и обновляет таблицы и добавляет или удаляет колонки в этих таблицах, а так же создает индексы в этих таблицах
    /// </summary>
    /// <param name="context">Экземпляр sql сервера и доменная модель</param>
    /// <returns>Скрипт модификации</returns>
    public IDatabaseScriptResult GenerateScript(IDatabaseScriptGeneratorContext context)
    {
        return this.source.GenerateScript(new DatabaseScriptGeneratorContext(this.nextDatabaseNameFunc(context), context.SqlDatabaseFactory, context.AssemblyMetadata));
    }
}
