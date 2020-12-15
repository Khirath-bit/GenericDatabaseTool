using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using GenericDatabaseTool.Managers;
using GenericDatabaseTool.Models;
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
            var cols = new List<KeyValuePair<string, string>>();

            var dataCorrect = new List<dynamic>();

            foreach (var row in data[0])
            {
                cols.Add(new KeyValuePair<string, string>(row.Key, Guid.NewGuid().ToString()));
            }

            foreach (var row in cols)
            {
                ResultGrid.Columns.Add(new DataGridTextColumn { Header = row.Key, Binding = new Binding(row.Value) });
            }

            SetRows(data.ToList(), cols);
        }

        /// <summary>
        /// Set dynamic rows
        /// </summary>
        private void SetRows(List<IDictionary<string, object>> data, List<KeyValuePair<string, string>> cols)
        {
            var correctData = new List<DynamicRow>();

            foreach (var d in data)
            {
                var props = new Dictionary<string, object>();

                for (int i = 0; i < cols.Count; i++)
                {
                    props.Add(cols.ElementAt(i).Value, d.ElementAt(i).Value);
                }

                correctData.Add(new DynamicRow(props));
            }

            foreach (var d in correctData)
            {
                ResultGrid.Items.Add(d);
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
