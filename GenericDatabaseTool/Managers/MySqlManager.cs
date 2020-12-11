using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using Dapper;
using GenericDatabaseTool.Database;
using GenericDatabaseTool.Models;

namespace GenericDatabaseTool.Managers
{
    public static class MySqlManager
    {
        /// <summary>
        /// Checks if the sql command is a command to query
        /// </summary>
        public static bool? IsSelectStatement(string plainSql)
        {
            try
            {
                var commands = plainSql.Split(';');

                //Does not contain any ; or the final statement does not end with a ;
                if (commands.Length < 2 || !string.IsNullOrWhiteSpace(commands[commands.Length - 1]))
                    return null;

                if (commands[commands.Length - 2].Trim().StartsWith("select", StringComparison.OrdinalIgnoreCase) 
                    || commands[commands.Length - 2].Trim().StartsWith("show", StringComparison.OrdinalIgnoreCase)
                    || commands[commands.Length - 2].Trim().StartsWith("descripe", StringComparison.OrdinalIgnoreCase))
                    return true;

                return false;
            }
            catch (Exception e)
            {
                return null;
            }
            
        }

        /// <summary>
        /// Gets all tables
        /// </summary>
        public static List<TreeViewItem> GetTables()
        {
            try
            {
                var tables = MySqlDatabase.Connection.Query<string>("SHOW TABLES").ToList();

                var items = new List<TreeViewItem>();

                tables.ForEach(t => items.Add(new TreeViewItem
                {
                    Header = t,
                    ItemsSource = GetColumns(t)
                }));

                return items;
            }
            catch (Exception e)
            {
                return new List<TreeViewItem>();
            }
        }

        /// <summary>
        /// Gets the columns for a table
        /// </summary>
        private static List<TreeViewItem> GetColumns(string tableName)
        {
            try
            {
                var cols = MySqlDatabase.Connection.Query<TableDescription>($"DESCRIBE {tableName}").ToList();

                var items = new List<TreeViewItem>();

                cols.ForEach(c => items.Add(new TreeViewItem
                {
                    Header = GetName(c),
                    ItemsSource = new List<TreeViewItem>
                    {
                        new TreeViewItem
                        {
                            Header = $"Type: {c.Type}"
                        }
                    }
                }));

                return items;
            }
            catch (Exception e)
            {
                return new List<TreeViewItem>();
            }
        }

        /// <summary>
        /// Gets the formatted name
        /// </summary>
        private static string GetName(TableDescription desc)
        {
            var name = desc.Field;

            if (desc.Key == "MUL")
                name += $" (FK)";

            if (desc.Key == "PRI")
                name += $" (PK)";

            return name;
        }
    }
}
