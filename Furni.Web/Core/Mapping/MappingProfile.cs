﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Furni.Web.Core.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Categories
            CreateMap<Category, SelectListItem>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Name));

            // Products
            CreateMap<Product, ProductViewModel>();
            CreateMap<ProductFormViewModel, Product>().ReverseMap()
                .ForMember(dest => dest.Categories, opt => opt.Ignore());
        }
    }
}
