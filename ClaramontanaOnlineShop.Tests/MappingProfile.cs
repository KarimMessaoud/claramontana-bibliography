using AutoMapper;
using ClaramontanaOnlineShop.Data.Entities;
using ClaramontanaOnlineShop.WebApi.Dtos;

namespace ClaramontanaOnlineShop.Tests
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product,ProductDto>();
            CreateMap<CreateProductDto, Product>().ReverseMap();
            CreateMap<UpdateProductDto, Product>().ReverseMap();
            // Additional mappings here...
        }
    }
}
