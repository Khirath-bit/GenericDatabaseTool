using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GenericDatabaseTool.Managers;
using GenericDatabaseTool.ViewModels;
using MahApps.Metro.Controls;

namespace GenericDatabaseTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if(DataContext is MainWindowViewModel vm)
                vm.GetSelectedSql = () => sqleditor.SelectedText;

            AvalonManager.InitAvalonEditor(sqleditor);

            sqleditor.Text = "SELECT * FROM kunden";
        }

        private void Sqleditor_OnKeyDown(object sender, KeyEventArgs e)
        {
            if(!(DataContext is MainWindowViewModel vm))
                return;

            if(e.Key == Key.F5)
                vm.RunSqlCommand.Execute(null);
        }
    }
}
