using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;

namespace Framework.Authorization.Domain;

/// <summary>
/// Класс считывает настройки из коллекции settings в базе
/// </summary>
public static class SettingExtensions
{
    /// <summary>
    /// Метод возвращает агрегированный типизированный объект Settings
    /// </summary>
    /// <param name="preSettings">Коллекция settings</param>
    /// <returns>Неперсистентный объект settings</returns>
    public static Settings ToSettings(this IEnumerable<Setting> preSettings)
    {
        if (preSettings == null) throw new ArgumentNullException(nameof(preSettings));

        var settings = preSettings.ToDictionary(setting => setting.Key, setting => setting.Value, StringComparer.CurrentCultureIgnoreCase);

        var delegatePermissionLevel = settings.ReadEnum<DelegatePermissionLevel>("delegatePermissionLevel").GetValueOrDefault(DelegatePermissionLevel.Many);

        var delegatePermissionRoleLevel = settings.ReadEnum<DelegatePermissionRoleLevel>("delegatePermissionRoleLevel").GetValueOrDefault(DelegatePermissionRoleLevel.All);

        return new Settings(delegatePermissionLevel, delegatePermissionRoleLevel);
    }

    /// <summary>
    /// Reads the enum.
    /// </summary>
    /// <typeparam name="TEnum">The type of the t enum.</typeparam>
    /// <param name="settings">The settings.</param>
    /// <param name="key">The key.</param>
    /// <returns>Maybe&lt;TEnum&gt;.</returns>
    private static Maybe<TEnum> ReadEnum<TEnum>(this IReadOnlyDictionary<string, string> settings, string key)
            where TEnum : struct, Enum
    {
        return from strValue in settings.GetMaybeValue(key)

               from value in EnumHelper.MaybeParse<TEnum>(strValue)

               select value;
    }
}
