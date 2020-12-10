using System;
using System.Collections.Generic;
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
        private IEnumerable<Dictionary<string, string>> _itemSource;

        /// <summary>
        /// Gets or sets the item source
        /// </summary>
        public IEnumerable<Dictionary<string, string>> ItemSource
        {
            get => _itemSource;
            set => SetField(ref _itemSource, value);
        }

        /// <summary>
        /// Gets or sets the column function
        /// </summary>
        public List<Dictionary<string, string>> Cols { get; set; }

        /// <summary>
        /// Set columns for later use
        /// </summary>
        public void SetCols(List<IDictionary<string, object>> data)
        {
            var correct = data.Select(s => s.Select(x => new KeyValuePair<string, string>(x.Key, x.Value.ToString())).ToDictionary(x => x.Key, x => x.Value)).ToList();

            Cols = correct;
        }
    }
}
