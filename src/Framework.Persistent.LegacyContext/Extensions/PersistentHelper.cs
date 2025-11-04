namespace Framework.Persistent;

/// <summary>
/// Класс системных констант.
/// </summary>
public static class PersistentHelper
{
    /// <summary>
    /// Имя базовой системы.
    /// </summary>
    public static string BaseTargetSystemName { get; } = "Base";

    /// <summary>
    /// Идентификатор базовой системы.
    /// </summary>
    public static Guid BaseTargetSystemId { get; } = new ("{E197EEA5-5750-4990-9A4B-6E9ACBC95FA0}");


    public static IReadOnlyDictionary<Type, Guid> BaseTypes { get; } =
        new Dictionary<Type, Guid>
        {
            { typeof(string), new ("{0255b380-68f9-43d5-a731-daf3b860ad09}") },
            { typeof(bool), new ("{21B0FF17-B9E2-4F66-942D-2DFCA09DE861}") },
            { typeof(Guid), new ("{24CEE0A5-330F-4B14-8C64-F4245F79FC6B}") },
            { typeof(int), new ("{73F41360-864F-4C73-B5B3-893A6DF3E400}") },
            { typeof(DateTime), new ("{4A4D65CB-C4A8-4EBC-A1DD-06C00A25D728}") },
            { typeof(decimal), new ("{9499A3CB-26DB-4803-9C53-BB93A6645338}") },
            { typeof(byte), new ("{FECF4BEF-DC2F-44B0-AE9C-9A28B1C5AD3A}") },
            { typeof(double), new ("{68F69CA7-263B-4559-BD96-4A13A28823CC}") },
        };
}
