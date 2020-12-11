using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Controls;
using System.Windows.Input;
using Dapper;
using GenericDatabaseTool.Database;
using GenericDatabaseTool.Managers;
using GenericDatabaseTool.Models;
using GenericDatabaseTool.Views;
using Utility.MVVM;
using Application = System.Windows.Application;

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
        /// Contains the editor page
        /// </summary>
        private ObservableCollection<TabItem> _editorPages = new ObservableCollection<TabItem>();

        /// <summary>
        /// Gets or sets the editor pages
        /// </summary>
        public ObservableCollection<TabItem> EditorPages
        {
            get => _editorPages;
            set => SetField(ref _editorPages, value);
        }

        /// <summary>
        /// Contains if the databsae is connected
        /// </summary>
        private bool _isDatabaseConnected;

        /// <summary>
        /// Gets or sets if the database is connected
        /// </summary>
        public bool IsDatabaseConnected
        {
            get => _isDatabaseConnected;
            set => SetField(ref _isDatabaseConnected, value);
        }

        /// <summary>
        /// Contains the scripts
        /// </summary>
        private List<ScriptInfo> _scripts;

        /// <summary>
        /// Gets or sets the scripts
        /// </summary>
        public List<ScriptInfo> Scripts
        {
            get => _scripts;
            set => SetField(ref _scripts, value);
        }

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

        #region TableView

        /// <summary>
        /// Contains the table view width
        /// </summary>
        private string _tableViewWidth = "0";

        /// <summary>
        /// Gets or sets the table view width
        /// </summary>
        public string TableViewWidth
        {
            get => _tableViewWidth;
            set => SetField(ref _tableViewWidth, value);
        }

        /// <summary>
        /// Contains if the table view grid splitter is visible
        /// </summary>
        private bool _isTableViewGridSplitterVisible;

        /// <summary>
        /// Gets or sets if the table view grid splitter is visible
        /// </summary>
        public bool IsTableViewGridSplitterVisible
        {
            get => _isTableViewGridSplitterVisible;
            set => SetField(ref _isTableViewGridSplitterVisible, value);
        }

        /// <summary>
        /// Contains the table view items
        /// </summary>
        private List<TreeViewItem> _tableViewItems;

        /// <summary>
        /// Gets or sets the table view items
        /// </summary>
        public List<TreeViewItem> TableViewItems
        {
            get => _tableViewItems;
            set => SetField(ref _tableViewItems, value);
        }

        #endregion

        /// <summary>
        /// Command to open the settings window
        /// </summary>
        public ICommand OpenSettingsWindow => new DelegateCommand(OpenSettings);

        /// <summary>
        /// Command to run the sql script
        /// </summary>
        public ICommand RunSqlCommand => new RelayCommand((obj) => RunSql(), () => EditorPages.Any());

        /// <summary>
        /// Command to select a databsae
        /// </summary>
        public ICommand SelectDatabaseCommand => new RelayCommand(SelectDatabase, CanSelectDatabase);

        /// <summary>
        /// Command to add a new editor page
        /// </summary>
        public ICommand AddPageCommand => new DelegateCommand(() => AddPage());

        /// <summary>
        /// Command to show table view
        /// </summary>
        public ICommand ShowTableViewCommand => new DelegateCommand(ShowTableView);

        public ICommand SelectScriptCommand => new RelayCommand(SelectScript);

        /// <summary>
        /// Creates a new instance of <see cref="MainWindowViewModel"/>
        /// </summary>
        public MainWindowViewModel()
        {
            SettingsManager.SettingsChanged += SettingsManager_SettingsChanged;
            WindowsUser = Environment.UserName;
            TrySetServerInfos();
            AddPage();
            Scripts = SettingsManager.GetExampleScriptInfos();
        }

        /// <summary>
        /// Selects an example script
        /// </summary>
        /// <param name="param"></param>
        private void SelectScript(object param)
        {
            try
            {
                if (!File.Exists(param.ToString()))
                    return;

                AddPage(File.ReadAllText(param.ToString()));
            }
            catch (Exception)
            {
                //TODO: LOGGING
            }
        }

        /// <summary>
        /// Determines if the database can be selected
        /// </summary>
        private bool CanSelectDatabase()
        {
            return Databases?.Any() == true;
        }

        /// <summary>
        /// Shows table view
        /// </summary>
        private void ShowTableView()
        {
            TableViewWidth = TableViewWidth != "0" ? "0" : "Auto";
            IsTableViewGridSplitterVisible = TableViewWidth != "0";
            TableViewItems = TableViewWidth == "0" ? new List<TreeViewItem>() : MySqlManager.GetTables();
        }

        /// <summary>
        /// Selects the new database
        /// </summary>
        private void SelectDatabase(object name)
        {
            SettingsManager.Database = name.ToString();
            TrySetServerInfos();
            IsDatabaseConnected = true;
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
                IsDatabaseConnected = !string.IsNullOrWhiteSpace(SettingsManager.Database);
            }
            catch (Exception e)
            {
                IsDatabaseConnected = false;
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
            if(!EditorPages.Any())
                return;

            try
            {
                var watch = new Stopwatch();
                var sql = TabManager.GetSqlFromTab(EditorPages.First(f => f.IsSelected));

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

        /// <summary>
        /// Adds a new editor page
        /// </summary>
        public void AddPage(string sql = "SELECT * FROM KUNDEN")
        {
            var newTab = new SqlEditorView {Tag = Guid.NewGuid()};

            TabManager.StartSession(newTab.Tag, sql);

            var nextNum = EditorPages.Count == 0 ? 1 : (int) EditorPages.Last().Tag + 1;

            var newItem = new TabItem()
            {
                Content = newTab,
                Header = $"new{nextNum}",
                IsSelected = true,
                Tag = nextNum
            };

            EditorPages.Add(newItem);

            newItem.MouseDown += NewItem_MouseDown;
        }

        private void NewItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var item = sender as TabItem;

                var control = item.Content as SqlEditorView;

                TabManager.EndSession(control.Tag);

                if (e.ChangedButton == MouseButton.Middle && e.ButtonState == MouseButtonState.Pressed)
                    EditorPages.Remove(item);
            }
            catch (Exception exception)
            {
                
            }
            
        }
    }
}
