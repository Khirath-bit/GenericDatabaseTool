using System.Windows;
using System.Windows.Input;
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
            KeyDown += MainWindow_MouseDown;
        }

        private void MainWindow_MouseDown(object sender, KeyEventArgs e)
        {
            if (!(DataContext is MainWindowViewModel vm) || e.Key != Key.F5)
                return;

            if(vm.RunSqlCommand.CanExecute(null))
                vm.RunSqlCommand.Execute(null);
        }
    }
}
