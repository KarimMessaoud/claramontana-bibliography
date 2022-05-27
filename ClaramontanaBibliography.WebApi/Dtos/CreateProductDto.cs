using System;
using System.ComponentModel.DataAnnotations;

namespace ClaramontanaBibliography.WebApi.Dtos
{
    public class CreateProductDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string Image { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public bool IsAvailable { get; set; }
    }
}
