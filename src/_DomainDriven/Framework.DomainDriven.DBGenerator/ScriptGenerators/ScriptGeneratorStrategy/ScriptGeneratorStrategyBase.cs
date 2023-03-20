using System;
using System.Collections.Generic;
using System.Linq;

using Framework.DomainDriven.DAL.Sql;
using Framework.DomainDriven.DBGenerator.Team;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace Framework.DomainDriven.DBGenerator.ScriptGenerators.ScriptGeneratorStrategy;

/// <summary>
/// Базовый класс для генерации части скрипта модификации базы данных
/// </summary>
internal abstract class ScriptGeneratorStrategyBase
{
    protected readonly DatabaseScriptGeneratorStrategyInfo Parameter;
    protected readonly Server Server;

    protected ScriptGeneratorStrategyBase(DatabaseScriptGeneratorStrategyInfo parameter)
    {
        this.Parameter = parameter;
        this.Server = parameter.Context.GetServer();
    }

    /// <summary>
    /// Мод применяемого миграционого скрипта
    /// </summary>
    public abstract ApplyMigrationDbScriptMode ApplyMigrationDbScriptMode { get; }

    /// <summary>
    /// Генерирует SQL скрипт по алгоритму реализованному в конкретной стратегии
    /// </summary>
    /// <returns>Функция возвращающая коллекцию SQL команд</returns>
    public Func<IEnumerable<string>> Execute()
    {
        return () =>
               {
                   this.ExecuteStart();
                   this.ExecuteBase();
                   return this.ExecuteEnd();
               };
    }

    /// <summary>
    /// Модификации базы данных по определенной стратегии
    /// </summary>
    protected abstract void ExecuteBase();

    /// <summary>
    /// Ищет колонку с именем <paramref name="sqlMapping.Name"/> в таблицы <paramref name="table"/>
    /// </summary>
    /// <param name="table">Таблица в которой будет искатся колонка</param>
    /// <param name="sqlMapping">Информация о колонке</param>
    /// <returns>Найденная колонка иначе null</returns>
    protected Column GetColumn(Table table, SqlFieldMappingInfo sqlMapping)
    {
        var fieldName = sqlMapping.Name;
        var column = table.Columns.Cast<Column>().FirstOrDefault(c => c.Name.Equals(fieldName, StringComparison.CurrentCultureIgnoreCase));
        return column;
    }

    private void ExecuteStart()
    {
        this.Server.ConnectionContext.CapturedSql.Clear();
        this.Server.ConnectionContext.SqlExecutionModes = SqlExecutionModes.ExecuteAndCaptureSql;
        this.Server.ConnectionContext.CapturedSql.Add(Environment.NewLine);
        this.Server.ConnectionContext.CapturedSql.Add("------------------------------ START -----------------------");
    }

    private IEnumerable<string> ExecuteEnd()
    {
        this.Server.ConnectionContext.CapturedSql.Add("------------------------------ END -----------------------");
        var result = this.Server.ConnectionContext.CapturedSql.GetScriptsForBatchExecuting();
        return result;
    }
}
