using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaramontanaBibliography.Data.Entities
{
    public class Video : LibraryItem
    {
        public string Director { get; set; }
        public int DurationInMinutes { get; set; }
    }
}
