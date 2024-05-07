﻿using AutoMapper;
using Furni.Web.Core.ViewModels;
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
            CreateMap<CategoryFormViewModel, Category>().ReverseMap();
            CreateMap<Category, CategoryViewModel>();


            // Products
            CreateMap<Product, ProductViewModel>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category!.Name));
            CreateMap<ProductFormViewModel, Product>().ReverseMap()
                .ForMember(dest => dest.Categories, opt => opt.Ignore());
        }
    }
}
