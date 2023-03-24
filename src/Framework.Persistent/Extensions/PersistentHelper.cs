using System;

namespace Framework.Persistent;

/// <summary>
/// Класс системных констант.
/// </summary>
public static class PersistentHelper
{
    /// <summary>
    /// Имя базовой системы.
    /// </summary>
    public static readonly string BaseTargetSystemName = "Base";

    /// <summary>
    /// Идентификатор базовой системы.
    /// </summary>
    public static readonly Guid BaseTargetSystemId = new Guid("{E197EEA5-5750-4990-9A4B-6E9ACBC95FA0}");

    /// <summary>
    /// Идентификатор доменного типа string.
    /// </summary>
    public static readonly Guid StringDomainTypeId = new Guid("{0255b380-68f9-43d5-a731-daf3b860ad09}");

    /// <summary>
    /// Идентификатор доменного типа bool.
    /// </summary>
    public static readonly Guid BooleanDomainTypeId = new Guid("{21B0FF17-B9E2-4F66-942D-2DFCA09DE861}");

    /// <summary>
    /// Идентификатор доменного типа Guid.
    /// </summary>
    public static readonly Guid GuidDomainTypeId = new Guid("{24CEE0A5-330F-4B14-8C64-F4245F79FC6B}");

    /// <summary>
    /// Идентификатор доменного типа int.
    /// </summary>
    public static readonly Guid Int32DomainTypeId = new Guid("{73F41360-864F-4C73-B5B3-893A6DF3E400}");

    /// <summary>
    /// Идентификатор доменного типа DateTime.
    /// </summary>
    public static readonly Guid DateTimeTypeId = new Guid("{4A4D65CB-C4A8-4EBC-A1DD-06C00A25D728}");

    /// <summary>
    /// Идентификатор доменного типа decimal.
    /// </summary>
    public static readonly Guid DecimalTypeId = new Guid("{9499A3CB-26DB-4803-9C53-BB93A6645338}");
}
