namespace Framework.Database.NHibernate.Fix2100;

public static class SqlHelper
{
    public const int MaxStoredProcedureParametersCount = 2000;

    public static string EscapeSquareBrackets(string text) => text.Replace("[", "[[]");
}
