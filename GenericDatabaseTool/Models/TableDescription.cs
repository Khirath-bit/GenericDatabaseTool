using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace GenericDatabaseTool.Models
{
    public class TableDescription
    {
        /// <summary>
        /// Gets or sets the field name
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// Gets or sets the datatype
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the key
        /// </summary>
        public string Key { get; set; }
    }
}
