using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaramontanaBibliography.Data.Entities
{
    public abstract class LibraryItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
    }
}
