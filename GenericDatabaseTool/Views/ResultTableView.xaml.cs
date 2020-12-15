using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using GenericDatabaseTool.Managers;
using GenericDatabaseTool.ViewModels;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Macs;
using ServiceStack;
using Utility.MVVM;
using Binding = System.Windows.Data.Binding;
using UserControl = System.Windows.Controls.UserControl;

namespace GenericDatabaseTool.Views
{
    /// <summary>
    /// Interaction logic for ResultTableView.xaml
    /// </summary>
    public partial class ResultTableView : UserControl
    {
        private List<IDictionary<string, object>> _data;

        public ResultTableView( List<IDictionary<string, object>> data)
        {
            InitializeComponent();

            Loaded += ResultTableView_Loaded;

            _data = data;
        }

        private void ResultTableView_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is ResultTableViewViewModel vm)
            {
                vm.SetCols(_data);
                SetColumns(vm.Cols, vm);
            }
        }

        /// <summary>
        /// Set columns based on dictionary
        /// </summary>
        private void SetColumns(List<IDictionary<string, object>> data, ResultTableViewViewModel dataContext)
        {
            var cols = new List<dynamic>();

            var dataCorrect = new List<dynamic>();

            foreach (var row in data[0])
            {
                cols.Add(new KeyValuePair<string, string>(row.Key, Guid.NewGuid().ToString()));
            }

            foreach (var row in cols)
            {
                ResultGrid.Columns.Add(new DataGridTextColumn { Header = row.Key, Binding = new Binding(row.Key) });
            }

            SetRows(data.ToList());
        }

        /// <summary>
        /// Set dynamic rows
        /// </summary>
        private void SetRows(List<IDictionary<string, object>> data)
        {
            foreach (var row in data)
            {
                ResultGrid.Items.Add(row);
            }

            FixCells(data);
        }

        private void FixCells(List<IDictionary<string, object>> data)
        {
            for (int i = 0; i < data.Count(); i++)
            {
                for (int j = 0; j < data[i].Count; j++)
                {
                    ResultGrid.GetCell(i, j).Content = data[i].Values.ElementAt(j);
                }
            }
        }

        private void ExportCsv_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog();

            dialog.FileName = "exportCsv";
            dialog.Filter = "CSV Datei (*.csv)|*.csv";

            if (dialog.ShowDialog() != DialogResult.Cancel)
                File.WriteAllText(dialog.FileName, _data.ToCsv());
        }

        private void ExportJson_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog();

            dialog.FileName = "exportJson";
            dialog.Filter = "JSON Datei (*.json)|*.json";

            if (dialog.ShowDialog() != DialogResult.Cancel)
                File.WriteAllText(dialog.FileName, JsonConvert.SerializeObject(_data));
        }


    }
}
