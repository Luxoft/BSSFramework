using CommonFramework;

using Framework.Core;

using Microsoft.SqlServer.Management.Common;

namespace Framework.DomainDriven.DBGenerator;

/// <summary> Вспомогательный класс для работы со скриптами </summary>
internal static class ScriptsHelper
{
    /// <summary> Ключевое слово "GO" </summary>
    public const string KeywordGo = "GO";

    /// <summary> Получить скрипты для пакетного выполнения </summary>
    /// <param name="capturedSql">Источник скриптов</param>
    /// <param name="clearCapturedSql">Очистить источник скриптов</param>
    public static string GetScriptForBatchExecuting(this CapturedSql capturedSql, bool clearCapturedSql = true)
    {
        if (capturedSql == null)
        {
            throw new ArgumentNullException(nameof(capturedSql));
        }

        var result = GetScriptsForBatchExecutingInternal(capturedSql).Join(Environment.NewLine);
        TryClearCapturedSql(capturedSql, clearCapturedSql);
        return result;
    }

    /// <summary> Получить скрипты для пакетного выполнения </summary>
    /// <param name="capturedSql">Источник скриптов</param>
    /// <param name="clearCapturedSql">Очистить источник скриптов</param>
    public static List<string> GetScriptsForBatchExecuting(this CapturedSql capturedSql, bool clearCapturedSql = true)
    {
        var result = GetScriptsForBatchExecutingInternal(capturedSql).ToList();
        TryClearCapturedSql(capturedSql, clearCapturedSql);
        return result;
    }

    /// <summary> Получить скрипты для пакетного выполнения </summary>
    /// <param name="capturedSql">Источник скриптов</param>
    private static IEnumerable<string> GetScriptsForBatchExecutingInternal(CapturedSql capturedSql)
    {
        if (capturedSql == null)
        {
            throw new ArgumentNullException(nameof(capturedSql));
        }

        return capturedSql.Text.Cast<string>().SelectMany(z => new[] { z, KeywordGo });
    }

    /// <summary> Выполнить попытку очистить источник скриптов </summary>
    /// <param name="capturedSql">Источник скриптов</param>
    /// <param name="clear">Очистить</param>
    private static void TryClearCapturedSql(CapturedSql capturedSql, bool clear)
    {
        if (capturedSql == null)
        {
            throw new ArgumentNullException(nameof(capturedSql));
        }

        if (clear)
        {
            capturedSql.Clear();
        }
    }
}
