using System;
using System.ComponentModel.DataAnnotations;

namespace ClaramontanaBibliography.WebApi.Dtos
{
    public class CreateVideoDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Director { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        public int DurationInMinutes { get; set; }
    }
}
