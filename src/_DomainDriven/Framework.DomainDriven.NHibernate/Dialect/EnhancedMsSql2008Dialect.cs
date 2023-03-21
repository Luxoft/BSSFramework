using System;

using NHibernate.Dialect;

namespace Framework.DomainDriven.NHibernate;

/// <summary>
/// Represents extended dialect derived from MsSql2008Dialect. Defines nHibernate extensions that works with dates
/// </summary>
[Obsolete("Use EnhancedMsSql2012Dialect")] // 17.1
public class EnhancedMsSql2008Dialect : MsSql2008Dialect
{
    /// <summary>
    /// Registers all our custom functions and defines corresponding MS SQL functions
    /// </summary>
    protected override void RegisterFunctions() => throw new NotImplementedException();
}
