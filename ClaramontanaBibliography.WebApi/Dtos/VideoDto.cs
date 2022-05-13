using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClaramontanaBibliography.WebApi.Dtos
{
    public class VideoDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Director { get; set; }
        public int Year { get; set; }
        public string ImageUrl { get; set; }
    }
}
