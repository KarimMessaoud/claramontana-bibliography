using AutoMapper;
using ClaramontanaOnlineShop.Data.Entities;


namespace ClaramontanaOnlineShop.Service
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, Product>();
            // Additional mappings here...
        }
    }
}
