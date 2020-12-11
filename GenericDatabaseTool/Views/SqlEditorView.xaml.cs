using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GenericDatabaseTool.Managers;
using GenericDatabaseTool.ViewModels;

namespace GenericDatabaseTool.Views
{
    /// <summary>
    /// Interaction logic for SqlEditorView.xaml
    /// </summary>
    public partial class SqlEditorView : UserControl
    {

        public SqlEditorView()
        {
            InitializeComponent();
            Loaded += SqlEditorView_Loaded;
            sqleditor.TextChanged += Sqleditor_TextChanged;
        }

        /// <summary>
        /// Occurs when the text changes
        /// </summary>
        private void Sqleditor_TextChanged(object sender, System.EventArgs e)
        {
            TabManager.UpdateSession(Tag, sqleditor.Text);
        }

        /// <summary>
        /// Occurs when the control is loaded
        /// </summary>
        private void SqlEditorView_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is SqlEditorViewViewModel vm)
                vm.GetSelectedSql = () => sqleditor.SelectedText;

            AvalonManager.InitAvalonEditor(sqleditor, Tag);
        }

        private void SqlEditorView_OnDrop(object sender, DragEventArgs e)
        {
            try
            {
                if (!e.Data.GetDataPresent(DataFormats.FileDrop))
                    return;

                if (e.Data.GetData(DataFormats.FileDrop) is string[] files && files.Length > 0)
                {
                    sqleditor.Text = File.ReadAllText(files[0]);
                }
            }
            catch (Exception exception)
            {
                
            }
            
        }
    }
}
