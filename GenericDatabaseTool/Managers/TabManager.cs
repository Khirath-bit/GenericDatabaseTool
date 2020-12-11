using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using GenericDatabaseTool.ViewModels;
using GenericDatabaseTool.Views;

namespace GenericDatabaseTool.Managers
{
    public static class TabManager
    {
        /// <summary>
        /// Contains session sql strings
        /// </summary>
        private static readonly Dictionary<object, string> SessionSql = new Dictionary<object, string>();

        /// <summary>
        /// Gets the sql of the current session or default
        /// </summary>
        public static string GetSessionSql(object tag)
        {
            return tag != null && SessionSql.ContainsKey(tag) ? SessionSql[tag] : "SELECT * FROM kunden";
        }

        /// <summary>
        /// Starts a new session
        /// </summary>
        public static void StartSession(object tag, string sql)
        {
            SessionSql.Add(tag, sql);
        }

        /// <summary>
        /// Updates a session
        /// </summary>
        public static void UpdateSession(object tag, string sql)
        {
            if(!SessionSql.ContainsKey(tag))
                return;

            SessionSql[tag] = sql;
        }

        /// <summary>
        /// Ends session
        /// </summary>
        public static void EndSession(object tag)
        {
            if (!SessionSql.ContainsKey(tag))
                return;

            SessionSql.Remove(tag);
        }

        /// <summary>
        /// Gets the sql from currently selected tab
        /// </summary>
        public static string GetSqlFromTab(TabItem tab)
        {
            if (!(tab.Content is SqlEditorView view))
                return "";

            if (!(view.DataContext is SqlEditorViewViewModel vm))
                return "";

            return string.IsNullOrWhiteSpace(vm.GetSelectedSql()) ? vm.PlainSql : vm.GetSelectedSql();
        }
    }
}
