using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.MVVM;

namespace GenericDatabaseTool.ViewModels
{
    public class ErrorMessageViewModel : ObservableObject
    {
        /// <summary>
        /// Contains the message
        /// </summary>
        private string _message;

        /// <summary>
        /// Gets or sets the message
        /// </summary>
        public string Message
        {
            get => _message;
            set => SetField(ref _message, value);
        }
    }
}
