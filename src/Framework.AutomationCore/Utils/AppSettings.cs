﻿using System;
using System.IO;

using Microsoft.Extensions.Configuration;

namespace Automation.Utils
{
    public class AppSettings
    {
        public static IConfigurationRoot Default { get; set; }

        public static void Initialize()
        {
            Console.WriteLine(Directory.GetCurrentDirectory());

            Default = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($@"appsettings.json", false)
                .AddJsonFile($@"{Environment.MachineName}.appsettings.json", true)
                .Build();

            Console.WriteLine(Default.GetDebugView());
        }
    }
}
