using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Dapper;
using GenericDatabaseTool.Database;
using GenericDatabaseTool.Managers;
using GenericDatabaseTool.Models;
using GenericDatabaseTool.Views;
using MySqlX.XDevAPI.Common;
using Utility.MVVM;

namespace GenericDatabaseTool.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        /// <summary>
        /// Contains the view
        /// </summary>
        private object _view;

        /// <summary>
        /// Gets or sets the view
        /// </summary>
        public object View
        {
            get => _view;
            set => SetField(ref _view, value);
        }

        /// <summary>
        /// Contains the sql plain text
        /// </summary>
        private string _sqlPlain;

        /// <summary>
        /// Gets or sets the sql plain text
        /// </summary>
        public string PlainSql
        {
            get => _sqlPlain;
            set => SetField(ref _sqlPlain, value);
        }

        /// <summary>
        /// Contains the selected plain sql 
        /// </summary>
        private string _selectedPlainSql;

        /// <summary>
        /// Gets or sets the selected plain sql
        /// </summary>
        public string SelectedPlainSql
        {
            get => _selectedPlainSql;
            set => SetField(ref _selectedPlainSql, value);
        }

        /// <summary>
        /// Contains the database names
        /// </summary>
        private List<DatabaseInfo> _databases;

        /// <summary>
        /// Gets or sets the databases
        /// </summary>
        public List<DatabaseInfo> Databases
        {
            get => _databases;
            set => SetField(ref _databases, value);
        }

        /// <summary>
        /// Contains the selected database
        /// </summary>
        private string _selectedDatabase;

        /// <summary>
        /// Gets or sets the selected database
        /// </summary>
        public string SelectedDatabase
        {
            get => _selectedDatabase;
            set => SetField(ref _selectedDatabase, value);
        }

        /// <summary>
        /// Contains the method to get selected sql
        /// </summary>
        public Func<string> GetSelectedSql { get; set; }

        #region StatusBar

        /// <summary>
        /// Contains the hostname
        /// </summary>
        private string _connectionString;

        /// <summary>
        /// Gets or sets the hostname
        /// </summary>
        public string ConnectionString
        {
            get => _connectionString;
            set => SetField(ref _connectionString, value);
        }

        /// <summary>
        /// Contains the elapsed query time
        /// </summary>
        private TimeSpan _elapsedQueryTime;

        /// <summary>
        /// Gets or sets the elapsed time of the query
        /// </summary>
        public TimeSpan ElapsedQueryTime
        {
            get => _elapsedQueryTime;
            set => SetField(ref _elapsedQueryTime, value);
        }

        /// <summary>
        /// Contains the windows user
        /// </summary>
        private string _windowsUser;

        /// <summary>
        /// Gets or sets the windows user
        /// </summary>
        public string WindowsUser
        {
            get => _windowsUser;
            set => SetField(ref _windowsUser, value);
        }

        #endregion

        /// <summary>
        /// Command to open the settings window
        /// </summary>
        public ICommand OpenSettingsWindow => new DelegateCommand(OpenSettings);

        /// <summary>
        /// Command to run the sql script
        /// </summary>
        public ICommand RunSqlCommand => new DelegateCommand(RunSql);

        /// <summary>
        /// Command to select a databsae
        /// </summary>
        public ICommand SelectDatabaseCommand => new RelayCommand(SelectDatabase, CanSelectDatabase);

        /// <summary>
        /// Creates a new instance of <see cref="MainWindowViewModel"/>
        /// </summary>
        public MainWindowViewModel()
        {
            SettingsManager.SettingsChanged += SettingsManager_SettingsChanged;
            WindowsUser = Environment.UserName;
            TrySetServerInfos();
        }

        /// <summary>
        /// Determines if the database can be selected
        /// </summary>
        private bool CanSelectDatabase()
        {
            return Databases?.Any() == true;
        }

        /// <summary>
        /// Selects the new database
        /// </summary>
        private void SelectDatabase(object name)
        {
            SettingsManager.Database = name.ToString();
            TrySetServerInfos();
        }

        /// <summary>
        /// Tries to initially set server infos
        /// </summary>
        private void TrySetServerInfos()
        {
            try
            {
                ConnectionString = MySqlDatabase.Connection.ConnectionString;
                Databases = MySqlDatabase.Connection.Query<DatabaseInfo>("SHOW DATABASES").ToList();
                ChangeView(new ErrorMessageViewModel { Message = "" });
            }
            catch (Exception e)
            {
                ConnectionString = "ERROR";
                Databases = new List<DatabaseInfo>();
                ChangeView(new ErrorMessageViewModel { Message = e.Message });
            }
        }

        /// <summary>
        /// Runs the sql script
        /// </summary>
        private void RunSql()
        {
            try
            {
                var watch = new Stopwatch();

                var selectedSql = GetSelectedSql();

                var sql = string.IsNullOrWhiteSpace(selectedSql) ? PlainSql : selectedSql;

                var isSelect = MySqlManager.IsSelectStatement(sql);

                if (isSelect == null)
                {
                    ChangeView(new ErrorMessageViewModel { Message = "Syntax error! Please add a ';' after every statement." });
                    return;
                }

                if (isSelect == true)
                {
                    watch.Start();
                    var response = MySqlDatabase.Connection.Query(sql).ToList();
                    watch.Stop();
                    UpdateQueryTimeStatus(watch.Elapsed);
                    var result = response.Select(x => (IDictionary<string, object>)x).ToList();

                    if (!result.Any())
                    {
                        ChangeView(new ErrorMessageViewModel { Message = "The query yielded 0 results." });
                        return;
                    }

                    var v = new ResultTableView(result);
                    ChangeView(v);

                    return;
                }

                watch.Start();
                var rowsAffected = MySqlDatabase.Connection.Execute(sql);
                watch.Stop();
                UpdateQueryTimeStatus(watch.Elapsed);
                ChangeView(new ErrorMessageViewModel { Message = $"{rowsAffected} rows affected." });
            }
            catch (Exception e)
            {
                ChangeView(new ErrorMessageViewModel { Message = e.Message });
            }

        }

        /// <summary>
        /// Update query time status
        /// </summary>
        private void UpdateQueryTimeStatus(TimeSpan time)
        {
            ElapsedQueryTime = time;
        }

        /// <summary>
        /// Occurs when the settings changed
        /// </summary>
        private void SettingsManager_SettingsChanged(object sender, EventArgs e)
        {
            TrySetServerInfos();
        }

        /// <summary>
        /// Changes the view
        /// </summary>
        /// <param name="view">viewmodel of the view to change to</param>
        public void ChangeView(object view)
        {
            View = view;
        }

        /// <summary>
        /// Opens the settings window
        /// </summary>
        public void OpenSettings()
        {
            var settingsWindow =
                new SettingsWindow { Owner = Application.Current.MainWindow };
            settingsWindow.ShowDialog();
        }
    }
}
