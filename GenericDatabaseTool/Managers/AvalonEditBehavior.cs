using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ICSharpCode.AvalonEdit;
using Microsoft.Xaml.Behaviors;

namespace GenericDatabaseTool.Managers
{
    public sealed class AvalonEditBehaviour : Behavior<TextEditor>
    {
        /// <summary>
        /// Registers a dependency property
        /// </summary>
        public static readonly DependencyProperty SqlProperty =
            DependencyProperty.Register("Sql", typeof(string), typeof(AvalonEditBehaviour),
                new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, PropertyChangedCallback));


        /// <summary>
        /// Gets or sets the sql
        /// </summary>
        public string Sql
        {
            get => (string)GetValue(SqlProperty);
            set => SetValue(SqlProperty, value);
        }

        /// <summary>
        /// Occurs on attaching
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            if (AssociatedObject != null)
            {
                AssociatedObject.TextChanged += AssociatedObjectOnTextChanged;
            }
        }

        /// <summary>
        /// Occurs on detaching
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            if (AssociatedObject != null)
                AssociatedObject.TextChanged -= AssociatedObjectOnTextChanged;
        }

        /// <summary>
        /// Occurs when binding changes
        /// </summary>
        private void AssociatedObjectOnTextChanged(object sender, EventArgs eventArgs)
        {
            var textEditor = sender as TextEditor;

            if (textEditor?.Document != null)
                Sql = textEditor.Document.Text;
        }

        /// <summary>
        /// Property changed callback
        /// </summary>
        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var behavior = dependencyObject as AvalonEditBehaviour;

            var editor = behavior?.AssociatedObject;

            if (editor?.Document == null) 
                return;
            var caretOffset = editor.CaretOffset;
            editor.Document.Text = dependencyPropertyChangedEventArgs.NewValue.ToString();
            editor.CaretOffset = caretOffset;
        }
    }
}
