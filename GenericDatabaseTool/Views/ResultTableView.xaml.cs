using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using GenericDatabaseTool.ViewModels;
using Org.BouncyCastle.Crypto.Macs;
using Utility.MVVM;

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
        private void SetColumns(IReadOnlyList<Dictionary<string, string>> data, ResultTableViewViewModel dataContext)
        {
            foreach (var row in data[0])
            {
                ResultGrid.Columns.Add(new DataGridTextColumn{Header = row.Key, Binding = new Binding(row.Key)});
            }

            SetRows(data);
        }

        /// <summary>
        /// Set dynamic rows
        /// </summary>
        private void SetRows(IEnumerable<Dictionary<string, string>> data)
        {
            foreach (var row in data)
            {
                dynamic eo = row.Aggregate(new ExpandoObject() as IDictionary<string, object>,
                    (a, p) => { a.Add(p.Key, p.Value); return a; });

                ResultGrid.Items.Add(eo);
            }
        }
    }
}
