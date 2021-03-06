﻿using System;
using System.Configuration;
using System.Windows;

namespace WinAdminClientCore.UIHelpers
{
    public class PropertiesManager
    {
        private static Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        public static string UserName
        {
            get => config.AppSettings.Settings["UserName"].Value;
            set
            {
                config.AppSettings.Settings["UserName"].Value = value;
                config.Save(ConfigurationSaveMode.Modified);
            }
        }

        public static string Password
        {
            get => config.AppSettings.Settings["Password"].Value;
            set
            {
                config.AppSettings.Settings["Password"].Value = value;
                config.Save(ConfigurationSaveMode.Modified);
            }
        }

        public static bool RememberMe
        {
            get
            {
                try
                {
                    return Convert.ToBoolean(config.AppSettings.Settings["Remember"].Value);
                }
                catch
                {
                    return false;
                }
            }
            set
            {
                config.AppSettings.Settings["Remember"].Value = value.ToString();
                config.Save(ConfigurationSaveMode.Modified);
            }
        }

        public static string SignalRServer
        {
            get => config.AppSettings.Settings["SignalRServer"].Value;
            set
            {
                config.AppSettings.Settings["SignalRServer"].Value = value;
                config.Save(ConfigurationSaveMode.Modified);
            }
        }

        public static string Hub
        {
            get => config.AppSettings.Settings["Hub"].Value;
            set
            {
                config.AppSettings.Settings["Hub"].Value = value;
                config.Save(ConfigurationSaveMode.Modified);
            }
        }

        public static string Token
        {
            get => config.AppSettings.Settings["Token"].Value;
            set
            {
                config.AppSettings.Settings["Token"].Value = value;
                config.Save(ConfigurationSaveMode.Modified);
            }
        }
    }
}