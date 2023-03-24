using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text.RegularExpressions;

using Framework.Core;

using Microsoft.Data.SqlClient;

using NHibernate.Driver;

namespace Framework.DomainDriven.NHibernate;

// <summary>
/// Костыль для обхода ограничения в 2100 параметров в sql запросе (в основном для секьюрити - оптимизирует только in запрос по guid-ам)
/// <see cref="https://iad-tfs-12.luxoft.com/tfs/DefaultCollection/_git/LuxStaff/pullrequest/44424?_a=files&amp;path=%2Fsrc%2FIntHR2.Core%2FNHibernate%2FIntHR2SqlClientDriver.cs"/>
/// </summary>
public class Fix2100SqlClientDriver : MicrosoftDataSqlClientDriver
{
    private const int CountLimit = SqlHelper.MaxStoredProcedureParametersCount;

    protected override void OnBeforePrepare(DbCommand command)
    {
        if (command.Parameters.Count > CountLimit)
        {
            var guidParameters = command.Parameters.Cast<SqlParameter>()
                                        .Where(x => x.SqlDbType == SqlDbType.UniqueIdentifier)
                                        .ToList();
            if (guidParameters.Any())
            {
                InlineIds(command, guidParameters);
            }
        }

        base.OnBeforePrepare(command);
    }

    private static void InlineIds(DbCommand sqlCommand, IList<SqlParameter> guidParameters)
    {
        var replacementMap = guidParameters.ToDictionary(
                                                         x => $"{x.ParameterName}",
                                                         x => x.Value == DBNull.Value ? "null" : $"'{x.Value}'");

        var regex = new Regex(string.Join(@"\b|", replacementMap.Keys));

        sqlCommand.CommandText = regex.Replace(sqlCommand.CommandText, match => replacementMap[match.Value]);

        guidParameters.Foreach(x => sqlCommand.Parameters.Remove(x));
    }
}
