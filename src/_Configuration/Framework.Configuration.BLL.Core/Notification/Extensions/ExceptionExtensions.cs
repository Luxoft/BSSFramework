using System;
using System.Collections.Generic;
using Framework.Core;
using Framework.Core.Services;

namespace Framework.Notification;

public static class ExceptionExtensions
{
    private static string GetMachineName()
    {
        try { return Environment.MachineName; }
        catch { return string.Empty; }
    }

    private static string GetLocation()
    {
        try { return AppDomain.CurrentDomain.BaseDirectory; }
        catch { return string.Empty; }
    }

    public static string ToFormattedString(this Exception source, IUserAuthenticationService userAuthenticationService)
    {
        var strings = new[]
                      {
                              "User: " + userAuthenticationService.GetUserName(),
                              "Machine: " + GetMachineName(),
                              "Path: " + GetLocation(),
                              "---------------------------------------",
                              source.GetFormattedLines().Join(Environment.NewLine)
                      };

        return strings.Join(Environment.NewLine);
    }

    private static IEnumerable<string> GetFormattedLines(this Exception source)
    {
        yield return "Message:";
        yield return source.Message;
        yield return Environment.NewLine;
        yield return "StackTrace:";
        yield return source.StackTrace;
        yield return "---------------------------------------";

        if (source.InnerException != null)
        {
            yield return "InnerException:";

            foreach (var innerLine in source.InnerException.GetFormattedLines())
            {
                yield return innerLine;
            }
        }
    }
}
