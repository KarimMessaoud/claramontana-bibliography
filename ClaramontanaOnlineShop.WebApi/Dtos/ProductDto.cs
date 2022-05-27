using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClaramontanaOnlineShop.WebApi.Dtos
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public int Quantity { get; set; }
        public bool IsAvailable { get; set; }
    }
}
