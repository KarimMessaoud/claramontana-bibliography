using System;
using System.ComponentModel.DataAnnotations;

namespace ClaramontanaBibliography.WebApi.Dtos
{
    public class CreateBookDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        public int NumberOfPages { get; set; }
    }
}
