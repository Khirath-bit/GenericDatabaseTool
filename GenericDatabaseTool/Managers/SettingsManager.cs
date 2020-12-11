using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Xml;
using GenericDatabaseTool.Models;
using Newtonsoft.Json;

namespace GenericDatabaseTool.Managers
{
    public static class SettingsManager
    {
        /// <summary>
        /// Contains the event handler for SettingsChanged
        /// </summary>
        public static event EventHandler SettingsChanged;

        /// <summary>
        /// Contains the hostname
        /// </summary>
        public static string Host { get; set; }

        /// <summary>
        /// Contains the port
        /// </summary>
        public static int Port { get; set; }

        /// <summary>
        /// Contains the database
        /// </summary>
        public static string Database { get; set; }

        /// <summary>
        /// Contains the user
        /// </summary>
        public static string User { get; set; }

        /// <summary>
        /// Contains the password
        /// </summary>
        public static string Password { get; set; }

        /// <summary>
        /// Save the settings
        /// </summary>
        public static void Save()
        {
            try
            {
                var settings = new { Host, Port, User, Password };

                var json = JsonConvert.SerializeObject(settings);

                File.WriteAllText("appsettings.json", json);

                SettingsChanged?.Invoke(null, null);
            }
            catch (Exception e)
            {
#if DEBUG
                MessageBox.Show(e.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
#else
                MessageBox.Show("Failed to save the settings.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
#endif
            }
        }

        /// <summary>
        /// Loads the settings
        /// </summary>
        public static void Load(bool fromStartup = false)
        {
            try
            {
                var json = File.ReadAllText("appsettings.json");

                var settings = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

                Host = settings[nameof(Host)];

                Password = settings[nameof(Password)];

                User = settings[nameof(User)];

                if (int.TryParse(settings[nameof(Port)], out var port))
                    Port = port;

                SettingsChanged?.Invoke(null, null);
            }
            catch (Exception e)
            {
#if DEBUG
                if(!fromStartup)
                    MessageBox.Show(e.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
#else
                MessageBox.Show("Failed to load the settings. File does not exist?", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
#endif
            }
        }

        /// <summary>
        /// Gets example script information
        /// </summary>
        public static List<ScriptInfo> GetExampleScriptInfos()
        {
            return new List<ScriptInfo>
            {
                new ScriptInfo
                {
                    Name = "Create Table",
                    Path = Path.Combine(GetBaseFolder(), "ExampleScripts\\CreateTable.sql")
                },
                new ScriptInfo
                {
                    Name = "Select",
                    Path = Path.Combine(GetBaseFolder(), "ExampleScripts\\Select.sql")
                },
                new ScriptInfo
                {
                    Name = "Insert",
                    Path = Path.Combine(GetBaseFolder(), "ExampleScripts\\Insert.sql")
                },
                new ScriptInfo
                {
                    Name = "Join",
                    Path = Path.Combine(GetBaseFolder(), "ExampleScripts\\Join.sql")
                },
            };
        }

        /// <summary>
        /// Gets the path of the base folder, for the application that is currently running
        /// </summary>
        /// <returns>The path of the base folder</returns>
        public static string GetBaseFolder()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

    }
}
