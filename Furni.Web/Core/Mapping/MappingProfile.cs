using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Furni.Web.Core.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Categories

            // Products
            CreateMap<Product, ProductViewModel>();
            CreateMap<ProductFormViewModel, Product>().ReverseMap();
        }
    }
}
