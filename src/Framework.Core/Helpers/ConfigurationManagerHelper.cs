using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using Framework.Core.Serialization;

namespace Framework.Core
{
    public static class ConfigurationManagerHelper
    {
        public static string GetAppSettings(string name, bool raiseNotInitializeError)
        {
            var value = ConfigurationManager.AppSettings[name];

            if (raiseNotInitializeError && string.IsNullOrWhiteSpace(value))
            {
                throw new Exception($"\"{name}\" section in appsettings not initialized");
            }

            return value;
        }

        public static T GetAppSettings<T>(string name)
        {
            return ParserHelper.Parse<T>(GetAppSettings(name, true));
        }
    }
}
