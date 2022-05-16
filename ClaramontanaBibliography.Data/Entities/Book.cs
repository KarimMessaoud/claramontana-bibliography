using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaramontanaBibliography.Data.Entities
{
    public class Book : LibraryItem
    {
        public string Author { get; set; }
        public int NumberOfPages { get; set; }
    }
}
