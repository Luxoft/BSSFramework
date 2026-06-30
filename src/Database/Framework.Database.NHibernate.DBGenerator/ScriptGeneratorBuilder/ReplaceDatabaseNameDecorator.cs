using Framework.Database.NHibernate.DBGenerator.Contracts;
using Framework.Database.NHibernate.DBGenerator.ScriptGenerators.Support;

namespace Framework.Database.NHibernate.DBGenerator.ScriptGeneratorBuilder;

/// <summary>
/// Генерирует коллекцию скриптов для модификации основной базы данных с измененым именем, таких, чтобы она соответсвовала доменной медели
/// </summary>
internal class ReplaceDatabaseNameDecorator(Func<IDatabaseScriptGeneratorContext, DatabaseName> nextDatabaseNameFunc, IDatabaseScriptGenerator source)
    : IDatabaseScriptGenerator
{
    private readonly IDatabaseScriptGenerator source = source ?? throw new ArgumentNullException(nameof(source));
    private readonly Func<IDatabaseScriptGeneratorContext, DatabaseName> nextDatabaseNameFunc = nextDatabaseNameFunc ?? throw new ArgumentNullException(nameof(nextDatabaseNameFunc));

    /// <summary>
    /// Генерирует sql скрипт, который создает и обновляет таблицы и добавляет или удаляет колонки в этих таблицах, а так же создает индексы в этих таблицах
    /// </summary>
    /// <param name="context">Экземпляр sql сервера и доменная модель</param>
    /// <returns>Скрипт модификации</returns>
    public IDatabaseScriptResult GenerateScript(IDatabaseScriptGeneratorContext context) => this.source.GenerateScript(new DatabaseScriptGeneratorContext(this.nextDatabaseNameFunc(context), context.SqlDatabaseFactory, context.AssemblyMetadata));
}

