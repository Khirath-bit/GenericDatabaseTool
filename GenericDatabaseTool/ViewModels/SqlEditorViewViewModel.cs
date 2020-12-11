using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.MVVM;

namespace GenericDatabaseTool.ViewModels
{
    public class SqlEditorViewViewModel : ObservableObject
    {
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
        /// Contains the method to get selected sql
        /// </summary>
        public Func<string> GetSelectedSql { get; set; }
    }
}
