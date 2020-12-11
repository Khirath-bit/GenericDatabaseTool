using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;

namespace GenericDatabaseTool.Managers
{
    public static class AvalonManager
    {
        /// <summary>
        /// Init the avalon editor
        /// </summary>
        /// <param name="editor">The desired editor</param>
        public static void InitAvalonEditor(TextEditor editor, object tag)
        {
            editor.Options.HighlightCurrentLine = true;
            editor.SyntaxHighlighting = LoadSqlSchema(true);
            editor.Foreground = new SolidColorBrush(Colors.White);
            editor.Text = TabManager.GetSessionSql(tag);
        }

        /// <summary>
        /// Loads the highlight definition for the avalon editor
        /// </summary>
        /// <returns>The definition</returns>
        public static IHighlightingDefinition LoadSqlSchema(bool dark)
        {
            var fileName = "sqldark.xml";
            var file = Path.Combine("Schemes", fileName);

            using (var reader = File.Open(file, FileMode.Open))
            {
                using (var xmlReader = new XmlTextReader(reader))
                {
                    return HighlightingLoader.Load(xmlReader, HighlightingManager.Instance);
                }
            }
        }
    }
}
