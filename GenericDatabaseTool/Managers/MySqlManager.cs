using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using GenericDatabaseTool.Database;
using MySql.Data.MySqlClient;

namespace GenericDatabaseTool.Managers
{
    public static class MySqlManager
    {
        public static bool? IsSelectStatement(string plainSql)
        {
            var commands = plainSql.Split(';');

            //Does not contain any ; or the final statement does not end with a ;
            if (commands.Length < 2 || !string.IsNullOrWhiteSpace(commands[commands.Length - 1]))
                return null;

            if (commands[commands.Length - 2].Trim().StartsWith("select", StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        }
    }
}
