using Anch.Core;

using Microsoft.SqlServer.Management.Common;

namespace Framework.Database.NHibernate.DBGenerator;

/// <summary> Helper class for working with scripts </summary>
internal static class ScriptsHelper
{
    /// <summary> The "GO" keyword </summary>
    public const string KeywordGo = "GO";

    /// <summary> Get scripts for batch execution </summary>
    /// <param name="capturedSql">Script source</param>
    /// <param name="clearCapturedSql">Clear the script source</param>
    public static string GetScriptForBatchExecuting(this CapturedSql capturedSql, bool clearCapturedSql = true)
    {
        if (capturedSql is null)
        {
            throw new ArgumentNullException(nameof(capturedSql));
        }

        var result = GetScriptsForBatchExecutingInternal(capturedSql).Join(Environment.NewLine);
        TryClearCapturedSql(capturedSql, clearCapturedSql);
        return result;
    }

    /// <summary> Get scripts for batch execution </summary>
    /// <param name="capturedSql">Script source</param>
    /// <param name="clearCapturedSql">Clear the script source</param>
    public static List<string> GetScriptsForBatchExecuting(this CapturedSql capturedSql, bool clearCapturedSql = true)
    {
        var result = GetScriptsForBatchExecutingInternal(capturedSql).ToList();
        TryClearCapturedSql(capturedSql, clearCapturedSql);
        return result;
    }

    /// <summary> Get scripts for batch execution </summary>
    /// <param name="capturedSql">Script source</param>
    private static IEnumerable<string> GetScriptsForBatchExecutingInternal(CapturedSql capturedSql)
    {
        if (capturedSql is null)
        {
            throw new ArgumentNullException(nameof(capturedSql));
        }

        return capturedSql.Text.Cast<string>().SelectMany(z => new[] { z, KeywordGo });
    }

    /// <summary> Try to clear the script source </summary>
    /// <param name="capturedSql">Script source</param>
    /// <param name="clear">Clear</param>
    private static void TryClearCapturedSql(CapturedSql capturedSql, bool clear)
    {
        if (capturedSql is null)
        {
            throw new ArgumentNullException(nameof(capturedSql));
        }

        if (clear)
        {
            capturedSql.Clear();
        }
    }
}
