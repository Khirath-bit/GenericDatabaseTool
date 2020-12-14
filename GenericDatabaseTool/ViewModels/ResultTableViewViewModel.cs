using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.MVVM;

namespace GenericDatabaseTool.ViewModels
{
    public class ResultTableViewViewModel : ObservableObject
    {
        /// <summary>
        /// Contains the item source
        /// </summary>
        private List<object> _itemSource;

        /// <summary>
        /// Gets or sets the item source
        /// </summary>
        public List<object> ItemSource
        {
            get => _itemSource;
            set => SetField(ref _itemSource, value);
        }

        /// <summary>
        /// Gets or sets the column function
        /// </summary>
        public List<IDictionary<string, object>> Cols { get; set; }

        /// <summary>
        /// Set columns for later use
        /// </summary>
        public void SetCols(List<IDictionary<string, object>> data)
        {
            Cols = data;
        }
    }
}
