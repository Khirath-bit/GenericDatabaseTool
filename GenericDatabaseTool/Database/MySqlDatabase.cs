using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericDatabaseTool.Managers;
using MySql.Data.MySqlClient;

namespace GenericDatabaseTool.Database
{
    public class MySqlDatabase
    {
        /// <summary>
        /// Contains the database connection
        /// </summary>
        private static MySqlConnection _connection;

        /// <summary>
        /// Gets the connection
        /// </summary>
        public static MySqlConnection Connection
        {
            get
            {
                CreateConnection();
                return _connection;
            }
        }

        /// <summary>
        /// Creates a new connection to the database
        /// </summary>
        private static void CreateConnection()
        {
            var connectionString = new MySqlConnectionStringBuilder
            {
                Server = SettingsManager.Host,
                Port = (uint)SettingsManager.Port
            };

            if (!string.IsNullOrWhiteSpace(SettingsManager.Database))
                connectionString.Database = SettingsManager.Database;

            if (!string.IsNullOrWhiteSpace(SettingsManager.User))
                connectionString.UserID = SettingsManager.User;

            if (!string.IsNullOrWhiteSpace(SettingsManager.Password))
                connectionString.Password = SettingsManager.Password;

            _connection = new MySqlConnection(connectionString.ConnectionString);

            _connection.Open();
        }

    }
}
